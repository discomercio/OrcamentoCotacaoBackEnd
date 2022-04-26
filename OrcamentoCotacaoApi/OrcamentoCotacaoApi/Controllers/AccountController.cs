﻿using InfraBanco.Constantes;
using InfraIdentity;
using Loja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrcamentistaEindicador;
using OrcamentistaEIndicadorVendedor;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Interfaces;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Threading.Tasks;
using Usuario;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : BaseController
    {
        private readonly IServicoAutenticacao _servicoAutenticacao;
        private readonly IConfiguration _configuration;
        private readonly OrcamentoCotacaoBusiness.Bll.AcessoBll _acessoBll;
        private readonly ILogger<AccountController> _logger;
        private readonly ITokenService _tokenService;
        private readonly UsuarioBll _usuarioBll;
        private readonly OrcamentistaEIndicadorBll _orcamentistaEindicadorBll;
        private readonly OrcamentistaEIndicadorVendedorBll _orcamentistaEindicadorVendedorBll;
        private readonly LojaBll _lojaBll;

        public AccountController(IServicoAutenticacao servicoAutenticacao, IConfiguration configuration, ILogger<AccountController> logger,
            OrcamentoCotacaoBusiness.Bll.AcessoBll acessoBll, ITokenService tokenService, UsuarioBll usuarioBll, OrcamentistaEIndicadorBll orcamentistaEindicadorBll,
            OrcamentistaEIndicadorVendedorBll orcamentistaEindicadorVendedorBll, LojaBll lojaBll)
        {
            _servicoAutenticacao = servicoAutenticacao;
            _configuration = configuration;
            _acessoBll = acessoBll;
            _tokenService = tokenService;
            _usuarioBll = usuarioBll;
            _orcamentistaEindicadorBll = orcamentistaEindicadorBll;
            _orcamentistaEindicadorVendedorBll = orcamentistaEindicadorVendedorBll;
            _lojaBll = lojaBll;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<object>> Login(LoginRequestViewModel login)
        {
            try
            {
                var appSettingsSection = _configuration.GetSection("AppSettings");
                var appSettings = appSettingsSection.Get<OrcamentoCotacaoApi.Utils.Configuracao>();
                string apelido = login.Login.ToUpper();
                string senha = login.Senha;

                UsuarioLogin objUsuarioLogin = new UsuarioLogin()
                {
                    Apelido = apelido,
                    Senha = senha
                };

                objUsuarioLogin = _servicoAutenticacao.ObterTokenAutenticacao(
                    objUsuarioLogin, 
                    appSettings.SegredoToken, 
                    appSettings.ValidadeTokenMinutos,
                    OrcamentoCotacaoApi.Utils.Autenticacao.RoleAcesso, 
                    new ServicoAutenticacaoProvider(_acessoBll, _usuarioBll, _orcamentistaEindicadorBll, _orcamentistaEindicadorVendedorBll, _lojaBll),
                    out bool unidade_negocio_desconhecida
                    );

                if (objUsuarioLogin == null || objUsuarioLogin.Token == null)
                {
                    return BadRequest(new LoginResponseViewModel
                    {
                        Authenticated = false,
                        Created = "",
                        Expiration = "",
                        AccessToken = "",
                        Message = "Usuário ou senha incorretos!"
                    });
                }

                _logger.LogInformation("Gerando token");

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(10000);

                if (objUsuarioLogin.TipoUsuario == (int)Constantes.TipoUsuario.VENDEDOR &&
                    !objUsuarioLogin.Permissoes.Contains(((int)Constantes.ePermissoes.ACESSO_AO_MODULO_100100).ToString()))
                {
                    return await Task.FromResult(new LoginResponseViewModel
                    {
                        Authenticated = false,
                        Created = "",
                        Expiration = "",
                        AccessToken = "",
                        Message = "Usuário não possui acesso ao Módulo."
                    });
                }

                return await Task.FromResult(new LoginResponseViewModel
                {
                    Authenticated = true,
                    Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    AccessToken = objUsuarioLogin.Token,
                    Message = "OK"
                });
            }
            catch 
            {
                throw;
            }
        }
    }
}
