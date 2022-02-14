using AutoMapper;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orcamento;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class VendedorController : BaseController
    {
        private readonly ILogger<VendedorController> _logger;
        private readonly IMapper _mapper;
        private readonly OrcamentoBll _orcamentoBll;

        public VendedorController(ILogger<VendedorController> logger, IMapper mapper, OrcamentoBll orcamentoBll)
        {
            _logger = logger;
            _mapper = mapper;
            _orcamentoBll = orcamentoBll;
        }

        //[HttpGet]
        //public IActionResult PorFiltro(int page, int pageItens, string origem)
        //{
        //    _logger.LogInformation("Buscando lista de vendedores");
        //    var lojas = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Surname).Value.Split(',');

        //    var saida = _orcamentoBll.PorFiltro(new TvendedorFiltro { Page = page, RecordsPerPage = pageItens, Origem = origem, Loja = lojas });

        //    if (saida != null)
        //        return Ok(saida);
        //    else
        //        return NoContent();
        //}
    }
}
