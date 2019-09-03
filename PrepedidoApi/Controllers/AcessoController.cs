using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PrepedidoApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PrepedidoApi.Controllers
{
    /*
     * para testar:
     * http://localhost:60877/api/acesso/fazerLogin?apelido=nada&senha=nada
     * http://localhost:60877/api/acesso/fazerLogin?apelido=test&senha=test
     * 
     * e para ter o acesso, colocar no header do request:
     * Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJUZXN0IiwibmJmIjoxNTY1NjQ3NzM0LCJleHAiOjE1NjYyNTI1MzQsImlhdCI6MTU2NTY0NzczNH0.bEFZgFNhwq29UdTjeV8-aPkqoDdm5ojsInO-Xqlxxvo
     * 
     * Testado: alterar o Secret do appsettings.json efetivamente invalida o token
     * 
     * Para usar roles:
     * [Authorize(Roles = "RoleUsarGet")]
     * 
     * e ao gerar o token: 
     * 
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName.ToString()),
                    new Claim(ClaimTypes.Role, "RoleUsarGet")
                }),
     * 
     * */


    [Authorize(Roles = Utils.Autenticacao.RoleAcesso)]
    [ApiController]
    [Route("api/[controller]")]
    public class AcessoController : ControllerBase
    {
        private readonly IServicoAutenticacao servicoAutenticacao;
        private readonly IConfiguration configuration;
        private readonly PrepedidoBusiness.Bll.AcessoBll acessoBll;
        private readonly IServicoDecodificarToken servicoDecodificarToken;

        public AcessoController(IServicoAutenticacao servicoAutenticacao, IConfiguration configuration, PrepedidoBusiness.Bll.AcessoBll acessoBll,
            IServicoDecodificarToken servicoDecodificarToken)
        {
            this.servicoAutenticacao = servicoAutenticacao;
            this.configuration = configuration;
            this.acessoBll = acessoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
        }


        [HttpGet("renovarToken")]
        public IActionResult RenovarToken()
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Utils.Configuracao>();
            string apelido = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.NameIdentifier).Value;
            string nome = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Name).Value;
            string token = servicoAutenticacao.RenovarTokenAutenticacao(apelido, nome, appSettings.SegredoToken, appSettings.ValidadeTokenMinutos, Utils.Autenticacao.RoleAcesso);

            if (token == null)
                return BadRequest(new { message = "Erro no sistema de autenticação. O usuário pode ter sido editando no banco de dados." });

            return Ok(token);
        }

        //este é o único que permite acesso anônimo
        [AllowAnonymous]
        [HttpGet("fazerLogin")]
        public async Task<IActionResult> FazerLogin(string apelido, string senha)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Utils.Configuracao>();

            string token = await servicoAutenticacao.ObterTokenAutenticacao(apelido, senha, appSettings.SegredoToken, appSettings.ValidadeTokenMinutos, Utils.Autenticacao.RoleAcesso, new ServicoAutenticacaoProvider(acessoBll));
            
            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string userAgent = Request.Headers["User-agent"];

            await acessoBll.GravarSessao(ip, apelido, userAgent);

            if (token == null)
                return BadRequest(new { message = "Usuário ou senha incorreta." });

            return Ok(token);
        }

        [HttpPut("fazerLogout")]
        public async Task<IActionResult> FazerLogout()
        {
            //para testar: http://localhost:60877/api/acesso/fazerLogout
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            //string apelido = "SALOMÃO";

            //Faz um update na t_Usuario e update no t_SESSAO_HISTORICO
            await acessoBll.FazerLogout(apelido);

            return Ok();
        }

    }
}
