using Newtonsoft.Json;
using System;

namespace OrcamentoCotacaoBusiness.Models.Response
{
    public class ProdutoCompostoFilhosResponseViewModel : IViewModelResponse
    {
        [JsonProperty("fabricante")]
        public string Fabricante { get; set; }

        [JsonProperty("fabricante_Nome")]
        public string FabricanteNome { get; set; }

        [JsonProperty("produto")]
        public string Produto { get; set; }

        public int? Qtde { get; set; }

        internal static ProdutoCompostoFilhosResponseViewModel ConverterProdutoFilhoDados(Produto.Dados.ProdutoDados produto, int? qtdeFilho, Produto.Dados.CoeficienteDados coeficienteDados)
        {
            var precoLista = produto.Preco_lista;
            if (coeficienteDados != null)
            {
                precoLista = Convert.ToDecimal(coeficienteDados.Coeficiente) * precoLista.GetValueOrDefault();
            }

            return new ProdutoCompostoFilhosResponseViewModel()
            {
                Fabricante = produto.Fabricante,
                FabricanteNome = produto.Fabricante_Nome,
                Produto = produto.Produto,
                Qtde = qtdeFilho.HasValue ? qtdeFilho : null
            };
        }
    }
}
