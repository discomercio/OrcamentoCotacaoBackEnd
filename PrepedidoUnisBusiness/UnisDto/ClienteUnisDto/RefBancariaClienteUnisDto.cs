using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class RefBancariaClienteUnisDto
    {
        [Required]
        [MaxLength(4)]
        public string Banco { get; set; }

        [Required]
        [MaxLength(8)]
        public string Agencia { get; set; }

        [Required]
        [MaxLength(12)]
        public string Conta { get; set; }

        [MaxLength(2)]
        public string Ddd { get; set; }

        [MaxLength(9)]
        public string Telefone { get; set; }

        [MaxLength(40)]
        public string Contato { get; set; }

        public int Ordem { get; set; }
    }
}
