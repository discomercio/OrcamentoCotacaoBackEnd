using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Bll;
using OrcamentoCotacaoBusiness.Models.Request;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class OrcamentoController : BaseController
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly OrcamentoCotacaoBll _orcamentoBll;

        public OrcamentoController(ILogger<OrcamentoController> logger, OrcamentoCotacaoBll orcamentoBll)
        {
            _logger = logger;
            _orcamentoBll = orcamentoBll;
        }

        [HttpPost("porfiltro")]
        public IActionResult PorFiltro(TorcamentoFiltro filtro)
        {
            _logger.LogInformation("Buscando lista de orçamentos");

            var saida = _orcamentoBll.PorFiltro(filtro, LoggedUser);

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("status")]
        public async Task<IActionResult> ObterListaStatus(string origem, string lojaLogada)
        {
            _logger.LogInformation("Buscando status");

            var saida = await _orcamentoBll.ObterListaStatus(new TorcamentoFiltro { Origem = origem, Loja = lojaLogada });

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpGet("validade")]
        public IActionResult BuscarConfigValidade()
        {
            _logger.LogInformation("Buscando ConfigValidade");

            var saida = _orcamentoBll.BuscarConfigValidade();

            if (saida != null)
                return Ok(saida);
            else
                return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrcamentoRequestViewModel model)
        {
            _logger.LogInformation("Inserindo Orcamento");

            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);

            var id = _orcamentoBll.CadastrarOrcamentoCotacao(model, user);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var orcamentoCotacao = _orcamentoBll.PorFiltro(id);

            return Ok(orcamentoCotacao);
        }

        [HttpGet("buscarDadosParaMensageria")]
        public IActionResult BuscarDadosParaMensageria(int idOrcamento, bool usuarioInterno)
        {
            var user = JsonSerializer.Deserialize<UsuarioLogin>(User.Claims.FirstOrDefault(x => x.Type == "UsuarioLogin").Value);
            var dados = _orcamentoBll.BuscarDadosParaMensageria(user, idOrcamento, usuarioInterno);
            return Ok(dados);
        }

        [HttpPost("{id}/prorrogar")]
        public IActionResult ProrrogarOrcamento(int id)
        {
            return Ok(_orcamentoBll.ProrrogarOrcamento(id, LoggedUser.Id));
        }
    }
}
