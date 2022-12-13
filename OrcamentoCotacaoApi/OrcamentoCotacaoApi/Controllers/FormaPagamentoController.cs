using InfraBanco.Constantes;
using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(ResourceFilter))]
    public class FormaPagamentoController : BaseController
    {
        private readonly ILogger<FormaPagamentoController> _logger;
        private readonly FormaPagtoOrcamentoCotacaoBll _formaPagtoOrcamentoCotacaoBll;
        private readonly IServicoDecodificarToken _servicoDecodificarToken;

        public FormaPagamentoController(
            ILogger<FormaPagamentoController> logger,
            FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll,
            IServicoDecodificarToken servicoDecodificarToken)
        {
            _logger = logger;
            _formaPagtoOrcamentoCotacaoBll = formaPagtoOrcamentoCotacaoBll;
            _servicoDecodificarToken = servicoDecodificarToken;
        }

        [HttpPost("buscarFormasPagamentos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult BuscarFormasPagamentos(FormaPagtoRequestViewModel formaPagtoRequest)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido, FormaPagtoRequest = formaPagtoRequest };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarFormasPagamentos/POST - Request => [{JsonSerializer.Serialize(request)}].");

            var retorno = _formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos(
                formaPagtoRequest.TipoCliente,
                formaPagtoRequest.TipoUsuario,
                formaPagtoRequest.Apelido,
                formaPagtoRequest.ComIndicacao);

            if (retorno == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarFormasPagamentos/POST - Response => [Não tem response].");
                return NoContent();
            }

            var response = new
            {
                FormasPagamentos = retorno
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarFormasPagamentos/POST - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(retorno);   
        }

        [HttpGet("buscarQtdeMaxPacelas")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarQtdeMaxPacelas()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarQtdeMaxPacelas/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var retorno = await _formaPagtoOrcamentoCotacaoBll.GetMaximaQtdeParcelasCartaoVisa((Constantes.TipoUsuario)LoggedUser.TipoUsuario);

            if (retorno == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarQtdeMaxPacelas/GET - Response => [Não tem response].");
                return NoContent();
            }

            _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarQtdeMaxPacelas/GET - Response => [{JsonSerializer.Serialize(retorno)}].");

            return Ok(retorno);
        }

        [HttpGet("buscarMeiosPagtos")]
        public IActionResult BuscarMeiosPagtos(List<int> tiposPagtos, string tipoCliente, byte comIndicacao)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                TiposPagtos = tiposPagtos.Count,
                TipoCliente = tipoCliente,
                ComIndicacao = comIndicacao
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarMeiosPagtos/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var retorno = _formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos(tipoCliente, (Constantes.TipoUsuario)LoggedUser.TipoUsuario, LoggedUser.Apelido, comIndicacao);

            if (retorno == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarMeiosPagtos/POST - Response => [Não tem response].");
                return NoContent();
            }

            var response = new
            {
                FormasPagamentos = retorno.Count
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. FormaPagamentoController/BuscarMeiosPagtos/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(retorno);
        }
    }
}