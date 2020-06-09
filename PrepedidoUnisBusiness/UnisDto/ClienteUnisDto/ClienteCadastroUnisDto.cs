using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class ClienteCadastroUnisDto
    {
        public string TokenAcesso { get; set; }
        public DadosClienteCadastroUnisDto DadosCliente { get; set; }
        public List<RefBancariaClienteUnisDto> RefBancaria { get; set; }
        public List<RefComercialClienteUnisDto> RefComercial { get; set; }
    }
}
