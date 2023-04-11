using InfraBanco.Modelos.Filtros;
using OrcamentoCotacaoBusiness.Dto.Pedido;
using OrcamentoCotacaoBusiness.Dto.Pedido.DetalhesPedido;
using Prepedido.Dto;
using Prepedido.PedidoVisualizacao;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll;

namespace OrcamentoCotacaoBusiness.Bll
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

        public PedidoConsultaDto ListarPedidos(TorcamentoFiltro tOrcamentoFiltro)
        {
            return _pedidoBll.ListarPedidosPorFiltro(tOrcamentoFiltro);
        }
    }
}