using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UtilsGlobais;
using static InfraBanco.Constantes.Constantes;

namespace ArClube.Mensageria
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConfigurationBuilder config;
        public List<TcfgUnidadeNegocio> _cfgUnidadeNegocios { get; set; }

        public Worker(ILogger<Worker> logger,
            IServiceScopeFactory scopeFactory,
            ConfigurationBuilder config)
        {
            _logger = logger;
            this._scopeFactory = scopeFactory;
            this.config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Started");
            using var scope = _scopeFactory.CreateScope();
            // Access scoped services like this:
            var _CfgUnidadeNegocioParametroBll = scope.ServiceProvider.GetService<Cfg.CfgUnidadeNegocioParametro.CfgUnidadeNegocioParametroBll>();
            var _sendQueueBLL = scope.ServiceProvider.GetService<OrcamentoCotacaoEmailQueue.OrcamentoCotacaoEmailQueueBll>();
            var _CfgUnidadeNegocioBll = scope.ServiceProvider.GetService<Cfg.CfgUnidadeNegocio.CfgUnidadeNegocioBll>();

            _cfgUnidadeNegocios = _CfgUnidadeNegocioBll.PorFiltro(new InfraBanco.Modelos.Filtros.TcfgUnidadeNegocioFiltro() { });

            return;

            while (!stoppingToken.IsCancellationRequested)
            {
                // Create service scope
                var configuration = config.AddEnvironmentVariables().AddJsonFile("appsettings.json").Build();
                try
                {
                  

                    var emails = _sendQueueBLL.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentoCotacaoEmailQueueFiltro() { Sent = false, Page = 1, RecordsPerPage = configuration.GetSection("EmailsPerCicleToGetAndSend").Value.ToInt().Value });
                    if (emails.Count > 0)
                    {
                        _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Using SMTPServer " + configuration.GetSection("SmtpServer").Value + " SMTPPort " + configuration.GetSection("SmtpPort").Value.ToInt().Value);
                        using (EmailService emailService = new EmailService(configuration.GetSection("SmtpServer").Value, configuration.GetSection("SmtpPort").Value.ToInt().Value,
                            configuration.GetSection("SmtpSSL").Value == "1" ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None,
                            configuration.GetSection("SmtpUsername").Value, configuration.GetSection("SmtpPassword").Value))
                        {
                            foreach (var email in emails)
                            {
                                string ret;
                                if (emailService.Send(_logger, email, out ret))
                                {
                                    _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - Email sent #" + email.Id + " [TO] [" + email.To + "] [CC]: [" + email.Cc + "] [FROM] [" + email.From + "]");
                                    email.DateSent = DateTime.Now;
                                    email.Status = (int)eCfgOrcamentoCotacaoEmailStatus.EnvioComSucesso;
                                    _sendQueueBLL.Atualizar(email);
                                }
                                else
                                {
                                    _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Error: #" + email.Id + " - " + ret);
                                    email.Status = (int)eCfgOrcamentoCotacaoEmailStatus.FalhaNoEnvioTemporario;
                                    email.DateLastAttempt = DateTime.Now;
                                    /*Marcar como enviado?*/
                                    _sendQueueBLL.Atualizar(email);
                                }
                            }
                        }
                    }

                    _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "End of While -- Will delay " + configuration.GetSection("SecondsToDelayAfterSend").Value.ToInt().Value + " seconds and execute again");
                }
                catch (Exception ex)
                {

                    _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Exception: " + ex.Message);
                    _logger.LogError(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "Exception: " + ex.InnerException);
                }

                await Task.Delay(TimeSpan.FromSeconds(configuration.GetSection("SecondsToDelayAfterSend").Value.ToInt().Value), stoppingToken);
                _logger.LogInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ff") + " - " + "End of Delay");
            }
        }
    }
}