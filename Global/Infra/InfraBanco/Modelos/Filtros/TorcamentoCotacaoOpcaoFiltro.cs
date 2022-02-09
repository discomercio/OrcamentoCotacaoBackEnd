using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoCotacaoOpcaoFiltro : IFilter
    {
        public int RecordsPerPage { get; set; }
        public int Page { get; set; }
    }
}
