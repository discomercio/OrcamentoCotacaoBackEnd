using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoEmail
{
    public class OrcamentoCotacaoEmailBll : BaseBLL<TorcamentoCotacaoEmail, TorcamentoCotacaoEmailFiltro>
    {
        private OrcamentoCotacaoEmailData _data { get; set; }

        public OrcamentoCotacaoEmailBll(ContextoBdProvider contextoBdProvider) : base(new OrcamentoCotacaoEmailData(contextoBdProvider))
        {
            _data = new OrcamentoCotacaoEmailData(contextoBdProvider);
        }
    }
}
