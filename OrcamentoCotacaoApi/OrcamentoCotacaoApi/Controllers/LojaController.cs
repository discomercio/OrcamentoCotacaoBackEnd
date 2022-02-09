using InfraBanco.Modelos.Filtros;
using Loja;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LojaController : ControllerBase
    {
        private readonly ILogger<LojaController> _logger;
        private readonly LojaBll _lojaBll;

        public LojaController(ILogger<LojaController> logger, LojaBll lojaBll)
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
    }
}
