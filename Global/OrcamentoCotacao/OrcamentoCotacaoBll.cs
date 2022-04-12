using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacao
{
    public class OrcamentoCotacaoBll:BaseBLL<TorcamentoCotacao, TorcamentoCotacaoFiltro>
    {
        private OrcamentoCotacaoData _data { get; set; }

        public OrcamentoCotacaoBll(ContextoBdProvider contextoBdProvider):base(new OrcamentoCotacaoData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoData(contextoBdProvider);
        }
    }
}
