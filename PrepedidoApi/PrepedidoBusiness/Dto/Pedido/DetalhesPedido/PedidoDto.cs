using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
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
        public List<ProdutoDevolvidoDtoPedido> ListaProdutoDevolvido { get; set; }
        public List<PedidoPerdasDtoPedido> ListaPerdas { get; set; }
        public List<OcorrenciasDtoPedido> ListaOcorrencia { get; set; }
        public List<BlocoNotasDtoPedido> ListaBlocoNotas { get; set; }
        public List<BlocoNotasDevolucaoMercadoriasDtoPedido> ListaBlocoNotasDevolucao { get; set; }

        public static PedidoDto PedidoDto_De_PedidoDados(PedidoDados origem)
        {
            if (origem == null) return null;
            return new PedidoDto()
            {
                NumeroPedido = origem.NumeroPedido,
                Lista_NumeroPedidoFilhote = origem.Lista_NumeroPedidoFilhote,
                StatusHoraPedido = StatusPedidoDtoPedido.StatusPedidoDtoPedido_De_StatusPedidoPedidoDados(origem.StatusHoraPedido),
                DataHoraPedido = origem.DataHoraPedido,
                DadosCliente = DadosClienteCadastroDto.DadosClienteCadastroDto_De_DadosClienteCadastroDados(origem.DadosCliente),
                EnderecoEntrega = EnderecoEntregaDtoClienteCadastro.EnderecoEntregaDtoClienteCadastro_De_EnderecoEntregaClienteCadastroDados(origem.EnderecoEntrega),
                ListaProdutos = PedidoProdutosDtoPedido.ListaPedidoProdutosDtoPedido_De_PedidoProdutosPedidoDados(origem.ListaProdutos),
                TotalFamiliaParcelaRA = origem.TotalFamiliaParcelaRA,
                PermiteRAStatus = origem.PermiteRAStatus,
                OpcaoPossuiRA = origem.OpcaoPossuiRA,
                PercRT = origem.PercRT,
                ValorTotalDestePedidoComRA = origem.ValorTotalDestePedidoComRA,
                VlTotalDestePedido = origem.VlTotalDestePedido,
                DetalhesNF = DetalhesNFPedidoDtoPedido.DetalhesNFPedidoDtoPedido_De_DetalhesNFPedidoPedidoDados(origem.DetalhesNF),
                DetalhesFormaPagto = DetalhesFormaPagamentos.DetalhesFormaPagamentos_De_DetalhesFormaPagamentosDados(origem.DetalhesFormaPagto),
                ListaProdutoDevolvido = ProdutoDevolvidoDtoPedido.ListaProdutoDevolvidoDtoPedido_De_ProdutoDevolvidoPedidoDados(origem.ListaProdutoDevolvido),
                ListaPerdas = PedidoPerdasDtoPedido.ListaPedidoPerdasDtoPedido_De_PedidoPerdasPedidoDados(origem.ListaPerdas),
                ListaOcorrencia = OcorrenciasDtoPedido.ListaOcorrenciasDtoPedido_De_OcorrenciasPedidoDados(origem.ListaOcorrencia),
                ListaBlocoNotas = BlocoNotasDtoPedido.ListaBlocoNotasDtoPedido_De_BlocoNotasPedidoDados(origem.ListaBlocoNotas),
                ListaBlocoNotasDevolucao = BlocoNotasDevolucaoMercadoriasDtoPedido.ListaBlocoNotasDevolucaoMercadoriasDtoPedido_De_BlocoNotasDevolucaoMercadoriasPedidoDados(origem.ListaBlocoNotasDevolucao)
            };
        }

    }
}
