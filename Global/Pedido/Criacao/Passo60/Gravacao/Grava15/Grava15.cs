using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava15
{
    partial class Grava15 : PassoBaseGravacao
    {
        public Grava15(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public async Task ExecutarAsync()
        {
            await PedidoMagentoRepetido();
            await PedidoRepetido.PedidoJaCadastrado(ContextoBdGravacao, Pedido, Retorno.ListaErros);
        }
    }
}
