using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using OrcamentoCotacao.Dto;
using System;
using System.Linq;

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

        public OrcamentoCotacaoConsultaDto ConsultaOrcamento(TorcamentoFiltro filtro)
        {
            return _data.ConsultaOrcamento(filtro);
        }
    }
}