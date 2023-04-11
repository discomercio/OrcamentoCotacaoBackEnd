namespace ProdutoCatalogo.Dto
{
    public sealed class ProdutoCatalogoListarDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string CodigoFabricante { get; set; }
        public string Fabricante { get; set; }
        public string CodAlfanumericoFabricante { get; set; }
        public string DescricaoCompleta { get; set; }
        public int? IdCapacidade { get; set; }
        public string Capacidade { get; set; }
        public int? IdCiclo { get; set; }
        public string Ciclo { get; set; }
        public int? IdTipoUnidade { get; set; }
        public string TipoUnidade { get; set; }
        public int? IdDescargaCondensadora { get; set; }
        public string DescargaCondensadora { get; set; }
        public int? IdVoltagem { get; set; }
        public string Voltagem { get; set; }
        public bool Imagem { get; set; }
        public bool Ativo { get; set; }
    }
}