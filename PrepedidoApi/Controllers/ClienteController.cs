using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    public class ClienteController : ControllerBase
    {
        private readonly PrepedidoBusiness.Bll.ClienteBll clienteBll;
        public ClienteController(PrepedidoBusiness.Bll.ClienteBll clienteBll)
        {
            this.clienteBll = clienteBll;
        }

        //[AllowAnonymous]
        //[HttpGet("listarClientesPrePedidosCombo")]
        //public async Task<IActionResult> ListarClientesPrepedidosCombo()
        //{
        //    //para testar :http://localhost:60877/api/cliente/listarClientesPrePedidosCombo
        //    string apelido = "SALOMÃO";
        //    var lista = await prepedido.ListarClientesPrepedidosCombo(apelido);

        //    return Ok(lista);
        //}

        [AllowAnonymous]
        [HttpGet("buscarCliente/{cnpj_cpf}")]
        public async Task<IActionResult> BuscarCliente(string cnpj_cpf)
        {
            //para testar: http://localhost:60877/api/cliente/buscarCliente
            //TODO: passar para utils
            //string apelido = User.Claims.FirstOrDefault(r => r.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            string apelido = "SALOMÃO";
            var dadosCliente = await clienteBll.BuscarCliente(cnpj_cpf);
            return Ok(dadosCliente);

        }
    }
}