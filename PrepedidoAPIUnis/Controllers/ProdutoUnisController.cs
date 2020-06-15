using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll;
using PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto;

namespace PrepedidoAPIUnis.Controllers
{
    public class ProdutoUnisController : Controller
    {
        private readonly ProdutoUnisBll produtoUnisBll;
        public ProdutoUnisController(ProdutoUnisBll produtoUnisBll)
        {
            this.produtoUnisBll = produtoUnisBll;
        }

        /// <summary>
        /// Rotina para buscar listagem de todos os produtos
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="loja"></param>
        /// <param name="id_cliente"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("buscarProduto")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<ProdutoComboUnisDto>> BuscarProduto(string tokenAcesso, string loja, string id_cliente)
        {
            //todo: validar token
            
            var retorno = await produtoUnisBll.ListaProdutosCombo(loja, id_cliente);

            return Ok(retorno);
        }
    }
}