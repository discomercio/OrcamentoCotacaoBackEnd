using ClassesBase;
using InfraBanco;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfg.CfgModulo
{
    public class CfgModuloBll : BaseBLL<TcfgModulo, TcfgModuloFiltro>
    {
        private CfgModuloData _data { get; set; }

        public CfgModuloBll(ContextoBdProvider contextoBdProvider) : base(new CfgModuloData(contextoBdProvider))
        {
            _data = new CfgModuloData(contextoBdProvider);
        }
    }
}
