using Newtonsoft.Json;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class OrcamentoOpcaoDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idOrcamentoCotacao")]
        public int IdOrcamentoCotacao { get; set; }

        [JsonProperty("sequencia")]
        public int Sequencia { get; set; }

        [JsonProperty("listaProdutos")]
        public List<ProdutoOrcamentoOpcaoResponseViewModel> ListaProdutos { get; set; }

        [JsonProperty("vlTotal")]
        public decimal VlTotal { get; set; }

        [JsonProperty("formaPagto")]
        public List<FormaPagtoDto> FormaPagto { get; set; }

        [JsonProperty("percRT")]
        public float PercRT { get; set; }
    }
}
