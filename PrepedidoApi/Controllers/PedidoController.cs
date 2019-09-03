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

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarNumerosPedidosCombo")]
        public async Task<IActionResult> ListarNumerosPedidosCombo()
        {
            //para testar: http://localhost:60877/api/pedido/listarNumerosPedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //string apelido = "SALOMÃO";
            var ret = await pedidoBll.ListarNumerosPedidoCombo(apelido);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarPedidos")]
        public async Task<IActionResult> ListarPedidos(string clienteBusca, int tipoBusca, string numPedido,
            DateTime? dataInicial, DateTime? dataFinal)
        {
            //para testar: http://localhost:60877/api/pedido/listarPedidos
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var ret = await pedidoBll.ListarPedidos(apelido,
                (PrepedidoBusiness.Bll.PedidoBll.TipoBuscaPedido)tipoBusca, clienteBusca,
                numPedido, dataInicial, dataFinal);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarCpfCnpjPedidosCombo")]
        public async Task<IActionResult> ListarCpfCnpjPedidosCombo()
        {
            //para testar: http://localhost:60877/api/pedido/listarCpfCnpjPedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var ret = await pedidoBll.ListarCpfCnpjPedidosCombo(apelido);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarPedido")]
        public async Task<IActionResult> BuscarPedido(string numPedido)
        {
            //para testar: http://localhost:60877/api/pedido/buscarPedido
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var ret = await pedidoBll.BuscarPedido(apelido, numPedido);

            return Ok(ret);
        }
    }
}