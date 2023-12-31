﻿using Prepedido.Dados.DetalhesPrepedido;
using System;
using System.Collections.Generic;

namespace Prepedido.Dto
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
        public int? UsuarioCadastroId { get; set; }
        public short? UsuarioCadastroIdTipoUsuarioContexto { get; set; }
        public string Usuario_cadastro { get; set; }

        public static PrePedidoDto PrePedidoDto_De_PrePedidoDados(PrePedidoDados origem)
        {
            if (origem == null) return null;
            return new PrePedidoDto()
            {
                CorHeader = origem.CorHeader,
                TextoHeader = origem.TextoHeader,
                CanceladoData = origem.CanceladoData,
                NumeroPrePedido = origem.NumeroPrePedido,
                DataHoraPedido = origem.DataHoraPedido,
                Hora_Prepedido = origem.Hora_Prepedido,
                DadosCliente = DadosClienteCadastroDto.DadosClienteCadastroDto_De_DadosClienteCadastroDados(origem.DadosCliente),
                EnderecoCadastroClientePrepedido = EnderecoCadastralClientePrepedidoDto.EnderecoCadastralClientePrepedidoDto_De_EnderecoCadastralClientePrepedidoDados(origem.EnderecoCadastroClientePrepedido),
                EnderecoEntrega = EnderecoEntregaDtoClienteCadastro.EnderecoEntregaDtoClienteCadastro_De_EnderecoEntregaClienteCadastroDados(origem.EnderecoEntrega),
                ListaProdutos = PrepedidoProdutoDtoPrepedido.ListaPrepedidoProdutoDtoPrepedido_De_PrepedidoProdutoPrepedidoDados(origem.ListaProdutos),
                TotalFamiliaParcelaRA = origem.TotalFamiliaParcelaRA,
                PermiteRAStatus = origem.PermiteRAStatus,
                OpcaoPossuiRA = origem.OpcaoPossuiRA,
                CorTotalFamiliaRA = origem.CorTotalFamiliaRA,
                PercRT = origem.PercRT,
                ValorTotalDestePedidoComRA = origem.Vl_total_NF,
                VlTotalDestePedido = origem.Vl_total,
                DetalhesPrepedido = DetalhesDtoPrepedido.DetalhesDtoPrepedido_De_DetalhesPrepedidoDados(origem.DetalhesPrepedido),
                FormaPagto = origem.FormaPagto,
                FormaPagtoCriacao = FormaPagtoCriacaoDto.FormaPagtoCriacaoDto_De_FormaPagtoCriacaoDados(origem.FormaPagtoCriacao),
                St_Orc_Virou_Pedido = origem.St_Orc_Virou_Pedido,
                NumeroPedido = origem.NumeroPedido
            };
        }


        public static PrePedidoDados PrePedidoDados_De_PrePedidoDto(PrePedidoDto origem)
        {
            //Constantes.TipoUsuarioContexto tipoUsuarioContexto
            if (origem == null) return null;

            PrePedidoDados ret = new PrePedidoDados()
            {
                CorHeader = origem.CorHeader,
                TextoHeader = origem.TextoHeader,
                CanceladoData = origem.CanceladoData,
                NumeroPrePedido = origem.NumeroPrePedido,
                DataHoraPedido = origem.DataHoraPedido,
                Hora_Prepedido = origem.Hora_Prepedido,
                DadosCliente = DadosClienteCadastroDto.DadosClienteCadastroDados_De_DadosClienteCadastroDto(origem.DadosCliente),
                EnderecoCadastroClientePrepedido = EnderecoCadastralClientePrepedidoDto.EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClientePrepedidoDto(origem.EnderecoCadastroClientePrepedido),
                EnderecoEntrega = EnderecoEntregaDtoClienteCadastro.EnderecoEntregaClienteCadastroDados_De_EnderecoEntregaDtoClienteCadastro(origem.EnderecoEntrega),
                ListaProdutos = PrepedidoProdutoDtoPrepedido.ListaPrepedidoProdutoPrepedidoDados_De_PrepedidoProdutoDtoPrepedido(origem.ListaProdutos, origem.PermiteRAStatus),
                TotalFamiliaParcelaRA = origem.TotalFamiliaParcelaRA,
                PermiteRAStatus = origem.PermiteRAStatus,
                OpcaoPossuiRA = origem.OpcaoPossuiRA,
                CorTotalFamiliaRA = origem.CorTotalFamiliaRA,
                PercRT = origem.PercRT,
                Vl_total_NF = origem.ValorTotalDestePedidoComRA ?? 0,
                Vl_total = origem.VlTotalDestePedido ?? 0,
                DetalhesPrepedido = DetalhesDtoPrepedido.DetalhesPrepedidoDados_De_DetalhesDtoPrepedido(origem.DetalhesPrepedido),
                FormaPagto = origem.FormaPagto,
                FormaPagtoCriacao = FormaPagtoCriacaoDto.FormaPagtoCriacaoDados_De_FormaPagtoCriacaoDto(origem.FormaPagtoCriacao),
                St_Orc_Virou_Pedido = origem.St_Orc_Virou_Pedido,
                NumeroPedido = origem.NumeroPedido,
                UsuarioCadastroId = origem.UsuarioCadastroId,//Id do registro do usuário logado (t_ORCAMENTISTA_E_INDICADOR_VENDEDOR.Id, t_ORCAMENTISTA_E_INDICADOR.Id ou t_USUARIO.Id) Se aprovação pelo cliente em rota pública deve ser nulo
                UsuarioCadastroIdTipoUsuarioContexto = origem.UsuarioCadastroIdTipoUsuarioContexto,//Id do contexto do usuário logado de acordo com a tabela t_CFG_TIPO_USUARIO_CONTEXTO.Id Se aprovação pelo cliente em rota pública preencher com 4
                Usuario_cadastro = origem.Usuario_cadastro,//preencher com "[N] 999999", onde  N = UsuarioCadastroIdTipoUsuarioContexto  999999 = UsuarioCadastroId

            };

            return ret;
        }
    }
}
