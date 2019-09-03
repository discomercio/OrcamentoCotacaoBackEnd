using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class PrepedidoController : ControllerBase
    {
        private readonly PrepedidoBusiness.Bll.PrepedidoBll prepedidoBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;

        public PrepedidoController(PrepedidoBusiness.Bll.PrepedidoBll prepedidoBll, InfraIdentity.IServicoDecodificarToken servicoDecodificarToken)
        {
            this.prepedidoBll = prepedidoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
        }

        //para teste, anonimo
#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarNumerosPrepedidosCombo")]
        public async Task<IActionResult> ListarNumerosPrepedidosCombo()
        {
            //para testar: http://localhost:60877/api/prepedido/listarNumerosPrepedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var ret = await prepedidoBll.ListarNumerosPrepedidosCombo(apelido);
            return Ok(ret);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarCpfCnpjPrepedidosCombo")]
        public async Task<IActionResult> ListarCpfCnpjPrepedidosCombo()
        {
            //para testar :http://localhost:60877/api/prepedido/listarCpfCnpjPrepedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var lista = await prepedidoBll.ListarCpfCnpjPrepedidosCombo(apelido);

            return Ok(lista);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("listarPrePedidos")]
        public async Task<IActionResult> ListarPrePedidos(int tipoBusca, string clienteBusca, string numeroPrePedido, 
            DateTime? dataInicial, DateTime? dataFinal)
        {
            //para testar: http://localhost:60877/api/prepedido/listarPrePedidos
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var lista = await prepedidoBll.ListarPrePedidos(apelido, 
                (PrepedidoBusiness.Bll.PrepedidoBll.TipoBuscaPrepedido)tipoBusca, 
                clienteBusca, numeroPrePedido, dataInicial, dataFinal);
            return Ok(lista);
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPut("removerPrePedido/{numeroPrePedido}")]
        public async Task<IActionResult> RemoverPrePedido(string numeroPrePedido)
        {
            //para testar: http://localhost:60877/api/prepedido/removerPrePedido/{numeroPrePedido}
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            if (numeroPrePedido == null || numeroPrePedido == "")
            {
                return NotFound();
            }

            var ret = await prepedidoBll.RemoverPrePedido(numeroPrePedido, apelido);

            if (ret == true)
                return NoContent();
            else
                return NotFound();
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet("buscarPrepedido")]
        public async Task<IActionResult> BuscarPrePedido(string numPrepedido)
        {
            //para testar: http://localhost:60877/api/prepedido/buscarPrepedido
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var ret = await prepedidoBll.BuscarPrePedido(apelido, numPrepedido);

            return Ok(ret);
        }
    }
}

