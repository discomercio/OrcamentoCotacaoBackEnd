using Prepedido.PedidoVisualizacao.Dados;
using System;
using System.Collections.Generic;

namespace PrepedidoBusiness.Dto.Pedido
{
    public class PedidoDtoPedido
    {
        public string NumeroPedido { get; set; }
        public DateTime? DataPedido { get; set; }
        public string NomeCliente { get; set; }
        public decimal? ValorTotal { get; set; }
        public string Status { get; set; }

        public static PedidoDtoPedido PedidoDtoPedido_De_PedidosPedidoDados(PedidosPedidoDados origem)
        {
            if (origem == null) return null;
            return new PedidoDtoPedido()
            {
                NumeroPedido = origem.NumeroPedido,
                DataPedido = origem.DataPedido,
                NomeCliente = origem.NomeCliente,
                ValorTotal = origem.ValorTotal,
                Status = origem.Status
            };
        }
        public static List<PedidoDtoPedido> ListaPedidoDtoPedido_De_PedidosPedidoDados(IEnumerable<PedidosPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PedidoDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PedidoDtoPedido_De_PedidosPedidoDados(p));
            return ret;
        }

    }
}