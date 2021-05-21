using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao
{
    class PassoBase
    {
        //esta classe existe somente para facilitar o acesso aos dados da criação
        public readonly PedidoCriacaoDados Pedido;
        public readonly PedidoCriacaoRetornoDados Retorno;
        public readonly Pedido.Criacao.PedidoCriacao Criacao;
        public PassoBase(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao)
        {
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = criacao ?? throw new ArgumentNullException(nameof(criacao));
        }
    }
}
