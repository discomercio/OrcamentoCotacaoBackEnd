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
        public int Ordem { get; set; }


        public static RefComercialDtoCliente RefComercialDtoClienteDeRefComercialClienteUnisDto(RefComercialClienteUnisDto refCoemrcial)
        {
            var ret = new RefComercialDtoCliente()
            {
                Contato = refCoemrcial.Contato,
                Ddd = refCoemrcial.Ddd,
                Telefone = refCoemrcial.Telefone,
                Nome_Empresa = refCoemrcial.Nome_Empresa,
                Ordem = refCoemrcial.Ordem
            };

            return ret;
        }
    }
}
