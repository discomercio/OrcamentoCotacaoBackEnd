using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orcamento;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoApi.Utils;
using OrcamentoCotacaoBusiness.Models.Response.Relatorios;
using static OrcamentoCotacaoBusiness.Enums.Enums;
using System;
using UtilsGlobais.Configs;
using OrcamentoCotacaoBusiness.Bll;
using System.Text.Json;
using OrcamentoCotacaoBusiness.Models.Request.Relatorios;
using NuGet.Packaging;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Linq;

namespace OrcamentoCotacaoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
    [TypeFilter(typeof(ExceptionFilter))]
    public class RelatoriosController : BaseController
    {
        private readonly ILogger<RelatoriosController> _logger;
        private readonly RelatoriosBll _relatoriosBll;

        public RelatoriosController(ILogger<RelatoriosController> logger, RelatoriosBll relatoriosBll)
        {
            _logger = logger;
            _relatoriosBll = relatoriosBll;
        }

        [HttpPost("relatorioItensOrcamento")]
        public IActionResult RelatorioItensOrcamento(ItensOrcamentoRequest model)
        {
            var request = new
            {
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. RelatoriosController/RelatorioItensOrcamento/POST - Request => [{JsonSerializer.Serialize(model)}].");

            var response = new RelatorioItensOrcamentosResponse();
            //verificar a permissão
            var permissao = User.ValidaPermissao((int)ePermissao.RelatoriosGerenciais);
            if (!permissao)
            {
                response.Sucesso = false;
                response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
                return Ok(response);
            }

            var acessoUniversal = User.ValidaPermissao((int)ePermissao.AcessoUniversalOrcamentoPedidoPrepedidoConsultar);
            if (!acessoUniversal)
            {
                var usuario = new List<string>
                {
                    LoggedUser.Id.ToString()
                };
                model.Vendedores = usuario.ToArray();
            }

            response = _relatoriosBll.RelatorioItensOrcamento(model);

            return Ok(response);
        }

        [HttpPost("relatorioDadosOrcamento")]
        public IActionResult RelatorioDadosOrcamento(DadosOrcamentoRequest model)
        {
            var request = new
            {
                CorrelationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]),
                IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
            };

            _logger.LogInformation($"CorrelationId => [{request.CorrelationId}]. RelatoriosController/RelatorioDadosOrcamento/POST - Request => [{JsonSerializer.Serialize(model)}].");

            var response = new RelatorioDadosOrcamentosResponse();
            //verificar a permissão
            var permissao = User.ValidaPermissao((int)ePermissao.RelatoriosGerenciais);
            if (!permissao)
            {
                response.Sucesso = false;
                response.Mensagem = "Não encontramos a permissão necessária para acessar essa funcionalidade!";
                return Ok(response);
            }

            response = _relatoriosBll.RelatorioDadosOrcamento(model);

            return Ok(response);
        }
    }
}
