using Cep;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Dto.Cep;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PublicoController : BaseController
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly OrcamentoCotacaoBll _orcamentoBll;
        private readonly PrepedidoBusiness.Bll.CepPrepedidoBll _cepPrepedidoBll;
        private readonly CepBll _cepBll;
        private readonly ClientePrepedidoBll _clientePrepedidoBll;

        public PublicoController(
            ILogger<OrcamentoController> logger,
            OrcamentoCotacaoBll orcamentoBll,
            PrepedidoBusiness.Bll.CepPrepedidoBll cepPrepedidoBll,
            CepBll cepBll,
            ClientePrepedidoBll _clientePrepedidoBll
            )
        {
            _logger = logger;
            _orcamentoBll = orcamentoBll;
            _cepPrepedidoBll = cepPrepedidoBll;
            _cepBll = cepBll;
            this._clientePrepedidoBll = _clientePrepedidoBll;
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

        [HttpGet("buscarCep")]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            return Ok(await _cepPrepedidoBll.BuscarCep(cep, endereco, uf, cidade));
        }

        [HttpGet("buscarUfs")]
        public async Task<ActionResult<IEnumerable<string>>> BuscarUfs()
        {
            return Ok(await _cepBll.BuscarUfs());
        }

        [HttpGet("buscarCepPorEndereco")]
        public async Task<ActionResult<IEnumerable<CepDto>>> BuscarCepPorEndereco(string endereco, string localidade, string uf)
        {
            return Ok(await _cepPrepedidoBll.BuscarCepPorEndereco(endereco, localidade, uf));
        }

        [HttpGet("buscarLocalidades")]
        public async Task<ActionResult<IEnumerable<string>>> BuscarLocalidades(string uf)
        {
            return Ok(await _cepBll.BuscarLocalidades(uf));
        }

        [HttpGet("listarComboJustificaEndereco/{loja}")]
        public async Task<IActionResult> ListarComboJustificaEndereco(string loja)
        {
            return Ok(await _clientePrepedidoBll.ListarComboJustificaEndereco(null, loja));
        }

        [HttpPost("aprovarOrcamento")]
        public IActionResult AprovarOrcamento()
        {
            return Ok();
        }
    }
}
