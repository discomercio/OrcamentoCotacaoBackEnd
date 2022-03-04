using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;

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
        public IActionResult BuscarFormasPagamentos(string tipoCliente, bool comIndicacao)
        {
            var retorno = _formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos(tipoCliente, (short)LoggedUser.TipoUsuario, LoggedUser.Apelido);

            if (retorno == null)
                return NoContent();
            else
                return Ok(retorno);
        }

    }
}
