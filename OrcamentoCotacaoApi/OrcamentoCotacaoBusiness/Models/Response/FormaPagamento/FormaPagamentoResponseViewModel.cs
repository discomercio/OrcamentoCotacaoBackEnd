using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrcamentoCotacaoBusiness.Models.Response.FormaPagamento
{
    public class FormaPagamentoResponseViewModel
    {
        [JsonProperty("idTipoPagamento")]
        public int IdTipoPagamento { get; set; }

        [JsonProperty("Meios")]
        public List<MeiosPagamento.MeioPagamentoResponseViewModel> MeiosPagamentos { get; set; }
    }


}
