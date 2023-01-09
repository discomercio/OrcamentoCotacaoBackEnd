using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TcfgUnidadeNegocioFiltro : IFilter
    {
        public string Sigla { get; set; }
        public string NomeCurto { get; set; }
    }
}