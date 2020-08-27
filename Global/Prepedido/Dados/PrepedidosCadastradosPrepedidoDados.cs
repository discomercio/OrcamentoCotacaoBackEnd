using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dados
{
    public class PrepedidosCadastradosPrepedidoDados
    {
        public string Status { get; set; }
        public string NumeroPrepedido { get; set; }
        public DateTime? DataPrePedido { get; set; }
        public string NomeCliente { get; set; }
        public decimal? ValoTotal { get; set; }
    }
}
