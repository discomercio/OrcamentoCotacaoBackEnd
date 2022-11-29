using Newtonsoft.Json;
using System.Collections.Generic;
using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacaoBusiness.Models.Request.Orcamento
{
    public class CadastroOrcamentoOpcaoRequest : RequestBase
    {
        [JsonProperty("listaProdutos")]
        public List<CadastroOrcamentoOpcaoProdutoRequest> ListaProdutos { get; set; }

        [JsonProperty("vlTotal")]
        public decimal VlTotal { get; set; }

        [JsonProperty("formaPagto")]
        public List<CadastroOrcamentoOpcaoFormaPagtoRequest> FormaPagto { get; set; }

        [JsonProperty("percRT")]
        public float PercRT { get; set; }


    }
}
