using Arquivo.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrcamentoCotacaonet7Api.Filters;
using OrcamentoCotacaonet7Api.Utils;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request.Dashboard;
using OrcamentoCotacaoBusiness.Models.Response.Dashoard;
using InfraBanco.Modelos.Filtros;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;
using static OrcamentoCotacaoBusiness.Enums.Enums;

namespace OrcamentoCotacaonet7Api.Controllers
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

        [HttpGet("orcamento/parceiro")]
        public async Task<IActionResult> DashboardOrcamentoParceiro([FromQuery] DashboardRequest request)
        {

            if (!User.ValidaPermissao((int)ePermissao.AcessoAoModulo))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar essa atividade!" });

            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.Usuario = LoggedUser.Apelido;
            request.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. DashboardController/DashboardOrcamentoParceiro/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = _orcamentoCotacaoBll.Dashboard(new TorcamentoFiltro() { Origem = request.Origem, StatusId = request.Status }, LoggedUser) ;

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. DashboardController/DashboardOrcamentoParceiro/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet("orcamento/vendedor-parceiro")]
        public async Task<IActionResult> DashboardOrcamentoVendedorParceiro([FromQuery] DashboardRequest request)
        {

            if (!User.ValidaPermissao((int)ePermissao.AcessoAoModulo))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar essa atividade!" });

            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.Usuario = LoggedUser.Apelido;
            request.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. DashboardController/DashboardOrcamentoVendedorParceiro/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = _orcamentoCotacaoBll.Dashboard(new TorcamentoFiltro() { Origem = request.Origem,  StatusId = request.Status }, LoggedUser);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. DashboardController/DashboardOrcamentoVendedorParceiro/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet("orcamento/vendedor-interno")]
        public async Task<IActionResult> DashboardOrcamentoVendedorInterno([FromQuery] DashboardRequest request)
        {
            
            if (!User.ValidaPermissao((int)ePermissao.AcessoAoModulo))
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar essa atividade!" });

            request.CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);
            request.Usuario = LoggedUser.Apelido;
            request.IP = HttpContext.Connection.RemoteIpAddress.ToString();

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. DashboardController/DashboardOrcamentoVendedorInterno/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = _orcamentoCotacaoBll.Dashboard(new TorcamentoFiltro() { Loja = request.Loja, Origem = request.Origem, StatusId = request.Status }, LoggedUser);

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. DashboardController/DashboardOrcamentoVendedorInterno/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

    }
}