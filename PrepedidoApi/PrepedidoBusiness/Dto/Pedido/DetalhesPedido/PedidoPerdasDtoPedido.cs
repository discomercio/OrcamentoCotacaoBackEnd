using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
{
    public class PedidoPerdasDtoPedido
    {
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public decimal Valor { get; set; }
        public string Obs { get; set; }
    }
}
