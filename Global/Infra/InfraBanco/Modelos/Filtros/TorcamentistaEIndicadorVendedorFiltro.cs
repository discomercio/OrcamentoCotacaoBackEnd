using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentistaEIndicadorVendedorFiltro : IFilter
    {
        public string email { get; set; }
        public string senha { get; set; }
        public string loja { get; set; }
        public string IdIndicador { get; set; }
        public string Apelido { get; set; }
    }
}
