using Newtonsoft.Json;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class OrcamentoOpcaoRequestViewModel : IViewModelRequest
    {
        [JsonProperty("listaProdutos")]
        public List<ProdutoRequestViewModel> ListaProdutos { get; set; }

        [JsonProperty("vlTotal")]
        public decimal VlTotal { get; set; }

        [JsonProperty("formaPagto")]
        public List<FormaPagtoCriacaoRequestViewModel> FormaPagto { get; set; }

        [JsonProperty("percRT")]
        public float PercRT { get; set; }


    }
}
