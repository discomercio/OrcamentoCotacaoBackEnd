using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.UI.Models.Pedido
{
    public class UltimosPedidosViewModel
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
