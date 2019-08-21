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
    public class PedidoController : ControllerBase
    {
        private readonly PrepedidoBusiness.Bll.PedidoBll pedidoBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;

        public PedidoController(PrepedidoBusiness.Bll.PedidoBll pedidoBll, InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this.pedidoBll = pedidoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
        }

        [AllowAnonymous]
        [HttpGet("listarNumerosPedidosCombo")]
        public async Task<IActionResult> ListarNumerosPedidosCombo()
        {
            //string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            string apelido = "SALOMÃO";
            var ret = await pedidoBll.ListarNumerosPedidoCombo(apelido);

            return Ok(ret);
        }

        [AllowAnonymous]
        [HttpGet("listarPedidos")]
        public async Task<IActionResult> ListarPedidos(string clienteBusca, int tipoBusca, string numPedido,
            DateTime? dataInicial, DateTime? dataFinal)
        {
            //string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var apelido = "SALOMÃO";
            var ret = await pedidoBll.ListarPedidos(apelido,
                (PrepedidoBusiness.Bll.PedidoBll.TipoBuscaPedido)tipoBusca, clienteBusca,
                numPedido, dataInicial, dataFinal);

            return Ok(ret);
        }
    }
}