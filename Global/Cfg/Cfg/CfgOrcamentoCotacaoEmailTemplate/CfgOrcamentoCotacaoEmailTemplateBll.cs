using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cfg.CfgOrcamentoCotacaoEmailTemplate
{
    public class CfgOrcamentoCotacaoEmailTemplateBll : BaseBLL<TcfgOrcamentoCotacaoEmailTemplate, TcfgOrcamentoCotacaoEmailTemplateFiltro>
    {
        private CfgOrcamentoCotacaoEmailTemplateData _data { get; set; }

        public CfgOrcamentoCotacaoEmailTemplateBll(ContextoBdProvider contextoBdProvider) : base(new CfgOrcamentoCotacaoEmailTemplateData(contextoBdProvider))
        {
            _data = new CfgOrcamentoCotacaoEmailTemplateData(contextoBdProvider);
        }
    }
}

