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
    public class PrepedidoUnisController : Controller
    {
        private readonly PrePedidoUnisBll prepedidoUnisBll;
        public PrepedidoUnisController(PrePedidoUnisBll prepedidoUnisBll)
        {
            this.prepedidoUnisBll = prepedidoUnisBll;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpPost("cadastrarPrepedido")]
        public async Task<IActionResult> CadastrarPrepedido(PrePedidoUnisDto prePedido)
        {
            var ret = await prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido);

            return Ok(ret);
        }

    }
}