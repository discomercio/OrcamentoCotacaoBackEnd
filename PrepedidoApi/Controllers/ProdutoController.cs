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
        private readonly PrepedidoBusiness.Bll.ProdutoBll produtoBll;
        public ProdutoController(PrepedidoBusiness.Bll.ProdutoBll produtoBll, InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this.produtoBll = produtoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarProduto")]
        public async Task<IActionResult> BuscarProduto(string codProduto, string loja)
        {
            //para testar: http://localhost:60877/api/produto/buscarProduto
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //string apelido = "SALOMÃO";
            //var ret = await produtoBll.BuscarProduto(codProduto, loja, apelido);

            return Ok();
        }
    }
}