using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dtos.ClienteCadastro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrepedidoBusiness.Dtos.Prepedido.DetalhesPrepedido
{
    public class PrePedidoDto
    {
        public string NumeroPrePedido { get; set; }
        //public StatusPedidoDtoPedido StatusHoraPedido { get; set; }//Verificar se todos pedidos marcam a data também
        public DateTime? DataHoraPedido { get; set; }
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public EnderecoEntregaDtoClienteCadastro EnderecoEntrega { get; set; }
        public List<PrepedidoProdutoDtoPrepedido> ListaProdutos { get; set; }
        public decimal TotalFamiliaParcelaRA { get; set; }
        public short PermiteRAStatus { get; set; }
        public string OpcaoPossuiRA { get; set; }
        public float? PercRT { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }

        public string Obs { get; set; }
        //public DetalhesNFPedidoDtoPedido DetalhesNF { get; set; }
        //public DetalhesFormaPagamentos DetalhesFormaPagto { get; set; }
        public DetalhesDtoPrepedido DetalhesFormaPagto { get; set; }
        public List<string> FormaPagto { get; set; }//analisar se uso direto aqui ou pelo DetalhesDtoPrepedido
    }
}