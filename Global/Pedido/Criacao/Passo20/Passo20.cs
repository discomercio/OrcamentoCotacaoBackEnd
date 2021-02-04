using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo20
{
    class Passo20 : PassoBase
    {
        public Passo20(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
            : base(pedido, retorno, pedidoCriacao)

        {
        }

        public async Task ValidarEnderecoEntregaAsync()
        {
            /* valida endereço de entrega */
            await Criacao.ValidacoesPrepedidoBll.ValidarEnderecoEntrega(Pedido.EnderecoEntrega, Retorno.ListaErros,
                Pedido.Ambiente.Indicador, Pedido.Cliente.Tipo.ParaString(), false, Pedido.Ambiente.Loja);
        }
    }
}
