using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TcfgUnidadeNegocioParametroFiltro : IFilter
    {
        public int? IdCfgUnidadeNegocio { get; set; }
        public int? IdCfgParametro { get; set; }
    }
}
