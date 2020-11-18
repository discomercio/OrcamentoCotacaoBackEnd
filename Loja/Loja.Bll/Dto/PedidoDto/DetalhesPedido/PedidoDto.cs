using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.Dto.PrepedidoDto.DetalhesPrepedido;
using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    //Esse DTO é para mostrar o pedido já criado
    public class PedidoDto
    {
        public string NumeroPedido { get; set; }
        public List<List<string>> Lista_NumeroPedidoFilhote { get; set; }
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
                PercRT = origem.PercRT ?? 0,
                ValorTotalDestePedidoComRA = origem.ValorTotalDestePedidoComRA ?? 0,
                VlTotalDestePedido = origem.VlTotalDestePedido ?? 0,
                DetalhesNF = DetalhesNFPedidoDtoPedido.DetalhesNFPedidoDtoPedido_De_DetalhesNFPedidoPedidoDados(origem.DetalhesNF),
                DetalhesFormaPagto = DetalhesFormaPagamentos.DetalhesFormaPagamentos_De_DetalhesFormaPagamentosDados(origem.DetalhesFormaPagto),
                ListaProdutoDevolvido = ProdutoDevolvidoDtoPedido.ListaProdutoDevolvidoDtoPedido_De_ProdutoDevolvidoPedidoDados(origem.ListaProdutoDevolvido),
                ListaPerdas = PedidoPerdasDtoPedido.ListaPedidoPerdasDtoPedido_De_PedidoPerdasPedidoDados(origem.ListaPerdas),
                ListaOcorrencia = OcorrenciasDtoPedido.ListaOcorrenciasDtoPedido_De_OcorrenciasPedidoDados(origem.ListaOcorrencia),
                ListaBlocoNotas = BlocoNotasDtoPedido.ListaBlocoNotasDtoPedido_De_BlocoNotasPedidoDados(origem.ListaBlocoNotas),
                ListaBlocoNotasDevolucao = BlocoNotasDevolucaoMercadoriasDtoPedido.ListaBlocoNotasDevolucaoMercadoriasDtoPedido_De_BlocoNotasDevolucaoMercadoriasPedidoDados(origem.ListaBlocoNotasDevolucao)
            };
        }

        public static Pedido.Dados.Criacao.PedidoCriacaoDados PedidoCriacaoDados_De_PedidoDto(PedidoDto pedidoDto,
            string lojaUsuario, string usuario, bool venda_externa, decimal limiteArredondamento,
            decimal maxErroArredondamento, string pedido_bs_x_ac, string marketplace_codigo_origem, string pedido_bs_x_marketplace,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavelCadastro)
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
                PercRT = (float)pedidoDto.PercRT,
                OpcaoPossuiRa = pedidoDto.OpcaoPossuiRA == "S" ? true : false,
                IdNfeSelecionadoManual = pedidoDto.CDManual,
                Venda_Externa = venda_externa,
                OpcaoVendaSemEstoque = pedidoDto.OpcaoVendaSemEstoque,
                Vl_total = pedidoDto.VlTotalDestePedido,
                Vl_total_NF = pedidoDto.ValorTotalDestePedidoComRA,
                PermiteRAStatus = pedidoDto.PermiteRAStatus,
                LimiteArredondamento = limiteArredondamento,
                MaxErroArredondamento = maxErroArredondamento,
                Pedido_bs_x_ac = pedido_bs_x_ac,
                Marketplace_codigo_origem = marketplace_codigo_origem,
                Pedido_bs_x_marketplace = pedido_bs_x_marketplace,
                SistemaResponsavelCadastro = sistemaResponsavelCadastro,
            };
            return ret;
        }
    }
}
