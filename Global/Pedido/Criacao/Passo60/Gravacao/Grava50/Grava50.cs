using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava50
{
    class Grava50 : PassoBaseGravacao
    {
        public Grava50(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao)
        {
        }

        public async Task Executar()
        {


            //Passo50: ANALISA A QUANTIDADE DE PEDIDOS QUE SERÃO CADASTRADOS(AUTO-SPLIT) -linha 1184
            //			'	OS CD'S ESTÃO ORDENADOS DE ACORDO C/ A PRIORIZAÇÃO DEFINIDA PELA REGRA DE CONSUMO DO ESTOQUE
            //			'	SE O PRIMEIRO CD HABILITADO NÃO PUDER ATENDER INTEGRALMENTE A QUANTIDADE SOLICITADA DO PRODUTO,
            //			'	A QUANTIDADE RESTANTE SERÁ CONSUMIDA DOS DEMAIS CD'S.
            //			'	SE HOUVER ALGUMA QUANTIDADE RESIDUAL P/ FICAR NA LISTA DE PRODUTOS SEM PRESENÇA NO ESTOQUE:
            //			'		1) SELEÇÃO AUTOMÁTICA DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD DEFINIDO P/ TAL
            //			'		2) SELEÇÃO MANUAL DE CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD SELECIONADO MANUALMENTE

            //	Para cada produto:
            //			Aloca a quantidade solicitada nos CDs ordenados até alocar todos.
            //		Se não conseguir alocar todos, marca a quantidade residual no CD manual ou no CD de t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente
        }
    }
}
