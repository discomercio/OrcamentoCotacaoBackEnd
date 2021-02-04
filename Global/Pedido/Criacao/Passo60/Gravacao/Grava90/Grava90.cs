using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava90
{
    class Grava90 : PassoBaseGravacao
    {
        public Grava90(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao)
        {
        }

        public async Task ExecutarAsync()
        {
            //todo: Passo90: log(Passo90 / Log.feature)

            //Passo90: log(Passo90 / Log.feature)
        }
    }
}
