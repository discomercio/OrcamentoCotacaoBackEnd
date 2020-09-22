using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dados.DetalhesPrepedido
{
    public class DetalhesPrepedidoDados
    {
        public string Observacoes { get; set; }
        public string NumeroNF { get; set; }
        public string EntregaImediata { get; set; }
        public DateTime? EntregaImediataData { get; set; }
        public short BemDeUso_Consumo { get; set; }
        public short InstaladorInstala { get; set; }
        public string GarantiaIndicador { get; set; }
        public string FormaDePagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
        public string PrevisaoEntrega { get; set; }//para mostrar finalizado na tela
    }
}
