
using Cfg.CfgOrcamentoCotacaoEndpointFilter;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog.Targets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Threading;
using UtilsGlobais;

namespace OrcamentoCotacaoApi.Filters
{
    public class ControleDelayFilter : IActionFilter
    {
        private readonly ContextoBdProvider _contextoBdProvider;
        private readonly TcfgOrcamentoCotacaoEndpointFilterBll _bll;
        private readonly IConfiguration _configuration;

        public ControleDelayFilter(ContextoBdProvider _contextoBdProvider, TcfgOrcamentoCotacaoEndpointFilterBll _bll, IConfiguration _configuration)
        {
            this._contextoBdProvider = _contextoBdProvider;
            this._bll = _bll;
            this._configuration = _configuration;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var appSettingsSection = _configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<Configuracao>();

            if (appSettings.CadastrarEControlarTempoRespostaEndpoint)
            {
                using (var contexto = _contextoBdProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.NENHUM))
                {
                    var lista = _bll.PorFiltro(new TcfgOrcamentoCotacaoEndpointFilterFiltro());

                    var actionName = context.RouteData.Values.Values.ToList()[1] + "/" + context.RouteData.Values.Values.ToList()[0].ToString();
                    var itemEncontrado = lista.Where(x => x.Endpoint == actionName).FirstOrDefault();
                    if (itemEncontrado == null)
                    {
                        _bll.InserirComTransacao(new TcfgOrcamentoCotacaoEndpointFilter()
                        {
                            Id = 0,
                            Endpoint = actionName,
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
        }
    }
}
