using Cep;
using InfraBanco.Constantes;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo10
{
    static class Passo10
    {
        static public async Task ValidarCliente(
            PedidoCriacaoDados pedido,
            PedidoCriacaoRetornoDados pedidoRetorno,
            InfraBanco.ContextoBdProvider contextoProvider,
            CepBll cepBll,
            IBancoNFeMunicipio bancoNFeMunicipio)
        {
            //vamos validar os dados do cliente que esta vindo no pedido
            var dadosClienteCadastroDados = Cliente.Dados.DadosClienteCadastroDados.DadosClienteCadastroDadosDeEnderecoCadastralClientePrepedidoDados(pedido.EnderecoCadastralCliente,
                pedido.Ambiente.Indicador, pedido.Ambiente.Loja,
                "", null, pedido.Cliente.Id_cliente);
            await Cliente.ValidacoesClienteBll.ValidarDadosCliente(dadosClienteCadastroDados,
                false,
                null,
                null,
                pedidoRetorno.ListaErrosValidacao,
                contextoProvider,
                cepBll,
                bancoNFeMunicipio,
                null,
                pedido.Cliente.Tipo.PessoaFisica(),
                pedido.Configuracao.SistemaResponsavelCadastro,
                false);
        }
        static public void Permissoes(
            PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno,
            UtilsGlobais.Usuario.UsuarioPermissao usuarioPermissao)
        {
            /*
	        #em loja/ClienteEdita.asp
	        #<% if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then %>
            */
            if (!usuarioPermissao.Permitido(Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO))
                retorno.ListaErros.Add("Usuário não tem permissão para criar pedido (OP_LJA_CADASTRA_NOVO_PEDIDO)");
            /*
            #loja/PedidoNovoConsiste.asp
            #	if operacao_permitida(OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO, s_lista_operacoes_permitidas) then intColSpan = intColSpan + 1
            #nesse caso, instalador_instala fica vazio
            #temos que verificar que não posso dar essa iinformação se não tiver a permissão
            */
            if (!usuarioPermissao.Permitido(Constantes.OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO))
                if (pedido.DetalhesPedido.InstaladorInstala != 0)
                    retorno.ListaErros.Add("Usuário não tem permissão para informar o campo InstaladorInstala (OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO)");
        }
    }
}
