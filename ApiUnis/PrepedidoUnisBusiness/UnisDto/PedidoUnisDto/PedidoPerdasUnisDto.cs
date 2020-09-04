using Pedido.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class PedidoPerdasUnisDto
    {
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public decimal Valor { get; set; }
        public string Obs { get; set; }


        public static List<PedidoPerdasUnisDto> ListaPedidoPerdasUnisDto_De_PedidoPerdasPedidoDados(IEnumerable<PedidoPerdasPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PedidoPerdasUnisDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PedidoPerdasUnisDto_De_PedidoPerdasPedidoDados(p));
            return ret;
        }
        public static PedidoPerdasUnisDto PedidoPerdasUnisDto_De_PedidoPerdasPedidoDados(PedidoPerdasPedidoDados origem)
        {
            if (origem == null) return null;
            return new PedidoPerdasUnisDto()
            {
                Data = origem.Data,
                Hora = origem.Hora,
                Valor = origem.Valor,
                Obs = origem.Obs
            };
        }
    }
}
