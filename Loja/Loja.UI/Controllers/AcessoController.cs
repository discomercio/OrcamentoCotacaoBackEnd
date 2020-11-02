using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.ClienteBll;
using Loja.Bll.Util;
using Loja.UI.Models.Acesso;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

#nullable enable

namespace Loja.UI.Controllers
{
    [AllowAnonymous]
    public class AcessoController : Controller
    {
        private readonly Configuracao configuracao;
        private readonly ClienteBll clienteBll;
        private readonly AcessoAuthorizationHandlerBll acessoBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;

        public AcessoController(Configuracao configuracao, ClienteBll clienteBll, AcessoAuthorizationHandlerBll acessoBll, UsuarioAcessoBll usuarioAcessoBll,
            ILogger<UsuarioLogado> loggerUsuarioLogado)
        {
            this.configuracao = configuracao;
            this.clienteBll = clienteBll;
            this.acessoBll = acessoBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
        }

        [HttpGet]
        public IActionResult AcessoNegado(string ReturnUrl)
        {
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = ReturnUrl,
                PermitirManterConectado = configuracao.PermitirManterConectado,
                AcessoNegado = true
            };
            return View("Login", loginViewModel);
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = ReturnUrl,
                PermitirManterConectado = configuracao.PermitirManterConectado
            };
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);
            usuarioLogado.EncerrarSessao();
            await usuarioAcessoBll.LogoutUsuario(usuarioLogado);

            return Redirect("Login");
        }

        [HttpPost]
        public async Task<IActionResult> LoginSubmit(LoginSubmitViewModel loginSubmitViewModel)
        {
            var loginViewModel = new LoginViewModel();
            loginViewModel.Loja = loginSubmitViewModel.Loja;
            loginViewModel.Apelido = loginSubmitViewModel.Apelido;
            loginViewModel.Senha = loginSubmitViewModel.Senha;
            loginViewModel.ReturnUrl = loginSubmitViewModel.ReturnUrl;

            loginViewModel.Apelido = loginViewModel.Apelido?.ToUpper().Trim() ?? "";
            loginViewModel.Senha = loginViewModel.Senha?.ToUpper() ?? "";
            loginViewModel.ReturnUrl = loginViewModel.ReturnUrl ?? configuracao.Diretorios.RaizSiteLojaMvc + "/";

            if (String.IsNullOrEmpty(loginViewModel.Apelido) || String.IsNullOrEmpty(loginViewModel.Senha))
            {
                loginViewModel.ErroUsuarioSenha = true;
                loginViewModel.PermitirManterConectado = configuracao.PermitirManterConectado;
                return View("Login", loginViewModel);
            }

            loginViewModel.LoginUsuarioRetorno = await usuarioAcessoBll.LoginUsuario(loginViewModel.Apelido, loginViewModel.Senha, loginViewModel.Loja, HttpContext.Session, configuracao,
                HttpContext.Connection.RemoteIpAddress.ToString(), HttpContext.Request.Headers["User-Agent"]);
            if (!loginViewModel.LoginUsuarioRetorno.Sucesso)
            {
                loginViewModel.ErroUsuarioSenha = true;
                loginViewModel.PermitirManterConectado = configuracao.PermitirManterConectado;
                return View("Login", loginViewModel);
            }
            if (loginViewModel.LoginUsuarioRetorno.PrecisaAlterarSenha)
            {
                return View("PrecisaAlterarSenha", loginViewModel);
            }

            var claims = UsuarioLogado.ClaimsUsuario.CriarClaims(loginViewModel.Apelido?.Trim() ?? "");
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = configuracao.ExpiracaoMovel,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.Add(configuracao.ExpiracaoCookieMinutos),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = configuracao.PermitirManterConectado ? loginViewModel.ManterConectado : false,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                IssuedUtc = DateTimeOffset.UtcNow
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(loginViewModel.ReturnUrl);
        }
    }
}