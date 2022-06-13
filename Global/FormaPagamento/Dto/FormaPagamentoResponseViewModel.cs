using Newtonsoft.Json;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento.MeiosPagamento;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response.FormaPagamento
{
    public class FormaPagamentoResponseViewModel
    {
        [JsonProperty("idTipoPagamento")]
        public int IdTipoPagamento { get; set; }

        [JsonProperty("tipoPagamentoDescricao")]
        public string TipoPagamentoDescricao { get; set; }

        [JsonProperty("meios")]
        public List<MeioPagamentoResponseViewModel> MeiosPagamentos { get; set; }
    }
}
