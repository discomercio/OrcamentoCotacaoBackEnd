using FormaPagamento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FormaPagamentoController : BaseController
    {
        private readonly ILogger<FormaPagamentoController> _logger;
        private readonly FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll;

        public FormaPagamentoController(ILogger<FormaPagamentoController> logger, FormaPagtoOrcamentoCotacaoBll formaPagtoOrcamentoCotacaoBll)
        {
            this._logger = logger;
            this.formaPagtoOrcamentoCotacaoBll = formaPagtoOrcamentoCotacaoBll;
        }

#if DEBUG
        [AllowAnonymous]
#endif
        [HttpGet]
        [Route("buscarFormasPagamentos12")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> BuscarFormasPagamentos(string tipoCliente)
        {
            var retorno = formaPagtoOrcamentoCotacaoBll.BuscarFormasPagamentos("PF", "");

            if (retorno == null) return NotFound();

            return Ok(retorno);
        }
    }
}
