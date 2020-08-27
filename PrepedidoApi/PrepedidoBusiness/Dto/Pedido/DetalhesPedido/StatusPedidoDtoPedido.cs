using Pedido.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
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

        public static StatusPedidoDtoPedido StatusPedidoDtoPedido_De_StatusPedidoPedidoDados(StatusPedidoPedidoDados origem)
        {
            if (origem == null) return null;
            return new StatusPedidoDtoPedido()
            {
                Status = origem.Status,
                St_Entrega = origem.St_Entrega,
                Entregue_Data = origem.Entregue_Data,
                CorEntrega = origem.CorEntrega,
                Cancelado_Data = origem.Cancelado_Data,
                Pedido_Data = origem.Pedido_Data,
                Pedido_Hora = origem.Pedido_Hora,
                Recebida_Data = origem.Recebida_Data,
                PedidoRecebidoStatus = origem.PedidoRecebidoStatus,
                Marketplace_Codigo_Origem = origem.Marketplace_Codigo_Origem,
                Descricao_Pedido_Bs_X_Marketplace = origem.Descricao_Pedido_Bs_X_Marketplace,
                Pedido_Bs_X_Marketplace = origem.Pedido_Bs_X_Marketplace,
                Cor_Pedido_Bs_X_Marketplace = origem.Cor_Pedido_Bs_X_Marketplace,
                Pedido_Bs_X_Ac = origem.Pedido_Bs_X_Ac,
                Cor_Pedido_Bs_X_Ac = origem.Cor_Pedido_Bs_X_Ac
            };
        }
    }
}
