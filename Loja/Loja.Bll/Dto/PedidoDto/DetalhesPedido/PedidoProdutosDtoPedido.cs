using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class PedidoProdutosDtoPedido
    {
        public string Fabricante { get; set; }
        public string NumProduto { get; set; }
        public string Descricao { get; set; }
        public short? Qtde { get; set; }
        public short? Faltando { get; set; }
        public string CorFaltante { get; set; }
        public decimal? Preco { get; set; }
        public decimal? Preco_Lista { get; set; }
        public decimal VlLista { get; set; }
        public float? Desconto { get; set; }
        public decimal VlUnitario { get; set; }
        public decimal? VlTotalItem { get; set; }
        public decimal? VlTotalItemComRA { get; set; }
        public decimal? VlVenda { get; set; }
        public decimal? VlTotal { get; set; }
        public float? Comissao { get; set; }
        public short? Qtde_estoque_total_disponivel { get; set; }
        public string Alertas { get; set; }

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
                NumProduto = origem.Produto,
                Descricao = origem.Descricao,
                Qtde = origem.Qtde,
                Faltando = origem.Faltando,
                CorFaltante = origem.CorFaltante,
                Preco_Lista = origem.Preco_NF,
                VlLista = origem.Preco_Lista,
                Desconto = origem.Desc_Dado,
                VlVenda = origem.Preco_Venda ?? 0m,
                VlTotalItem = origem.VlTotalItem,
                VlTotalItemComRA = origem.VlTotalItemComRA,
                Comissao = origem.Comissao
            };
        }
    }
}
