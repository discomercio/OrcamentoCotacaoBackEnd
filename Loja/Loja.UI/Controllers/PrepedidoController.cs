using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Loja.Bll.Bll.AcessoBll;
using Loja.Bll.Bll.PrepedidoBll;
using Loja.Bll.ClienteBll;
using Loja.Bll.Constantes;
using Loja.Bll.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

#nullable enable
namespace Loja.UI.Controllers
{
    public class PrepedidoController : Controller
    {
        public static bool PermissaoPagina(string pagina, UsuarioLogado usuarioLogado)
        {
            if (!usuarioLogado.Operacao_permitida(Constantes.OP_LJA_CONSULTA_ORCAMENTO) && !usuarioLogado.Operacao_permitida(Constantes.OP_LJA_CONSULTA_UNIVERSAL_PEDIDO_ORCAMENTO))
                return false;
            return true;
        }


        private readonly PrepedidoBll prepedidoBll;
        private readonly UsuarioAcessoBll usuarioAcessoBll;
        private readonly Configuracao configuracao;
        private readonly ILogger<UsuarioLogado> loggerUsuarioLogado;
        private readonly ClienteBll clienteBll;

        public PrepedidoController(Loja.Bll.Bll.PrepedidoBll.PrepedidoBll prepedidoBll, UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            ILogger<UsuarioLogado> loggerUsuarioLogado, ClienteBll clienteBll)
        {
            this.prepedidoBll = prepedidoBll;
            this.usuarioAcessoBll = usuarioAcessoBll;
            this.configuracao = configuracao;
            this.loggerUsuarioLogado = loggerUsuarioLogado;
            this.clienteBll = clienteBll;
        }
        public async Task<IActionResult> Index()
        {
            var usuarioLogado = new UsuarioLogado(loggerUsuarioLogado, User, HttpContext.Session, clienteBll, usuarioAcessoBll, configuracao);
            if (!PermissaoPagina("Index", usuarioLogado))
                return Forbid();

            var resumoPrepedidoListaDto = await prepedidoBll.ResumoPrepedidoLista(usuarioLogado);
            var itensLoja = (from i in resumoPrepedidoListaDto.Itens group i by i.LojaId into g select new Models.Comuns.ListaLojasViewModel.ItemLoja { Loja = g.Key, NumeroItens = g.Count() });
            var model = new Loja.UI.Models.Prepedido.PrepedidoIndexViewModel(resumoPrepedidoListaDto,
                new Models.Comuns.ListaLojasViewModel(usuarioLogado, itensLoja.ToList()));
            return View(model);
        }
    }
}