using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArclubePrepedidosWebapi.Dtos.Prepedido
{
    public class PrepedidoQueViraramPedidoDtoPedido
    {
        public string NumeroPrePedido { get; set; }
        public DateTime DataOrcamento { get; set; }
        public string NumeroPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public string NomeCliente { get; set; }
        public decimal ValorTotal { get; set; }
        public string Status { get; set; }
    }
}