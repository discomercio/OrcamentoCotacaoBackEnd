﻿using Loja.Bll.Dto.ClienteDto;
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
        public bool OpcaoVendaSemEstoque { get; set; } = true;

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
                OpcaoPossuiRA = origem.OpcaoPossuiRA ? "S" : "",
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
            var ret = new Pedido.Dados.Criacao.PedidoCriacaoDados(
                ambiente: new Pedido.Dados.Criacao.PedidoCriacaoAmbienteDados(
                    loja: lojaUsuario,
                    usuario: usuario,
                    comIndicador: pedidoDto.ComIndicador != 0,
                    indicador_Orcamentista: pedidoDto.NomeIndicador,
                    idNfeSelecionadoManual: pedidoDto.CDManual,
                    venda_Externa: venda_externa,
                    opcaoVendaSemEstoque: pedidoDto.OpcaoVendaSemEstoque,
                    vendedor: usuario
                    ),
                cliente: Pedido.Dados.Criacao.PedidoCriacaoClienteDados.PedidoCriacaoClienteDados_de_DadosClienteCadastroDados(
                    DadosClienteCadastroDto.DadosClienteCadastroDados_De_DadosClienteCadastroDto(pedidoDto.DadosCliente)),
                enderecoCadastralCliente: DadosClienteCadastroDto.EnderecoCadastralClientePrepedidoDados_De_DadosClienteCadastroDto(pedidoDto.DadosCliente),
                enderecoEntrega: EnderecoEntregaDtoClienteCadastro.EnderecoEntregaClienteCadastroDados_De_EnderecoEntregaDtoClienteCadastro(pedidoDto.EnderecoEntrega),
                listaProdutos: PedidoProdutosDtoPedido.List_PedidoProdutoPedidoDados_De_PedidoProdutosDtoPedido(pedidoDto.ListaProdutos),
                formaPagtoCriacao: pedidoDto.FormaPagtoCriacao,
                detalhesPedido: DetalhesNFPedidoDtoPedido.DetalhesPrepedidoDados_De_DetalhesNFPedidoDtoPedido(pedidoDto.DetalhesNF),
                valor: new Pedido.Dados.Criacao.PedidoCriacaoValorDados(
                    percRT: pedidoDto.PercRT ?? 0,
                    opcaoPossuiRa: pedidoDto.OpcaoPossuiRA == "S" ? true : false,
                    vl_total: pedidoDto.VlTotalDestePedido ?? 0,
                    vl_total_NF: pedidoDto.ValorTotalDestePedidoComRA ?? 0,
                    permiteRAStatus: pedidoDto.PermiteRAStatus
                    ),
                configuracao: new Pedido.Dados.Criacao.PedidoCriacaoConfiguracaoDados(
                    limiteArredondamento: limiteArredondamento,
                    maxErroArredondamento: maxErroArredondamento,
                    sistemaResponsavelCadastro: sistemaResponsavelCadastro
                    ),
                marketplace: new Pedido.Dados.Criacao.PedidoCriacaoMarketplaceDados(
                    pedido_bs_x_ac: pedido_bs_x_ac,
                    marketplace_codigo_origem: marketplace_codigo_origem,
                    pedido_bs_x_marketplace: pedido_bs_x_marketplace)
            );
            return ret;
        }
    }
}
