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
    [Route("api/pedidoUnis")]
    [ApiController]
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
        /// <returns>PedidoUnisDto</returns>
        /// <response code="204">Pedido não existe ou número do pedido inválido</response>
        [AllowAnonymous]
        [HttpGet("buscarPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<PedidoUnisDto>> BuscarPedido(string tokenAcesso, string pedido)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(tokenAcesso, out _))
                return Unauthorized();

            PedidoUnisDto ret = await pedidoUnisBll.BuscarPedido(pedido);
            if (!string.IsNullOrEmpty(ret.NumeroPedido))
                return Ok(ret);

            return NoContent();
        }
    }
}