using FormaPagamento;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FormaPagamentoController : BaseController
    {
        private readonly ILogger<FormaPagamentoController> _logger;
        private readonly FormaPagtoBll _formaPagtoBll;

        public FormaPagamentoController(ILogger<FormaPagamentoController> logger, FormaPagtoBll formaPagtoBll)
        {
            this._logger = logger;
            this._formaPagtoBll = formaPagtoBll;
        }

        [HttpGet]
        [Route("qtde-parcelas-cartao-visa")]
        public async Task<int> GetQtdeMaxParcelasCartaoVisa()
        {
            _logger.LogInformation("Buscando quantidade máxima de parcelas cartão visa.");
            var qtdeParcelas = await _formaPagtoBll.GetMaximaQtdeParcelasCartaoVisa();
            return qtdeParcelas;
        }
    }
}
