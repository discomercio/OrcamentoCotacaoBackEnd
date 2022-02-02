using ClassesBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentistaEindicadorFiltro : IFilter
    {
        public string apelido { get; set; }
        public string datastamp { get; set; }
        public string vendedorId { get; set; }
        public string loja { get; set; }
    }
}
