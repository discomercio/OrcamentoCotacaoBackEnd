using Pedido.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class PedidoProdutosUnisDto
    {
        public string Fabricante { get; set; }
        public string NumProduto { get; set; }
        public string Descricao { get; set; }
        public short? Qtde { get; set; }
        public short? Faltando { get; set; }
        public string CorFaltante { get; set; }
        public decimal? Preco { get; set; }
        public decimal VlLista { get; set; }
        public float? Desconto { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal? VlTotalItemComRA { get; set; }
        public decimal? VlVenda { get; set; }
        public decimal? VlTotal { get; set; }
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
                NumProduto = origem.NumProduto,
                Descricao = origem.Descricao,
                Qtde = origem.Qtde,
                Faltando = origem.Faltando,
                CorFaltante = origem.CorFaltante,
                Preco = origem.Preco,
                VlLista = origem.VlLista,
                Desconto = origem.Desconto,
                VlUnitario = origem.VlUnitario,
                VlTotalItem = origem.VlTotalItem,
                VlTotalItemComRA = origem.VlTotalItemComRA,
                VlVenda = origem.VlVenda,
                VlTotal = origem.VlTotal,
                Comissao = origem.Comissao
            };
        }
    }
}

