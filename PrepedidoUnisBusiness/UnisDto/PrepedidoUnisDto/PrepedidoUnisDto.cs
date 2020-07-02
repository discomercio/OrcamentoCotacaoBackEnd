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

        public EnderecoCadastralClientePrepedidoUnisDto EnderecoCadastralCliente { get; set; }

        [Required]
        public bool OutroEndereco { get; set; }
        public EnderecoEntregaClienteCadastroUnisDto EnderecoEntrega { get; set; }
        public List<PrePedidoProdutoPrePedidoUnisDto> ListaProdutos { get; set; }
        public bool PermiteRAStatus { get; set; }
        public decimal? ValorTotalDestePedidoComRA { get; set; }
        public decimal? VlTotalDestePedido { get; set; }
        public DetalhesPrePedidoUnisDto DetalhesPrepedido { get; set; }
        public FormaPagtoCriacaoUnisDto FormaPagtoCriacao { get; set; }

        [Required]
        public float Perc_Desagio_RA_Liquida { get; set; }


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
                DadosCliente = dadosClienteCadastroDto
            };

            return ret;
        }


    }
}
