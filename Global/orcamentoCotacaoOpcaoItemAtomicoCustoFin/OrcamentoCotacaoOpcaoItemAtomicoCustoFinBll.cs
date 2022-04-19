using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoOpcaoItemAtomicoCustoFin
{
    public class OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll:BaseBLL<TorcamentoCotacaoOpcaoItemAtomicoCustoFin, TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro>
    {
        private OrcamentoCotacaoOpcaoItemAtomicoCustoFinData _data { get; set; }

        public OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentoCotacaoOpcaoItemAtomicoCustoFinData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoOpcaoItemAtomicoCustoFinData(contextoBdProvider);
        }
    }
}
