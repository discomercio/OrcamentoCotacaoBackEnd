using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoApi.Controllers;
using OrcamentoCotacaoApi.Filters;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using Prepedido.PedidoVisualizacao;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using UtilsGlobais;
using UtilsGlobais.Configs;
using System.Linq;

namespace PrepedidoApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ResourceFilter))]
    [Authorize(Roles = Autenticacao.RoleAcesso)]
    public class PedidoController : BaseController
    {
        private readonly ILogger<PedidoController> _logger;
        private readonly PedidoVisualizacaoBll pedidoBll;
        private readonly PedidoPrepedidoApiBll pedidoPrepedidoApiBll;
        private readonly InfraIdentity.IServicoDecodificarToken servicoDecodificarToken;
        private readonly PermissaoBll permissaoBll;

        public PedidoController(
            ILogger<PedidoController> logger,
            PedidoVisualizacaoBll pedidoBll,
            InfraIdentity.IServicoDecodificarToken servicoDecodificarToken,
            PedidoPrepedidoApiBll pedidoPrepedidoApiBll,
            PermissaoBll permissaoBll)
        {
            this._logger = logger;
            this.pedidoBll = pedidoBll;
            this.servicoDecodificarToken = servicoDecodificarToken;
            this.pedidoPrepedidoApiBll = pedidoPrepedidoApiBll;
            this.permissaoBll = permissaoBll;
        }

        [HttpGet("listarNumerosPedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarNumerosPedidosCombo()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PedidoController/ListarNumerosPedidosCombo/GET - Request => [{JsonSerializer.Serialize(request)}].");


            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var response = await pedidoBll.ListarNumerosPedidoCombo(apelido.Trim());

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/Get/GET - Response => [{JsonSerializer.Serialize(response.Count())}].");

            return Ok(response);
        }

        [HttpGet("listarPedidos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarPedidos(
            string clienteBusca, 
            int tipoBusca, 
            string numPedido,
            DateTime? dataInicial, 
            DateTime? dataFinal)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                ClienteBusca = clienteBusca,
                TipoBusca = tipoBusca,
                NumPedido = numPedido,
                DataInicial = dataInicial,
                DataFinal = dataFinal
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PedidoController/ListarPedidos/GET - Request => [{JsonSerializer.Serialize(request)}].");


            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);
            var ret = await pedidoPrepedidoApiBll.ListarPedidos(
                apelido.Trim(),
                (PedidoVisualizacaoBll.TipoBuscaPedido)tipoBusca,
                clienteBusca,
                numPedido,
                dataInicial,
                dataFinal
                );

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/Get/GET - Response => [{JsonSerializer.Serialize(ret.Count())}].");

            return Ok(ret);
        }

        [HttpGet("listarCpfCnpjPedidosCombo")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> ListarCpfCnpjPedidosCombo()
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PedidoController/ListarCpfCnpjPedidosCombo/GET - Request => [{JsonSerializer.Serialize(request)}].");

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var response = await pedidoBll.ListarCpfCnpjPedidosCombo(apelido.Trim());

            _logger.LogInformation($"CorrelationId => [{correlationId}]. LojaController/Get/GET - Response => [{JsonSerializer.Serialize(response.Count())}].");

            return Ok(response);
        }

        [HttpGet("buscarPedido")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarPedido(string numPedido)
        {
            var correlationId = Guid.Parse(Request.Headers[HttpHeader.CorrelationIdHeader]);

            var request = new
            {
                Usuario = LoggedUser.Apelido,
                NumPedido = numPedido
            };

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PedidoController/BuscarPedido/GET - Request => [{JsonSerializer.Serialize(request)}].");

            var permissao = this.ObterPermissaoPedido(numPedido);

            if (!permissao.VisualizarPedido)
                return BadRequest(new { message = "Não encontramos a permissão necessária para realizar atividade!" });

            string apelido = servicoDecodificarToken.ObterApelidoOrcamentista(User);

            var response = await pedidoPrepedidoApiBll.BuscarPedido(apelido.Trim(), numPedido);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. PedidoController/BuscarPedido/GET - Response => [{JsonSerializer.Serialize(response)}].");

            return Ok(response);
        }

        private PermissaoPedidoResponse ObterPermissaoPedido(string IdPedido)
        {
            return this.permissaoBll.RetornarPermissaoPedido(new PermissaoPedidoRequest()
            {
                IdPedido = IdPedido,
                IdUsuario = LoggedUser.Id,
                PermissoesUsuario = LoggedUser.Permissoes,
                TipoUsuario = LoggedUser.TipoUsuario.Value,
                Usuario = LoggedUser.Apelido
            }).Result;
        }
    }
}