using Pedido.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
{
    public class PedidoProdutosDtoPedido
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
        public decimal Preco_Venda { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal? VlTotalItemComRA { get; set; }
        public float? Comissao { get; set; }


        public static List<PedidoProdutosDtoPedido> ListaPedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(IEnumerable<PedidoProdutosPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PedidoProdutosDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(p));
            return ret;
        }
        public static PedidoProdutosDtoPedido PedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(PedidoProdutosPedidoDados origem)
        {
            if (origem == null) return null;
            return new PedidoProdutosDtoPedido()
            {
                Fabricante = origem.Fabricante,
                Produto = origem.Produto,
                Descricao = origem.Descricao,
                Qtde = origem.Qtde,
                Faltando = origem.Faltando,
                CorFaltante = origem.CorFaltante,
                Preco_NF = origem.Preco_NF,
                Preco_Lista = origem.Preco_Lista,
                Desc_Dado = origem.Desc_Dado,
                Preco_Venda = origem.Preco_Venda ?? 0m,
                VlTotalItem = origem.VlTotalItem,
                VlTotalItemComRA = origem.VlTotalItemComRA,
                Comissao = origem.Comissao
            };
        }
    }
}

