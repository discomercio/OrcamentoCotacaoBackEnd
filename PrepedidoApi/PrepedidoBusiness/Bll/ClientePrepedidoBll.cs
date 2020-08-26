using PrepedidoBusiness.Dto.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class ClientePrepedidoBll
    {
        private readonly ClienteBll.ClienteBll clienteBll;

        public ClientePrepedidoBll(PrepedidoBusiness.Bll.ClienteBll.ClienteBll clienteBll)
        {
            this.clienteBll = clienteBll;
        }
        public async Task<IEnumerable<ListaBancoDto>> ListarBancosCombo()
        {
            IEnumerable<Cliente.Dados.ListaBancoDados> listaBancosDados = await clienteBll.ListarBancosCombo();
            IEnumerable<ListaBancoDto> listaBancos = ListaBancoDto.ListaBancoDtoDeBancoDadosLista(listaBancosDados);
            return listaBancos;
        }

        public async Task<IEnumerable<EnderecoEntregaJustificativaDto>> ListarComboJustificaEndereco(string apelido)
        {
            IEnumerable<Cliente.Dados.EnderecoEntregaJustificativaDados> retorno = await clienteBll.ListarComboJustificaEndereco(apelido.Trim());
            IEnumerable<EnderecoEntregaJustificativaDto> enderecoEntregaJustificativaDtos = EnderecoEntregaJustificativaDto.EnderecoEntregaJustificativaDtoDeEnderecoEntregaJustificativaDadosLista(retorno);
            return enderecoEntregaJustificativaDtos;
        }
        public async Task<List<string>> AtualizarClienteParcial(string apelido, DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            List<string> retorno = await clienteBll.AtualizarClienteParcial(apelido.Trim(), DadosClienteCadastroDto.DadosClienteCadastroDados_De_DadosClienteCadastroDto(dadosClienteCadastroDto));
            return retorno;
        }
    }
}
