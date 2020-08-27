using Pedido;
using Pedido.Dados;
using PrepedidoBusiness.Dto.Pedido;
using PrepedidoBusiness.Dto.Pedido.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Pedido.PedidoBll;

namespace PrepedidoBusiness.Bll
{
    public class PedidoPrepedidoApiBll
    {
        private readonly PedidoBll pedidoBll;

        public PedidoPrepedidoApiBll(Pedido.PedidoBll pedidoBll)
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
            Pedido.Dados.DetalhesPedido.PedidoDados ret = await pedidoBll.BuscarPedido(apelido.Trim(), numPedido);
            return PedidoDto.PedidoDto_De_PedidoDados(ret);
        }
    }
}
