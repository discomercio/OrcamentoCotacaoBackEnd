using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class CoeficienteRequestViewModel
    {
        [JsonProperty("lstFabricantes")]
        public List<string> LstFabricantes { get; set; }

        [JsonProperty("tipoParcela")]
        public string TipoParcela { get; set; }

        [JsonProperty("qtdeParcelas")]
        public short QtdeParcelas { get; set; }

        [JsonProperty("dataRefCoeficiente")]
        public DateTime DataRefCoeficiente { get; set; }
    }
}
