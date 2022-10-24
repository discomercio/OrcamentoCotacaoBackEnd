using InfraIdentity;
using InfraBanco.Modelos.Filtros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Operacao;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OperacaoController : BaseController
    {
        private readonly ILogger<OperacaoController> _logger;
        private readonly OperacaoBll _operacaoBll;

        public OperacaoController(ILogger<OperacaoController> logger, OperacaoBll operacaoBll)
        {
            _logger = logger;
            _operacaoBll = operacaoBll;
        }

        [HttpGet("modulo")]
        public async Task<IActionResult> ObterOperacaoPorModulo(string modulo)
        {
            _logger.LogInformation("ObterOperacaoPorModulo");

            var saida =  _operacaoBll.PorFiltro(new ToperacaoFiltro() { Modulo = modulo });

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }
    }
}
