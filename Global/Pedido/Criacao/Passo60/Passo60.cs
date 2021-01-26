using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60
{
    class Passo60
    {
        private readonly PedidoCriacaoDados Pedido;
        private readonly PedidoCriacaoRetornoDados Retorno;
        private readonly Pedido.Criacao.PedidoCriacao Criacao;
        public Passo60(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao)
        {
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = criacao ?? throw new ArgumentNullException(nameof(criacao));
        }

        public async Task Executar()
        {
            await new Validacao.Validacao(Pedido, Retorno, Criacao).Executar();
            //se tem erro de validação não coeçamos a gravação
            if (Retorno.AlgumErro())
                return;

            await new Gravacao.Gravacao(Pedido, Retorno, Criacao).Executar();
        }
    }
}
