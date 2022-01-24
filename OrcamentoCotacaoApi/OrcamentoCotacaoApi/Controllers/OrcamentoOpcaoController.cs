using AutoMapper;
using InfraBanco.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Orcamento;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrcamentoOpcaoController : ControllerBase
    {
        private readonly ILogger<OrcamentoController> _logger;
        private readonly IMapper _mapper;
        private readonly OrcamentoOpcaoBll _orcamentoOpcaoBll;

        public OrcamentoOpcaoController(ILogger<OrcamentoController> logger, IMapper mapper, OrcamentoOpcaoBll OrcamentoOpcaoBll)
        {
            _logger = logger;
            _mapper = mapper;
            _orcamentoOpcaoBll = OrcamentoOpcaoBll;
        }


        [HttpGet]
        public async Task<IEnumerable<OrcamentoOpcaoResponseViewModel>> Get(int page, int pageItens)
        {
            _logger.LogInformation("Buscando lista de opções do orçamento");
            var orcamento = _orcamentoOpcaoBll.PorFiltro(new InfraBanco.Modelos.Filtros.TorcamentoCotacaoOpcaoFiltro() { Page = page, RecordsPerPage = pageItens });
            var orcamentoOpcaoRetorno = _mapper.Map<List<OrcamentoOpcaoResponseViewModel>>(orcamento);
            return orcamentoOpcaoRetorno;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<OrcamentoOpcaoResponseViewModel> Get(string id)
        {
            _logger.LogInformation("Buscando opção do orçamento");
            var orcamento = _orcamentoOpcaoBll.GetById(id);
            var orcamentoOpcaoRetorno = _mapper.Map<OrcamentoOpcaoResponseViewModel>(orcamento);
            return orcamentoOpcaoRetorno;
        }

        [HttpPost]
        public async Task<IActionResult> Post(OrcamentoOpcaoRequestViewModel model)
        {
            var user = User.Identity.Name;
            var tipoUsuario = Int32.Parse(User.Claims.FirstOrDefault(x => x.Type == "TipoUsuario").Value);

            _logger.LogInformation("Inserindo opção orçamento");
            var error = new CustomError();

            var orcamentoOpcao = _mapper.Map<TorcamentoCotacaoOpcao>(model);
            var orcamento = _orcamentoOpcaoBll.Inserir(orcamentoOpcao);//, user, (TipoUsuario)tipoUsuario, error);
            if (orcamento == null)
                return BadRequest(error);

            return Ok(orcamento);
        }

        [HttpPut]
        public async Task<OrcamentoOpcaoResponseViewModel> Put(OrcamentoOpcaoRequestViewModel model)
        {
            _logger.LogInformation("Alterando opção orçamento");
            var orcamentoOpcao = _mapper.Map<TorcamentoCotacaoOpcao>(model);
            var orcamento = _orcamentoOpcaoBll.Atualizar(orcamentoOpcao);
            var orcamentoOpcaoRetorno = _mapper.Map<OrcamentoOpcaoResponseViewModel>(orcamento);
            return orcamentoOpcaoRetorno;
        }

    }
}
