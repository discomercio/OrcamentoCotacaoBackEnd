using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto
{
    public class FiltroInfosStatusPrepedidosUnisDto
    {
        public string TokenAcesso { get; set; }
        public List<string> ListaPrepedidos { get; set; }
        public bool VirouPedido { get; set; }
        public bool Pendentes { get; set; }
        public bool Cancelados { get; set; }
    }
}
