using Interfaces;
using System.Collections.Generic;

namespace InfraBanco.Modelos.Filtros
{
    public class TlojaFiltro : IFilter
    {
        public string Loja { get; set; }

        public List<string> Lojas  { get; set; }   
    }
}
