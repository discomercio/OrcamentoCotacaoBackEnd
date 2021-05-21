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
    class Passo10 : PassoBase
    {
        public Passo10(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
            : base(pedido, retorno, pedidoCriacao)

        {
        }

        public async Task ValidarCliente()
        {
            //estas validações estão especificadas em Especificacao\Pedido\Passo60\Validacao\DadosCadastrais\DadosCadastrais.feature
            var idCLiente = await Cliente.ClienteBll.BuscarIdCliente(Pedido.Cliente.Cnpj_Cpf, Criacao.ContextoProvider.GetContextoLeitura());
            if (idCLiente == null)
            {
                Retorno.ListaErros.Add($"O cliente não está cadastrado: {Pedido.Cliente.Cnpj_Cpf}");
                return;
            }
            Criacao.Execucao.Id_cliente = idCLiente;


            //vamos validar os dados do cliente que esta vindo no pedido
            var dadosClienteCadastroDados = Cliente.Dados.DadosClienteCadastroDados.DadosClienteCadastroDadosDeEnderecoCadastralClientePrepedidoDados(Pedido.EnderecoCadastralCliente,
                Pedido.Ambiente.Indicador, Pedido.Ambiente.Loja,
                "", null, Criacao.Execucao.Id_cliente);
            await Cliente.ValidacoesClienteBll.ValidarDadosCliente(dadosClienteCadastroDados,
                false,
                null,
                null,
                Retorno.ListaErrosValidacao,
                Criacao.ContextoProvider,
                Criacao.CepBll,
                Criacao.BancoNFeMunicipio,
                null,
                Pedido.Cliente.Tipo.PessoaFisica(),
                Pedido.Configuracao.SistemaResponsavelCadastro,
                false);
        }
        public void Permissoes()
        {
            /*
	        #em loja/ClienteEdita.asp
	        #<% if operacao_permitida(OP_LJA_CADASTRA_NOVO_PEDIDO, s_lista_operacoes_permitidas) then %>
            */
            if (!Criacao.Execucao.UsuarioPermissao.Operacao_permitida(Constantes.OP_LJA_CADASTRA_NOVO_PEDIDO))
                Retorno.ListaErros.Add("Usuário não tem permissão para criar pedido (OP_LJA_CADASTRA_NOVO_PEDIDO)");
            /*
            #loja/PedidoNovoConsiste.asp
            #	if operacao_permitida(OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO, s_lista_operacoes_permitidas) then intColSpan = intColSpan + 1
            #nesse caso, instalador_instala fica vazio
            #temos que verificar que não posso dar essa iinformação se não tiver a permissão
            */
            if (!Criacao.Execucao.UsuarioPermissao.Operacao_permitida(Constantes.OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO))
                if (Pedido.DetalhesPedido.InstaladorInstala != (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO_DEFINIDO)
                    Retorno.ListaErros.Add("Usuário não tem permissão para informar o campo InstaladorInstala (OP_LJA_EXIBIR_CAMPO_INSTALADOR_INSTALA_AO_CADASTRAR_NOVO_PEDIDO)");
        }
    }
}
