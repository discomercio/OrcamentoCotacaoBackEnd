using InfraIdentity;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;
using System;
using OrcamentoCotacaoApi.Filters;
using UtilsGlobais;
using UtilsGlobais.Configs;
using Microsoft.Extensions.Configuration;
using OrcamentoCotacaoBusiness.Models.Response.Mensagem;

namespace OrcamentoCotacaoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("[controller]")]
    [ApiController]
    [TypeFilter(typeof(ResourceFilter))]
    public class MensagemController : BaseController
    {
        private readonly ILogger<MensagemController> _logger;
        private readonly MensagemOrcamentoCotacaoBll _mensagemBll;
        private readonly OrcamentoCotacaoBll _orcamentoCotacaoBll;
        private readonly IConfiguration configuration;

        public MensagemController(
            ILogger<MensagemController> logger,
            IConfiguration configuration,
            MensagemOrcamentoCotacaoBll mensagemBll, 
            OrcamentoCotacaoBll orcamentoCotacaoBll)
        {
            _logger = logger;            
            _mensagemBll = mensagemBll;
            _orcamentoCotacaoBll = orcamentoCotacaoBll;
            this.configuration = configuration;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdOrcamentoCotacao = IdOrcamentoCotacao
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagem/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = await _mensagemBll.ObterListaMensagem(IdOrcamentoCotacao);

            if (saida != null)
            {
                var response = new
                {
                    ListaMensagem = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagem/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagem/GET - Response => [Não tem response].");
            return NoContent();
        }

        [Authorize]
        [HttpGet("pendente")]
        public async Task<IActionResult> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {            

            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdOrcamentoCotacao = IdOrcamentoCotacao
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagemPendente/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = await _mensagemBll.ObterListaMensagemPendente(IdOrcamentoCotacao);

            if (saida != null)
            {
                var response = new
                {
                    MensagemPendente = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagemPendente/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagemPendente/GET - Response => [Não tem response].");
            return NoContent();
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                OrcamentoCotacaoMensagem = orcamentoCotacaoMensagem
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/EnviarMensagem/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = false;

            saida = _mensagemBll.EnviarMensagem(orcamentoCotacaoMensagem, JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value).Id);

            if (saida)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/EnviarMensagem/POST - Response => [Mensagem criada com sucesso.].");

                return Ok(new
                {
                    message = "Mensagem criada com sucesso."
                });
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/EnviarMensagem/POST - Response => [Não foi possível criar a mensagem.].");

                return BadRequest(new
                {
                    message = "Não foi possível criar a mensagem."
                });
            }
        }

        [Authorize]
        [HttpGet("pendente/quantidadePorLoja")]
        public IActionResult ObterQuantidadeMensagemPendentePorLojas()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Configuracao>();

            if (appSettings.GerarLogProcessoAutomatizado)
            {
                var request = new
                {
                    Usuario = LoggedUser.Apelido
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterQuantidadeMensagemPendentePorLojas/GET - Request => [{JsonSerializer.Serialize(request)}].");
            }

            var response = new ListaQuantidadeMensagemPendenteResponse();
            response.Sucesso = false;
            if (User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin") != null)
            {

                if (appSettings.GerarLogProcessoAutomatizado)
                {
                    _logger.LogInformation("ObterQuantidadeMensagemPendentePorLojas");
                }
                var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

                response = _mensagemBll.ObterQuantidadeMensagemPendentePorLojas(user);

                if (appSettings.GerarLogProcessoAutomatizado)
                {
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterQuantidadeMensagemPendentePorLojas/GET - Response => [{JsonSerializer.Serialize(response)}].");
                }

                return Ok(response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPut]
        [Route("marcar/lida")]
        public async Task<IActionResult> MarcarLida(int IdOrcamentoCotacao)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdOrcamentoCotacao = IdOrcamentoCotacao
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarLida/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = false;

            _logger.LogInformation("MarcarLida");

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            saida = _mensagemBll.MarcarLida(IdOrcamentoCotacao, user.Id);

            if (saida)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarLida/GET - Response => [Mensagens marcadas como lida.].");

                return Ok(new
                {
                    message = "Mensagens marcadas como lida"
                });
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarLida/GET - Response => [Não foi possível marcar como lida.].");

                return BadRequest(new
                {
                    message = "Não foi possível marcar como lida."
                });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("marcar/pendencia")]
        public async Task<IActionResult> MarcarPendencia(int IdOrcamentoCotacao)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdOrcamentoCotacao = IdOrcamentoCotacao
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarPendencia/PUT - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = _mensagemBll.MarcarPendencia(IdOrcamentoCotacao);

            if (saida)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarPendencia/PUT - Response => [Mensagens marcadas como pendência tratada.].");

                return Ok(new
                {
                    message = "Mensagens marcadas como pendência tratada."
                });
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarPendencia/PUT - Response => [Mensagens marcadas como pendência tratada.].");

                return BadRequest(new
                {
                    message = "Mensagens marcadas como pendência tratada."
                });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("desmarcar/pendencia")]
        public async Task<IActionResult> DesmarcarPendencia(int IdOrcamentoCotacao)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                IdOrcamentoCotacao = IdOrcamentoCotacao
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/DesmarcarPendencia/PUT - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = _mensagemBll.DesmarcarPendencia(IdOrcamentoCotacao);

            if (saida)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/DesmarcarPendencia/PUT - Response => [Mensagens desmarcadas como pendência tratada.].");

                return Ok(new
                {
                    message = "Mensagens desmarcadas como pendência tratada."
                });
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/DesmarcarPendencia/PUT - Response => [Mensagens desmarcadas como pendência tratada.].");

                return BadRequest(new
                {
                    message = "Mensagens desmarcadas como pendência tratada."
                });
            }
        }

        #region "Métodos Públicos"

        /* Para aumentar a segurança,
         * todos os métodos publicos (AllowAnonymous) DEVEM receber o guid ao invés do número do orçamento,
         * isso evita que um usuário não autenticado, consiga obter informações de um determiado orçamento
         * Receba sempre o guid e obtenha o id do orçamento dentro do método - l1ng
         */

        [AllowAnonymous]
        [HttpPut]
        [Route("publico/marcar/lida")]
        public async Task<IActionResult> MarcarLidaRotaPublica(String guid)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Valor = guid
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarLidaRotaPublica/PUT - Request => [{JsonSerializer.Serialize(request)}].");

            var orcamento = _orcamentoCotacaoBll.ObterIdOrcamentoCotacao(guid);

            if (orcamento == null) return BadRequest(new
            {
                message = "Acesso negado."
            });

            var saida = false;

            _logger.LogInformation("MarcarLida");
            saida = _mensagemBll.MarcarLida(orcamento.id, 0);

            if (saida)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarLidaRotaPublica/PUT - Response => [Mensagens marcadas como lida].");

                return Ok(new
                {
                    message = "Mensagens marcadas como lida"
                });
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/MarcarLidaRotaPublica/PUT - Response => [Não foi possível marcar como lida.].");

                return BadRequest(new
                {
                    message = "Não foi possível marcar como lida."
                });
            }
        }

        [HttpGet()]
        [Route("publico")]
        public async Task<IActionResult> ObterListaMensagemRotaPublica(String guid)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Valor = guid
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagemRotaPublica/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var orcamento = _orcamentoCotacaoBll.ObterIdOrcamentoCotacao(guid);

            if (orcamento == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagemRotaPublica/GET - Response => [Acesso negado.].");
                return BadRequest(new { message = "Acesso negado." });
            }

            var saida = await _mensagemBll.ObterListaMensagem(orcamento.id);

            if (saida != null)
            {
                var response = new
                {
                    ListaMensagem = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagemRotaPublica/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/ObterListaMensagemRotaPublica/GET - Response => [Não tem response].");
                return NoContent();
            }
        }

        [HttpPost()]
        [Route("publico")]
        public async Task<IActionResult> EnviarMensagemRotaPublica(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem, string guid)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                OrcamentoCotacaoMensagem = orcamentoCotacaoMensagem,
                Valor = guid
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/EnviarMensagemRotaPublica/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = false;
            var orcamento = _orcamentoCotacaoBll.ObterIdOrcamentoCotacao(guid);

            if (orcamento == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/EnviarMensagemRotaPublica/GET - Response => [Acesso negado.].");
                return BadRequest(new { message = "Acesso negado." });
            }

            if (orcamento.id != orcamentoCotacaoMensagem.IdOrcamentoCotacao)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/EnviarMensagemRotaPublica/GET - Response => [Acesso negado.].");
                return BadRequest(new { message = "Acesso negado." });
            }

            _logger.LogInformation("EnviarMensagem");
            saida = _mensagemBll.EnviarMensagem(orcamentoCotacaoMensagem, orcamentoCotacaoMensagem.IdUsuarioRemetente);

            if (saida)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/EnviarMensagemRotaPublica/GET - Response => [Mensagem criada com sucesso.].");

                return Ok(new
                {
                    message = "Mensagem criada com sucesso."
                });
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. MensagemController/EnviarMensagemRotaPublica/GET - Response => [Não foi possível criar a mensagem.].");

                return BadRequest(new
                {
                    message = "Não foi possível criar a mensagem."
                });
            }
        }

        #endregion

    }
}