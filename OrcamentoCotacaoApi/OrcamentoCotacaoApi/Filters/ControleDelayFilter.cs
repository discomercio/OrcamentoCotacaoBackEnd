
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using InfraBanco;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using NLog.Targets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Threading;
using TesteEndpoint;
using static InfraBanco.ContextoBdGravacao;

namespace OrcamentoCotacaoApi.Filters
{
    public class ControleDelayFilter : IActionFilter
    {
        private readonly ContextoBdProvider _contextoBdProvider;
        private readonly TesteEndpointBll _bll;

        public ControleDelayFilter(ContextoBdProvider _contextoBdProvider, TesteEndpointBll _bll)
        {
            this._contextoBdProvider = _contextoBdProvider;
            this._bll = _bll;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            using (var contexto = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
            {
                var lista = _bll.PorFiltro(new InfraBanco.Modelos.Filtros.EndpointsFiltro());

                var actionName = context.RouteData.Values.Values.ToList()[1]+"/"+context.RouteData.Values.Values.ToList()[0].ToString();
                var itemEncontrado = lista.Where(x => x.ActionName == actionName).FirstOrDefault();
                if (itemEncontrado == null)
                {
                    _bll.InserirComTransacao(new TEndpoints()
                    {
                        ActionName = actionName,
                        Delay = 0
                    }, contexto);

                    contexto.SaveChanges();
                    contexto.transacao.Commit();
                }
                else
                {
                    if (itemEncontrado.Delay > 0)
                    {
                        Thread.Sleep(itemEncontrado.Delay);
                    }
                }

            }
        }

        private List<ControleDelay> Banco { get; set; } = new List<ControleDelay>();

        private class ControleDelay
        {
            [Description("ActionName")]
            public string ActionName { get; set; }

            [Description("Delay")]
            public int Delay { get; set; }
        }
    }
}
