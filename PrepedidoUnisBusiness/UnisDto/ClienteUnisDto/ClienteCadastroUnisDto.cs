using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class ClienteCadastroUnisDto
    {
        [Required]
        public string TokenAcesso { get; set; }
        public DadosClienteCadastroUnisDto DadosCliente { get; set; }
        public List<RefBancariaClienteUnisDto> RefBancaria { get; set; }
        public List<RefComercialClienteUnisDto> RefComercial { get; set; }
    }
}
