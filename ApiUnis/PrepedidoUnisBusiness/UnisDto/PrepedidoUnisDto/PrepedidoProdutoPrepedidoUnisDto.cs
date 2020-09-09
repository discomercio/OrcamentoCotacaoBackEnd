using InfraBanco.Constantes;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class PrePedidoProdutoPrePedidoUnisDto
    {
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Required]
        [MaxLength(8)]
        public string Produto { get; set; } //  = NumProduto

        [Required]
        public short Qtde { get; set; }

        [Required]
        public float Desc_Dado { get; set; }// = Desconto

        /// <summary>
        /// Preco_Venda = (Preco_Fabricante * CustoFinancFornecCoeficiente) * (1 - Desc_Dado / 100)
        /// </summary>
        [Required]
        public decimal Preco_Venda { get; set; }// = VlUnitario

        [Required]
        public decimal NormalizacaoCampos_CustoFinancFornecPrecoListaBase { get; set; }

        /// <summary>
        /// Preco_NF = PrePedidoUnisDto.PermiteRAStatus == true ? Preco_NF : Preco_Venda
        /// </summary>
        [Required]
        public decimal Preco_NF { get; set; } // se permite RA = Preco_NF / senão VlUnitario

        /// <summary>
        /// Caso seja pagamento a vista, deve ser 1. Caso contrário, o coeficiente do fabricante para a quantidade de parcelas e forma de pagamento.
        /// </summary>
        [Required]
        public float CustoFinancFornecCoeficiente { get; set; }

        /// <summary>
        /// CustoFinancFornecPrecoListaBase = Preco_Fabricante * CustoFinancFornecCoeficiente
        /// </summary>
        [Required]
        public decimal NormalizacaoCampos_Preco_Lista { get; set; } //recebe Preco_Lista

        public static PrePedidoProdutoPrePedidoUnisDto PrePedidoProdutoPrePedidoUnisDtoDePrepedidoProdutoDtoPrepedido(PrepedidoProdutoDtoPrepedido produtoDto,
            float CustoFinancFornecCoeficiente)
        {
            var ret = new PrePedidoProdutoPrePedidoUnisDto()
            {
                Fabricante = produtoDto.Fabricante,
                Produto = produtoDto.NumProduto,
                Qtde = produtoDto.Qtde.HasValue ? produtoDto.Qtde.Value : (short)1,
                Desc_Dado = produtoDto.Desconto.HasValue ? produtoDto.Desconto.Value : 0,
                Preco_Venda = produtoDto.VlUnitario,
                NormalizacaoCampos_CustoFinancFornecPrecoListaBase = produtoDto.Preco.HasValue ? produtoDto.Preco.Value : 0,
                Preco_NF = produtoDto.Permite_Ra_Status == 1 ? (produtoDto.Preco_Lista.HasValue ? produtoDto.Preco_Lista.Value : 0) : produtoDto.VlUnitario,
                CustoFinancFornecCoeficiente = CustoFinancFornecCoeficiente,
                NormalizacaoCampos_Preco_Lista = produtoDto.VlLista
            };

            return ret;
        }

        public static PrepedidoProdutoDtoPrepedido PrepedidoProdutoDtoPrepedidoDePrePedidoProdutoPrePedidoUnisDto(PrePedidoProdutoPrePedidoUnisDto produtoDto,
            short permiteRaStatus)
        {
            var ret = new PrepedidoProdutoDtoPrepedido()
            {
                Fabricante = produtoDto.Fabricante,
                NumProduto = produtoDto.Produto,
                Qtde = produtoDto.Qtde,
                Permite_Ra_Status = permiteRaStatus,
                BlnTemRa = produtoDto.Preco_NF != produtoDto.Preco_Venda ? true : false,
                Preco = produtoDto.NormalizacaoCampos_CustoFinancFornecPrecoListaBase,
                VlLista = produtoDto.NormalizacaoCampos_Preco_Lista,
                Desconto = produtoDto.Desc_Dado,
                VlUnitario = produtoDto.Preco_Venda,
                TotalItem = Math.Round((decimal)(produtoDto.Preco_Venda * produtoDto.Qtde), 2),
                TotalItemRA = Math.Round((decimal)(produtoDto.Preco_NF * produtoDto.Qtde), 2),
                CustoFinancFornecCoeficiente = produtoDto.CustoFinancFornecCoeficiente,
                Preco_NF = produtoDto.Preco_NF
            };

            return ret;
        }

        public static Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados PrepedidoProdutoPrepedidoDadosDePrePedidoProdutoPrePedidoUnisDto(PrePedidoProdutoPrePedidoUnisDto produtoDto,
            short permiteRaStatus)
        {
            var ret = new Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados()
            {
                Fabricante = produtoDto.Fabricante,
                NumProduto = produtoDto.Produto,
                Qtde = produtoDto.Qtde,
                Permite_Ra_Status = permiteRaStatus,
                BlnTemRa = produtoDto.Preco_NF != produtoDto.Preco_Venda ? true : false,
                Preco = produtoDto.NormalizacaoCampos_CustoFinancFornecPrecoListaBase,
                VlLista = produtoDto.NormalizacaoCampos_Preco_Lista,
                Desconto = produtoDto.Desc_Dado,
                VlUnitario = produtoDto.Preco_Venda,
                TotalItem = Math.Round((decimal)(produtoDto.Preco_Venda * produtoDto.Qtde), 2),
                TotalItemRA = Math.Round((decimal)(produtoDto.Preco_NF * produtoDto.Qtde), 2),
                CustoFinancFornecCoeficiente = produtoDto.CustoFinancFornecCoeficiente,
                Preco_NF = produtoDto.Preco_NF
            };

            return ret;
        }
    }
}
