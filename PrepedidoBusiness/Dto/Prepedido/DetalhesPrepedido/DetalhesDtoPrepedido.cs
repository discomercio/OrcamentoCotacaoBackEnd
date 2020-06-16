using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dtos.Prepedido.DetalhesPrepedido
{
    public class DetalhesDtoPrepedido
    {
        public string Observacoes { get; set; }
        public string NumeroNF { get; set; }
        public string EntregaImediata { get; set; }
        public DateTime? EntregaImediataData { get; set; }
        public string BemDeUso_Consumo { get; set; }
        public string InstaladorInstala { get; set; }
        public string GarantiaIndicador { get; set; }
        public string FormaDePagamento { get; set; }
        public string DescricaoFormaPagamento { get; set; }
    }
}