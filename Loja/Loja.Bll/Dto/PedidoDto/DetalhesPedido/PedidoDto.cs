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
        public bool OpcaoPossuiRA { get; set; }
        public float PercRT { get; set; }
        public decimal ValorTotalDestePedidoComRA { get; set; }
        public decimal VlTotalDestePedido { get; set; }
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

        public static Pedido.Dados.Criacao.PedidoCriacaoDados PedidoCriacaoDados_De_PedidoDto(PedidoDto pedidoDto,
            string lojaUsuario, string usuario, bool vendedorExterno)
        {
            if (pedidoDto == null) return null;
            var ret = new Pedido.Dados.Criacao.PedidoCriacaoDados()
            {
                LojaUsuario = lojaUsuario,
                Usuario = usuario,
                DadosCliente = DadosClienteCadastroDto.DadosClienteCadastroDados_De_DadosClienteCadastroDto(pedidoDto.DadosCliente),
                EnderecoCadastralCliente = DadosClienteCadastroDto.EnderecoCadastralClientePrepedidoDados_De_DadosClienteCadastroDto(pedidoDto.DadosCliente),
                EnderecoEntrega = EnderecoEntregaDtoClienteCadastro.EnderecoEntregaClienteCadastroDados_De_EnderecoEntregaDtoClienteCadastro(pedidoDto.EnderecoEntrega),
                ListaProdutos = PedidoProdutosDtoPedido.List_PedidoProdutoPedidoDados_De_PedidoProdutosDtoPedido(pedidoDto.ListaProdutos),
                FormaPagtoCriacao = pedidoDto.FormaPagtoCriacao,
                DetalhesPedido = DetalhesNFPedidoDtoPedido.DetalhesPrepedidoDados_De_DetalhesNFPedidoDtoPedido(pedidoDto.DetalhesNF),
                ComIndicador = pedidoDto.ComIndicador != 0,
                NomeIndicador = pedidoDto.NomeIndicador,
                PercRT = pedidoDto.PercRT,
                OpcaoPossuiRa = pedidoDto.OpcaoPossuiRA,
                IdNfeSelecionadoManual = pedidoDto.CDManual,
                //todo: corrigir
                VendedorExterno = vendedorExterno ? "" : "Vendedor externo",
                OpcaoVendaSemEstoque = pedidoDto.OpcaoVendaSemEstoque,
                Vl_total = pedidoDto.VlTotalDestePedido,
                Vl_total_NF = pedidoDto.ValorTotalDestePedidoComRA,
                PermiteRAStatus = pedidoDto.PermiteRAStatus
            };
            return ret;
        }
    }
}
