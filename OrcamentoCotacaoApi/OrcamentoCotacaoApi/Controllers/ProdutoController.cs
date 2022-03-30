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
    //[ApiExplorerSettings(IgnoreApi = true)]
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
            ProdutoOrcamentoCotacaoBll produtoBll,
            CoeficienteBll coeficienteBll)
        {
            _logger = logger;
            _produtoBll = produtoBll;
            _coeficienteBll = coeficienteBll;
        }

        [HttpPost("buscarProdutos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProduto(ProdutosRequestViewModel produtos)
        {
            var ret = await _produtoBll.ListaProdutosCombo(produtos);

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

        [HttpGet("propriedades")]
        public async Task<IActionResult> ObterListaPropriedadesProdutos()
        {
            _logger.LogInformation("Buscando propriedades do produto");

            var ret = await _produtoBll.ObterListaPropriedadesProdutos();

            if (ret == null)
                return NoContent();
            else
                return Ok(ret);
        }

        [HttpPost("propriedades")]
        public async Task<IActionResult> GravarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            _logger.LogInformation("Inserindo Propriedade do Produto");

            var saida = _produtoBll.GravarPropriedadesProdutos(produtoCatalogoPropriedade);

            if (saida)
            {
                return Ok(new
                {
                    message = "Propriedade do produto criada com sucesso."
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Não foi possível criar a propriedade do produto."
                });
            }
        }
    }
}