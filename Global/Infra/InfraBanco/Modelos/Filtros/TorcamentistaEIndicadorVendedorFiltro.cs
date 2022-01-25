using ClassesBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentistaEIndicadorVendedorFiltro : IFilter
    {
        public string email { get; set; }
        public string senha { get; set; }
    }
}
