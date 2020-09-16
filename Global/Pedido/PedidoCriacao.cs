using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido
{
    public class PedidoCriacao
    {
        //vai retornar um PedidoCriacaoRetorno com o id do pedido, dos filhotes, 
        //as mensagens de erro e as mensagens de erro da validação dos 
        //dados cadastrais (quer dizer, duas listas de erro.) 
        //É que na loja o tratamento dos erros dos dados cadastrais vai ser diferente).
        public async Task<PedidoCriacaoRetornoDados> CadastrarPedido(PedidoCriacaoDados pedido)
        {
            PedidoCriacaoRetornoDados pedidoRetorno = new PedidoCriacaoRetornoDados();
            pedidoRetorno.ListaErros.Add("Ainda não implementado");

            return await Task.FromResult(pedidoRetorno);
        }

    }
}
