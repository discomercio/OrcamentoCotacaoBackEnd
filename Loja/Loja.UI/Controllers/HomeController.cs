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
using Loja.Bll.PrepedidoBll;

namespace Loja.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly Configuracao configuracao;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;
        private readonly PrepedidoBll prepedidoBll;

        public HomeController(ILogger<HomeController> logger, ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado, PrepedidoBll prepedidoBll)
        {
            _logger = logger;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
            this.prepedidoBll = prepedidoBll;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        public async Task<IActionResult> Index(string novaloja)
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

            //vamos buscar a quantidade de orcamentos novos
            var resumoPrepedidoListaDto = await prepedidoBll.ResumoPrepedidoLista(usuarioLogado);
            var itensLoja = (from i in resumoPrepedidoListaDto.Itens
                             group i by i.LojaId
                             into g
                             select new Models.Comuns.ListaLojasViewModel.ItemLoja
                             {
                                 Loja = g.Key,
                                 NumeroItens = g.Count()
                             });

            var select = resumoPrepedidoListaDto.Itens.Where(x => x.LojaId == usuarioLogado.Loja_atual_id);

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
