using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using System.Linq;
using System.Text.Json;

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
        public IActionResult BuscarPercMaxPorLoja(string loja, string tipoCliente)
        {
            var saida = _lojaBll.BuscarPercMaxPorLoja(loja);

            if (saida != null)
                return Ok(saida);
            else
                return NotFound();
        }

        //buscarPercMaxPorLojaAlcada
        [HttpGet("buscarPercMaxPorLojaAlcada")]
        public IActionResult BuscarPercMaxPorLojaAlcada(string loja, string tipoCliente)
        {
            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var lstPermissoes = user.Permissoes;

            var saida = _lojaBll.BuscarPercMaxPorLojaAlcada(loja, tipoCliente, lstPermissoes);

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
