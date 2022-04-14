using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoOpcao
{
    public class OrcamentoCotacaoOpcaoBll: BaseBLL<TorcamentoCotacaoOpcao, TorcamentoCotacaoOpcaoFiltro>
    {
        private OrcamentoCotacaoOpcaoData _data { get; set; }

        public OrcamentoCotacaoOpcaoBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentoCotacaoOpcaoData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoOpcaoData(contextoBdProvider);
        }
    }
}
