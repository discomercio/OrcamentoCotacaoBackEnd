using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prepedido.PedidoVisualizacao;
using PrepedidoBusiness.Bll;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoBll pedidoBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly PedidoPrepedidoApiBll pedidoPrepedidoApiBll;

        public PedidoController(PedidoBll pedidoBll, InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            PrepedidoBusiness.Bll.PedidoPrepedidoApiBll pedidoPrepedidoApiBll)
        {
            this.pedidoBll = pedidoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.pedidoPrepedidoApiBll = pedidoPrepedidoApiBll;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarNumerosPedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarNumerosPedidosCombo()
        {
            //para testar: http://localhost:60877/api/pedido/listarNumerosPedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            IEnumerable<string> ret = await pedidoBll.ListarNumerosPedidoCombo(apelido.Trim());

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarPedidos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarPedidos(string clienteBusca, int tipoBusca, string numPedido,
            DateTime? dataInicial, DateTime? dataFinal)
        {
            //para testar: http://localhost:60877/api/pedido/listarPedidos
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            IEnumerable<PrepedidoBusiness.Dto.Pedido.PedidoDtoPedido> ret = await pedidoPrepedidoApiBll.ListarPedidos(apelido.Trim(),
                (PedidoBll.TipoBuscaPedido)tipoBusca, clienteBusca,
                numPedido, dataInicial, dataFinal);

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarCpfCnpjPedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarCpfCnpjPedidosCombo()
        {
            //para testar: http://localhost:60877/api/pedido/listarCpfCnpjPedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            IEnumerable<string> ret = await pedidoBll.ListarCpfCnpjPedidosCombo(apelido.Trim());

            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarPedido(string numPedido)
        {
            //para testar: http://localhost:60877/api/pedido/buscarPedido
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            PrepedidoBusiness.Dto.Pedido.DetalhesPedido.PedidoDto ret = await pedidoPrepedidoApiBll.BuscarPedido(apelido.Trim(), numPedido);

            return Ok(ret);
        }
    }
}