using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoOpcaoPagto
{
    public class OrcamentoCotacaoOpcaoPagtoBll:BaseBLL<TorcamentoCotacaoOpcaoPagto, TorcamentoCotacaoOpcaoPagtoFiltro>
    {
        private OrcamentoCotacaoOpcaoPagtoData _data { get; set; }

        public OrcamentoCotacaoOpcaoPagtoBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentoCotacaoOpcaoPagtoData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoOpcaoPagtoData(contextoBdProvider);
        }
    }
}
