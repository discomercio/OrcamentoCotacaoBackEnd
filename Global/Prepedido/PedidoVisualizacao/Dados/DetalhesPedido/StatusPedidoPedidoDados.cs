﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.PedidoVisualizacao.Dados.DetalhesPedido
{
    public class StatusPedidoPedidoDados
    {
        public string Status { get; set; }
        public string St_Entrega { get; set; }
        public string Entregue_Data { get; set; }
        public string CorEntrega { get; set; }//montar uma função para definir a cor do status e da data
        public string Cancelado_Data { get; set; }
        public string Pedido_Data { get; set; }
        public string Pedido_Hora { get; set; }
        public string Recebida_Data { get; set; }
        public string PedidoRecebidoStatus { get; set; }
        public string Marketplace_Codigo_Origem { get; set; }
        public string Descricao_Pedido_Bs_X_Marketplace { get; set; }
        public string Pedido_Bs_X_Marketplace { get; set; }
        public string Cor_Pedido_Bs_X_Marketplace { get; set; }
        public string Pedido_Bs_X_Ac { get; set; }
        public string Cor_Pedido_Bs_X_Ac { get; set; }
    }
}
