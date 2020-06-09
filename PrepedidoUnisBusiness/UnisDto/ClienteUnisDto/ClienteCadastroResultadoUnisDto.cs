using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class ClienteCadastroResultadoUnisDto
    {
        public string IdClienteCadastrado { get; set; }
        public List<string> ListaErros { get; set; }
    }
}
