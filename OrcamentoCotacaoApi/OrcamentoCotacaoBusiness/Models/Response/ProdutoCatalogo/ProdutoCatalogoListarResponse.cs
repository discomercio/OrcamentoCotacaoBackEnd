using Newtonsoft.Json;

namespace OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo
{
    public sealed class ProdutoCatalogoListarResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("codigoFabricante")]
        public string CodigoFabricante { get; set; }

        [JsonProperty("fabricante")]
        public string Fabricante { get; set; }

        [JsonProperty("codAlfanumericoFabricante")]
        public string CodAlfanumericoFabricante { get; set; }

        [JsonProperty("descricaoCompleta")]
        public string DescricaoCompleta { get; set; }

        [JsonProperty("idCapacidade")]
        public int? IdCapacidade { get; set; }

        [JsonProperty("capacidade")]
        public string Capacidade { get; set; }

        [JsonProperty("idCiclo")]
        public int? IdCiclo { get; set; }

        [JsonProperty("ciclo")]
        public string Ciclo { get; set; }

        [JsonProperty("idTipoUnidade")]
        public int? IdTipoUnidade { get; set; }

        [JsonProperty("tipoUnidade")]
        public string TipoUnidade { get; set; }

        [JsonProperty("idDescargaCondensadora")]
        public int? IdDescargaCondensadora { get; set; }

        [JsonProperty("descargaCondensadora")]
        public string DescargaCondensadora { get; set; }

        [JsonProperty("idVoltagem")]
        public int? IdVoltagem { get; set; }

        [JsonProperty("voltagem")]
        public string Voltagem { get; set; }

        [JsonProperty("imagem")]
        public bool Imagem { get; set; }

        [JsonProperty("ativo")]
        public bool Ativo { get; set; }
    }
}