using System;
using System.Collections.Generic;
using System.Text;
using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class StatusPedidoDtoPedido
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


        public static StatusPedidoDtoPedido StatusPedidoDtoPedido_De_StatusPedidoPedidoDados(StatusPedidoPedidoDados statusPedidoDados)
        {
            StatusPedidoDtoPedido ret = new StatusPedidoDtoPedido();

            ret.Status = statusPedidoDados.Status;
            ret.St_Entrega = statusPedidoDados.St_Entrega;
            ret.Entregue_Data = statusPedidoDados.Entregue_Data;
            ret.CorEntrega = statusPedidoDados.CorEntrega;
            ret.Cancelado_Data = statusPedidoDados.Cancelado_Data;
            ret.Pedido_Data = statusPedidoDados.Pedido_Data;
            ret.Pedido_Hora = statusPedidoDados.Pedido_Hora;
            ret.Recebida_Data = statusPedidoDados.Recebida_Data;
            ret.PedidoRecebidoStatus = statusPedidoDados.PedidoRecebidoStatus;
            ret.Marketplace_Codigo_Origem = statusPedidoDados.Marketplace_Codigo_Origem;
            ret.Descricao_Pedido_Bs_X_Marketplace = statusPedidoDados.Descricao_Pedido_Bs_X_Marketplace;
            ret.Pedido_Bs_X_Marketplace = statusPedidoDados.Pedido_Bs_X_Marketplace;
            ret.Cor_Pedido_Bs_X_Marketplace = statusPedidoDados.Cor_Pedido_Bs_X_Marketplace;
            ret.Pedido_Bs_X_Ac = statusPedidoDados.Pedido_Bs_X_Ac;
            ret.Cor_Pedido_Bs_X_Ac = statusPedidoDados.Cor_Pedido_Bs_X_Ac;

            return ret;
        }
    }
}










