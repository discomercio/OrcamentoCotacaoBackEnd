using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoBusiness.Dto.ClienteCadastro.Referencias
{
    public class RefComercialDtoCliente
    {
        [Required]
        [MaxLength(60)]
        public string Nome_Empresa { get; set; }

        [MaxLength(40)]
        public string Contato{ get; set; }

        [MaxLength(2)]
        public string Ddd { get; set; }

        [MaxLength(9)]
        public string Telefone { get; set; }


        public int Ordem { get; set; }
    }
}
