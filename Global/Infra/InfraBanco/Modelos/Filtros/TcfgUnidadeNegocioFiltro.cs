using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco.Modelos.Filtros
{
    public class TcfgUnidadeNegocioFiltro : IFilter
    {
        public string Sigla { get; set; }
        public string NomeCurto { get; set; }
    }
}
