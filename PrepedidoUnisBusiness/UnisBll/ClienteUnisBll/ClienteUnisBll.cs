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

        /*
         * AINDA ESTA FALTANDO INCLUIR OS NOVOS CAMPOS NO CADASTRO DO CLIENTE E 
         * AS REGRAS QUE DE VALIDAÇÕES PARA OS NOVOS CAMPOS
         */
        public async Task<IEnumerable<string>> CadastrarClienteUnis(ClienteCadastroUnisDto clienteUnis)
        {
            List<string> lstErros = new List<string>();

            var db = contextoProvider.GetContextoLeitura();

            //vamos verificar se orcamentista do cadastro existe
            if (await ValidacoesClienteUnisBll.ValidarOrcamentista(clienteUnis.DadosCliente.Indicador_Orcamentista,
                clienteUnis.DadosCliente.Loja, contextoProvider))
            {
                ClienteBll clienteArclubeBll = new ClienteBll(contextoProvider, contextoCepProvider);

                //busca o cliente para saber se ele já esta cadastrado
                ClienteCadastroDto clienteExiste = await clienteArclubeBll.
                    BuscarCliente(clienteUnis.DadosCliente.Cnpj_Cpf, clienteUnis.DadosCliente.Indicador_Orcamentista);

                if (clienteUnis != null)
                {
                    //precisamos passar os dados para o DTO de ClienteCadastroDto da ArClube
                    ClienteCadastroDto clienteArclube = Inicializar_ClienteCadastroDto_Arclube(clienteUnis);

                    /*vamos fazer a validação e retornar uma lista de erros, 
                     * se estiver vazia foi cadastrado com sucesso
                     * se estiver com itens na lista, ocorreu erro na validação
                     */
                    lstErros = (await clienteArclubeBll.CadastrarCliente(clienteArclube,
                        clienteArclube.DadosCliente.Indicador_Orcamentista)).ToList();
                }
            }
            else
            {
                lstErros.Add("O Orçamentista não existe!");
            }

            return lstErros;
        }

        private static ClienteCadastroDto Inicializar_ClienteCadastroDto_Arclube(ClienteCadastroUnisDto clienteUnis)
        {
            ClienteCadastroDto clienteArclube = new ClienteCadastroDto();

            clienteArclube.DadosCliente = new DadosClienteCadastroDto();
            clienteArclube.DadosCliente = Ini_DadosCliente_Arclube(clienteUnis.DadosCliente);

            clienteArclube.RefBancaria = new List<RefBancariaDtoCliente>();
            clienteArclube.RefBancaria = Ini_RefBancaria_Arclube(clienteUnis.RefBancaria);

            clienteArclube.RefComercial = new List<RefComercialDtoCliente>();
            clienteArclube.RefComercial = Ini_RefComercial_Arclube(clienteUnis.RefComercial);

            return clienteArclube;
        }

        private static DadosClienteCadastroDto Ini_DadosCliente_Arclube(DadosClienteCadastroUnisDto dadosClienteUnis)
        {
            DadosClienteCadastroDto dadosClienteArclube = new DadosClienteCadastroDto();
            dadosClienteArclube.Indicador_Orcamentista = dadosClienteUnis.Indicador_Orcamentista;
            dadosClienteArclube.Loja = dadosClienteUnis.Loja;
            dadosClienteArclube.Nome = dadosClienteUnis.Nome;
            dadosClienteArclube.Cnpj_Cpf = dadosClienteUnis.Cnpj_Cpf;
            dadosClienteArclube.Tipo = dadosClienteUnis.Tipo;
            dadosClienteArclube.Sexo = dadosClienteUnis.Sexo;
            dadosClienteArclube.Rg = dadosClienteUnis.Rg;
            dadosClienteArclube.Nascimento = dadosClienteUnis.Nascimento;
            dadosClienteArclube.DddCelular = dadosClienteUnis.DddCelular;
            dadosClienteArclube.Celular = dadosClienteUnis.Celular;
            dadosClienteArclube.DddResidencial = dadosClienteUnis.DddResidencial;
            dadosClienteArclube.TelefoneResidencial = dadosClienteUnis.TelefoneResidencial;
            dadosClienteArclube.DddComercial = dadosClienteUnis.DddComercial;
            dadosClienteArclube.TelComercial = dadosClienteUnis.TelComercial;
            dadosClienteArclube.Ramal = dadosClienteUnis.Ramal;
            dadosClienteArclube.DddComercial2 = dadosClienteUnis.DddComercial2;
            dadosClienteArclube.TelComercial2 = dadosClienteUnis.TelComercial2;
            dadosClienteArclube.Ramal2 = dadosClienteUnis.Ramal2;
            dadosClienteArclube.Ie = dadosClienteUnis.Ie;
            dadosClienteArclube.ProdutorRural = dadosClienteUnis.ProdutorRural;
            dadosClienteArclube.Contribuinte_Icms_Status = dadosClienteUnis.Contribuinte_Icms_Status;
            dadosClienteArclube.Email = dadosClienteUnis.Email;
            dadosClienteArclube.EmailXml = dadosClienteUnis.EmailXml;
            dadosClienteArclube.Vendedor = dadosClienteUnis.Vendedor;
            dadosClienteArclube.Cep = dadosClienteUnis.Cep;
            dadosClienteArclube.Endereco = dadosClienteUnis.Endereco;
            dadosClienteArclube.Numero = dadosClienteUnis.Numero;
            dadosClienteArclube.Bairro = dadosClienteUnis.Bairro;
            dadosClienteArclube.Cidade = dadosClienteUnis.Cidade;
            dadosClienteArclube.Uf = dadosClienteUnis.Uf;
            dadosClienteArclube.Complemento = dadosClienteUnis.Complemento;

            return dadosClienteArclube;
        }

        private static List<RefBancariaDtoCliente> Ini_RefBancaria_Arclube(List<RefBancariaClienteUnisDto> lstRefBancariaUnis)
        {
            RefBancariaDtoCliente refBancariaArclube = new RefBancariaDtoCliente();
            List<RefBancariaDtoCliente> lstRefBancariaArclube = new List<RefBancariaDtoCliente>();

            lstRefBancariaUnis.ForEach(x =>
            {
                refBancariaArclube.Agencia = x.Agencia;
                refBancariaArclube.Banco = x.Banco;
                refBancariaArclube.BancoDescricao = x.BancoDescricao;
                refBancariaArclube.Conta = x.Conta;
                refBancariaArclube.Contato = x.Contato;
                refBancariaArclube.Ddd = x.Ddd;
                refBancariaArclube.Telefone = x.Telefone;
                refBancariaArclube.Ordem = x.Ordem;

                lstRefBancariaArclube.Add(refBancariaArclube);
            });

            return lstRefBancariaArclube;
        }

        private static List<RefComercialDtoCliente> Ini_RefComercial_Arclube(List<RefComercialClienteUnisDto> lstRefComercialUnis)
        {
            RefComercialDtoCliente refComercialArclube = new RefComercialDtoCliente();
            List<RefComercialDtoCliente> lstRefComercialArclube = new List<RefComercialDtoCliente>();

            lstRefComercialUnis.ForEach(x =>
            {
                refComercialArclube.Contato = x.Contato;
                refComercialArclube.Ddd = x.Ddd;
                refComercialArclube.Telefone = x.Telefone;
                refComercialArclube.Nome_Empresa = x.Nome_Empresa;
                refComercialArclube.Ordem = x.Ordem;

                lstRefComercialArclube.Add(refComercialArclube);
            });

            return lstRefComercialArclube;
        }
    }
}
