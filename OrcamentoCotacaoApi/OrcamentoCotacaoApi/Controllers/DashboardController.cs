using Arquivo.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request.Dashboard;
using OrcamentoCotacaoBusiness.Models.Response.Dashoard;
using InfraBanco.Modelos.Filtros;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly OrcamentoCotacaoBll _orcamentoCotacaoBll;
        private readonly IOptions<Configuracoes> _appSettings;

        public DashboardController(
            ILogger<DashboardController> logger,
            IOptions<Configuracoes> appSettings,
            OrcamentoCotacaoBll orcamentoCotacaoBll
            )
        {
            _logger = logger;
            _appSettings = appSettings;
            _orcamentoCotacaoBll = orcamentoCotacaoBll;
        }


    }
}