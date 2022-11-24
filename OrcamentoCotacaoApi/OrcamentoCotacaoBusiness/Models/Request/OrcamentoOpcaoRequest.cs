using Newtonsoft.Json;
using System.Collections.Generic;
using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class OrcamentoOpcaoRequest : RequestBase
    {
        [JsonProperty("listaProdutos")]
        public List<ProdutoRequestViewModel> ListaProdutos { get; set; }

        [JsonProperty("vlTotal")]
        public decimal VlTotal { get; set; }

        [JsonProperty("formaPagto")]
        public List<FormaPagtoCriacaoRequest> FormaPagto { get; set; }

        [JsonProperty("percRT")]
        public float PercRT { get; set; }


    }
}
