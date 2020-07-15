using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PrepedidoBusiness.Dto.ClienteCadastro;
using Microsoft.EntityFrameworkCore;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using InfraBanco.Modelos;
using PrepedidoBusiness.Bll.ClienteBll;
using InfraBanco.Constantes;

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
                if (clienteUnis != null)
                {
                    string loja = orcamentista.Loja;
                    //precisamos passar os dados para o DTO de ClienteCadastroDto da ArClube
                    ClienteCadastroDto clienteArclube = ClienteCadastroUnisDto.ClienteCadastroDtoDeClienteCadastroUnisDto(clienteUnis, loja);

                    /*VERIFICAR SE É RETORNA VAZIO MESMO
                     * vamos fazer a validação e retornar uma lista de erros, 
                     * se estiver vazia foi cadastrado com sucesso
                     * se estiver com itens na lista, ocorreu erro na validação
                     */
                    retorno.ListaErros = (await clienteArclubeBll.CadastrarCliente(clienteArclube,
                        orcamentista.Apelido,
                        (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__UNIS)).ToList();

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

            cpf_cnpj = PrepedidoBusiness.Utils.Util.SoDigitosCpf_Cnpj(cpf_cnpj);

            retorno = await (from c in db.Tclientes
                             where c.Cnpj_Cpf == cpf_cnpj
                             select c.Id).FirstOrDefaultAsync();

            return retorno;
        }

        public async Task<ClienteBuscaRetornoUnisDto> BuscarCliente(string cpf_cnpj, string apelido)
        {
            var cliente = await clienteArclubeBll.BuscarCliente(cpf_cnpj, apelido);
            if (cliente == null)
                return null;
            ClienteBuscaRetornoUnisDto retorno = ClienteBuscaRetornoUnisDto.ClienteCadastroUnisDtoDeClienteCadastroDto(cliente);

            return retorno;
        }

        public async Task<VerificarInscricaoEstadualValidaRetornoUnisDto> VerificarInscricaoEstadualValida(string inscricaoEstadual, string uf)
        {
            var erros = new List<string>();
            PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.VerificarInscricaoEstadualValida(inscricaoEstadual, uf, erros);
            var retorno = new VerificarInscricaoEstadualValidaRetornoUnisDto
            {
                InscricaoEstadualValida = (erros.Count == 0)
            };
            return await Task.FromResult(retorno);
        }
    }
}
