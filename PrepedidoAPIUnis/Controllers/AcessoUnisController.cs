using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrepedidoApiUnisBusiness.UnisDto.AcessoDto;
using PrepedidoUnisBusiness.UnisBll;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/acessoUnis")]
    [ApiController]
    public class AcessoUnisController : Controller
    {
        private readonly AcessoUnisBll acessoUnisBll;

        public AcessoUnisController(PrepedidoUnisBusiness.UnisBll.AcessoUnisBll acessoUnisBll)
        {
            this.acessoUnisBll = acessoUnisBll;
        }


        /// <summary>
        /// Obtém um token de acesso
        /// </summary>
        [AllowAnonymous]
        [HttpPost("fazerLogin")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LoginResultadoUnisDto>> FazerLogin(LoginUnisDto login)
        {
            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string userAgent = Request.Headers["User-agent"];
            return Ok(await acessoUnisBll.FazerLogin(login, ip, userAgent));
        }

        [AllowAnonymous]
        [HttpGet("fazerLogout")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LogoutResultadoUnisDto>> FazerLogout(LogoutUnisDto logout)
        {
            return Ok(await FazerLogoutInterno(logout));

        }
        private async Task<LogoutResultadoUnisDto> FazerLogoutInterno(LogoutUnisDto logout)
        {
            LogoutResultadoUnisDto ret = new LogoutResultadoUnisDto();
            ret.ListaErros = new List<string>();
            ret.ListaErros.Add("Erro: ainda não implementado");
            //todo: login afazer 
            return ret;
        }

    }
}
