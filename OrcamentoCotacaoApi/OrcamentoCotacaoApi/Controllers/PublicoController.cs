using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PublicoController : BaseController
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly OrcamentoCotacaoBll _orcamentoBll;

        public PublicoController(
            ILogger<OrcamentoController> logger,
            OrcamentoCotacaoBll orcamentoBll
            )
        {
            _logger = logger;
            _orcamentoBll = orcamentoBll;
        }

        [HttpGet("orcamentoporguid/{guid}")]
        public IActionResult OrcamentoPorGuid(string guid)
        {
            _logger.LogInformation("Buscando orçamento por guid");

            var orcamento = _orcamentoBll.PorGuid(guid);

            if (orcamento != null)
                return Ok(orcamento);
            else
                return NoContent();            
        }

    }
}
