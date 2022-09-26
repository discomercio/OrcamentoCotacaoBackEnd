using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using OrcamentoCotacao.Dto;

namespace OrcamentoCotacao
{
    public class OrcamentoCotacaoBll:BaseBLL<TorcamentoCotacao, TorcamentoCotacaoFiltro>
    {
        private OrcamentoCotacaoData _data { get; set; }

        public OrcamentoCotacaoBll(ContextoBdProvider contextoBdProvider):base(new OrcamentoCotacaoData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoData(contextoBdProvider);
        }

        public OrcamentoCotacaoDto PorGuid(string guid)
        {
            return _data.PorGuid(guid);
        }

        public TcfgOrcamentoCotacaoStatus BuscarStatusParaOrcamentoCotacaoComtransacao(string status, ContextoBdGravacao dbGravacao)
        {
            return _data.BuscarStatusParaOrcamentoCotacaoComtransacao(status, dbGravacao);
        }
    }
}
