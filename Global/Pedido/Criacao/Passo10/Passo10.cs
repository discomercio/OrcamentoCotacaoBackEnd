using Cep;
using InfraBanco.Constantes;
using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UtilsGlobais.Usuario;

namespace Pedido.Criacao.Passo10
{
    class Passo10
    {
        private readonly PedidoCriacaoDados pedido;
        private readonly PedidoCriacaoRetornoDados retorno;
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        public Passo10(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.pedidoCriacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        public async Task ValidarCliente()
        {
            //vamos validar os dados do cliente que esta vindo no pedido
            var dadosClienteCadastroDados = Cliente.Dados.DadosClienteCadastroDados.DadosClienteCadastroDadosDeEnderecoCadastralClientePrepedidoDados(pedido.EnderecoCadastralCliente,
                pedido.Ambiente.Indicador, pedido.Ambiente.Loja,
                "", null, pedido.Cliente.Id_cliente);
            await Cliente.ValidacoesClienteBll.ValidarDadosCliente(dadosClienteCadastroDados,
                false,
                null,
                null,
                retorno.ListaErrosValidacao,
                pedidoCriacao.contextoProvider,
                pedidoCriacao.cepBll,
                pedidoCriacao.bancoNFeMunicipio,
                null,
                pedido.Cliente.Tipo.PessoaFisica(),
                pedido.Configuracao.SistemaResponsavelCadastro,
                false);
        }
        public void Permissoes()
        {
            /*
	        #em loja/ClienteEdita.asp
	        #<% if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then %>
            */
            if (!pedidoCriacao.UsuarioPermissao.Permitido(Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO))
                retorno.ListaErros.Add("Usuário não tem permissão para criar pedido (OP_LJA_CADASTRA_NOVO_PEDIDO)");
            /*
            #loja/PedidoNovoConsiste.asp
            #	if operacao_permitida(OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO, s_lista_operacoes_permitidas) then intColSpan = intColSpan + 1
            #nesse caso, instalador_instala fica vazio
            #temos que verificar que não posso dar essa iinformação se não tiver a permissão
            */
            if (!pedidoCriacao.UsuarioPermissao.Permitido(Constantes.OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO))
                if (pedido.DetalhesPedido.InstaladorInstala != 0)
                    retorno.ListaErros.Add("Usuário não tem permissão para informar o campo InstaladorInstala (OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO)");
        }
    }
}
