﻿using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PrepedidoApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<AcessoController> logger;

        public AcessoController(IServicoAutenticacao servicoAutenticacao, IConfiguration configuration, PrepedidoBusiness.Bll.AcessoBll acessoBll,
            IServicoDecodificarToken servicoDecodificarToken, ILogger<AcessoController> logger)
        {
            this.servicoAutenticacao = servicoAutenticacao;
            this.configuration = configuration;
            this.acessoBll = acessoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.logger = logger;
        }


        [HttpGet("renovarToken")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult RenovarToken()
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Utils.Configuracao>();
            string apelido = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.NameIdentifier).Value.Trim();
            string nome = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Name).Value;
            string loja = User.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Surname).Value;
            string unidade_negocio = User.Claims.FirstOrDefault(r => r.Type == "unidade_negocio").Value;

            UsuarioLogin objUsuarioLogin = new UsuarioLogin()
            {
                Apelido = apelido,
                Nome = nome,
                Loja = loja,
                Unidade_negocio = unidade_negocio
            };

            objUsuarioLogin = servicoAutenticacao.RenovarTokenAutenticacao(objUsuarioLogin, appSettings.SegredoToken, appSettings.ValidadeTokenMinutos,
                Utils.Autenticacao.RoleAcesso, out bool unidade_negocio_desconhecida);

            if (unidade_negocio_desconhecida)
            {
                logger.LogWarning($"RenovarToken unidade_negocio_desconhecida apelido:{apelido}");
                return Forbid();
            }

            if (objUsuarioLogin.Token == null)
                return BadRequest(new { message = "Erro no sistema de autenticação. O usuário pode ter sido editado no banco de dados." });

            return Ok(objUsuarioLogin.Token);
        }



        //este é o único que permite acesso anônimo
        //erro 403: t_LOJA.unidade_negocio desconhecida
        [AllowAnonymous]
        [HttpPost("fazerLogin")]
        public async Task<IActionResult> FazerLogin(PrepedidoBusiness.Dto.Acesso.LoginDto login)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Utils.Configuracao>();
            string apelido = login.Apelido;
            string senha = login.Senha;
            UsuarioLogin objUsuarioLogin = new UsuarioLogin()
            {
                Apelido = apelido,
                Senha = senha
            };

            objUsuarioLogin = servicoAutenticacao.ObterTokenAutenticacao(objUsuarioLogin, appSettings.SegredoToken, appSettings.ValidadeTokenMinutos,
                Utils.Autenticacao.RoleAcesso, new ServicoAutenticacaoProvider(acessoBll), out bool unidade_negocio_desconhecida);

            if (unidade_negocio_desconhecida)
            {
                logger.LogWarning($"FazerLogin unidade_negocio_desconhecida apelido:{login.Apelido}");
                return Forbid();
            }

            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string userAgent = Request.Headers["User-agent"];

            if (!string.IsNullOrEmpty(objUsuarioLogin.Token))
                await acessoBll.GravarSessaoComTransacao(ip, apelido, userAgent);

            if (objUsuarioLogin.Token == null)
                return BadRequest(new { message = "Usuário ou senha incorreta." });

            return Ok(objUsuarioLogin.Token);
        }

        [HttpGet("fazerLogout")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> FazerLogout()
        {
            //para testar: http://localhost:60877/api/acesso/fazerLogout
            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            await acessoBll.FazerLogout(apelido.Trim());

            return Ok();
        }

        [HttpPost("alterarSenha")]
        [AllowAnonymous]
        public async Task<IActionResult> AlterarSenha(PrepedidoBusiness.Dto.Acesso.AlterarSenhaDto alterarSenhaDto)
        {
            var retorno = "";

            if (!string.IsNullOrEmpty(alterarSenhaDto.Apelido))
            {
                retorno = await acessoBll.AlterarSenha(alterarSenhaDto);
            }

            retorno = Newtonsoft.Json.JsonConvert.SerializeObject(retorno);
            return Ok(retorno);
        }
    }
}
