using Newtonsoft.Json;
using OrcamentoCotacaoBusiness.Models.Request.Orcamento;
using System;
using System.Collections.Generic;
using System.Text;
using UtilsGlobais.RequestResponse;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public class AtualizarOrcamentoOpcaoRequest: RequestBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("idOrcamentoCotacao")]
        public int IdOrcamentoCotacao { get; set; }

        [JsonProperty("sequencia")]
        public int Sequencia { get; set; }

        [JsonProperty("listaProdutos")]
        public List<AtualizarOrcamentoOpcaoProdutoRequest> ListaProdutos { get; set; }

        [JsonProperty("vlTotal")]
        public decimal VlTotal { get; set; }

        [JsonProperty("formaPagto")]
        public List<CadastroOrcamentoOpcaoFormaPagtoRequest> FormaPagto { get; set; }

        [JsonProperty("percRT")]
        public float PercRT { get; set; }

        [JsonProperty("loja")]
        public string Loja { get; set; }
    }
}
