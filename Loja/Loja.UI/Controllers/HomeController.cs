using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Loja.UI.Models;
using Microsoft.AspNetCore.Session;
using Loja.UI.Models.Home;
using Loja.Bll.ClienteBll;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.Util;

namespace Loja.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly Configuracao configuracao;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;

        public HomeController(ILogger<HomeController> logger, ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado)
        {
            _logger = logger;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        public IActionResult Index(string novaloja)
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            var model = new HomeViewModel();
            if (!string.IsNullOrWhiteSpace(novaloja))
            {
                model.LojaTentandoChavearId = novaloja;
                if (!usuarioLogado.Loja_atual_alterar(novaloja, usuarioAcessoBll))
                {
                    model.ErroChavearLoja = true;
                }
            }
            model.LojaAtivaId = usuarioLogado.Loja_atual_id;
            model.LojaAtivaNome = usuarioLogado.LojasDisponiveis.FirstOrDefault(r => r.Id == model.LojaAtivaId)?.Nome;
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
