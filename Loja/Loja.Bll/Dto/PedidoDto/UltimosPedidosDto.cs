using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto
{
    public class UltimosPedidosDto
    {
        public DateTime? Data { get; set; }
        public string Pedido { get; set; }
        public string St_Entrega { get; set; }
        public string Vendedor { get; set; }
        public string CnpjCpf { get; set; }
        public string NomeIniciaisEmMaiusculas { get; set; }
        public string AnaliseCredito { get; set; }
        public string AnaliseCreditoPendenteVendasMotivo { get; set; }
    }
}
