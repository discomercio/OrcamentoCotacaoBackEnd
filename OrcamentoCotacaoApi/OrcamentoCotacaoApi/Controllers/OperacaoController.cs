using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Operacao;
using OrcamentoCotacaoApi.Filters;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("[controller]")]
    [ApiController]
    [TypeFilter(typeof(ResourceFilter))]
    public class OperacaoController : BaseController
    {
        private readonly ILogger<OperacaoController> _logger;
        private readonly OperacaoBll _operacaoBll;

        public OperacaoController(
            ILogger<OperacaoController> logger, 
            OperacaoBll operacaoBll)
        {
            _logger = logger;
            _operacaoBll = operacaoBll;
        }

        [HttpGet("modulo")]
        public async Task<IActionResult> ObterOperacaoPorModulo(string modulo)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Modulo = modulo
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. OperacaoController/ObterOperacaoPorModulo/GET - Request => [{JsonSerializer.Serialize(request)}].");

            try
            {
                var saida = _operacaoBll.PorFiltro(new ToperacaoFiltro() { Modulo = modulo });

                var response = new
                {
                    Operacao = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. OperacaoController/ObterOperacaoPorModulo/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}