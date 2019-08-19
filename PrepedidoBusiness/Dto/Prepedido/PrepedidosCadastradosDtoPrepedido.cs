using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dtos.Prepedido
{
    public class PrepedidosCadastradosDtoPrepedido
    {
        public string Status { get; set; }
        public string NumeroPrepedido { get; set; }
        public DateTime? DataPrePedido { get; set; }
        public string NomeCliente { get; set; }
        public decimal? ValoTotal { get; set; }
    }
}