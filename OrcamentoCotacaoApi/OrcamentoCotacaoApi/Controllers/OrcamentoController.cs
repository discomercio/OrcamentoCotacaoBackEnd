using AutoMapper;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orcamento;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly OrcamentoBll _orcamentoBll;

        public OrcamentoController(ILogger<OrcamentoController> logger, IMapper mapper, OrcamentoBll orcamentoBll)
        {
            _logger = logger;
            _mapper = mapper;
            _orcamentoBll = orcamentoBll;
        }

        [HttpGet]
        public async Task<IEnumerable<OrcamentoResponseViewModel>> Get(int page, int pageItens)
        {
            _logger.LogInformation("Buscando lista de orçamentos");
            List<Torcamento> orcamento = _orcamentoBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentoFiltro { Page = page, RecordsPerPage = pageItens });// (page, pageItens);
            return _mapper.Map<List<OrcamentoResponseViewModel>>(orcamento);
        }

        [HttpGet]
        [Route("id")]
        public async Task<OrcamentoResponseViewModel> Get(string id)
        {
            _logger.LogInformation("Buscando orcamento");
            Torcamento orcamento = _orcamentoBll.GetById(id);
            return _mapper.Map<OrcamentoResponseViewModel>(orcamento);
        }

        [HttpPost]
        public async Task<OrcamentoResponseViewModel> Post(OrcamentoRequestViewModel model)
        {
            var user = User.Identity.Name;

            _logger.LogInformation("Inserindo Orcamento");
            var orcamento = _orcamentoBll.Inserir(_mapper.Map<Torcamento>(model));//(model, user);
            return _mapper.Map<OrcamentoResponseViewModel>(orcamento);
        }

        [HttpPut]
        public async Task<OrcamentoResponseViewModel> Put(OrcamentoRequestViewModel model)
        {
            var user = User.Identity.Name;

            _logger.LogInformation("Alterando orcamento");
            var orcamento = _orcamentoBll.Atualizar(_mapper.Map<Torcamento>(model));//model, user);
            return _mapper.Map<OrcamentoResponseViewModel>(orcamento);
        }
    }
}
