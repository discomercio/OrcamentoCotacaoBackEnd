using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class PrePedidoResultadoUnisDto
    {
        public string IdPrePedidoCadastrado { get; set; }
        public List<string> ListaErros { get; set; } = new List<string>();
    }
}
