using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentoFiltro : IFilter
    {
        public int? Page { get; set; }
        public int? RecordsPerPage { get; set; }
        public string Origem { get; set; }
        public string[] Loja { get; set; }
    }
}
