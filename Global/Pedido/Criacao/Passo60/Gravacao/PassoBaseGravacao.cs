using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao
{
    class PassoBaseGravacao
    {
        //esta classe existe somente para facilitar o acesso aos dados da criação
        public readonly ContextoBdGravacao ContextoBdGravacao;
        public readonly PedidoCriacaoDados Pedido;
        public readonly PedidoCriacaoRetornoDados Retorno;
        public readonly Pedido.Criacao.PedidoCriacao Criacao;
        public readonly Pedido.Criacao.Execucao.Execucao Execucao;
        public readonly Execucao.Gravacao Gravacao;

        public PassoBaseGravacao(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
        {
            this.ContextoBdGravacao = contextoBdGravacao ?? throw new ArgumentNullException(nameof(contextoBdGravacao));
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = criacao ?? throw new ArgumentNullException(nameof(criacao));
            this.Execucao = execucao ?? throw new ArgumentNullException(nameof(execucao));
            this.Gravacao = gravacao ?? throw new ArgumentNullException(nameof(gravacao));
        }

    }
}
