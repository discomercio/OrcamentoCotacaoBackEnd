using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using PrepedidoBusiness.Dto.ClienteCadastro;
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

        [Required]
        public DadosClienteCadastroUnisDto DadosCliente { get; set; }
        /// <summary>
        /// RefBancaria: somente para PJ, máximo 1
        /// </summary>
        public List<RefBancariaClienteUnisDto> RefBancaria { get; set; }
        /// <summary>
        /// RefComercial: somente para PJ, máximo 3
        /// </summary>
        public List<RefComercialClienteUnisDto> RefComercial { get; set; }

        public static ClienteCadastroDto ClienteCadastroDtoDeClienteCadastroUnisDto(ClienteCadastroUnisDto clienteUnis, string loja)
        {
            ClienteCadastroDto clienteArclube = new ClienteCadastroDto();
            clienteArclube.DadosCliente = DadosClienteCadastroUnisDto.DadosClienteCadastroDtoDeDadosClienteCadastroUnisDto(clienteUnis.DadosCliente, loja);
            clienteArclube.RefBancaria = new List<RefBancariaDtoCliente>();
            if (clienteUnis.RefBancaria != null)
                clienteUnis.RefBancaria.ForEach(x =>
                {
                    clienteArclube.RefBancaria.Add(
                        RefBancariaClienteUnisDto.RefBancariaClienteDtoDeRefBancariaClienteUnisDto(x));
                });

            clienteArclube.RefComercial = new List<RefComercialDtoCliente>();
            if (clienteUnis.RefComercial != null)
                clienteUnis.RefComercial.ForEach(x =>
            {
                clienteArclube.RefComercial.Add(
                    RefComercialClienteUnisDto.RefComercialDtoClienteDeRefComercialClienteUnisDto(x));
            });

            return clienteArclube;
        }


    }
}
