using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.BaseController
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly ProdutoOrcamentoCotacaoBll _produtoBll;
        private readonly CoeficienteBll _coeficienteBll;

        public ProdutoController(ILogger<ProdutoController> logger, ProdutoOrcamentoCotacaoBll produtoBll,
            CoeficienteBll coeficienteBll)
        {
            _logger = logger;
            _produtoBll = produtoBll;
            _coeficienteBll = coeficienteBll;
        }

        [HttpPost("buscarProdutos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProduto(ProdutosRequestViewModel produtosRequest)
        {
            var response = await _produtoBll.ListaProdutosCombo(produtosRequest);

            if (response == null)
                return NoContent();
            else
                return Ok(response);
        }

        [HttpPost("buscarCoeficientes")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> buscarCoeficientes(CoeficienteRequestViewModel coeficienteRequest)
        {
            var response = await _coeficienteBll.BuscarListaCoeficientesFabricantesHistoricoDistinct(coeficienteRequest);

            if (response == null)
                return NoContent();
            else
                return Ok(response);
        }
    }
}