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
        public bool? ativo { get; set; }
        public string[] Parceiros { get; set; }
        public string Parceiro { get; set; }
        public string Pesquisa { get; set; }
        public int Pagina { get; set; }
        public int QtdeItensPagina { get; set; }
        public bool OrdenacaoAscendente { get; set; }
        public string NomeColuna { get; set; }
        public int TipoUsuario { get; set; }
    }
}
