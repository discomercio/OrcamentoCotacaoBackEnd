using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoCompostoResponseViewModel
    {
        [JsonProperty("paiFabricante")]
        public string PaiFabricante { get; set; }

        [JsonProperty("paiFabricanteNome")]
        public string PaiFabricanteNome { get; set; }

        [JsonProperty("paiProduto")]
        public string PaiProduto { get; set; }

        [JsonProperty("paiDescricao")]
        public string PaiDescricao { get; set; }

        [JsonProperty("paiPrecoTotal")]
        public decimal? PaiPrecoTotal { get; set; }
        [JsonProperty("paiPrecoTotalBase")]
        public decimal? PaiPrecoTotalBase { get; set; }

        [JsonProperty("filhos")]
        public List<ProdutoCompostoFilhosResponseViewModel> Filhos { get; set; }

        internal static ProdutoCompostoResponseViewModel ConverterProdutoCompostoDados(Produto.Dados.ProdutoCompostoDados produto)
        {
            return new ProdutoCompostoResponseViewModel()
            {
                PaiFabricante = produto.PaiFabricante,
                PaiFabricanteNome = produto.PaiFabricanteNome,
                PaiProduto = produto.PaiProduto,
                PaiDescricao = produto.PaiDescricao,
                PaiPrecoTotal = Decimal.Parse(produto?.PaiPrecoTotal.ToString("N")),
                PaiPrecoTotalBase = Decimal.Parse(produto.PaiPrecoTotalBase.ToString("N")),
                Filhos = new List<ProdutoCompostoFilhosResponseViewModel>()
            };
        }
    }
}
