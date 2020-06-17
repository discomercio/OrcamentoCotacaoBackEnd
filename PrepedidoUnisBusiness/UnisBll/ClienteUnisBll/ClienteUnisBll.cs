using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Dto.ClienteCadastro;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using PrepedidoBusiness.Dto.ClienteCadastro.Referencias;
using Microsoft.EntityFrameworkCore;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using InfraBanco.Modelos;

namespace PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll
{
    public class ClienteUnisBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;

        public ClienteUnisBll(InfraBanco.ContextoBdProvider contextoProvider,
            InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
        }


        public async Task<ClienteCadastroResultadoUnisDto> CadastrarClienteUnis(ClienteCadastroUnisDto clienteUnis)
        {
            List<string> lstErros = new List<string>();
            ClienteCadastroResultadoUnisDto retorno = new ClienteCadastroResultadoUnisDto();

            //vamos verificar se orcamentista do cadastro existe para 
            TorcamentistaEindicador orcamentista =
                await ValidacoesClienteUnisBll.ValidarBuscarOrcamentista(clienteUnis.DadosCliente.Indicador_Orcamentista,
                contextoProvider);

            if (orcamentista != null)
            {
                ClienteBll clienteArclubeBll = new ClienteBll(contextoProvider, contextoCepProvider);

                if (clienteUnis != null)
                {
                    string loja = orcamentista.Loja;
                    //precisamos passar os dados para o DTO de ClienteCadastroDto da ArClube
                    ClienteCadastroDto clienteArclube = Inicializar_ClienteCadastroDto_Arclube(clienteUnis, loja);

                    /*VERIFICAR SE É RETORNA VAZIO MESMO
                     * vamos fazer a validação e retornar uma lista de erros, 
                     * se estiver vazia foi cadastrado com sucesso
                     * se estiver com itens na lista, ocorreu erro na validação
                     */
                    retorno.ListaErros = (await clienteArclubeBll.CadastrarCliente(clienteArclube,
                        orcamentista.Apelido, clienteUnis.DadosCliente.UsuarioCadastro)).ToList();

                    if (retorno.ListaErros.Count <= 0)
                    {
                        //não teve erros ao cadastrar o novo cliente, então vamos buscar o Id do cliente para devolver
                        retorno.IdClienteCadastrado = await BuscarIdCliente(clienteArclube.DadosCliente.Cnpj_Cpf);
                    }
                }
            }
            else
            {
                retorno.ListaErros.Add("O Orçamentista não existe!");
            }

            return retorno;
        }

        private static ClienteCadastroDto Inicializar_ClienteCadastroDto_Arclube(ClienteCadastroUnisDto clienteUnis, string loja)
        {
            ClienteCadastroDto clienteArclube = new ClienteCadastroDto();

            clienteArclube.DadosCliente = new DadosClienteCadastroDto();
            clienteArclube.DadosCliente =
                DadosClienteCadastroUnisDto.DadosClienteCadastroDtoDeDadosClienteCadastroUnisDto(clienteUnis.DadosCliente, loja);

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

        private async Task<string> BuscarIdCliente(string cpf_cnpj)
        {
            string retorno = "";

            var db = contextoProvider.GetContextoLeitura();

            retorno = await (from c in db.Tclientes
                             where c.Cnpj_Cpf == cpf_cnpj
                             select c.Id).FirstOrDefaultAsync();

            return retorno;
        }

        public async Task<ClienteBuscaRetornoUnisDto> BuscarCliente(string cpf_cnpj, string apelido)
        {
            ClienteBuscaRetornoUnisDto retorno = new ClienteBuscaRetornoUnisDto();

            return retorno;
        }
    }
}
