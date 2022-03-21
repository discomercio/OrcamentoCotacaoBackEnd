using Newtonsoft.Json;

namespace OrcamentoCotacaoBusiness.Models.Response.FormaPagamento
{
    public class PercMaxDescEComissaoResponseViewModel
    {
        [JsonProperty("percMaxComissao")]
        public float PercMaxComissao { get; set; }

        [JsonProperty("percMaxComissaoEDesconto")]
        public float PercMaxComissaoEDesconto { get; set; }

        [JsonProperty("percMaxComissaoEDescontoPJ")]
        public float PercMaxComissaoEDescontoPJ { get; set; }

        [JsonProperty("percMaxComissaoEDescontoNivel2")]
        public float PercMaxComissaoEDescontoNivel2 { get; set; }

        [JsonProperty("percMaxComissaoEDescontoNivel2PJ")]
        public float PercMaxComissaoEDescontoNivel2PJ { get; set; }
    }
}
