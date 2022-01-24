using ClassesBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoOpcaoFiltro : IFilter
    {
        public int RecordsPerPage { get; set; }
        public int Page { get; set; }
    }
}
