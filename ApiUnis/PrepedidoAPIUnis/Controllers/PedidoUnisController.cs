using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoUnisBusiness.UnisBll.AcessoBll;
using PrepedidoUnisBusiness.UnisBll.PedidoUnisBll;
using PrepedidoUnisBusiness.UnisDto.PedidoUnisDto;

namespace PrepedidoAPIUnis.Controllers
{
    public class PedidoUnisController : Controller
    {
        private readonly IServicoValidarTokenApiUnis servicoValidarTokenApiUnis;
        private readonly PedidoUnisBll pedidoUnisBll;

        public PedidoUnisController(IServicoValidarTokenApiUnis servicoValidarTokenApiUnis, PedidoUnisBll pedidoUnisBll)
        {
            this.servicoValidarTokenApiUnis = servicoValidarTokenApiUnis;
            this.pedidoUnisBll = pedidoUnisBll;
        }

        /// <summary>
        /// Rotina para buscar Pedido
        /// </summary>
        /// <param name="tokenAcesso"></param>
        /// <param name="pedido"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("buscarPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<PedidoUnisDto>> BuscarPedido(string tokenAcesso, string pedido)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            PedidoUnisDto ret = await pedidoUnisBll.BuscarPedido(pedido);

            return Ok(ret);
        }
    }
}