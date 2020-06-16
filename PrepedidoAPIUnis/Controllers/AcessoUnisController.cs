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
using InfraIdentity.ApiUnis;
using PrepedidoAPIUnis.Utils;

namespace PrepedidoAPIUnis.Controllers
{
    [Route("api/acessoUnis")]
    [ApiController]
    public class AcessoUnisController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IServicoAutenticacaoApiUnis servicoAutenticacaoApiUnis;

        public AcessoUnisController(IConfiguration configuration, IServicoAutenticacaoApiUnis servicoAutenticacaoApiUnis)
        {
            this.configuration = configuration;
            this.servicoAutenticacaoApiUnis = servicoAutenticacaoApiUnis;
        }


        /// <summary>
        /// Obtém um token de acesso
        /// </summary>
        [AllowAnonymous]
        [HttpPost("fazerLogin")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<LoginResultadoUnisDto>> FazerLogin(LoginUnisDto login)
        {
            return Ok(await FazerLoginInterno(login));
        }

        private async Task<LoginResultadoUnisDto> FazerLoginInterno(LoginUnisDto login)
        {
            LoginResultadoUnisDto ret = new LoginResultadoUnisDto();
            ret.ListaErros = new List<string>();

            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Utils.ConfiguracaoApiUnis>();
            var tokenClasse = await servicoAutenticacaoApiUnis.ObterTokenAutenticacaoApiUnis(login.Usuario, login.Senha, appSettings.SegredoToken, appSettings.ValidadeTokenMinutos, 
                Utils.AutenticacaoApiUnis.RoleAcesso, new ServicoAutenticacaoProviderApiUnis());

            //todo: login
            /*            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                        string userAgent = Request.Headers["User-agent"];

                        if (!string.IsNullOrEmpty(token))
                            await acessoBll.GravarSessaoComTransacao(ip, apelido, userAgent);

                        if (token == null)
                            return BadRequest(new { message = "Usuário ou senha incorreta." });
                            */
            ret.TokenAcesso = tokenClasse.Token;
            return ret;
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
            //todo: afazer
            return ret;
        }

    }
}
