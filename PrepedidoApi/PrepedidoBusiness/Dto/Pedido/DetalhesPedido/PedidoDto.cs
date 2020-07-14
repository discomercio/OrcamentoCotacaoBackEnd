using PrepedidoBusiness.Dto.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Pedido.DetalhesPedido
{
    public class PedidoDto
    {
        public string NumeroPedido { get; set; }
        // estou incluindo esse campo para trazer o pedido filhote ao visualizar o pedido pai
        public List<List<string>> Lista_NumeroPedidoFilhote { get; set; }
        public StatusPedidoDtoPedido StatusHoraPedido { get; set; }//Verificar se todos pedidos marcam a data também
        public DateTime? DataHoraPedido { get; set; }
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public EnderecoEntregaDtoClienteCadastro EnderecoEntrega { get; set; }
        public List<PedidoProdutosDtoPedido> ListaProdutos { get; set; }
        public decimal TotalFamiliaParcelaRA { get; set; }
        public short PermiteRAStatus { get; set; }
        public string OpcaoPossuiRA { get; set; }
        public float? PercRT { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public DetalhesNFPedidoDtoPedido DetalhesNF { get; set; }
        public DetalhesFormaPagamentos DetalhesFormaPagto { get; set; }
        public List<ProdutoDevolvidoDtoPedido> ListaProdutoDevolvido{ get; set; }
        public List<PedidoPerdasDtoPedido> ListaPerdas { get; set; }
        public List<OcorrenciasDtoPedido> ListaOcorrencia { get; set; }
        public List<BlocoNotasDtoPedido> ListaBlocoNotas { get; set; }
        public List<BlocoNotasDevolucaoMercadoriasDtoPedido> ListaBlocoNotasDevolucao { get; set; }


    }
}
