﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrepedidoApiUnisBusiness.UnisDto.AcessoDto;
using PrepedidoUnisBusiness.UnisBll.AcessoBll;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/acessoUnis")]
    [ApiController]
    public class AcessoUnisController : Controller
    {
        private readonly AcessoUnisBll acessoUnisBll;
        private readonly IServicoValidarTokenApiUnis servicoValidarTokenApiUnis;

        public AcessoUnisController(AcessoUnisBll acessoUnisBll, IServicoValidarTokenApiUnis servicoValidarTokenApiUnis)
        {
            this.acessoUnisBll = acessoUnisBll;
            this.servicoValidarTokenApiUnis = servicoValidarTokenApiUnis;
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

        /// <summary>
        /// Registra o fim da sessão
        /// </summary>
        [AllowAnonymous]
        [HttpPost("fazerLogout")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LogoutResultadoUnisDto>> FazerLogout(LogoutUnisDto logout)
        {
            if (!servicoValidarTokenApiUnis.ValidarToken(logout.TokenAcesso, out string usuario))
                return Unauthorized();
            return Ok(await acessoUnisBll.FazerLogout(logout, usuario));
        }

        /// <summary>
        /// Obtém informações da versão da API
        /// </summary>
        [AllowAnonymous]
        [HttpPost("versaoApi")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<VersaoApiUnisDto>> VersaoApi()
        {
            return Ok(await acessoUnisBll.VersaoApi());
        }

    }
}
