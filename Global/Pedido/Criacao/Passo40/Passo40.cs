using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo40
{
    class Passo40
    {
        private readonly PedidoCriacaoDados pedido;
        private readonly PedidoCriacaoRetornoDados retorno;
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        public Passo40(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.pedidoCriacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }


        public async Task Executar()
        {
            NumeroProdutos();
            await ListaProdutosFormaPagamento();
        }

        private void NumeroProdutos()
        {
            if (pedido.ListaProdutos.Count > 12)
                retorno.ListaErros.Add("São permitidos no máximo 12 itens por pedido.");
            if (pedido.ListaProdutos.Count == 0)
                retorno.ListaErros.Add("Pedido sem nenhum produto na lista.");
        }
        private async Task ListaProdutosFormaPagamento()
        {
        }
    }
}
