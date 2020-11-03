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
        //para montar as URLs
        public static string UrlAction(IUrlHelper urlHelper, ListaPaginasColors pagina)
        {
            var ret = urlHelper.Action("Index", "SiteColors", new
            {
                //convertemos para int para não passar o texto
                pagina = (int)pagina
            });
            return ret;
        }
        public enum ListaPaginasColors
        {
            Pedidos_com_Credito_Pendente,
            Pedidos_com_Credito_Pendente_Vendas,
            Pedidos_Pendentes_Cartao_de_Credito,
            Pedidos_com_Endereco_Pendente,
            Pesquisa_pedidos_anteriormente_efetuados_por_um_cliente_nesta_loja
        }

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
        public async Task<IActionResult> Index(ListaPaginasColors? pagina)
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);

            var sessionCtrlInfo = await siteColorsBll.MontaSessionCtrlInfo(usuarioLogado);
            var paginaUrl = pagina switch
            {
                ListaPaginasColors.Pedidos_com_Credito_Pendente => "RelPedidosCredPendFiltro.asp",
                ListaPaginasColors.Pedidos_com_Credito_Pendente_Vendas => "RelPedidosCredPendVendasFiltro.asp",
                ListaPaginasColors.Pedidos_Pendentes_Cartao_de_Credito => "RelPedidosPendentesCartaoFiltro.asp",
                ListaPaginasColors.Pedidos_com_Endereco_Pendente => "RelPedidosEnderecoPendenteFiltro.asp",
                ListaPaginasColors.Pesquisa_pedidos_anteriormente_efetuados_por_um_cliente_nesta_loja => "RelPedidosAnteriores.asp",
                _ => "resumo.asp",
            };
            var model = new Models.SiteColors.SiteColorsViewModel(sessionCtrlInfo, paginaUrl, configuracao);

            return View(model);
        }
    }
}