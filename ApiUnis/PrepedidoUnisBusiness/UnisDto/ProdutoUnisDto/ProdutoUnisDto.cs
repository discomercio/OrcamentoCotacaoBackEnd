using Produto.Dados;
using System.ComponentModel.DataAnnotations;

namespace PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto
{
    public class ProdutoUnisDto
    {
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [MaxLength(30)]
        public string Fabricante_Nome { get; set; }

        [MaxLength(8)]
        public string Produto { get; set; }

        [MaxLength(4000)]
        public string Descricao_html { get; set; }

        [MaxLength(120)]
        public string Descricao { get; set; }

        public decimal? Preco_lista { get; set; }

        public int Estoque { get; set; }

        [MaxLength(2000)]
        public string Alertas { get; set; }
        public short? Qtde_Max_Venda { get; set; }

        public float? Desc_Max { get; set; }

        internal static ProdutoUnisDto ProdutoUnisDtoDeProdutoDados(ProdutoDados produtoDados)
        {
            var ret = new ProdutoUnisDto()
            {
                Fabricante = produtoDados.Fabricante,
                Fabricante_Nome = produtoDados.Fabricante_Nome,
                Produto = produtoDados.Produto,
                Descricao_html = produtoDados.Descricao_html,
                Descricao = produtoDados.Descricao,
                Preco_lista = produtoDados.Preco_lista,
                Estoque = produtoDados.Estoque,
                Alertas = produtoDados.Alertas,
                Qtde_Max_Venda = produtoDados.Qtde_Max_Venda,
                Desc_Max = produtoDados.Desc_Max
            };
            return ret;
        }
    }
}
