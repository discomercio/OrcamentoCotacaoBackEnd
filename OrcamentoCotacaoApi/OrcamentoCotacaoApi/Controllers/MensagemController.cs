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

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class MensagemController : BaseController
    {
        private readonly ILogger<MensagemController> _logger;
        private readonly MensagemOrcamentoCotacaoBll _mensagemBll;
        private readonly OrcamentoCotacaoBll _orcamentoCotacaoBll;

        public MensagemController(ILogger<MensagemController> logger, MensagemOrcamentoCotacaoBll mensagemBll, OrcamentoCotacaoBll orcamentoCotacaoBll)
        {
            _logger = logger;
            _mensagemBll = mensagemBll;
            _orcamentoCotacaoBll = orcamentoCotacaoBll;
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("ObterListaMensagem");

            var saida = await _mensagemBll.ObterListaMensagem(IdOrcamentoCotacao);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [Authorize]
        [HttpGet("pendente")]
        public async Task<IActionResult> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("ObterListaMensagemPendente");

            var saida = await _mensagemBll.ObterListaMensagemPendente(IdOrcamentoCotacao);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            _logger.LogInformation("EnviarMensagem");

            var saida = false;

            saida = _mensagemBll.EnviarMensagem(orcamentoCotacaoMensagem, JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value).Id);

            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagem criada com sucesso."
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Não foi possível criar a mensagem."
                });
            }
        }

        [Authorize]
        [HttpGet("pendente/quantidade")]
        public int ObterQuantidadeMensagemPendente()
        {
            if (User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin") != null)
            {
                _logger.LogInformation("ObterQuantidadeMensagemPendente");
                var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

                var saida = _mensagemBll.ObterQuantidadeMensagemPendente(user.Id, (int)user.TipoUsuario);

                return saida;
            }
            else
            {
                return 0;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("marcar/lida")]
        public async Task<IActionResult> MarcarLida(int IdOrcamentoCotacao)
        {
            var saida = false;

            _logger.LogInformation("MarcarLida");

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            saida = _mensagemBll.MarcarLida(IdOrcamentoCotacao, user.Id);

            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagens marcadas como lida"
                });
            }
            else
            {
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
            _logger.LogInformation("MarcarPendencia");

            var saida = _mensagemBll.MarcarPendencia(IdOrcamentoCotacao);

            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagens marcadas como pendência tratada."
                });
            }
            else
            {
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
            _logger.LogInformation("DesmarcarPendencia");

            var saida = _mensagemBll.DesmarcarPendencia(IdOrcamentoCotacao);

            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagens desmarcadas como pendência tratada."
                });
            }
            else
            {
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
                return Ok(new
                {
                    message = "Mensagens marcadas como lida"
                });
            }
            else
            {
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

            var orcamento = _orcamentoCotacaoBll.ObterIdOrcamentoCotacao(guid);

            if (orcamento == null) return BadRequest(new
            {
                message = "Acesso negado."
            });

            _logger.LogInformation("ObterListaMensagem");

            var saida = await _mensagemBll.ObterListaMensagem(orcamento.id);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpPost()]
        [Route("publico")]
        public async Task<IActionResult> EnviarMensagemRotaPublica(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem, string guid)
        {

            var saida = false;
            var orcamento = _orcamentoCotacaoBll.ObterIdOrcamentoCotacao(guid);

            if (orcamento == null) return BadRequest(new
            {
                message = "Acesso negado."
            });

            if (orcamento.id != orcamentoCotacaoMensagem.IdOrcamentoCotacao) return BadRequest(new
            {
                message = "Acesso negado."
            });

            _logger.LogInformation("EnviarMensagem");
            saida = _mensagemBll.EnviarMensagem(orcamentoCotacaoMensagem, orcamentoCotacaoMensagem.IdUsuarioRemetente);


            if (saida)
            {
                return Ok(new
                {
                    message = "Mensagem criada com sucesso."
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Não foi possível criar a mensagem."
                });
            }
        }

        #endregion

    }
}
