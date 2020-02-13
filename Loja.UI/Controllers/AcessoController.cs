﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Loja.UI.Models.Acesso;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Loja.UI.Controllers
{
    [AllowAnonymous]
    public class AcessoController : Controller
    {
        private readonly IConfiguration configuration;

        public AcessoController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        public IActionResult AcessoNegado(string ReturnUrl)
        {
            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = ReturnUrl,
                PermitirManterConectado = configuration.GetSection("Acesso").GetValue<bool>("PermitirManterConectado"),
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
                PermitirManterConectado = configuration.GetSection("Acesso").GetValue<bool>("PermitirManterConectado")
            };
            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("Login");
        }

        [HttpPost]
        public async Task<IActionResult> LoginSubmit(LoginViewModel loginViewModel)
        {

            var apelido = loginViewModel.Usuario?.ToUpper().Trim() ?? "";
            var senha = loginViewModel.Senha?.ToUpper() ?? "";
            loginViewModel.ReturnUrl = loginViewModel.ReturnUrl ?? "/";

            if (String.IsNullOrEmpty(apelido) || String.IsNullOrEmpty(senha))
            {
                loginViewModel.ErroUsuarioSenha = true;
                return View("Login", loginViewModel);
            }

            /*
             * todo: afazer: eduperez terminar o login
            var appSettingsSection = configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Utils.Configuracao>();
            string token = await servicoAutenticacao.ObterTokenAutenticacao(apelido, senha, appSettings.SegredoToken, appSettings.ValidadeTokenMinutos, Utils.Autenticacao.RoleAcesso, new ServicoAutenticacaoProvider(acessoBll));

            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            string userAgent = Request.Headers["User-agent"];

            await acessoBll.GravarSessaoComTransacao(ip, apelido, userAgent);

 * 
             string token = "nadfa";
            if (token == null)
            {
                loginViewModel.ErroUsuarioSenha = true;
                return View("Login", loginViewModel);
            }
* */


            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginViewModel.Usuario?.Trim() ?? "")
                };
            foreach (var role in PrepedidoBusiness.Bll.AcessoBll.RolesDoUsuario())
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = configuration.GetSection("Acesso").GetValue<bool>("ExpiracaoMovel"),
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(configuration.GetSection("Acesso").GetValue<int>("ExpiracaoCookieMinutos")),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = configuration.GetSection("Acesso").GetValue<bool>("PermitirManterConectado") ? loginViewModel.ManterConectado : false,
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