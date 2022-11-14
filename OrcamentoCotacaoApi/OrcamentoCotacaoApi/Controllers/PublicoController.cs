using Cep;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using Prepedido.Bll;
using Prepedido.Dto;
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
        private readonly CepPrepedidoBll _cepPrepedidoBll;
        private readonly CepBll _cepBll;
        private readonly ClientePrepedidoBll _clientePrepedidoBll;

        public PublicoController(
            ILogger<OrcamentoController> logger,
            OrcamentoCotacaoBll orcamentoBll,
            CepPrepedidoBll cepPrepedidoBll,
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
        public async Task<IActionResult> AprovarOrcamento(AprovarOrcamentoRequestViewModel aprovarOrcamento)
        {

            var orcamento = _orcamentoBll.ObterIdOrcamentoCotacao(aprovarOrcamento.Guid);

            if (orcamento == null) return BadRequest(new
            {
                message = "Acesso negado."
            });


            if (orcamento.id != aprovarOrcamento.IdOrcamento) return BadRequest(new
            {
                message = "Acesso negado."
            });

            var retorno = await _orcamentoBll.AprovarOrcamento(aprovarOrcamento, Constantes.TipoUsuarioContexto.Cliente,
                (int)Constantes.TipoUsuario.CLIENTE);
            return Ok(retorno);
        }

        [HttpGet("parametros")]
        public IActionResult BuscarParametros(int idCfgParametro, string lojaLogada)
        {
            return Ok(_orcamentoBll.BuscarParametros(idCfgParametro, lojaLogada));
        }
    }
}
