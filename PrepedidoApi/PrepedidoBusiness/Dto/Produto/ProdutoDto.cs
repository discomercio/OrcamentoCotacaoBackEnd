using Produto.ProdutoDados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class ProdutoDto
    {
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [MaxLength(30)]
        public string Fabricante_Nome { get; set; }

        [MaxLength(8)]
        public string Produto { get; set; }

        [MaxLength(4000)]
        public string Descricao_html { get; set; }

        public decimal? Preco_lista { get; set; }

        public int Estoque { get; set; }

        [MaxLength(2000)]
        public string Alertas { get; set; }
        public short? Qtde_Max_Venda { get; set; }

        internal static List<ProdutoDto> ProdutoDtoListaDeProdutoDados(List<ProdutoDados> produtoDados)
        {
            var ret = new List<ProdutoDto>();
            if (produtoDados != null)
                foreach (var p in produtoDados)
                    ret.Add(ProdutoDtoDeProdutoDados(p));
            return ret;
        }

        private static ProdutoDto ProdutoDtoDeProdutoDados(ProdutoDados p)
        {
            return new ProdutoDto()
            {

                Fabricante = p.Fabricante,
                Fabricante_Nome = p.Fabricante_Nome,
                Produto = p.Produto,
                Descricao_html = p.Descricao_html,
                Preco_lista = p.Preco_lista,
                Estoque = p.Estoque,
                Alertas = p.Alertas,
                Qtde_Max_Venda = p.Qtde_Max_Venda
            };
        }
    }
}








