using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoOpcaoItemAtomico
{
    public class OrcamentoCotacaoOpcaoItemAtomicoBll: BaseBLL<TorcamentoCotacaoOpcaoItemAtomico, TorcamentoCotacaoOpcaoItemAtomicoFiltro>
    {
        private OrcamentoCotacaoOpcaoItemAtomicoData _data { get; set; }

        public OrcamentoCotacaoOpcaoItemAtomicoBll(ContextoBdProvider contextoBdProvider) 
            : base(new OrcamentoCotacaoOpcaoItemAtomicoData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoOpcaoItemAtomicoData(contextoBdProvider);
        }
    }
}
