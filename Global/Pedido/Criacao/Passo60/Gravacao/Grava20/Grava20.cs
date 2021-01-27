using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava20
{
    class Grava20 : PassoBaseGravacao
    {
        public Grava20(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao)
            : base(contextoBdGravacao, pedido, retorno, criacao)
        {
        }

        public async Task Executar()
        {
            //Passo20: LER AS REGRAS DE CONSUMO DO ESTOQUE
            //	rotina obtemCtrlEstoqueProdutoRegra(arquivo bdd.asp)
            //		tipo_pessoa: especificado em Passo20 / multi_cd_regra_determina_tipo_pessoa.feature
            //		rotina obtemCtrlEstoqueProdutoRegra validações especificado em Passo20/ obtemCtrlEstoqueProdutoRegra.feature

            //	Traduzindo: para cada produto:
            //			Dado o produto, UF, tipo_cliente, contribuinte_icms_status, produtor_rural_status
            //			Descobrir em t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD os CDs que atendem em ordem de prioridade
            //		Lê as tabelas t_PRODUTO_X_WMS_REGRA_CD, t_WMS_REGRA_CD, t_WMS_REGRA_CD_X_UF, t_WMS_REGRA_CD_X_UF_X_PESSOA, t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD

            Prepedido.Dados.DetalhesPrepedido.PrePedidoDados prepedido = PedidoCriacaoDados.PrePedidoDadosDePedidoCriacaoDados(Pedido);
            //estes dados são lidso com o contexto de leitura, e ficam fora da transação

            //todo: otimizar os selects
            var listaregras = await Prepedido.PrepedidoBll.ObtemCtrlEstoqueProdutoRegra(Criacao.ContextoProvider,
                prepedido, Retorno.ListaErros);

            //agarante que tenha rgistrado o erro
            foreach (var regra in listaregras)
            {
                if (!regra.St_Regra_ok)
                {
                    Retorno.ListaErros.Add("Ocorreu algum erro na leitura das regras de consumo de estoque.");
                    return;
                }
            }
        }
    }
}
