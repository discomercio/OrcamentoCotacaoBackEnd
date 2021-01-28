using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava60
{
    class Grava60 : PassoBaseGravacao
    {
        public Grava60(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao)
        {
        }

        public async Task Executar()
        {

            //Passo60: criar pedidos
            //	Loop nos CDs a utilizar
            //		Gerar o número do pedido: Passo60 / Gerar_o_numero_do_pedido.feature
            //		Adiciona um novo pedido
            //		Preenche os campos do pedido: Passo60 / Preenche_os_campos_do_pedido.feature
            //			a maioria no pai e filhotes, alguns só no pai, alguns só nos filhotes
            //		Salva o registro em t_pedido

            //		Loop nas regras:
            //			Especificado em Passo60 / Itens / Gerar_t_PEDIDO_ITEM.feature
            //				Se essa regra cobrir um dos itens do pedido, adicionar registro em t_PEDIDO_ITEM(linha 2090 até 2122)
            //				Note que a quantidade rs("qtde") é a que foi alocada para esse filhote pela regra, não a quantidade total do pedido inteiro
            //				A sequencia do t_PEDIDO_ITEM para esse pedido(base ou filhote) começa de 1 e é sequencial.
            //			Se qtde_solicitada > qtde_estoque, qtde_spe(quantidade_sen_presença_estoque) fica com o número de itens faltando
            //		   chama rotina ESTOQUE_produto_saida_v2, em Passo60 / Itens / ESTOQUE_produto_saida_v2.feature
            //				A quantidade deste item ou efetivamente sai do estoque(atualizando t_ESTOQUE_ITEM)
            //				ou entra como venda sem presença no estoque(novo registro na tabela t_ESTOQUE_MOVIMENTO, operacao = OP_ESTOQUE_VENDA, estoque = ID_ESTOQUE_SEM_PRESENCA)
            //			Monta o log do item - Passo60 / Itens / Log.feature


            //		Determina o status st_entrega deste pedido(Passo60 / st_entrega.feature)
        }
    }
}
