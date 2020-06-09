using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/prepedidoUnis")]
    [ApiController]
    public class PrepedidoUnisController : Controller
    {
        private readonly PrePedidoUnisBll prepedidoUnisBll;
        public PrepedidoUnisController(PrePedidoUnisBll prepedidoUnisBll)
        {
            this.prepedidoUnisBll = prepedidoUnisBll;
        }

        [AllowAnonymous]
        [HttpPost("cadastrarPrepedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<PrePedidoResultadoUnisDto>> CadastrarPrepedido(PrePedidoUnisDto prePedido)
        {
            //todo: validar o token
            PrePedidoResultadoUnisDto retorno;
            //todo: retornar a estrutura certa
            var ret = await prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido);

            return Ok(ret);
        }

    }
}