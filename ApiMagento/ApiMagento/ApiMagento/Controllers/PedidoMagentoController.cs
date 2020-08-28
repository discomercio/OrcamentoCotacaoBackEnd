using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MagentoBusiness.MagentoBll.AcessoBll;
using MagentoBusiness.MagentoBll.PedidoMagentoBll;
using MagentoBusiness.MagentoDto.PedidoMagentoDto;

namespace ApiMagento.Controllers
{
    [Route("api/pedidoMagento")]
    [ApiController]
    public class PedidoMagentoController : Controller
    {
        private readonly IServicoValidarTokenApiMagento servicoValidarTokenApiMagento;
        private readonly PedidoMagentoBll pedidoMagentoBll;

        public PedidoMagentoController(IServicoValidarTokenApiMagento servicoValidarTokenApiMagento,
            PedidoMagentoBll pedidoMagentoBll)
        {
            this.servicoValidarTokenApiMagento = servicoValidarTokenApiMagento;
            this.pedidoMagentoBll = pedidoMagentoBll;
        }

        /// <summary>
        /// Rotina para cadastrar Pedido Magento
        /// </summary>
        /// <param name="pedido">PedidoMagentoDto</param>
        /// <returns>Não retorna dados</returns>
        [AllowAnonymous]
        [HttpPost("cadastrarPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult<PedidoResultadoMagentoDto>> CadastrarPrepedido(PedidoMagentoDto pedido)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!servicoValidarTokenApiMagento.ValidarToken(pedido.TokenAcesso, out _))
                return Unauthorized();

            await pedidoMagentoBll.CadastrarPedidoMagento(pedido);
            var ret = new PedidoResultadoMagentoDto();
            ret.ListaErros.Add("Ainda não implementado");
            return Ok(ret);
        }
    }
}