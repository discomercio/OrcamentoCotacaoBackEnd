using InfraBanco.Constantes;
using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
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
            var retorno = _formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos(formaPagtoRequest.TipoCliente, (Constantes.TipoUsuario)LoggedUser.TipoUsuario, LoggedUser.Apelido, byte.Parse(formaPagtoRequest.ComIndicacao));

            if (retorno == null)
                return NoContent();
            else
                return Ok(retorno);
        }

        [HttpGet("buscarQtdeMaxPacelas")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarQtdeMaxPacelas()
        {
            var retorno = await _formaPagtoOrcamentoCotacaoBll.GetMaximaQtdeParcelasCartaoVisa((Constantes.TipoUsuario)LoggedUser.TipoUsuario);

            if (retorno == null)
                return NoContent();
            else return Ok(retorno);
        }
    }
}
