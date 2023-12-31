﻿using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Prepedido.Dto;
using Microsoft.EntityFrameworkCore;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using InfraBanco.Modelos;
using InfraBanco.Constantes;
using Cliente;

namespace PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll
{
    public class ClienteUnisBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly ClienteBll clienteArclubeBll;

        public ClienteUnisBll(InfraBanco.ContextoBdProvider contextoProvider,
            InfraBanco.ContextoCepProvider contextoCepProvider, ClienteBll clienteArclubeBll)
        {
            this.contextoProvider = contextoProvider;
            this.contextoCepProvider = contextoCepProvider;
            this.clienteArclubeBll = clienteArclubeBll;
        }

        public async Task<ClienteCadastroResultadoUnisDto> CadastrarClienteUnis(ClienteCadastroUnisDto clienteUnis, string usuario_cadastro)
        {
            List<string> lstErros = new List<string>();
            ClienteCadastroResultadoUnisDto retorno = new ClienteCadastroResultadoUnisDto();

            //vamos verificar se orcamentista do cadastro existe para 
            clienteUnis.DadosCliente.Indicador_Orcamentista = clienteUnis.DadosCliente.Indicador_Orcamentista?.ToUpper();
            TorcamentistaEindicador orcamentista =
                await ValidacoesClienteUnisBll.ValidarBuscarOrcamentista(clienteUnis.DadosCliente.Indicador_Orcamentista,
                contextoProvider);

            if (orcamentista != null)
            {
                if (clienteUnis != null)
                {
                    string loja = orcamentista.Loja;
                    //precisamos passar os dados para o DTO de ClienteCadastroDto da ArClube
                    ClienteCadastroDto clienteArclube = ClienteCadastroUnisDto.ClienteCadastroDtoDeClienteCadastroUnisDto(clienteUnis, loja);

                    /*
                     * vamos fazer a validação e retornar uma lista de erros, 
                     * se estiver vazia foi cadastrado com sucesso
                     * se estiver com itens na lista, ocorreu erro na validação
                     */
                    retorno.ListaErros = (await clienteArclubeBll.CadastrarCliente(ClienteCadastroDto.ClienteCadastroDados_De_ClienteCadastroDto(clienteArclube),
                        orcamentista.Apelido,
                        Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__UNIS,
                        usuario_cadastro)).listaErros;

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

        private async Task<string> BuscarIdCliente(string cpf_cnpj)
        {
            string retorno = "";

            var db = contextoProvider.GetContextoLeitura();

            cpf_cnpj = UtilsGlobais.Util.SoDigitosCpf_Cnpj(cpf_cnpj);

            retorno = await (from c in db.Tcliente
                             where c.Cnpj_Cpf == cpf_cnpj
                             select c.Id).FirstOrDefaultAsync();

            return retorno;
        }

        public async Task<ClienteBuscaRetornoUnisDto> BuscarCliente(string cpf_cnpj, string apelido)
        {
            ClienteCadastroDto cliente = ClienteCadastroDto.ClienteCadastroDto_De_ClienteCadastroDados(await clienteArclubeBll.BuscarCliente(cpf_cnpj, apelido));
            if (cliente == null)
                return null;
            ClienteBuscaRetornoUnisDto retorno = ClienteBuscaRetornoUnisDto.ClienteCadastroUnisDtoDeClienteCadastroDto(cliente);

            return retorno;
        }

        public async Task<VerificarInscricaoEstadualValidaRetornoUnisDto> VerificarInscricaoEstadualValida(string inscricaoEstadual, string uf)
        {
            var erros = new List<string>();
            Cliente.ValidacoesClienteBll.VerificarInscricaoEstadualValida(inscricaoEstadual, uf, erros, false);
            var retorno = new VerificarInscricaoEstadualValidaRetornoUnisDto
            {
                InscricaoEstadualValida = (erros.Count == 0)
            };
            return await Task.FromResult(retorno);
        }
    }
}
