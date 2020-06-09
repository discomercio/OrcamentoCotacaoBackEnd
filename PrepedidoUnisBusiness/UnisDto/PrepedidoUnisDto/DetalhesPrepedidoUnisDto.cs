using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class DetalhesPrePedidoUnisDto
    {
        public string EntregaImediata { get; set; }
        public DateTime EntregaImediataData { get; set; }
        public string BemDeUso_Consumo { get; set; }
        public string InstaladorInstala { get; set; }
        public string DescricaoFormaPagamento { get; set; }
    }
}
