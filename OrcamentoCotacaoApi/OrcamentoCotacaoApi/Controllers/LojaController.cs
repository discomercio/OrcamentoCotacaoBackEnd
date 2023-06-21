using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Bll;
using System;
using System.Linq;
using System.Text.Json;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [TypeFilter(typeof(ControleDelayFilter))]
    [Route("[controller]")]
    [ApiController]
    [TypeFilter(typeof(ResourceFilter))]
    public class LojaController : BaseController
    {
        private readonly ILogger<LojaController> _logger;
        private readonly LojaOrcamentoCotacaoBll _lojaBll;

        public LojaController(
            ILogger<LojaController> logger, 
            LojaOrcamentoCotacaoBll lojaBll)
        {
            _logger = logger;
            _lojaBll = lojaBll;
        }

        [HttpGet]
        public IActionResult Get(int page, int pageItens)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Page = page,
                PageItens = pageItens
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/Get/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = _lojaBll.PorFiltro(new TlojaFiltro { });

            if (saida != null)
            {
                var response = new
                {
                    Lojas = saida.Count
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/Get/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/Get/GET - Response => [Não tem response].");
            return NoContent();
        }

        [HttpGet("buscarPercMaxPorLoja")]
        public IActionResult BuscarPercMaxPorLoja(string loja, string tipoCliente)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Loja = loja,
                TipoCliente = tipoCliente
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarPercMaxPorLoja/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = _lojaBll.BuscarPercMaxPorLoja(loja);

            if (saida != null)
            {
                var response = new
                {
                    PercMaxPorLoja = saida
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarPercMaxPorLoja/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarPercMaxPorLoja/GET - Response => [Não tem response].");
            return NotFound();
        }
        
        [HttpGet("buscarPercMaxPorLojaAlcada")]
        public IActionResult BuscarPercMaxPorLojaAlcada(string loja, string tipoCliente)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Loja = loja,
                TipoCliente = tipoCliente
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarPercMaxPorLojaAlcada/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var lstPermissoes = user.Permissoes;

            var saida = _lojaBll.BuscarPercMaxPorLojaAlcada(loja, tipoCliente, lstPermissoes);

            if (saida != null)
            {
                var response = new
                {
                    PercMaxPorLojaAlcada = saida
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarPercMaxPorLojaAlcada/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarPercMaxPorLojaAlcada/GET - Response => [Não tem response].");
            return NotFound();
        }

        [HttpGet("{loja}/estilo")]
        public IActionResult BuscarLojaEstilo(string loja)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarLojaEstilo/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var saida = _lojaBll.BuscarLojaEstilo(loja);

            if (saida != null)
            {
                var response = new
                {
                    LojaEstilo = saida
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarLojaEstilo/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(saida);
            }

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/BuscarLojaEstilo/GET - Response => [Não tem response].");
            return NotFound();
        }
    }
}