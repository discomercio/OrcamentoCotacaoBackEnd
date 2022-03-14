using System;
using System.Collections.Generic;
using System.Text;

namespace Produto.Dados
{
    public class CoeficienteDados
    {
        public string Fabricante { get; set; }
        public string TipoParcela { get; set; }
        public short QtdeParcelas { get; set; }
        public float Coeficiente { get; set; }
        public DateTime Data { get; set; }
    }
}
