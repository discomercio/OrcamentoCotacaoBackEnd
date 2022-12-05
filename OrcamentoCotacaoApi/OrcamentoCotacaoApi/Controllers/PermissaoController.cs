using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
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
    public class PermissaoController : BaseController
    {
        private readonly ILogger<PermissaoController> _logger;
        private readonly PermissaoBll _permissaoBll;

        public PermissaoController(
            ILogger<PermissaoController> logger, 
            PermissaoBll permissaoBll)
        {
            _logger = logger;
            _permissaoBll = permissaoBll;
        }
        
        [HttpGet]
        [Route("RetornarPermissaoOrcamento")]
        public async Task<IActionResult> RetornarPermissaoOrcamento(int idOrcamento)
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                var request = new PermissaoOrcamentoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                    Usuario = LoggedUser.Apelido,
                    IdOrcamento = idOrcamento,
                    TipoUsuario = LoggedUser.TipoUsuario.Value,
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PermissaoController/RetornarPermissaoOrcamento/GET - Request => [{JsonSerializer.Serialize(request)}].");

                var response = await _permissaoBll.RetornarPermissaoOrcamento(request);

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PermissaoController/RetornarPermissaoOrcamento/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet]
        [Route("RetornarPermissaoPrePedido")]
        public async Task<IActionResult> RetornarPermissaoPrePedido(string idPrePedido)
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                var request = new PermissaoPrePedidoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                    Usuario = LoggedUser.Apelido,
                    IdPrePedido = idPrePedido,
                    TipoUsuario = LoggedUser.TipoUsuario.Value,
                    IdUsuario = LoggedUser.Id
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PermissaoController/RetornarPermissaoPrePedido/GET - Request => [{JsonSerializer.Serialize(request)}].");

                var response = await _permissaoBll.RetornarPermissaoPrePedido(request);

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PermissaoController/RetornarPermissaoPrePedido/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("RetornarPermissaoPedido")]
        public async Task<IActionResult> RetornarPermissaoPedido(string idPedido)
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                var request = new PermissaoPedidoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                    Usuario = LoggedUser.Apelido,
                    IdPedido = idPedido,
                    TipoUsuario = LoggedUser.TipoUsuario.Value,
                    IdUsuario = LoggedUser.Id
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PermissaoController/RetornarPermissaoPedido/GET - Request => [{JsonSerializer.Serialize(request)}].");

                var response = await _permissaoBll.RetornarPermissaoPedido(request);

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PermissaoController/RetornarPermissaoPedido/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("RetornarPermissaoIncluirPrePedido")]
        public async Task<IActionResult> RetornarPermissaoIncluirPrePedido()
        {
            try
            {
                var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

                var request = new PermissaoIncluirPrePedidoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                };

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PermissaoController/RetornarPermissaoIncluirPrePedido/GET - Request => [{JsonSerializer.Serialize(request)}].");

                var response = await _permissaoBll.RetornarPermissaoIncluirPrePedido(request);

                _logger.LogInformation($"CorrelationId => [{correlationId}]. PermissaoController/RetornarPermissaoIncluirPrePedido/GET - Response => [{JsonSerializer.Serialize(response)}].");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}