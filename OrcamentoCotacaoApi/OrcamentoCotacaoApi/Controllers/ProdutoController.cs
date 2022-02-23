using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using Produto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.BaseController
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly ILogger<ProdutoController> _logger;
        private readonly ProdutoOrcamentoCotacaoBll _produtoBll;
        private readonly CoeficienteBll _coeficienteBll;

        public ProdutoController(
            ILogger<ProdutoController> logger,
            ProdutoOrcamentoCotacaoBll orcamentoBll,
            CoeficienteBll coeficienteBll)
        {
            _logger = logger;
            _produtoBll = orcamentoBll;
            _coeficienteBll = coeficienteBll;
        }

        [HttpGet("buscarProdutos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProduto(ProdutosRequestViewModel produtos)
        {
            var ret = await _produtoBll.ListaProdutosComboApiArclube(produtos);

            if (ret == null)
                return NoContent();
            else
                return Ok(ret);
        }

        [HttpGet("buscarCoeficientes")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> buscarCoeficientes(List<string> fabricantes)
        {
            var ret = await _coeficienteBll.BuscarListaCoeficientesFabricantesDistinct(fabricantes);

            if (ret == null)
                return NoContent();
            else
                return Ok(ret);
        }
    }
}