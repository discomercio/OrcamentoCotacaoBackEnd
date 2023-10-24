using Interfaces;
using System.Collections.Generic;

namespace InfraBanco.Modelos.Filtros
{
    public class TcfgUnidadeNegocioFiltro : IFilter
    {
        public string Sigla { get; set; }
        public string NomeCurto { get; set; }

        public List<string> Siglas { get; set; }
    }
}