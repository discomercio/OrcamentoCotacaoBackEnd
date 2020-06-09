using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class RefBancariaClienteUnisDto
    {
        public string Banco { get; set; }
        public string BancoDescricao { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string Ddd { get; set; }
        public string Telefone { get; set; }
        public string Contato { get; set; }
        public int Ordem { get; set; }
    }
}
