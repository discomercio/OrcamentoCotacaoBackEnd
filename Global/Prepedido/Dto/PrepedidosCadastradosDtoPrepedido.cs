using Prepedido.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prepedido.Dto
{
    public class PrepedidosCadastradosDtoPrepedido
    {
        public string Status { get; set; }
        public string NumeroPrepedido { get; set; }
        public DateTime? DataPrePedido { get; set; }
        public string NomeCliente { get; set; }
        public decimal? ValoTotal { get; set; }

        public static PrepedidosCadastradosDtoPrepedido PrepedidosCadastradosDtoPrepedido_De_PrepedidosCadastradosPrepedidoDados(PrepedidosCadastradosPrepedidoDados origem)
        {
            if (origem == null) return null;
            return new PrepedidosCadastradosDtoPrepedido()
            {
                Status = origem.Status,
                NumeroPrepedido = origem.NumeroPrepedido,
                DataPrePedido = origem.DataPrePedido,
                NomeCliente = origem.NomeCliente,
                ValoTotal = origem.ValoTotal
            };
        }
        public static List<PrepedidosCadastradosDtoPrepedido> ListaPrepedidosCadastradosDtoPrepedido_De_PrepedidosCadastradosPrepedidoDados(IEnumerable<PrepedidosCadastradosPrepedidoDados> origem)
        {
            if (origem == null) return null;
            var ret = new List<PrepedidosCadastradosDtoPrepedido>();
            if (origem != null)
                foreach (var p in origem)
                    ret.Add(PrepedidosCadastradosDtoPrepedido_De_PrepedidosCadastradosPrepedidoDados(p));
            return ret;
        }
    }
}