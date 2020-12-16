using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dados.DetalhesPrepedido
{
    public class BuscarStatusPrepedidoRetornoDados
    {
        public bool? St_orc_virou_pedido { get; set; }
        public List<StatusPedidoPrepedidoDados> Pedidos { get; set; }
    }
}
