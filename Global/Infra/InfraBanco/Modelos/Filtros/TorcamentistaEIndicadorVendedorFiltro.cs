using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentistaEIndicadorVendedorFiltro : IFilter
    {
        public int id { get; set; }
        public string email { get; set; }
        public string datastamp { get; set; }
        public string loja { get; set; }
        public int IdIndicador { get; set; }
        public string nomeVendedor { get; set; }
    }
}
