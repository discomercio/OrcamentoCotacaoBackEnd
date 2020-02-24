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

        public HomeController(ILogger<HomeController> logger, ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao)
        {
            _logger = logger;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        public IActionResult Index(string novaloja)
        {
            var usuarioLogado = new UsuarioLogado(User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);
            var model = new HomeViewModel();
            if (!string.IsNullOrWhiteSpace(novaloja))
            {
                model.LojaTentandoChavearId = novaloja;
                if (!usuarioLogado.LojaAtivaAlterar(novaloja))
                {
                    model.ErroChavearLoja = true;
                }
            }
            model.LojaAtivaId = usuarioLogado.LojaAtiva;
            model.LojaAtivaNome = usuarioLogado.Loja_troca_rapida_monta_itens_select.FirstOrDefault(r => r.Id == model.LojaAtivaId)?.Nome;
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
