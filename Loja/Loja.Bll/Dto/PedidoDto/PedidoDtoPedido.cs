using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto
{
    public class PedidoDtoPedido
    {
        public string NumeroPedido { get; set; }
        public DateTime? DataPedido { get; set; }
        public string NomeCliente { get; set; }
        public decimal? ValorTotal { get; set; }
        public string Status { get; set; }
    }
}
