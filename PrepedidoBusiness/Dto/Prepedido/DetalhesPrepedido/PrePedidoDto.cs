using PrepedidoBusiness.Dto.ClienteCadastro;
using System.Collections.Generic;

namespace PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido
{
    public class PrePedidoDto
    {
        public string CorHeader { get; set; }
        public string TextoHeader { get; set; }
        public string CanceladoData { get; set; }
        public string NumeroPrePedido { get; set; }
        
        public string DataHoraPedido { get; set; }
        public string Hora_Prepedido { get; set; }
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public EnderecoCadastralClientePrepedidoDto EnderecoCadastroClientePrepedido { get; set; }
        public EnderecoEntregaDtoClienteCadastro EnderecoEntrega { get; set; }
        public List<PrepedidoProdutoDtoPrepedido> ListaProdutos { get; set; }
        public decimal TotalFamiliaParcelaRA { get; set; }
        public short PermiteRAStatus { get; set; }
        public string OpcaoPossuiRA { get; set; }
        public string CorTotalFamiliaRA { get; set; }
        public float? PercRT { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public DetalhesDtoPrepedido DetalhesPrepedido { get; set; }
        public List<string> FormaPagto { get; set; }
        public FormaPagtoCriacaoDto FormaPagtoCriacao { get; set; }
        public bool St_Orc_Virou_Pedido { get; set; }//se virou pedido retornar esse campo
        public string NumeroPedido { get; set; }//se virou pedido retornar esse campo
    }
}