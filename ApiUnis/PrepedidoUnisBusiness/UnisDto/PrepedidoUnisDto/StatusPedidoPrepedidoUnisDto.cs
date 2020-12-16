using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto
{
    public class StatusPedidoPrepedidoUnisDto
    {
        /// <summary>
        /// Número do pedido
        /// </summary>
        public string Pedido { get; set; }

        /// <summary>
        /// St_Entrega: 
        ///     ST_ENTREGA_ESPERAR = "ESP",
        ///     ST_ENTREGA_SPLIT_POSSIVEL = "SPL",
        ///     ST_ENTREGA_SEPARAR = "SEP",
        ///     ST_ENTREGA_A_ENTREGAR = "AET",
        ///     ST_ENTREGA_ENTREGUE = "ETG",
        ///     ST_ENTREGA_CANCELADO = "CAN"
        /// </summary>
        public string St_Entrega{ get; set; }
    }
}
