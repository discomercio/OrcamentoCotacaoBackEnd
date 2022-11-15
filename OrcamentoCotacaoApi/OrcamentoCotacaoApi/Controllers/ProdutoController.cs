using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.BaseController
{
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
        public async Task<IActionResult> BuscarCoeficientes(CoeficienteRequestViewModel coeficienteRequest)
        {
            var response = await _coeficienteBll.BuscarListaCoeficientesFabricantesHistoricoDistinct(coeficienteRequest);

            if (response == null)
                return NoContent();
            else
                return Ok(response);
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

        [HttpGet("propriedades/{id}")]
        public async Task<IActionResult> ObterListaPropriedadesProdutos(int id)
        {
            _logger.LogInformation("Buscando propriedades do produto");

            var ret = await _produtoBll.ObterListaPropriedadesProdutos(id);

            if (ret == null)
                return NoContent();
            else
                return Ok(ret);
        }

        [HttpGet("itens/{idProdutoCatalogo}")]
        public async Task<IActionResult> ObterListaPropriedadesProdutosById(int idProdutoCatalogo)
        {
            _logger.LogInformation("Buscando propriedades do produto");

            var ret = await _produtoBll.ObterListaPropriedadesProdutosById(idProdutoCatalogo);

            if (ret == null)
                return NoContent();
            else
                return Ok(ret);
        }

        [HttpGet("opcoes/{IdProdutoCatalogoPropriedade}")]
        public async Task<IActionResult> ObterListaPropriedadesOpcoesProdutosById(int IdProdutoCatalogoPropriedade)
        {
            _logger.LogInformation("Buscando opções de propriedade do produto");

            var ret = await _produtoBll.ObterListaPropriedadesOpcoesProdutosById(IdProdutoCatalogoPropriedade);

            if (ret == null)
                return NoContent();
            else
                return Ok(ret);
        }

        [HttpGet("opcoes")]
        public async Task<IActionResult> ObterListaPropriedadesOpcoes()
        {
            _logger.LogInformation("Buscando opções de propriedade do produto");

            var ret = await _produtoBll.ObterListaPropriedadesOpcoes();

            if (ret == null)
                return NoContent();
            else
                return Ok(ret);
        }

        [HttpGet("fabricantes")]
        public async Task<IActionResult> ObterListaFabricante()
        {
            _logger.LogInformation("Buscando lista de fabricantes");

            var ret = await _produtoBll.ObterListaFabricante();

            if (ret == null)
                return NoContent();
            else
                return Ok(ret);
        }        

        [HttpGet("listar-produtos-propriedades-ativos")]
        public async Task<IActionResult> ObterListaProdutosPropriedadesProdutosAtivos()
        {
            var saida = await _produtoBll.ObterListaProdutosPropriedadesProdutosAtivos();

            return Ok(saida);
        }

        [HttpGet("listar-propriedades-opcoes-produtos-ativos")]
        public async Task<IActionResult> ObterPropriedadesEOpcoesProdutosAtivos()
        {
            var saida = await _produtoBll.ObterPropriedadesEOpcoesProdutosAtivos();

            return Ok(saida);
        }

        [HttpPost("buscar-produtos-opcoes-ativos")]
        public async Task<IActionResult> ObterPropriedadesEOpcoesProdutosPorProduto(ProdutosAtivosRequestViewModel obj)
        {
            _logger.LogInformation("BuscarProdutoCatalogoParaVisualizacao - Request: [idProduto: {0} - propriedadeOculta: {1} - propriedadeOcultaItem: {2}]", obj.idProduto, obj.propriedadeOculta, obj.propriedadeOcultaItem);

            var retorno = await _produtoBll.BuscarProdutoCatalogoParaVisualizacao(obj.idProduto, obj.propriedadeOculta, obj.propriedadeOcultaItem);

            _logger.LogInformation("BuscarProdutoCatalogoParaVisualizacao - Response: {0}", JsonSerializer.Serialize(retorno));
            
            return Ok(retorno);
        }

        [HttpGet("listar-produtos-ativos")]
        public async Task<IActionResult> ObterProdutosAtivos()
        {
           return Ok(await _produtoBll.ObterProdutosAtivos());
        }
    }
}