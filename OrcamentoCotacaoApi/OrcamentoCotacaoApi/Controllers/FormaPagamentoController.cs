using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;

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

        [HttpGet("buscarFormasPagamentos")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult BuscarFormasPagamentos(FormaPagtoRequestViewModel formaPagtoRequest)
        {
            var retorno = _formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos(formaPagtoRequest.TipoCliente, (short)LoggedUser.TipoUsuario, LoggedUser.Apelido, formaPagtoRequest.ComIndicacao);

            if (retorno == null)
                return NoContent();
            else
                return Ok(retorno);
        }

    }
}
