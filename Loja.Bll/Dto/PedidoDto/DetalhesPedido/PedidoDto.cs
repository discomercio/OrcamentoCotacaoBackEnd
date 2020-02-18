using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.Dto.PrepedidoDto.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    //Esse DTO é para mostrar o pedido já criado
    public class PedidoDto
    {
        public string NumeroPedido { get; set; }
        public StatusPedidoDtoPedido StatusHoraPedido { get; set; }//Verificar se todos pedidos marcam a data também
        public DateTime? DataHoraPedido { get; set; }
        public DadosClienteCadastroDto DadosCliente { get; set; }
        public EnderecoEntregaDtoClienteCadastro EnderecoEntrega { get; set; }
        public List<PedidoProdutosDtoPedido> ListaProdutos { get; set; }
        public int CDSelecionado { get; set; }
        public short CDManual { get; set; }
        public decimal TotalFamiliaParcelaRA { get; set; }
        public short PermiteRAStatus { get; set; }
        public string OpcaoPossuiRA { get; set; }
        public float? PercRT { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public FormaPagtoCriacaoDto FormaPagtoCriacao { get; set; }
        public int ComIndicador { get; set; }
        public string NomeIndicador { get; set; }
        public DetalhesNFPedidoDtoPedido DetalhesNF { get; set; }
        public bool OpcaoVendaSemEstoque { get; set; }

        //daqui para baixo antigo
        public DetalhesFormaPagamentos DetalhesFormaPagto { get; set; }
        public List<ProdutoDevolvidoDtoPedido> ListaProdutoDevolvido { get; set; }
        public List<PedidoPerdasDtoPedido> ListaPerdas { get; set; }
        public List<OcorrenciasDtoPedido> ListaOcorrencia { get; set; }
        public List<BlocoNotasDtoPedido> ListaBlocoNotas { get; set; }
        public List<BlocoNotasDevolucaoMercadoriasDtoPedido> ListaBlocoNotasDevolucao { get; set; }
        public string PedBonshop { get; set; }
    }
}
