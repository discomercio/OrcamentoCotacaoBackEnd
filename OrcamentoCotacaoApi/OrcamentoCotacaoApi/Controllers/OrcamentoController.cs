using AutoMapper;
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
    public class OrcamentoController : BaseController
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly IMapper _mapper;
        private readonly OrcamentoCotacaoBll _orcamentoBll;

        public OrcamentoController(ILogger<OrcamentoController> logger, IMapper mapper, OrcamentoCotacaoBll orcamentoBll)
        {
            _logger = logger;
            _mapper = mapper;
            _orcamentoBll = orcamentoBll;
        }

        [HttpGet]
        public IActionResult PorFiltro(TorcamentoFiltro filtro)
        {
            _logger.LogInformation("Buscando lista de orçamentos");

            filtro.Loja = ObterLoja(filtro.Loja);
            filtro.TipoUsuario = LoggedUser.TipoUsuario;
            filtro.Apelido = LoggedUser.Nome;

            var saida = _orcamentoBll.PorFiltro(filtro);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("status")]
        public async Task<IActionResult> ObterListaStatus(string origem, string lojaLogada)
        {
            _logger.LogInformation("Buscando status");

            var saida = await _orcamentoBll.ObterListaStatus(new TorcamentoFiltro { Origem = origem, Loja = lojaLogada });

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }


        [HttpGet("validade")]
        public IActionResult BuscarConfigValidade()
        {
            _logger.LogInformation("Buscando ConfigValidade");

            var saida = _orcamentoBll.BuscarConfigValidade();

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("mensagem")]
        public async Task<IActionResult> ObterListaMensagem(int IdOrcamentoCotacao)
        {
            _logger.LogInformation("Buscando Mensagens");

            var saida = await _orcamentoBll.ObterListaMensagem(IdOrcamentoCotacao);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("mensagem/pendente")]
        public async Task<IActionResult> ObterListaMensagemPendente(int IdOrcamentoCotacao, int IdUsuarioDestinatario)
        {
            _logger.LogInformation("Buscando Mensagens Pendentes");

            var saida = await _orcamentoBll.ObterListaMensagemPendente(IdOrcamentoCotacao, IdUsuarioDestinatario);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpPost("mensagem")]
        public async Task<IActionResult> EnviarMensagem(TorcamentoCotacaoMensagemFiltro orcamentoCotacaoMensagem)
        {
            _logger.LogInformation("Inserindo Mensagem");

            var saida = _orcamentoBll.EnviarMensagem(orcamentoCotacaoMensagem);

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

        [HttpPut("mensagem/lida")]
        public async Task<IActionResult> MarcarMensagemComoLida(int IdOrcamentoCotacao, int idUsuarioDestinatario)
        {
            _logger.LogInformation("Marcando mensagens como lida");

            var saida = _orcamentoBll.MarcarMensagemComoLida(IdOrcamentoCotacao, idUsuarioDestinatario);

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

        private string ObterLoja(string loja)
        {
            if (!string.IsNullOrEmpty(loja))
            {
                return loja;
            }
            else
            {
                return LoggedUser.Loja?.Length > 3 ? LoggedUser.Loja.Split(',')[0] : LoggedUser.Loja;
            }
        }
    }
}
