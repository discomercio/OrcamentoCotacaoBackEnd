using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class ClientePrepedidoBll
    {
        private readonly Cliente.ClienteBll clienteBll;

        public ClientePrepedidoBll(Cliente.ClienteBll clienteBll)
        {
            this.clienteBll = clienteBll;
        }
        public async Task<IEnumerable<ListaBancoDto>> ListarBancosCombo()
        {
            IEnumerable<Cliente.Dados.ListaBancoDados> listaBancosDados = await clienteBll.ListarBancosCombo();
            IEnumerable<ListaBancoDto> listaBancos = ListaBancoDto.ListaBancoDto_De_BancoDadosLista(listaBancosDados);
            return listaBancos;
        }

        public async Task<IEnumerable<EnderecoEntregaJustificativaDto>> ListarComboJustificaEndereco(string apelido)
        {
            IEnumerable<Cliente.Dados.EnderecoEntregaJustificativaDados> retorno = await clienteBll.ListarComboJustificaEndereco(apelido.Trim());
            IEnumerable<EnderecoEntregaJustificativaDto> enderecoEntregaJustificativaDtos = EnderecoEntregaJustificativaDto.EnderecoEntregaJustificativaDto_De_EnderecoEntregaJustificativaDadosLista(retorno);
            return enderecoEntregaJustificativaDtos;
        }
        public async Task<List<string>> AtualizarClienteParcial(string apelido, DadosClienteCadastroDto dadosClienteCadastroDto, InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavel)
        {
            List<string> retorno = await clienteBll.AtualizarClienteParcial(apelido.Trim(), DadosClienteCadastroDto.DadosClienteCadastroDados_De_DadosClienteCadastroDto(dadosClienteCadastroDto), sistemaResponsavel);
            return retorno;
        }
        public async Task<ClienteCadastroDto> BuscarCliente(string cpf_cnpj, string apelido)
        {
            ClienteCadastroDto dadosCliente = ClienteCadastroDto.ClienteCadastroDto_De_ClienteCadastroDados(await clienteBll.BuscarCliente(cpf_cnpj, apelido.Trim()));
            return dadosCliente;
        }
        public async Task<IEnumerable<string>> CadastrarCliente(ClienteCadastroDto clienteDto, string apelido)
        {
            IEnumerable<string> retorno = await clienteBll.CadastrarCliente(ClienteCadastroDto.ClienteCadastroDados_De_ClienteCadastroDto(clienteDto), apelido.Trim(),
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS, apelido.Trim());
            return retorno;
        }

    }
}
