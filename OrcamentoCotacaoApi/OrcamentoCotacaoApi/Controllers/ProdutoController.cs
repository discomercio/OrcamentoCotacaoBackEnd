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

        [HttpPut("propriedades")]
        public async Task<IActionResult> AtualizarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            _logger.LogInformation("Inserindo Propriedade do Produto");

            var saida = _produtoBll.AtualizarPropriedadesProdutos(produtoCatalogoPropriedade);

            if (saida)
            {
                return Ok(new
                {
                    message = "Propriedade do produto atualizada com sucesso."
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Não foi possível atualizar a propriedade do produto."
                });
            }
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

        [HttpGet("buscar-produtos-opcoes-ativos/{idProduto}&{propriedadeOculta}&{propriedadeOcultaItem}")]
        public async Task<IActionResult> ObterPropriedadesEOpcoesProdutosAtivosPorProduto(
            int idProduto, 
            bool propriedadeOculta, 
            bool propriedadeOcultaItem)
        {
            _logger.LogInformation("BuscarProdutoCatalogoParaVisualizacao - Request: [idProduto: {0} - propriedadeOculta: {1} - propriedadeOcultaItem: {2}]", idProduto, propriedadeOculta, propriedadeOcultaItem);

            var retorno = await _produtoBll.BuscarProdutoCatalogoParaVisualizacao(idProduto, propriedadeOculta, propriedadeOcultaItem);

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