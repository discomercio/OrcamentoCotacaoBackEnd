using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Controllers;
using OrcamentoCotacaoApi.Filters;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais;
using UtilsGlobais.Configs;

namespace PrepedidoApi.Controllers
{
    [Route("api/produto")]
    [ApiController]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    [TypeFilter(typeof(ResourceFilter))]
    public class ProdutoPrePedidoController : BaseController
    {
        private readonly ILogger<ProdutoPrePedidoController> _logger;
        private readonly Prepedido.Bll.ProdutoPrepedidoBll _bll;

        public ProdutoPrePedidoController(
            ILogger<ProdutoPrePedidoController> logger,
            Prepedido.Bll.ProdutoPrepedidoBll bll)
        {
            _logger = logger;
            _bll = bll;
        }

        [HttpGet("buscarProduto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProduto(string loja, string id_cliente)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Loja = loja,
                Idcliente = id_cliente
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. ProdutoPrePedidoController/BuscarProduto/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var ret = await _bll.ListaProdutosComboApiArclube(loja, id_cliente);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/Get/GET - Response => [Retorna ProdutoDto(s) e ProdutoCompostoDto(s)].");

            return Ok(ret);
        }
    }
}