using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo20
{
    class Passo20
    {
        private readonly PedidoCriacaoDados Pedido;
        private readonly PedidoCriacaoRetornoDados Retorno;
        private readonly Pedido.Criacao.PedidoCriacao Criacao;
        public Passo20(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        public async Task ValidarEnderecoEntrega()
        {
            /* valida endereço de entrega */
            await Criacao.ValidacoesPrepedidoBll.ValidarEnderecoEntrega(Pedido.EnderecoEntrega, Retorno.ListaErros,
                Pedido.Ambiente.Indicador, Pedido.Cliente.Tipo.ParaString(), false, Pedido.Ambiente.Loja);
        }
    }
}
