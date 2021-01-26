using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava55
{
    class Grava55 : PassoBaseGravacao
    {
        public Grava55(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao)
            : base(contextoBdGravacao, pedido, retorno, criacao)
        {
        }

        public async Task Executar()
        {

            //Passo55: Contagem de pedidos a serem gravados -Linha 1286
            //	'	CONTAGEM DE EMPRESAS QUE SERÃO USADAS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, 
            //	JÁ QUE CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA

            //	Conta todos os CDs que tem alguma quantidade solicitada.

        }
    }
}
