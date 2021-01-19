using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo20
{
    static class Passo20
    {
        static public async Task ValidarEnderecoEntrega(
            PedidoCriacaoDados pedido,
            PedidoCriacaoRetornoDados pedidoRetorno,
            Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll)
        {
            /* valida endereço de entrega */
            await validacoesPrepedidoBll.ValidarEnderecoEntrega(pedido.EnderecoEntrega, pedidoRetorno.ListaErros,
                pedido.Ambiente.Indicador, pedido.Cliente.Tipo.ParaString(), false, pedido.Ambiente.Loja);
        }
    }
}
