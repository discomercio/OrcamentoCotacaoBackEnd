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

        [JsonProperty("grupo")]
        public string Grupo { get; set; }

        [JsonProperty("subgrupo")]
        public string Subgrupo { get; set; }

        [JsonProperty("descricaoGrupoSubgrupo")]
        public string DescricaoGrupoSubgrupo { get; set; }

        [JsonProperty("capacidade")]
        public int? Capacidade { get; set; }

        [JsonProperty("ciclo")]
        public string Ciclo { get; set; }

        [JsonProperty("cicloDescricao")]
        public string CicloDescricao { get; set; }

        [JsonProperty("unitarioVendavel")]
        public bool UnitarioVendavel { get; set; } = true;

        internal static ProdutoSimplesResponseViewModel ConverterProdutoDados(Produto.Dados.ProdutoDados produto, int? qtdeFilho,
            CoeficienteResponseViewModel coeficienteDados)
        {
            var precoLista = produto.Preco_lista;
            if (coeficienteDados != null)
            {
                precoLista = Convert.ToDecimal(coeficienteDados.Coeficiente) * precoLista.GetValueOrDefault();
            }

            var retorno = new ProdutoSimplesResponseViewModel();
            retorno.Fabricante = produto.Fabricante;
            retorno.FabricanteNome = produto.Fabricante_Nome;
            retorno.Produto = produto.Produto;
            retorno.Qtde = qtdeFilho.HasValue ? qtdeFilho : null;
            retorno.DescricaoHtml = produto.Descricao_html;
            retorno.PrecoLista = Math.Round((decimal)precoLista, 2, MidpointRounding.AwayFromZero);
            retorno.PrecoListaBase = Math.Round((decimal)produto.Preco_lista, 2, MidpointRounding.AwayFromZero);
            retorno.QtdeMaxVenda = produto.Qtde_Max_Venda;
            retorno.DescMax = produto.Desc_Max;
            retorno.Estoque = produto.Estoque;
            retorno.Alertas = produto.Alertas;
            retorno.Capacidade = produto.Capacidade;
            retorno.Ciclo = produto.Ciclo;
            retorno.CicloDescricao = produto.CicloDescricao;
            retorno.Grupo = produto.Grupo;
            retorno.Subgrupo = produto.SubGrupo;
            if (string.IsNullOrEmpty(produto.Grupo) && string.IsNullOrEmpty(produto.SubGrupo))
            {
                retorno.Grupo = "V";
                retorno.Subgrupo = "V"; 
                retorno.DescricaoGrupoSubgrupo = "Vazio";
            }
            if (!string.IsNullOrEmpty(produto.Grupo) && !string.IsNullOrEmpty(produto.SubGrupo))
            {
                if (produto.Grupo == produto.SubGrupo)
                {
                    retorno.DescricaoGrupoSubgrupo = produto.GrupoDescricao;
                }
                else
                {
                    retorno.DescricaoGrupoSubgrupo = $"{produto.GrupoDescricao} - {produto.SubGrupoDescricao}";
                }
            }
            if (string.IsNullOrEmpty(produto.Grupo) && !string.IsNullOrEmpty(produto.SubGrupo))
            {
                retorno.DescricaoGrupoSubgrupo = produto.SubGrupoDescricao;
            }
            if (!string.IsNullOrEmpty(produto.Grupo) && string.IsNullOrEmpty(produto.SubGrupo))
            {
                retorno.DescricaoGrupoSubgrupo = produto.GrupoDescricao;
            }
            

            return retorno;
        }
    }

}

