using AutoMapper;
using InfraBanco.Modelos.Filtros;
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
    public class OrcamentoController : BaseController
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly IMapper _mapper;
        private readonly OrcamentoCotacaoBll _orcamentoBll;

        public OrcamentoController(ILogger<OrcamentoController> logger, IMapper mapper, OrcamentoCotacaoBll orcamentoBll)
        {
            _logger = logger;
            _mapper = mapper;
            _orcamentoBll = orcamentoBll;
        }

        [HttpGet]
        public IActionResult PorFiltro(int page, int pageItens, string origem, string lojaLogada)
        {
            _logger.LogInformation("Buscando lista de orçamentos");

            var saida = _orcamentoBll.PorFiltro(new TorcamentoFiltro { Page = page, RecordsPerPage = pageItens, Origem = origem, Loja = lojaLogada });

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

        //[HttpGet("id")]
        //public async Task<OrcamentoResponseViewModel> GetById(string id)
        //{
        //    _logger.LogInformation("Buscando orcamento");
        //    Torcamento orcamento = _orcamentoBll.GetById(id);
        //    return _mapper.Map<OrcamentoResponseViewModel>(orcamento);
        //}

        [HttpPost]
        public async Task<IActionResult> Post(OrcamentoRequestViewModel model)
        {
            var user = User.Identity.Name;

            _logger.LogInformation("Inserindo Orcamento");

            return Ok(model);
        }

        //[HttpPut]
        //public async Task<OrcamentoResponseViewModel> Put(OrcamentoRequestViewModel model)
        //{
        //    var user = User.Identity.Name;

        //    _logger.LogInformation("Alterando orcamento");
        //    var orcamento = _orcamentoBll.Atualizar(_mapper.Map<Torcamento>(model));//model, user);
        //    return _mapper.Map<OrcamentoResponseViewModel>(orcamento);
        //}
    }
}
