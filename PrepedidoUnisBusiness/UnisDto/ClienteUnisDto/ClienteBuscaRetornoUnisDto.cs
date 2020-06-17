using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoBusiness.Dtos.ClienteCadastro;
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

        public static ClienteBuscaRetornoUnisDto ClienteCadastroUnisDtoDeClienteCadastroDto(ClienteCadastroDto clienteArclube)
        {
            ClienteBuscaRetornoUnisDto clienteUnis = new ClienteBuscaRetornoUnisDto();
            clienteUnis.DadosCliente = DadosClienteCadastroUnisDto.DadosClienteCadastroUnisDtoDeDadosClienteCadastroDto(clienteArclube.DadosCliente);
            clienteUnis.RefBancaria = new List<RefBancariaClienteUnisDto>();
            clienteArclube.RefBancaria.ForEach(x =>
            {
                clienteUnis.RefBancaria.Add(
                    RefBancariaClienteUnisDto.RefBancariaClienteUnisDtoDeRefBancariaClienteDto(x));
            });
            clienteArclube.RefComercial.ForEach(x =>
            {
                clienteUnis.RefComercial.Add(
                    RefComercialClienteUnisDto.RefComercialClienteUnisDtoDeRefComercialDtoCliente(x));
            });

            return clienteUnis;
        }
    }
}
