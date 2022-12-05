using Cep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Controllers;
using OrcamentoCotacaoApi.Filters;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais;
using UtilsGlobais.Configs;

namespace PrepedidoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    [TypeFilter(typeof(ResourceFilter))]
    public class CepController : BaseController
    {
        private readonly ILogger<CepController> _logger;
        private readonly Prepedido.Bll.CepPrepedidoBll cepPrepedidoBll;
        private readonly CepBll cepBll;

        public CepController(
            ILogger<CepController> logger,
            Prepedido.Bll.CepPrepedidoBll cepPrepedidoBll, 
            Cep.CepBll cepBll)
        {
            _logger = logger;
            this.cepPrepedidoBll = cepPrepedidoBll;
            this.cepBll = cepBll;
        }

        [HttpGet("buscarCep")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido, Cep = cep, Endereco = endereco, Uf = uf, Cidade = cidade };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. CepController/BuscarCep/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await cepPrepedidoBll.BuscarCep(cep, endereco, uf, cidade);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. CepController/BuscarCep/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok();
        }

        [HttpGet("buscarUfs")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<string>>> BuscarUfs()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. CepController/BuscarUfs/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await cepBll.BuscarUfs();

            _logger.LogInformation($"CorrelationId => [{correlationId}]. CepController/BuscarUfs/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok();
        }

        [HttpGet("buscarCepPorEndereco")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCepPorEndereco(string endereco, string localidade, string uf)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido, Endereco = endereco, Localidade = localidade, Uf = uf };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. CepController/BuscarCepPorEndereco/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await cepPrepedidoBll.BuscarCepPorEndereco(endereco, localidade, uf);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. CepController/BuscarCepPorEndereco/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        [HttpGet("buscarLocalidades")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult<IEnumerable<string>>> BuscarLocalidades(string uf)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new { Usuario = LoggedUser.Apelido, Uf = uf };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. CepController/BuscarLocalidades/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await cepBll.BuscarLocalidades(uf);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. CepController/BuscarLocalidades/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok();
        }
    }
}