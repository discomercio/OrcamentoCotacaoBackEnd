﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.DetalhesPedido
{
    public class PedidoProdutosPedidoDados
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
    }
}