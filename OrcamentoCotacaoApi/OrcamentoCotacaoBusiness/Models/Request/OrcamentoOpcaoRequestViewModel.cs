using Newtonsoft.Json;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class OrcamentoOpcaoRequestViewModel: IViewModelRequest
    {
        [JsonProperty("idOrcamento")]
        public int IdOrcamentoCotacao { get; set; }

        [JsonProperty("listaProdutos")]
        public List<ProdutoRequestViewModel> ListaProdutos { get; set; }

        [JsonProperty("VlTotal")]
        public decimal VlTotal { get; set; }

        [JsonProperty("ValorTotalComRA")]
        public decimal ValorTotalComRA { get; set; }

        [JsonProperty("observacoes")]
        public string Observacoes { get; set; }

        [JsonProperty("formaPagto")]
        public List<FormaPagtoRequestViewModel> FormaPagto { get; set; }
    }
}
