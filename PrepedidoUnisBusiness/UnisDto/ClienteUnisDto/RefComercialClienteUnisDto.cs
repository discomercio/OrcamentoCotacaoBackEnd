using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto
{
    public class RefComercialClienteUnisDto
    {
        public string Nome_Empresa { get; set; }
        public string Contato { get; set; }
        public string Ddd { get; set; }
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
