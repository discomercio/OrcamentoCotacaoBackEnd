using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class ProdutoController : Controller
    {
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly PrepedidoBusiness.Bll.ProdutoBll.ProdutoGeralBll produtoBll;
        public ProdutoController(PrepedidoBusiness.Bll.ProdutoBll.ProdutoGeralBll produtoBll, InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this.produtoBll = produtoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarProduto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarProduto(string loja, string id_cliente)
        {
            //para testar: http://localhost:60877/api/produto/buscarProduto
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //nao usamos o apelido

            var ret = await produtoBll.ListaProdutosComboApiArclube(loja, id_cliente);
            
            return Ok(ret);
        }
    }
}