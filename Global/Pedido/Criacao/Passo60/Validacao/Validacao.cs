using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Validacao
{
    class Validacao
    {
        private readonly PedidoCriacaoDados pedido;
        private readonly PedidoCriacaoRetornoDados retorno;
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        public Validacao(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.pedidoCriacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Executar()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
        }
    }
}
