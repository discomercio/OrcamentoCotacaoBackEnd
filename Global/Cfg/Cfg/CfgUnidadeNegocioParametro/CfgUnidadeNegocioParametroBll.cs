using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cfg.CfgUnidadeNegocioParametro
{
    public class CfgUnidadeNegocioParametroBll : BaseBLL<TcfgUnidadeNegocioParametro, TcfgUnidadeNegocioParametroFiltro>
    {
        private CfgUnidadeNegocioParametroData _data { get; set; }

        public CfgUnidadeNegocioParametroBll(ContextoBdProvider contextoBdProvider) : base(new CfgUnidadeNegocioParametroData(contextoBdProvider))
        {
            _data = new CfgUnidadeNegocioParametroData(contextoBdProvider);
        }
    }
}
