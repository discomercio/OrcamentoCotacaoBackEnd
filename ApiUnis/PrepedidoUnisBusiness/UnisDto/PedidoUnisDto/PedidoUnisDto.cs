using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class PedidoUnisDto
    {
        public string NumeroPedido { get; set; }
        // estou incluindo esse campo para trazer o pedido filhote ao visualizar o pedido pai
        public List<List<string>> Lista_NumeroPedidoFilhote { get; set; }
        public StatusPedidoUnisDto StatusHoraPedido { get; set; }//Verificar se todos pedidos marcam a data também
        public DateTime? DataHoraPedido { get; set; }
        public DadosClienteCadastroUnisDto DadosCliente { get; set; }
        public EnderecoEntregaClienteCadastroUnisDto EnderecoEntrega { get; set; }
        public List<PedidoProdutosUnisDto> ListaProdutos { get; set; }
        public decimal TotalFamiliaParcelaRA { get; set; }
        public short PermiteRAStatus { get; set; }
        public bool OpcaoPossuiRA { get; set; }
        public float? PercRT { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public DetalhesNFPedidoUnisDto DetalhesNF { get; set; }
        public DetalhesFormaPagamentosUnisDto DetalhesFormaPagto { get; set; }
        public List<ProdutoDevolvidoUnisDto> ListaProdutoDevolvido { get; set; }
        public List<PedidoPerdasUnisDto> ListaPerdas { get; set; }
        public List<OcorrenciasUnisDto> ListaOcorrencia { get; set; }
        public List<BlocoNotasUnisDto> ListaBlocoNotas { get; set; }
        public List<BlocoNotasDevolucaoMercadoriasUnisDto> ListaBlocoNotasDevolucao { get; set; }

        //iremos receber a parte dadoscliente e endereco entrega
        public static PedidoUnisDto PedidoUnisDto_De_PedidoDados(PedidoDados origem, 
            PrepedidoBusiness.Dto.ClienteCadastro.DadosClienteCadastroDto dadosCliente,
            PrepedidoBusiness.Dto.ClienteCadastro.EnderecoEntregaDtoClienteCadastro enderecoEntrega)
        {
            if (origem == null) return null;
            return new PedidoUnisDto()
            {
                NumeroPedido = origem.NumeroPedido,
                Lista_NumeroPedidoFilhote = origem.Lista_NumeroPedidoFilhote,
                StatusHoraPedido = StatusPedidoUnisDto.StatusPedidoUnisDto_De_StatusPedidoPedidoDados(origem.StatusHoraPedido),
                DataHoraPedido = origem.DataHoraPedido,
                DadosCliente = DadosClienteCadastroUnisDto.DadosClienteCadastroUnisDtoDeDadosClienteCadastroDto(dadosCliente),
                EnderecoEntrega = EnderecoEntregaClienteCadastroUnisDto.EnderecoEntregaClienteCadastroUnisDtoDeEnderecoEntregaDtoClienteCadastro(enderecoEntrega),
                ListaProdutos = PedidoProdutosUnisDto.ListaPedidoProdutosUnisDto_De_PedidoProdutosPedidoDados(origem.ListaProdutos),
                TotalFamiliaParcelaRA = origem.TotalFamiliaParcelaRA,
                PermiteRAStatus = origem.PermiteRAStatus,
                OpcaoPossuiRA = origem.OpcaoPossuiRA,
                PercRT = origem.PercRT,
                ValorTotalDestePedidoComRA = origem.ValorTotalDestePedidoComRA,
                VlTotalDestePedido = origem.VlTotalDestePedido,
                DetalhesNF = DetalhesNFPedidoUnisDto.DetalhesNFPedidoUnisDto_De_DetalhesNFPedidoPedidoDados(origem.DetalhesNF),
                DetalhesFormaPagto = DetalhesFormaPagamentosUnisDto.DetalhesFormaPagamentosUnisDto_De_DetalhesFormaPagamentosDados(origem.DetalhesFormaPagto),
                ListaProdutoDevolvido = ProdutoDevolvidoUnisDto.ListaProdutoDevolvidoUnisDto_De_ProdutoDevolvidoPedidoDados(origem.ListaProdutoDevolvido),
                ListaPerdas = PedidoPerdasUnisDto.ListaPedidoPerdasUnisDto_De_PedidoPerdasPedidoDados(origem.ListaPerdas),
                ListaOcorrencia = OcorrenciasUnisDto.ListaOcorrenciasUnisDto_De_OcorrenciasPedidoDados(origem.ListaOcorrencia),
                ListaBlocoNotas = BlocoNotasUnisDto.ListaBlocoNotasUnisDto_De_BlocoNotasPedidoDados(origem.ListaBlocoNotas),
                ListaBlocoNotasDevolucao = BlocoNotasDevolucaoMercadoriasUnisDto.ListaBlocoNotasDevolucaoMercadoriasUnisDto_De_BlocoNotasDevolucaoMercadoriasPedidoDados(origem.ListaBlocoNotasDevolucao)
            };
        }

    }
}
