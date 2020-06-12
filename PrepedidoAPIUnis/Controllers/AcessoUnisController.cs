using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using PrepedidoApiUnisBusiness.UnisDto.AcessoDto;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/acessoUnis")]
    [ApiController]
    public class AcessoUnisController : Controller
    {
        public AcessoUnisController()
        {
        }


        /// <summary>
        /// Obtém um token de acesso
        /// </summary>
        [AllowAnonymous]
        [HttpPost("fazerLogin")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LoginResultadoUnisDto>> FazerLogin(LoginUnisDto login)
        {
            LoginResultadoUnisDto ret = new LoginResultadoUnisDto();
            ret.ListaErros = new List<string>();
            ret.ListaErros.Add("Erro: ainda não implementado");
            //todo: afazer
            return Ok(ret);
        }

        [AllowAnonymous]
        [HttpGet("fazerLogout")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LogoutResultadoUnisDto>> FazerLogout(LogoutUnisDto logout)
        {
            LogoutResultadoUnisDto ret = new LogoutResultadoUnisDto();
            ret.ListaErros = new List<string>();
            ret.ListaErros.Add("Erro: ainda não implementado");
            //todo: afazer
            return Ok(ret);
        }

    }
}
