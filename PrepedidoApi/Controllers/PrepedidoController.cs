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
        public PrepedidoController(PrepedidoBusiness.Bll.PrepedidoBll prepedidoBll)
        {
            this.prepedidoBll = prepedidoBll;
        }

        //para teste, anonimo
        [AllowAnonymous]
        //TODO: pq precisa do httpget com um nome?
        [HttpGet("listarNumerosPrepedidosCombo")]
        public async Task<IActionResult> ListarNumerosPrepedidosCombo()
        {
            //para testar: http://localhost:60877/api/prepedido/listarNumerosPrepedidosCombo
            //TODO: passar para utils
            //string apelido = User.Claims.FirstOrDefault(r => r.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            string apelido = "OFICINA DO GELO"; // "PARCEIRO_ITS";
            var ret = await prepedidoBll.ListarNumerosPrepedidosCombo(apelido);
            var rest2 = ret.ToList();
            return Ok(ret);
        }
    }
}

