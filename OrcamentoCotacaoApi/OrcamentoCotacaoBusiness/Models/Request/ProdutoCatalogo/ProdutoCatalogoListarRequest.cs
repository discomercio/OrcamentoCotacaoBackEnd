namespace OrcamentoCotacaoBusiness.Models.Request.ProdutoCatalogo
{
    public sealed class ProdutoCatalogoListarRequest
    {
        public string[] FabricantesSelecionados { get; set; }
        public string CodAlfaNumFabricanteSelecionado { get; set; }
        public string DescargaCondensadoraSelecionado { get; set; }
        public string[] VoltagemSelecionadas { get; set; }
        public string[] CapacidadeSelecionadas { get; set; }
        public string CicloSelecionado { get; set; }
        public string[] TipoUnidadeSelecionado { get; set; }
        public bool? ImagemSelecionado { get; set; }
        public bool? AtivoSelecionado { get; set; }
        public int Pagina { get; set; }
        public int QtdeItensPorPagina { get; set; }
        public bool OrdenacaoAscendente { get; set; }
        public string NomeColunaOrdenacao { get; set; }
    }
}