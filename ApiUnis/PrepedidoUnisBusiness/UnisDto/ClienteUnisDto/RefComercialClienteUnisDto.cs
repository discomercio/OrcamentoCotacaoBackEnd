using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class RefComercialClienteUnisDto
    {
        [MaxLength(60)]
        public string Nome_Empresa { get; set; }
        [MaxLength(40)]
        public string Contato { get; set; }
        [MaxLength(2)]
        public string Ddd { get; set; }
        [MaxLength(9)]
        public string Telefone { get; set; }
        public int Ordem { get; private set; }


        public static RefComercialDtoCliente RefComercialDtoClienteDeRefComercialClienteUnisDto(RefComercialClienteUnisDto refComercial)
        {
            var ret = new RefComercialDtoCliente()
            {
                Contato = refComercial.Contato,
                Ddd = refComercial.Ddd,
                Telefone = refComercial.Telefone,
                Nome_Empresa = refComercial.Nome_Empresa,
                Ordem = refComercial.Ordem
            };

            return ret;
        }

        public static RefComercialClienteUnisDto RefComercialClienteUnisDtoDeRefComercialDtoCliente(RefComercialDtoCliente refComercial)
        {
            var ret = new RefComercialClienteUnisDto()
            {
                Contato = refComercial.Contato,
                Ddd = refComercial.Ddd,
                Telefone = refComercial.Telefone,
                Nome_Empresa = refComercial.Nome_Empresa,
                Ordem = refComercial.Ordem
            };

            return ret;
        }
    }
}
