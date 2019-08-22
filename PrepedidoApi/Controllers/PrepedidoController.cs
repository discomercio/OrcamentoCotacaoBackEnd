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
        [AllowAnonymous]
        //TODO: pq precisa do httpget com um nome?
        [HttpGet("listarNumerosPrepedidosCombo")]
        public async Task<IActionResult> ListarNumerosPrepedidosCombo()
        {
            //para testar: http://localhost:60877/api/prepedido/listarNumerosPrepedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //string apelido = "RUI LUIS";
            var ret = await prepedidoBll.ListarNumerosPrepedidosCombo(apelido);
            return Ok(ret);
        }

        [AllowAnonymous]
        [HttpGet("listarCpfCnpjPrepedidosCombo")]
        public async Task<IActionResult> ListarCpfCnpjPrepedidosCombo()
        {
            //para testar :http://localhost:60877/api/prepedido/listarCpfCnpjPrepedidosCombo
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //string apelido = "RUI LUIS";
            var lista = await prepedidoBll.ListarCpfCnpjPrepedidosCombo(apelido);

            return Ok(lista);
        }

        [AllowAnonymous]
        [HttpGet("listarPrePedidos")]
        public async Task<IActionResult> ListarPrePedidos(int tipoBusca, string clienteBusca, string numeroPrePedido, 
            DateTime? dataInicial, DateTime? dataFinal)
        {
            //para testar: http://localhost:60877/api/prepedido/listarPrePedidos
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //string apelido = "A. SYSTEM";
            var lista = await prepedidoBll.ListarPrePedidos(apelido, 
                (PrepedidoBusiness.Bll.PrepedidoBll.TipoBuscaPrepedido)tipoBusca, 
                clienteBusca, numeroPrePedido, dataInicial, dataFinal);
            return Ok(lista);
        }

        [AllowAnonymous]
        [HttpPut("removerPrePedido/{numeroPrePedido}")]
        public IActionResult RemoverPrePedido(string numeroPrePedido)
        {
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //string apelido = "SALOMÃO";
            if (numeroPrePedido == null || numeroPrePedido == "")
            {
                return NotFound();
            }

            prepedidoBll.RemoverPrePedido(numeroPrePedido, apelido);

            return NoContent();
        }
    }
}

