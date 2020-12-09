using Prepedido.Dados.DetalhesPrepedido;
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
        public bool? St_orc_virou_pedido { get; set; }

        public List<StatusPedidoPrepedidoUnisDto> Pedidos { get; set; }

        public static BuscarStatusPrepedidoRetornoUnisDto BuscarStatusPrepedidoRetornoUnisDto_De_BuscarStatusPrepedidoRetornoDados(BuscarStatusPrepedidoRetornoDados retornoDados)
        {
            BuscarStatusPrepedidoRetornoUnisDto ret = new BuscarStatusPrepedidoRetornoUnisDto();

            ret.St_orc_virou_pedido = retornoDados.St_orc_virou_pedido;
            ret.Pedidos = new List<StatusPedidoPrepedidoUnisDto>();

            if (retornoDados.Pedidos?.Count > 0)
            {
                retornoDados.Pedidos.ForEach(x =>
                {
                    ret.Pedidos.Add(new StatusPedidoPrepedidoUnisDto()
                    {
                        Pedido = x.Pedido,
                        St_Entrega = x.St_Entrega
                    });
                });
            }

            return ret;
        }
    }
}
