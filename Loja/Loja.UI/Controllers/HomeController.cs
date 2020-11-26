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
using Loja.Bll.Bll.pedidoBll;
using Loja.Bll.Dto.AvisosDto;

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
        private readonly CancelamentoAutomaticoBll cancelamentoAutomaticoBll;

        public HomeController(ILogger<HomeController> logger, ClienteBll clienteBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado, PrepedidoBll prepedidoBll,
            Bll.Bll.pedidoBll.CancelamentoAutomaticoBll cancelamentoAutomaticoBll)
        {
            _logger = logger;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
            this.prepedidoBll = prepedidoBll;
            this.cancelamentoAutomaticoBll = cancelamentoAutomaticoBll;
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

            //vamos buscar a quantidade de orcamentos novos; na home somente mostramos da loja atual
            var taskResumoPrepedidoListaDto = prepedidoBll.ResumoPrepedidoLista(usuarioLogado, true);
            var taskCancelamentoAutomaticoViewModel = cancelamentoAutomaticoBll.DadosTela(usuarioLogado);

            //vamos buscar os avisos não lidos
            //Id / Mensagem / Usuario / Destinatario / Dt_Ult_Atualizacao

            model.ResumoPrepedidoListaDto = await taskResumoPrepedidoListaDto;
            model.CancelamentoAutomaticoItems = (await taskCancelamentoAutomaticoViewModel).cancelamentoAutomaticoItems;
            return View(model);
        }

        [HttpGet]
        public async Task<List<AvisoDto>> BuscarAvisosNaoLidos()
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            List<AvisoDto> lst = (await usuarioAcessoBll.BuscarAvisosNaoLidos(usuarioLogado.Loja_atual_id, usuarioLogado.Usuario_nome_atual)).ToList();

            return lst;
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
