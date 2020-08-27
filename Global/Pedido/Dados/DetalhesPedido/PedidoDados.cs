using Cliente.Dados;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedido.Dados.DetalhesPedido
{
    public class PedidoDados
    {
        public string NumeroPedido { get; set; }
        // estou incluindo esse campo para trazer o pedido filhote ao visualizar o pedido pai
        public List<List<string>> Lista_NumeroPedidoFilhote { get; set; }
        public StatusPedidoPedidoDados StatusHoraPedido { get; set; }//Verificar se todos pedidos marcam a data também
        public DateTime? DataHoraPedido { get; set; }
        public DadosClienteCadastroDados DadosCliente { get; set; }
        public EnderecoEntregaClienteCadastroDados EnderecoEntrega { get; set; }
        public List<PedidoProdutosPedidoDados> ListaProdutos { get; set; }
        public decimal TotalFamiliaParcelaRA { get; set; }
        public short PermiteRAStatus { get; set; }
        public string OpcaoPossuiRA { get; set; }
        public float? PercRT { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public DetalhesNFPedidoPedidoDados DetalhesNF { get; set; }
        public DetalhesFormaPagamentosDados DetalhesFormaPagto { get; set; }
        public List<ProdutoDevolvidoPedidoDados> ListaProdutoDevolvido { get; set; }
        public List<PedidoPerdasPedidoDados> ListaPerdas { get; set; }
        public List<OcorrenciasPedidoDados> ListaOcorrencia { get; set; }
        public List<BlocoNotasPedidoDados> ListaBlocoNotas { get; set; }
        public List<BlocoNotasDevolucaoMercadoriasPedidoDados> ListaBlocoNotasDevolucao { get; set; }

    }
}
