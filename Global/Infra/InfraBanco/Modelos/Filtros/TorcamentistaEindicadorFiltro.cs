using Interfaces;

namespace InfraBanco.Modelos.Filtros
{
    public class TorcamentistaEindicadorFiltro : IFilter
    {
        public int idParceiro { get; set; }
        public string apelido { get; set; }
        public string datastamp { get; set; }
        public string vendedorId { get; set; }
        public string vendedor { get; set; }
        public string loja { get; set; }
        public int acessoHabilitado { get; set; }
        public string status { get; set; }
    }
}
    