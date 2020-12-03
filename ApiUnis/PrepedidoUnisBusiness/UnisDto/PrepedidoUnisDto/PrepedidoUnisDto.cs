using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoBusiness.Dto.ClienteCadastro;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class PrePedidoUnisDto
    {
        [Required]
        public string TokenAcesso { get; set; }

        [MaxLength(14)]
        [Required]
        public string Cnpj_Cpf { get; set; }

        [Required]
        [MaxLength(20)]
        public string Indicador_Orcamentista { get; set; }

        [Required]
        public EnderecoCadastralClientePrepedidoUnisDto EnderecoCadastralCliente { get; set; }

        [Required]
        public bool OutroEndereco { get; set; }
        public EnderecoEntregaClienteCadastroUnisDto EnderecoEntrega { get; set; }
        [Required]
        public List<PrePedidoProdutoPrePedidoUnisDto> ListaProdutos { get; set; }

        [Required]
        public bool PermiteRAStatus { get; set; }

        /// <summary>
        /// ValorTotalDestePedidoComRA = soma de Preco_NF * Qtde
        /// </summary>
        [Required]
        public decimal ValorTotalDestePedidoComRA { get; set; }

        /// <summary>
        /// VlTotalDestePedido = soma de Preco_Venda * Qtde
        /// </summary>
        [Required]
        public decimal VlTotalDestePedido { get; set; }

        [Required]
        public DetalhesPrePedidoUnisDto DetalhesPrepedido { get; set; }

        [Required]
        public FormaPagtoCriacaoUnisDto FormaPagtoCriacao { get; set; }


        public static PrePedidoDto PrePedidoDtoDePrePedidoUnisDto(PrePedidoUnisDto prepedidoUnis,
            EnderecoCadastralClientePrepedidoDto endCadastraArclube, List<PrepedidoProdutoDtoPrepedido> lstProdutosArclube,
            DadosClienteCadastroDto dadosClienteCadastroDto)
        {
            var ret = new PrePedidoDto()
            {
                EnderecoCadastroClientePrepedido = endCadastraArclube,
                EnderecoEntrega = EnderecoEntregaClienteCadastroUnisDto.
                    EnderecoEntregaDtoClienteCadastroDeEnderecoEntregaClienteCadastroUnisDto(
                    prepedidoUnis.EnderecoEntrega, prepedidoUnis.OutroEndereco),
                ListaProdutos = lstProdutosArclube,
                PermiteRAStatus = Convert.ToInt16(prepedidoUnis.PermiteRAStatus),
                ValorTotalDestePedidoComRA = prepedidoUnis.ValorTotalDestePedidoComRA,
                VlTotalDestePedido = prepedidoUnis.VlTotalDestePedido,
                DetalhesPrepedido = DetalhesPrePedidoUnisDto.
                    DetalhesPrePedidoDtoDeDetalhesPrePedidoUnisDto(prepedidoUnis.DetalhesPrepedido),
                FormaPagtoCriacao = FormaPagtoCriacaoUnisDto.FormaPagtoCriacaoDtoDeFormaPagtoCriacaoUnisDto(
                    prepedidoUnis.FormaPagtoCriacao),
                DadosCliente = dadosClienteCadastroDto,
            };
            ret.DadosCliente.Indicador_Orcamentista = prepedidoUnis.Indicador_Orcamentista;

            return ret;
        }

        public static Prepedido.Dados.DetalhesPrepedido.PrePedidoDados PrePedidoDadosDePrePedidoUnisDto(PrePedidoUnisDto prepedidoUnis,
            Cliente.Dados.EnderecoCadastralClientePrepedidoDados endCadastraDados, 
            List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados> lstProdutosDados,
            Cliente.Dados.DadosClienteCadastroDados dadosClienteCadastroDados)
        {
            var ret = new Prepedido.Dados.DetalhesPrepedido.PrePedidoDados()
            {
                EnderecoCadastroClientePrepedido = endCadastraDados,
                EnderecoEntrega = EnderecoEntregaClienteCadastroUnisDto.
                    EnderecoEntregaClienteCadastroDadosDeEnderecoEntregaClienteCadastroUnisDto(
                    prepedidoUnis.EnderecoEntrega, prepedidoUnis.OutroEndereco),
                ListaProdutos = lstProdutosDados,
                PermiteRAStatus = Convert.ToInt16(prepedidoUnis.PermiteRAStatus),
                Vl_total_NF = prepedidoUnis.ValorTotalDestePedidoComRA,
                Vl_total = prepedidoUnis.VlTotalDestePedido,
                DetalhesPrepedido = DetalhesPrePedidoUnisDto.
                    DetalhesPrepedidoDadosDeDetalhesPrePedidoUnisDto(prepedidoUnis.DetalhesPrepedido),
                FormaPagtoCriacao = FormaPagtoCriacaoUnisDto.FormaPagtoCriacaoDadosDeFormaPagtoCriacaoUnisDto(
                    prepedidoUnis.FormaPagtoCriacao),
                DadosCliente = dadosClienteCadastroDados,
            };
            ret.DadosCliente.Indicador_Orcamentista = prepedidoUnis.Indicador_Orcamentista;

            return ret;
        }
    }
}