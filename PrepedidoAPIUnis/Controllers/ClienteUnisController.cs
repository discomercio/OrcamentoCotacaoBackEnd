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
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteUnisController : Controller
    {
        private readonly ClienteUnisBll clienteUnisBll;        

        public ClienteUnisController(ClienteUnisBll clienteUnisBll)
        {
            this.clienteUnisBll = clienteUnisBll;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("cadastrarCliente")]
        public async Task<IActionResult> CadastrarCliente(ClienteCadastroUnisDto clienteDto)
        {
            var retorno = await clienteUnisBll.CadastrarClienteUnis(clienteDto);

            return Ok(retorno);
        }

    }
}