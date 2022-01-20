using Newtonsoft.Json;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class FormaPagtoRequestViewModel : IViewModelRequest
    {
        [JsonProperty("codigo")]
        public long Codigo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("qtdeParcelas")]
        public long QtdeParcelas { get; set; }

        [JsonProperty("valores")]
        public List<decimal> Valores { get; set; }
    }
}
