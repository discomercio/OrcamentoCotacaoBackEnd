using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dto.Prepedido
{
    public class PrepedidoQueViraramPedidoDtoPedido
    {
        public string NumeroPrePedido { get; set; }
        public DateTime? DataOrcamento { get; set; }
        public string NumeroPedido { get; set; }
        public DateTime? DataPedido { get; set; }
        public string NomeCliente { get; set; }
        public decimal Vl_Total_Familia { get; set; }
        public string St_Entrega { get; set; }
    }
}