using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class PedidoProdutosUnisDto
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; }
        public string Descricao { get; set; }
        public short? Qtde { get; set; }
        public short? Faltando { get; set; }
        public string CorFaltante { get; set; }
        public decimal? Preco_NF { get; set; }
        public decimal Preco_Lista { get; set; }
        public float? Desc_Dado { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal? VlTotalItemComRA { get; set; }
        public decimal? Preco_Venda { get; set; }
        public float? Comissao { get; set; }

        public static List<PedidoProdutosUnisDto> ListaPedidoProdutosUnisDto_De_PedidoProdutosPedidoDados(IEnumerable<PedidoProdutosPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PedidoProdutosUnisDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PedidoProdutosUnisDto_De_PedidoProdutosPedidoDados(p));
            return ret;
        }
        public static PedidoProdutosUnisDto PedidoProdutosUnisDto_De_PedidoProdutosPedidoDados(PedidoProdutosPedidoDados origem)
        {
            if (origem == null) return null;
            return new PedidoProdutosUnisDto()
            {
                Fabricante = origem.Fabricante,
                Produto = origem.Produto,
                Descricao = origem.Produto,
                Qtde = origem.Qtde,
                Faltando = origem.Faltando,
                CorFaltante = origem.CorFaltante,
                Preco_NF = origem.Preco_NF,
                Preco_Lista = origem.Preco_Lista,
                Desc_Dado = origem.Desc_Dado,
                VlTotalItem = origem.VlTotalItem,
                VlTotalItemComRA = origem.VlTotalItemComRA,
                Preco_Venda = origem.Preco_Venda,
                Comissao = origem.Comissao
            };
        }
    }
}

