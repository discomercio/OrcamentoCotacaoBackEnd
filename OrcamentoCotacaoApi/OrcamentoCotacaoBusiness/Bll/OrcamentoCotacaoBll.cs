using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using Microsoft.Extensions.Options;
using Orcamento;
using Orcamento.Dto;
using OrcamentoCotacaoBusiness.Models.Response;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class OrcamentoCotacaoBll
    {
        private readonly OrcamentoBll _orcamentoBll;
        private readonly ConfigOrcamentoCotacao _appSettings;

        public OrcamentoCotacaoBll(OrcamentoBll orcamentoBll, IOptions<ConfigOrcamentoCotacao> appSettings)
        {
            _orcamentoBll = orcamentoBll;
            _appSettings = appSettings.Value;
        }

        public List<OrcamentoCotacaoListaDto> PorFiltro(TorcamentoFiltro tOrcamentoFiltro)
        {
            return _orcamentoBll.PorFiltro(tOrcamentoFiltro);
        }

        public async Task<List<TorcamentoCotacaoStatus>> ObterListaStatus(TorcamentoFiltro tOrcamentoFiltro)
        {
            return await _orcamentoBll.ObterListaStatus(tOrcamentoFiltro);
        }

        public ValidadeResponseViewModel BuscarConfigValidade()
        {
            return new ValidadeResponseViewModel
            {
                QtdeDiasValidade = _appSettings.QtdeDiasValidade,
                QtdeDiasProrrogacao = _appSettings.QtdeDiasProrrogacao,
                QtdeMaxProrrogacao = _appSettings.QtdeMaxProrrogacao,
                QtdeGlobalValidade = _appSettings.QtdeGlobalValidade,
            };
        }
    }
}
