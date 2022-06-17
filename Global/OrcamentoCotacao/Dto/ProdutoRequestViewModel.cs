﻿using Newtonsoft.Json;

namespace OrcamentoCotacaoBusiness.Models.Request
{
    public partial class ProdutoRequestViewModel
    {
        [JsonProperty("fabricante")]
        public string Fabricante { get; set; }

        [JsonProperty("fabricanteNome")]
        public string FabricanteNome { get; set; }

        [JsonProperty("produto")]
        public string Produto { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("precoLista")]
        public decimal PrecoLista { get; set; }

        [JsonProperty("precoListaBase")]
        public decimal CustoFinancFornecPrecoListaBase { get; set; }

        [JsonProperty("coeficienteDeCalculo")]
        public float CustoFinancFornecCoeficiente { get; set; }

        [JsonProperty("precoNF")]
        public decimal PrecoNf { get; set; }

        [JsonProperty("descDado")]
        public float DescDado { get; set; }

        [JsonProperty("precoVenda")]
        public decimal PrecoVenda { get; set; }

        [JsonProperty("qtde")]
        public int Qtde { get; set; }

        [JsonProperty("totalItem")]
        public decimal TotalItem { get; set; }

        [JsonProperty("alterouPrecoVenda")]
        public bool AlterouPrecoVenda { get; set; }
        
    }
}