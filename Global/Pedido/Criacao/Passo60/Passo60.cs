using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60
{
    class Passo60
    {
        private readonly PedidoCriacaoDados pedido;
        private readonly PedidoCriacaoRetornoDados retorno;
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        public Passo60(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.pedidoCriacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        public async Task Executar()
        {
            await new Validacao.Validacao(pedido, retorno, pedidoCriacao).Executar();
            //se tem erro de validação não coeçamos a gravação
            if (retorno.AlgumErro())
                return;

        }
    }
}
