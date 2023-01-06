namespace ArClube.Mensageria
{
    public sealed class UnidadeNegocioParametroDto
    {
        public int Id { get; set; }
        public int? IdCfgUnidadeNegocio { get; set; }
        public int IdCfgParametro { get; set; }
        public string Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataHoraCadastro { get; set; }
        public DateTime? DataHoraUltAtualizacao { get; set; }
    }
}