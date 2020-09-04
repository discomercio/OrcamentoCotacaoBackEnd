using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto
{
    public class BuscarStatusPrepedidoRetornoUnisDto
    {
        /// <summary>
        /// Flag para informar se virou pedido
        /// </summary>
        public bool St_orc_virou_pedido { get; set; }

        public List<StatusPedidoPrepedidoUnisDto> Pedidos { get; set; }
    }
}
