using Newtonsoft.Json;
using System;

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

        [JsonProperty("tipoParcela")]
        public string TipoParcela { get; set; }

        [JsonProperty("qtdeParcelas")]
        public short QtdeParcelas { get; set; }

        [JsonProperty("dataRefCoeficiente")]
        public DateTime DataRefCoeficiente { get; set; }

         
    }
}
