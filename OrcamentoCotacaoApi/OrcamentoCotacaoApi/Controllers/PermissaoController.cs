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

        [AllowAnonymous]
        [HttpGet]
        [Route("RetornarPermissaoOrcamento")]
        public async Task<IActionResult> RetornarPermissaoOrcamento(int idOrcamento)
        {
            try
            {
                var request = new PermissaoOrcamentoRequest()
                {
                    PermissoesUsuario = LoggedUser.Permissoes,
                    Usuario = LoggedUser.Nome,
                    IdOrcamento = idOrcamento,
                    TipoUsuario = LoggedUser.TipoUsuario.Value,
                };

                _logger.LogInformation($"Verificando permissões do usuario: {LoggedUser.Nome} para o orçamento: {idOrcamento}.");

                var response = await _permissaoBll.RetornarPermissaoOrcamento(request);

                _logger.LogInformation(
                    $"Retornando permissões do usuario: {LoggedUser.Nome} para o orçamento: {idOrcamento}. " +
                    $"VizualizarOrcamento = {response.VizualizarOrcamento}" +
                    $"ProrrogarOrcamento = {response.ProrrogarOrcamento}" +
                    $"EditarOrcamento = {response.EditarOrcamento}" +
                    $"CancelarOrcamento = {response.CancelarOrcamento}" +
                    $"ClonarOrcamento = {response.ClonarOrcamento}" +
                    $"ReenviarOrcamento = {response.ReenviarOrcamento}" +
                    $"EditarOpcaoOrcamento = {response.EditarOpcaoOrcamento}" +
                    $"AprovarOpcaoOrcamento = {response.AprovarOpcaoOrcamento}" +
                    $"NenhumaOpcaoOrcamento = {response.NenhumaOpcaoOrcamento}" +
                    $"DesabilitarBotoes = {response.DesabilitarBotoes}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}