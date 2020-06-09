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
    public class AcessoController : Controller
    {
        public AcessoController()
        {
        }


        [AllowAnonymous]
        [HttpPost("fazerLogin")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LoginResultadoUnisDto>> FazerLogin(LoginUnisDto login)
        {
            //todo: afazer
            string token = "nao implementado";
            //todo: retornar a estrutura certa
            return Ok(1);
        }

        [AllowAnonymous]
        [HttpGet("fazerLogout")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LogoutResultadoUnisDto>> FazerLogout(LogoutUnisDto logout)
        {
            //todo: afazer
            //todo: retornar a estrutura certa
            return Ok();
        }

    }
}
