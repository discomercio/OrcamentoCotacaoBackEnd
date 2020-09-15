using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.DetalhesPedido
{
    public class PedidoProdutosPedidoDados
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
    }
}
