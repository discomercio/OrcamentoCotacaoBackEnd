using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dados.DetalhesPrepedido
{
    public class InformacoesPedidoRetornoDados
    {
        public string Pedido { get; set; }
        public string St_entrega { get; set; }
        public string DescricaoStatusEntrega { get; set; }
        public DateTime Entregue_data { get; set; }
        public DateTime Cancelado_data { get; set; }
        public short PedidoRecebidoStatus { get; set; }
        public DateTime PedidoRecebidoData { get; set; }
        public short Analise_credito { get; set; }
        public string DescricaoAnaliseCredito { get; set; }
        public DateTime Analise_credito_data { get; set; }
        public string St_pagto { get; set; }
        public string DescricaoStatusPagto { get; set; }
    }
}
