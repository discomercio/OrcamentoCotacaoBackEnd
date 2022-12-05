using Cep;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using Prepedido.Bll;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais.Configs;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    [TypeFilter(typeof(ResourceFilter))]
    public class PublicoController : BaseController
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly OrcamentoCotacaoBll _orcamentoBll;
        private readonly CepPrepedidoBll _cepPrepedidoBll;
        private readonly CepBll _cepBll;
        private readonly ClientePrepedidoBll _clientePrepedidoBll;

        public PublicoController(
            ILogger<OrcamentoController> logger,
            OrcamentoCotacaoBll orcamentoBll,
            CepPrepedidoBll cepPrepedidoBll,
            CepBll cepBll,
            ClientePrepedidoBll _clientePrepedidoBll
            )
        {
            _logger = logger;
            _orcamentoBll = orcamentoBll;
            _cepPrepedidoBll = cepPrepedidoBll;
            _cepBll = cepBll;
            this._clientePrepedidoBll = _clientePrepedidoBll;
        }

        [HttpGet("orcamentoporguid/{guid}")]
        public IActionResult OrcamentoPorGuid(string guid)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Valor = guid
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/OrcamentoPorGuid/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var orcamento = _orcamentoBll.PorGuid(guid);

            if (orcamento != null)
            {
                var response = new
                {
                    Orcamento = orcamento.id
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/OrcamentoPorGuid/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(orcamento);
            }
            else
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/OrcamentoPorGuid/GET - Response => [Não tem response].");
                return NoContent();
            }
        }

        [HttpGet("buscarCep")]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Cep = cep,
                Endereco = endereco,
                Uf = uf,
                Cidade = cidade
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarCep/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _cepPrepedidoBll.BuscarCep(cep, endereco, uf, cidade);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarCep/GET - Response => [Retorna lista de CEP(s)].");

            return Ok(response);
        }

        [HttpGet("buscarUfs")]
        public async Task<ActionResult<IEnumerable<string>>> BuscarUfs()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarUfs/GET - Request => [Não tem request.].");

            var response = await _cepBll.BuscarUfs();

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarUfs/GET - Response => [Retorna lista de Uf(s)].");

            return Ok(response);
        }

        [HttpGet("buscarCepPorEndereco")]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCepPorEndereco(string endereco, string localidade, string uf)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Endereco = endereco,
                Localidade = localidade,
                Uf = uf
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarCepPorEndereco/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _cepPrepedidoBll.BuscarCepPorEndereco(endereco, localidade, uf);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarCepPorEndereco/GET - Response => [Retorna lista de CEP(s)].");

            return Ok(response);
        }

        [HttpGet("buscarLocalidades")]
        public async Task<ActionResult<IEnumerable<string>>> BuscarLocalidades(string uf)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Uf = uf
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarLocalidades/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _cepBll.BuscarLocalidades(uf);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarLocalidades/GET - Response => [Retorna lista de Localidade(s)].");

            return Ok(response);
        }

        [HttpGet("listarComboJustificaEndereco/{loja}")]
        public async Task<IActionResult> ListarComboJustificaEndereco(string loja)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Loja = loja
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/ListarComboJustificaEndereco/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = await _clientePrepedidoBll.ListarComboJustificaEndereco(null, loja);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/ListarComboJustificaEndereco/GET - Response => [Retorna lista de JustificaEndereco(s)].");

            return Ok(response);
        }

        [HttpPost("aprovarOrcamento")]
        public async Task<IActionResult> AprovarOrcamento(AprovarOrcamentoRequestViewModel aprovarOrcamento)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Orcamento = aprovarOrcamento.IdOrcamento
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/AprovarOrcamento/POST - Request => [{JsonSerializer.Serialize(request)}].");


            var orcamento = _orcamentoBll.ObterIdOrcamentoCotacao(aprovarOrcamento.Guid);

            if (orcamento == null)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/AprovarOrcamento/GET - Response => [Não tem response].");
                return BadRequest(new { message = "Acesso negado." });
            }


            if (orcamento.id != aprovarOrcamento.IdOrcamento)
            {
                _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/AprovarOrcamento/GET - Response => [Não tem response].");
                return BadRequest(new { message = "Acesso negado." });
            }

            var retorno = await _orcamentoBll.AprovarOrcamento(
                aprovarOrcamento, 
                Constantes.TipoUsuarioContexto.Cliente,
                (int)Constantes.TipoUsuario.CLIENTE);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/AprovarOrcamento/GET - Response => [Retorna lista de mensagens].");

            return Ok(retorno);
        }

        [HttpGet("parametros")]
        public IActionResult BuscarParametros(int idCfgParametro, string lojaLogada)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                IdCfgParametro = idCfgParametro,
                LojaLogada = lojaLogada
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarParametros/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var response = _orcamentoBll.BuscarParametros(idCfgParametro, lojaLogada);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PublicoController/BuscarParametros/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }
    }
}