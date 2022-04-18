using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoOpcaoItemUnificado
{
    public class OrcamentoCotacaoOpcaoItemUnificadoBll: BaseBLL<TorcamentoCotacaoItemUnificado, TorcamentoCotacaoOpcaoItemUnificadoFiltro>
    {
        private OrcamentoCotacaoOpcaoItemUnificadoData _data { get; set; }

        public OrcamentoCotacaoOpcaoItemUnificadoBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentoCotacaoOpcaoItemUnificadoData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoOpcaoItemUnificadoData(contextoBdProvider);
        }
    }
}
