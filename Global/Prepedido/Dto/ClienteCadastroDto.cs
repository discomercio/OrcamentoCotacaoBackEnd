using InfraBanco.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Prepedido.Dto
{
    public class ClienteCadastroDto
    {
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public List<RefBancariaDtoCliente> RefBancaria { get; set; }
        public List<RefComercialDtoCliente> RefComercial { get; set; }

        public static ClienteCadastroDto ClienteCadastroDto_De_ClienteCadastroDados(Cliente.Dados.ClienteCadastroDados origem)
        {
            if (origem == null) return null;
            return new ClienteCadastroDto()
            {
                DadosCliente = DadosClienteCadastroDto.DadosClienteCadastroDto_De_DadosClienteCadastroDados(origem.DadosCliente),
                RefBancaria = RefBancariaDtoCliente.ListaRefBancariaDtoCliente_De_RefBancariaClienteDados(origem.RefBancaria),
                RefComercial = RefComercialDtoCliente.ListaRefComercialDtoCliente_De_RefComercialClienteDados(origem.RefComercial)
            };
        }
        public static Cliente.Dados.ClienteCadastroDados ClienteCadastroDados_De_ClienteCadastroDto(ClienteCadastroDto origem)
        {
            if (origem == null) return null;
            return new Cliente.Dados.ClienteCadastroDados()
            {
                DadosCliente = DadosClienteCadastroDto.DadosClienteCadastroDados_De_DadosClienteCadastroDto(origem.DadosCliente),
                RefBancaria = RefBancariaDtoCliente.ListaRefBancariaClienteDados_De_RefBancariaDtoCliente(origem.RefBancaria),
                RefComercial = RefComercialDtoCliente.ListaRefComercialClienteDados_De_RefComercialDtoCliente(origem.RefComercial)
            };
        }
    }
}