using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UtilsGlobais;

namespace PrepedidoApi.Controllers
{
    [Route("api/produto")]
    [ApiController]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    public class ProdutoPrePedidoController : Controller
    {
        private readonly PrepedidoBusiness.Bll.ProdutoPrepedidoBll _bll;

        public ProdutoPrePedidoController(PrepedidoBusiness.Bll.ProdutoPrepedidoBll bll)
        {
            _bll = bll;
        }

        [HttpGet("buscarProduto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProduto(string loja, string id_cliente)
        {
            return Ok(await _bll.ListaProdutosComboApiArclube(loja, id_cliente));
        }
    }
}