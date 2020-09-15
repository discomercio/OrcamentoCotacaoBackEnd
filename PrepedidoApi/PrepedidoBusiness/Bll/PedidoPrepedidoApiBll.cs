using Prepedido.PedidoVisualizacao;
using PrepedidoBusiness.Dto.Pedido;
using PrepedidoBusiness.Dto.Pedido.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Prepedido.PedidoVisualizacao.PedidoBll;

namespace PrepedidoBusiness.Bll
{
    public class PedidoPrepedidoApiBll
    {
        private readonly PedidoBll pedidoBll;

        public PedidoPrepedidoApiBll(PedidoBll pedidoBll)
        {
            this.pedidoBll = pedidoBll;
        }
        public async Task<IEnumerable<PedidoDtoPedido>> ListarPedidos(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            var ret = await pedidoBll.ListarPedidos(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, dataFinal);
            return PedidoDtoPedido.ListaPedidoDtoPedido_De_PedidosPedidoDados(ret);
        }
        public async Task<PedidoDto> BuscarPedido(string apelido, string numPedido)
        {
            Prepedido.PedidoVisualizacao.Dados.DetalhesPedido.PedidoDados ret = await pedidoBll.BuscarPedido(apelido.Trim(), numPedido);
            return PedidoDto.PedidoDto_De_PedidoDados(ret);
        }
    }
}
