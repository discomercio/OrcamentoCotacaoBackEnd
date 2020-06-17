using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using PrepedidoBusiness.Dtos.ClienteCadastro;
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

        public static ClienteCadastroDto ClienteCadastroDtoDeClienteCadastroUnisDto(ClienteCadastroUnisDto clienteUnis, string loja)
        {
            ClienteCadastroDto clienteArclube = new ClienteCadastroDto();
            clienteArclube.DadosCliente = DadosClienteCadastroUnisDto.DadosClienteCadastroDtoDeDadosClienteCadastroUnisDto(clienteUnis.DadosCliente, loja);
            clienteArclube.RefBancaria = new List<RefBancariaDtoCliente>();
            clienteUnis.RefBancaria.ForEach(x =>
            {
                clienteArclube.RefBancaria.Add(
                    RefBancariaClienteUnisDto.RefBancariaClienteDtoDeRefBancariaClienteUnisDto(x));
            });
            clienteArclube.RefComercial = new List<RefComercialDtoCliente>();
            clienteUnis.RefComercial.ForEach(x =>
            {
                clienteArclube.RefComercial.Add(
                    RefComercialClienteUnisDto.RefComercialDtoClienteDeRefComercialClienteUnisDto(x));
            });

            return clienteArclube;
        }

        
    }
}
