using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;

namespace MeioPagamentos
{
    public class MeiosPagamentosBll : BaseBLL<TcfgPagtoMeioStatus, TcfgPagtoMeioStatusFiltro>
    {
        private MeiosPagamentosData _data { get; set; }
        public MeiosPagamentosBll(ContextoBdProvider contextoBdProvider) : base(new MeiosPagamentosData(contextoBdProvider))
        {
            _data = new MeiosPagamentosData(contextoBdProvider);
        }

        public List<TcfgPagtoMeioStatus> BuscarMeiosPagamento(TcfgPagtoMeioStatusFiltro obj)
        {
            return _data.PorFiltro(obj);
        }
    }
}
