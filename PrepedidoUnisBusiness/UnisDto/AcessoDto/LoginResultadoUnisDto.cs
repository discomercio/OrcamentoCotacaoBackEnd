using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.AcessoDto
{
    public class LoginResultadoUnisDto
    {
        public string TokenAcesso { get; set; }
        public List<string> ListaErros { get; set; }
    }
}
