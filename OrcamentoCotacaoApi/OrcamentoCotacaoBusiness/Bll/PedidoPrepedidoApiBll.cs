using InfraBanco.Modelos.Filtros;
using Orcamento.Dto;
using Prepedido.PedidoVisualizacao;
using PrepedidoBusiness.Dto.Pedido;
using PrepedidoBusiness.Dto.Pedido.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll;

namespace PrepedidoBusiness.Bll
{
    public class PedidoPrepedidoApiBll
    {
        private readonly PedidoVisualizacaoBll _pedidoBll;

        public PedidoPrepedidoApiBll(PedidoVisualizacaoBll pedidoBll)
        {
            _pedidoBll = pedidoBll;
        }

        public async Task<IEnumerable<PedidoDtoPedido>> ListarPedidos(string apelido, TipoBuscaPedido tipoBusca,
            string clienteBusca, string numeroPedido, DateTime? dataInicial, DateTime? dataFinal)
        {
            var ret = await _pedidoBll.ListarPedidos(apelido, tipoBusca, clienteBusca, numeroPedido, dataInicial, dataFinal);
            return PedidoDtoPedido.ListaPedidoDtoPedido_De_PedidosPedidoDados(ret);
        }

        public async Task<PedidoDto> BuscarPedido(string apelido, string numPedido)
        {
            Prepedido.PedidoVisualizacao.Dados.DetalhesPedido.PedidoDados ret = await _pedidoBll.BuscarPedido(apelido.Trim(), numPedido);
            return PedidoDto.PedidoDto_De_PedidoDados(ret);
        }

        public List<OrcamentoCotacaoListaDto> ListarPedidos(TorcamentoFiltro tOrcamentoFiltro)
        {
            return _pedidoBll.ListarPedidosPorFiltro(tOrcamentoFiltro);
        }
    }
}
