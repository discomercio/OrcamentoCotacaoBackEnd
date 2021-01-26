using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava30
{
    class Grava30 : PassoBaseGravacao
    {
        public Grava30(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao)
            : base(contextoBdGravacao, pedido, retorno, criacao)
        {
        }

        public async Task Executar()
        {


            //Passo30: OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE -linha 1083
            //	Para todas as regras linha 1086
            //		Se o CD deve ser usado(manual ou auto)
            //		para todos os CDs da regra linha 1088
            //			Procura esse produto na lista de produtos linha 1095
            //			estoque_verifica_disponibilidade_integral_v2 em estoque.asp, especificado em Passo30 / estoque_verifica_disponibilidade_integral_v2.feature
            //				'Calcula quantidade em estoque no CD especificado

            //	Traduzindo:
            //			Calcula o estoque de cada produto em cada CD que pode ser usado

        }
    }
}
