using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.ClienteBll;
using Loja.Bll.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Loja.UI.Controllers
{
    public class SiteColorsController : Controller
    {
        private readonly SiteColorsBll siteColorsBll;
        private readonly Configuracao configuracao;
        private readonly ClienteBll clienteBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;

        public SiteColorsController(SiteColorsBll siteColorsBll, Configuracao configuracao, ClienteBll clienteBll, 
            UsuarioAcessoBll usuarioAcessoBll,
            ILogger<UsuarioLogado> loggerUsuarioLogado)
        {
            this.siteColorsBll = siteColorsBll;
            this.configuracao = configuracao;
            this.clienteBll = clienteBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            var sessionCtrlInfo = await siteColorsBll.MontaSessionCtrlInfo(usuarioLogado);

            var model = new Models.SiteColors.SiteColorsViewModel(sessionCtrlInfo, "resumo.asp", configuracao);

            return View(model);
        }
    }
}