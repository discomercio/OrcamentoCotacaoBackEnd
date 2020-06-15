using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.ClienteUnisDto
{
    public class ClienteBuscaRetornoUnisDto
    {
        public DadosClienteCadastroUnisDto DadosCliente { get; set; }
        public List<RefBancariaClienteUnisDto> RefBancaria { get; set; }
        public List<RefComercialClienteUnisDto> RefComercial { get; set; }
    }
}
