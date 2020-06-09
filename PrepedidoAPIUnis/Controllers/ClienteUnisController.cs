using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoApiUnisBusiness;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/clienteUnis")]
    [ApiController]
    public class ClienteUnisController : Controller
    {
        private readonly ClienteUnisBll clienteUnisBll;        

        public ClienteUnisController(ClienteUnisBll clienteUnisBll)
        {
            this.clienteUnisBll = clienteUnisBll;
        }

        [AllowAnonymous]
        [HttpPost("cadastrarCliente")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<ClienteCadastroResultadoUnisDto>> CadastrarCliente(ClienteCadastroUnisDto clienteDto)
        {
            //todo: validar o token
            ClienteCadastroResultadoUnisDto retorno;
            //todo: retornar a estrutura certa
            var ret = await clienteUnisBll.CadastrarClienteUnis(clienteDto);

            return Ok(ret);
        }

    }
}