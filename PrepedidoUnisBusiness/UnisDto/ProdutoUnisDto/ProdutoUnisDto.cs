using PrepedidoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

        public static ProdutoUnisDto ProdutoUnisDtoDeProdutoDto(ProdutoDto produtoDto)
        {
            var ret = new ProdutoUnisDto()
            {
                Fabricante = produtoDto.Fabricante,
                Fabricante_Nome = produtoDto.Fabricante_Nome,
                Produto = produtoDto.Produto,
                Descricao_html = produtoDto.Descricao_html,
                Preco_lista = produtoDto.Preco_lista,
                Estoque = produtoDto.Estoque,
                Alertas = produtoDto.Alertas,
                Qtde_Max_Venda = produtoDto.Qtde_Max_Venda
            };
            return ret;
        }

        public float? Desc_Max { get; set; }
    }
}
