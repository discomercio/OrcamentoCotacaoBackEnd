using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LojaController : BaseController
    {
        private readonly ILogger<LojaController> _logger;
        private readonly LojaOrcamentoCotacaoBll _lojaBll;

        public LojaController(
            ILogger<LojaController> logger, 
            LojaOrcamentoCotacaoBll lojaBll)
        {
            _logger = logger;
            _lojaBll = lojaBll;
        }

        [HttpGet]
        public IActionResult Get(int page, int pageItens)
        {
            var saida = _lojaBll.PorFiltro(new TlojaFiltro { });

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("buscarPercMaxPorLoja")]
        public IActionResult BuscarPercMaxPorLoja(string loja)
        {
            var saida = _lojaBll.BuscarPercMaxPorLoja(loja);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        [HttpGet("{loja}/estilo")]
        public IActionResult BuscarLojaEstilo(string loja)
        {
            var saida = _lojaBll.BuscarLojaEstilo(loja);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }
    }
}
