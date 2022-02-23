using Newtonsoft.Json;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public partial class ProdutosRequestViewModel
    {
        [JsonProperty("loja")]
        public string Loja { get; set; }

        [JsonProperty("uf")]
        public string UF { get; set; }

        [JsonProperty("tipoCliente")]
        public string TipoCliente { get; set; }
    }
}
