using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class PedidoPerdasDtoPedido
    {
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public decimal Valor { get; set; }
        public string Obs { get; set; }

        public static List<PedidoPerdasDtoPedido> ListaPedidoPerdasDtoPedido_De_PedidoPerdasPedidoDados(IEnumerable<PedidoPerdasPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<PedidoPerdasDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(PedidoPerdasDtoPedido_De_PedidoPerdasPedidoDados(p));
            return ret;
        }
        public static PedidoPerdasDtoPedido PedidoPerdasDtoPedido_De_PedidoPerdasPedidoDados(PedidoPerdasPedidoDados origem)
        {
            if (origem == null) return null;
            return new PedidoPerdasDtoPedido()
            {
                Data = origem.Data,
                Hora = origem.Hora,
                Valor = origem.Valor,
                Obs = origem.Obs
            };
        }
    }
}
