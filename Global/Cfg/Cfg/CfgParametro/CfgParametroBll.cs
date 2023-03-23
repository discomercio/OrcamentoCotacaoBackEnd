using Cfg.CfgOperacao;
using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfg.CfgParametro
{
    public class CfgParametroBll : BaseBLL<TcfgParametro, TcfgParametroFiltro>
    {
        public CfgParametroData _data { get; set; }

        public CfgParametroBll(ContextoBdProvider contextoBdProvider) : base(new CfgParametroData(contextoBdProvider))
        {
            _data = new CfgParametroData(contextoBdProvider);
        }
    }
}
