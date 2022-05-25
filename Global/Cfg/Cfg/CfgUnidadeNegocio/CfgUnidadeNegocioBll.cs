using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cfg.CfgUnidadeNegocio
{
    public class CfgUnidadeNegocioBll : BaseBLL<TcfgUnidadeNegocio, TcfgUnidadeNegocioFiltro>
    {
        private CfgUnidadeNegocioData _data { get; set; }

        public CfgUnidadeNegocioBll(ContextoBdProvider contextoBdProvider) : base(new CfgUnidadeNegocioData(contextoBdProvider))
        {
            _data = new CfgUnidadeNegocioData(contextoBdProvider);
        }
    }
}
