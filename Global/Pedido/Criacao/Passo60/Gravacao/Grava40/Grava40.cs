using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava40
{
    class Grava40 : PassoBaseGravacao
    {
        public Grava40(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao)
            : base(contextoBdGravacao, pedido, retorno, criacao)
        {
        }

        public async Task Executar()
        {

            //	Passo40: Verifica se a disponibilidade do estoque foi alterada - Linha 1159
            //	HÁ PRODUTO C / ESTOQUE INSUFICIENTE(SOMANDO - SE O ESTOQUE DE TODAS AS EMPRESAS CANDIDATAS) - linha 1127

            //	Porque: avisamos o usuário que existem produtos sem presença no estoque e, no momento de salvar, os produtos sem presença no estoque foram alterados.
            //	No caso da ApiMagento, não temos essa verificação
        }
    }
}
