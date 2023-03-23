using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UtilsGlobais;
using static InfraBanco.Constantes.Constantes;

namespace ArClube.Mensageria
{
    public sealed class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ConfigurationBuilder _config;
        private readonly IMensageriaRepositorio _mensageriaRepositorio;

        public Worker(
            ILogger<Worker> logger,
            ConfigurationBuilder config,
            IMensageriaRepositorio mensageriaRepositorio)
        {
            this._logger = logger;
            this._config = config;
            this._mensageriaRepositorio = mensageriaRepositorio;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Started");

            this._logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Obter informações de arquivo de configuração");
            var configuration = _config.AddEnvironmentVariables().AddJsonFile("appsettings.json").Build();
            var recordsPerPage = configuration.GetSection("EmailsPerCicleToGetAndSend").Value.ToInt().Value;
            var secondsToDelayAfterSend = configuration.GetSection("SecondsToDelayAfterSend").Value.ToInt().Value;
            var shouldGenerateLog = bool.Parse(configuration.GetSection("GerarLogProcessoAutomatizado").Value);

            var attempt1_2 = await this._mensageriaRepositorio.ObterParametroPorId("OrctoCotacao_Mensageria_Queue_IntervaloMinEmSegundos_Tentativa_1_2");
            var attempt2_3 = await this._mensageriaRepositorio.ObterParametroPorId("OrctoCotacao_Mensageria_Queue_IntervaloMinEmSegundos_Tentativa_2_3");
            var attemptVery = await this._mensageriaRepositorio.ObterParametroPorId("OrctoCotacao_Mensageria_Queue_IntervaloMinEmSegundos_Tentativa_Demais");
            var attemptQtdMaxParam = await this._mensageriaRepositorio.ObterParametroPorId("OrctoCotacao_Mensageria_Queue_QtdeMaxTentativas");

            if (shouldGenerateLog)
            {
                this._logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Obter Unidade Neogocio Parametros");
            }

            var unidadeNeogocioParametros = await this._mensageriaRepositorio.ObterUnidadeNeogocioParametrosAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (shouldGenerateLog)
                    {
                        this._logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Obter Orcamento Cotacao Emails");
                    }

                    var orcamentoCotacaoEmailsQueue = await this._mensageriaRepositorio.ObterOrcamentoCotacaoEmailsQueueAsync(recordsPerPage);

                    if (orcamentoCotacaoEmailsQueue.Count > 0)
                    {
                        foreach (var orcamentoCotacaoEmailsQueueItem in orcamentoCotacaoEmailsQueue)
                        {
                            var parametroEmail = ObteParametroEmail(unidadeNeogocioParametros, orcamentoCotacaoEmailsQueueItem.IdCfgUnidadeNegocio);

                            using (var emailService = new EmailService(parametroEmail, shouldGenerateLog))
                            {
                                var valuesOfSendingEmail = await emailService.Send(_logger, orcamentoCotacaoEmailsQueueItem);
                                
                                if (valuesOfSendingEmail.Item1)
                                {
                                    if (shouldGenerateLog)
                                    {
                                        _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Email sent #" + orcamentoCotacaoEmailsQueueItem.Id + " [TO] [" + orcamentoCotacaoEmailsQueueItem.To + "] [CC]: [" + orcamentoCotacaoEmailsQueueItem.Cc + "] [FROM] [" + orcamentoCotacaoEmailsQueueItem.From + "]");
                                    }
                                    orcamentoCotacaoEmailsQueueItem.Sent = true;
                                    orcamentoCotacaoEmailsQueueItem.Status = (int)eCfgOrcamentoCotacaoEmailStatus.EnvioComSucesso;
                                    orcamentoCotacaoEmailsQueueItem.DateSent = DateTime.Now;
                                    orcamentoCotacaoEmailsQueueItem.AttemptsQty = 0;
                                    orcamentoCotacaoEmailsQueueItem.DateLastAttempt = null;
                                    orcamentoCotacaoEmailsQueueItem.ErrorMsgLastAttempt = valuesOfSendingEmail.Item2;
                                }
                                else
                                {
                                    if (shouldGenerateLog)
                                    {
                                        _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Error: #" + orcamentoCotacaoEmailsQueueItem.Id + " - " + valuesOfSendingEmail.Item2);
                                    }

                                    var attemptsQty = orcamentoCotacaoEmailsQueueItem.AttemptsQty + 1;

                                    if (attemptsQty >= attemptQtdMaxParam)
                                    {
                                        orcamentoCotacaoEmailsQueueItem.Status = (int)eCfgOrcamentoCotacaoEmailStatus.FalhaNoEnvioDefinitivo;
                                        orcamentoCotacaoEmailsQueueItem.DateScheduled = null;
                                    }
                                    else
                                    {
                                        var newdateScheduled = DateTime.Now.AddSeconds(attempt1_2);

                                        if (attemptsQty == 2)
                                        {
                                            newdateScheduled = DateTime.Now.AddSeconds(attempt2_3);
                                        }

                                        if (attemptsQty >= 3)
                                        {
                                            newdateScheduled = DateTime.Now.AddSeconds(attemptVery);
                                        }

                                        orcamentoCotacaoEmailsQueueItem.Status = (int)eCfgOrcamentoCotacaoEmailStatus.FalhaNoEnvioTemporario;
                                        orcamentoCotacaoEmailsQueueItem.DateScheduled = newdateScheduled;
                                    }
                                    
                                    orcamentoCotacaoEmailsQueueItem.Sent = false;
                                    orcamentoCotacaoEmailsQueueItem.DateSent = null;
                                    orcamentoCotacaoEmailsQueueItem.AttemptsQty = attemptsQty;
                                    orcamentoCotacaoEmailsQueueItem.DateLastAttempt = DateTime.Now;
                                    orcamentoCotacaoEmailsQueueItem.ErrorMsgLastAttempt = valuesOfSendingEmail.Item2;
                                }

                                await this._mensageriaRepositorio.AtualizarOrcamentoCotacaoEmailsQueue(orcamentoCotacaoEmailsQueueItem);
                            }
                        }
                    }

                    if (shouldGenerateLog)
                    {
                        _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "End of While -- Will delay " + secondsToDelayAfterSend + " seconds and execute again");
                    }
                }
                catch (Exception ex)
                {
                    if (shouldGenerateLog)
                    {
                        _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Exception: " + ex.Message);
                        _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Exception: " + ex.InnerException);
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(secondsToDelayAfterSend), stoppingToken);

                if (shouldGenerateLog)
                {
                    _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "End of Delay");
                }
            }
        }

        private ParametroEmailDto ObteParametroEmail(
            List<UnidadeNegocioParametroDto> unidadeNeogocioParametros, 
            int? idCfgUnidadeNegocio)
        {
            var ParametroEmailDto = new ParametroEmailDto()
            {
                ServerSMTP = unidadeNeogocioParametros.Where(x => x.IdCfgUnidadeNegocio == idCfgUnidadeNegocio && x.IdCfgParametro == 7).FirstOrDefault().Valor,
                Port = unidadeNeogocioParametros.Where(x => x.IdCfgUnidadeNegocio == idCfgUnidadeNegocio && x.IdCfgParametro == 8).FirstOrDefault().Valor.ToInt().Value,
                Options = unidadeNeogocioParametros.Where(x => x.IdCfgUnidadeNegocio == idCfgUnidadeNegocio && x.IdCfgParametro == 11).FirstOrDefault().Valor == "STARTTLS" ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None,
                UserName = unidadeNeogocioParametros.Where(x => x.IdCfgUnidadeNegocio == idCfgUnidadeNegocio && x.IdCfgParametro == 9).FirstOrDefault().Valor,
                Password = unidadeNeogocioParametros.Where(x => x.IdCfgUnidadeNegocio == idCfgUnidadeNegocio && x.IdCfgParametro == 10).FirstOrDefault().Valor
            };

            return ParametroEmailDto;
        }
    }
}