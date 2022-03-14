using Newtonsoft.Json;
using System;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoSimplesResponseViewModel : IViewModelResponse
    {
        [JsonProperty("fabricante")]
        public string Fabricante { get; set; }

        [JsonProperty("fabricante_Nome")]
        public string FabricanteNome { get; set; }

        [JsonProperty("produto")]
        public string Produto { get; set; }

        [JsonProperty("descricaoHtml")]
        public string DescricaoHtml { get; set; }

        [JsonProperty("precoLista")]
        public decimal PrecoLista { get; set; }

        [JsonProperty("precoListaBase")]
        public decimal PrecoListaBase { get; set; }

        [JsonProperty("estoque")]
        public int Estoque { get; set; }

        [JsonProperty("alertas")]
        public string Alertas { get; set; }

        [JsonProperty("qtdeMaxVenda")]
        public short? QtdeMaxVenda { get; set; }

        [JsonProperty("descMax")]
        public float? DescMax { get; set; }

        public int? Qtde { get; set; }

        internal static ProdutoSimplesResponseViewModel ConverterProdutoDados(Produto.Dados.ProdutoDados produto, int? qtdeFilho, Produto.Dados.CoeficienteDados coeficienteDados)
        {
            var precoLista = produto.Preco_lista;
            if (coeficienteDados != null)
            {
                precoLista = Convert.ToDecimal(coeficienteDados.Coeficiente) * precoLista.GetValueOrDefault();
            }

            return new ProdutoSimplesResponseViewModel()
            {
                Fabricante = produto.Fabricante,
                FabricanteNome = produto.Fabricante_Nome,
                Produto = produto.Produto,
                Qtde = qtdeFilho.HasValue ? qtdeFilho : null,
                DescricaoHtml = produto.Descricao_html,
                PrecoLista = (decimal)precoLista,
                PrecoListaBase = (decimal)produto.Preco_lista,
                QtdeMaxVenda = produto.Qtde_Max_Venda,
                DescMax = produto.Qtde_Max_Venda,
                Estoque = produto.Estoque,
                Alertas = produto.Alertas
            };
        }

    }
}
