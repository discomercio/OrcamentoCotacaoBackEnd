using ClassesBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TusuarioFiltro : IFilter
    {
        public string id;

        public string usuario { get; set; }
        public string senha { get; set; }
        public bool? bloqueado { get; set; }
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
    }
}
