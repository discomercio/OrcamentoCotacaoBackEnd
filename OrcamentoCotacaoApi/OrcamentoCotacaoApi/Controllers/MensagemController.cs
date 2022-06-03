using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class MensagemController : BaseController
    {
        private readonly ILogger<MensagemController> _logger;
        private readonly MensagemOrcamentoCotacaoBll _mensagemBll;

        public MensagemController(ILogger<MensagemController> logger, MensagemOrcamentoCotacaoBll mensagemBll)
        {
            _logger = logger;
            _mensagemBll = mensagemBll;
        }

        [HttpGet()]
        public async Task<IActionResult> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("Buscando Mensagens");

            var saida = await _mensagemBll.ObterListaMensagem(IdOrcamentoCotacao);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("pendente")]
        public async Task<IActionResult> ObterListaMensagemPendente(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("Buscando Mensagens Pendentes");

            var saida = await _mensagemBll.ObterListaMensagemPendente(IdOrcamentoCotacao);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpPost()]
        public async Task<IActionResult> EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            _logger.LogInformation("Inserindo Mensagem");

            var saida = _mensagemBll.EnviarMensagem(orcamentoCotacaoMensagem);

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

        [HttpPut]
        [Route("lida")]
        public async Task<IActionResult> MarcarMensagemComoLida(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("Marcando mensagens como lida");

            var saida = _mensagemBll.MarcarMensagemComoLida(IdOrcamentoCotacao);

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
        
        [HttpPut]
        [Route("pendencia")]
        public async Task<IActionResult> MarcarMensagemPendenciaTratada(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("Marcando mensagens como lida");

            var saida = _mensagemBll.MarcarMensagemPendenciaTratada(IdOrcamentoCotacao);

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

    }
}
