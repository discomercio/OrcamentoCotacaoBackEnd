using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MagentoBusiness.MagentoBll.AcessoBll;
using MagentoBusiness.MagentoDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMagento.Controllers
{
    [Route("api/acessoMagento")]
    [ApiController]
    public class AcessoMagentoController : Controller
    {
        private readonly AcessoMagentoBll acessoMagentoBll;
        private readonly IServicoValidarTokenApiMagento servicoValidarTokenApiMagento;

        public AcessoMagentoController(AcessoMagentoBll acessoMagentoBll, IServicoValidarTokenApiMagento servicoValidarTokenApiMagento)
        {
            this.acessoMagentoBll = acessoMagentoBll;
            this.servicoValidarTokenApiMagento = servicoValidarTokenApiMagento;
        }

        /// <summary>
        /// Obtém um token de acesso
        /// </summary>
        [AllowAnonymous]
        [HttpPost("fazerLogin")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LoginResultadoMagentoDto>> FazerLogin(LoginMagentoDto login)
        {
            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string userAgent = Request.Headers["User-agent"];
            return Ok(await acessoMagentoBll.FazerLogin(login, ip, userAgent));
        }

        /// <summary>
        /// Registra o fim da sessão
        /// </summary>
        [AllowAnonymous]
        [HttpPost("fazerLogout")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LogoutResultadoMagentoDto>> FazerLogout(LogoutMagentoDto logout)
        {
            if (!servicoValidarTokenApiMagento.ValidarToken(logout.TokenAcesso, out string? usuario))
                return Unauthorized();
            if(string.IsNullOrEmpty(usuario))
                return Unauthorized();
            return Ok(await acessoMagentoBll.FazerLogout(usuario));
        }

        /// <summary>
        /// Obtém informações da versão da API
        /// </summary>
        [AllowAnonymous]
        [HttpPost("versaoApi")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<VersaoApiMagentoDto>> VersaoApi()
        {
            return Ok(await acessoMagentoBll.VersaoApi());
        }
    }
}
