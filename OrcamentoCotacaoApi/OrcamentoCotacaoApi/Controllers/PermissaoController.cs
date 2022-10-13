using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using System;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PermissaoController : BaseController
    {
        private readonly ILogger<PermissaoController> _logger;
        private readonly PermissaoBll _permissaoBll;

        public PermissaoController(ILogger<PermissaoController> logger, PermissaoBll permissaoBll)
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
                var request = new PermissaoOrcamentoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                    Usuario = LoggedUser.Apelido,
                    IdOrcamento = idOrcamento,
                    TipoUsuario = LoggedUser.TipoUsuario.Value,
                };

                _logger.LogInformation($"Verificando permissões do usuario: {LoggedUser.Nome} para o orçamento: {idOrcamento}.");

                var response = await _permissaoBll.RetornarPermissaoOrcamento(request);

                _logger.LogInformation(
                    $"Retornando permissões do usuario: {LoggedUser.Nome} para o orçamento: {idOrcamento}. " +
                    $"VizualizarOrcamento = {response.VisualizarOrcamento}" +
                    $"ProrrogarOrcamento = {response.ProrrogarOrcamento}" +
                    $"EditarOrcamento = {response.EditarOrcamento}" +
                    $"CancelarOrcamento = {response.CancelarOrcamento}" +
                    $"ClonarOrcamento = {response.ClonarOrcamento}" +
                    $"ReenviarOrcamento = {response.ReenviarOrcamento}" +
                    $"EditarOpcaoOrcamento = {response.EditarOpcaoOrcamento}" +
                    $"AprovarOpcaoOrcamento = {response.DesabilitarAprovarOpcaoOrcamento}" +
                    $"NenhumaOpcaoOrcamento = {response.NenhumaOpcaoOrcamento}" +
                    $"DesabilitarBotoes = {response.DesabilitarBotoes}");

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
                var request = new PermissaoPrePedidoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                    Usuario = LoggedUser.Apelido,
                    IdPrePedido = idPrePedido,
                    TipoUsuario = LoggedUser.TipoUsuario.Value,
                    IdUsuario = LoggedUser.Id
                };

                _logger.LogInformation($"Verificando permissões do usuario: {LoggedUser.Nome} para o orçamento: {idPrePedido}.");

                var response = await _permissaoBll.RetornarPermissaoPrePedido(request);

                _logger.LogInformation(
                    $"Retornando permissões do usuario: {LoggedUser.Nome} para o pré pedido: {idPrePedido}. " +
                    $"VizualizarOrcamento = {response.VisualizarPrePedido}" +
                    $"ProrrogarOrcamento = {response.CancelarPrePedido}");

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
                var request = new PermissaoPedidoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                    Usuario = LoggedUser.Apelido,
                    IdPedido = idPedido,
                    TipoUsuario = LoggedUser.TipoUsuario.Value,
                    IdUsuario = LoggedUser.Id
                };

                _logger.LogInformation($"Verificando permissões do usuario: {LoggedUser.Nome} para o pedido: {idPedido}.");

                var response = await _permissaoBll.RetornarPermissaoPedido(request);

                _logger.LogInformation(
                    $"Retornando permissões do usuario: {LoggedUser.Nome} para o pedido: {idPedido}. " +
                    $"VizualizarOrcamento = {response.VisualizarPedido}");

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
                var request = new PermissaoIncluirPrePedidoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                };

                _logger.LogInformation($"Verificando permissões do usuario: {LoggedUser.Nome} para incluir pré pedido.");

                var response = await _permissaoBll.RetornarPermissaoIncluirPrePedido(request);

                _logger.LogInformation($"Retornando permissões do usuario: {LoggedUser.Nome} para incluir pré pedido. {response.IncluirPrePedido}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}