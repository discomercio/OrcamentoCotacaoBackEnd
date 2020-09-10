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
        public float Desc_Dado { get; set; }// = NormalizacaoCampos_Desc_Dado

        /// <summary>
        /// Preco_Venda = (CustoFinancFornecPrecoListaBase * CustoFinancFornecCoeficiente) * (1 - Desc_Dado / 100)
        /// </summary>
        [Required]
        public decimal Preco_Venda { get; set; }

        [Required]
        public decimal NormalizacaoCampos_CustoFinancFornecPrecoListaBase { get; set; }

        /// <summary>
        /// Preco_NF = PrePedidoUnisDto.PermiteRAStatus == true ? Preco_NF : Preco_Venda
        /// </summary>
        [Required]
        public decimal Preco_NF { get; set; } // se permite RA = Preco_NF / senão Preco_Venda

        /// <summary>
        /// Caso seja pagamento a vista, deve ser 1. Caso contrário, o coeficiente do fabricante para a quantidade de parcelas e forma de pagamento.
        /// </summary>
        [Required]
        public float CustoFinancFornecCoeficiente { get; set; }

        /// <summary>
        /// CustoFinancFornecPrecoListaBase = CustoFinancFornecPrecoListaBase * CustoFinancFornecCoeficiente
        /// </summary>
        [Required]
        public decimal NormalizacaoCampos_Preco_Lista { get; set; } //recebe Preco_Lista

        public static PrePedidoProdutoPrePedidoUnisDto PrePedidoProdutoPrePedidoUnisDtoDePrepedidoProdutoDtoPrepedido(PrepedidoProdutoDtoPrepedido produtoDto,
            float CustoFinancFornecCoeficiente)
        {
            var ret = new PrePedidoProdutoPrePedidoUnisDto()
            {
                Fabricante = produtoDto.Fabricante,
                Produto = produtoDto.NormalizacaoCampos_Produto,
                Qtde = produtoDto.Qtde.HasValue ? produtoDto.Qtde.Value : (short)1,
                Desc_Dado = produtoDto.NormalizacaoCampos_Desc_Dado.HasValue ? produtoDto.NormalizacaoCampos_Desc_Dado.Value : 0,
                Preco_Venda = produtoDto.NormalizacaoCampos_Preco_Venda,
                NormalizacaoCampos_CustoFinancFornecPrecoListaBase = produtoDto.NormalizacaoCampos_CustoFinancFornecPrecoListaBase.HasValue ? produtoDto.NormalizacaoCampos_CustoFinancFornecPrecoListaBase.Value : 0,
                Preco_NF = produtoDto.Permite_Ra_Status == 1 ? (produtoDto.NormalizacaoCampos_Preco_NF.HasValue ? produtoDto.NormalizacaoCampos_Preco_NF.Value : 0) : produtoDto.NormalizacaoCampos_Preco_Venda,
                CustoFinancFornecCoeficiente = CustoFinancFornecCoeficiente,
                NormalizacaoCampos_Preco_Lista = produtoDto.NormalizacaoCampos_Preco_Lista
            };

            return ret;
        }

        public static PrepedidoProdutoDtoPrepedido PrepedidoProdutoDtoPrepedidoDePrePedidoProdutoPrePedidoUnisDto(PrePedidoProdutoPrePedidoUnisDto produtoDto,
            short permiteRaStatus)
        {
            var ret = new PrepedidoProdutoDtoPrepedido()
            {
                Fabricante = produtoDto.Fabricante,
                NormalizacaoCampos_Produto = produtoDto.Produto,
                Qtde = produtoDto.Qtde,
                Permite_Ra_Status = permiteRaStatus,
                BlnTemRa = produtoDto.Preco_NF != produtoDto.Preco_Venda ? true : false,
                NormalizacaoCampos_CustoFinancFornecPrecoListaBase = produtoDto.NormalizacaoCampos_CustoFinancFornecPrecoListaBase,
                NormalizacaoCampos_Preco_Lista = produtoDto.NormalizacaoCampos_Preco_Lista,
                NormalizacaoCampos_Desc_Dado = produtoDto.Desc_Dado,
                NormalizacaoCampos_Preco_Venda = produtoDto.Preco_Venda,
                TotalItem = Math.Round((decimal)(produtoDto.Preco_Venda * produtoDto.Qtde), 2),
                TotalItemRA = Math.Round((decimal)(produtoDto.Preco_NF * produtoDto.Qtde), 2),
                CustoFinancFornecCoeficiente = produtoDto.CustoFinancFornecCoeficiente,
                NormalizacaoCampos_Preco_NF = produtoDto.Preco_NF
            };

            return ret;
        }

        public static Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados PrepedidoProdutoPrepedidoDadosDePrePedidoProdutoPrePedidoUnisDto(PrePedidoProdutoPrePedidoUnisDto produtoDto,
            short permiteRaStatus)
        {
            var ret = new Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados()
            {
                Fabricante = produtoDto.Fabricante,
                NormalizacaoCampos_Produto = produtoDto.Produto,
                Qtde = produtoDto.Qtde,
                Permite_Ra_Status = permiteRaStatus,
                BlnTemRa = produtoDto.Preco_NF != produtoDto.Preco_Venda ? true : false,
                NormalizacaoCampos_CustoFinancFornecPrecoListaBase = produtoDto.NormalizacaoCampos_CustoFinancFornecPrecoListaBase,
                NormalizacaoCampos_Preco_Lista = produtoDto.NormalizacaoCampos_Preco_Lista,
                NormalizacaoCampos_Desc_Dado = produtoDto.Desc_Dado,
                NormalizacaoCampos_Preco_Venda = produtoDto.Preco_Venda,
                TotalItem = Math.Round((decimal)(produtoDto.Preco_Venda * produtoDto.Qtde), 2),
                TotalItemRA = Math.Round((decimal)(produtoDto.Preco_NF * produtoDto.Qtde), 2),
                CustoFinancFornecCoeficiente = produtoDto.CustoFinancFornecCoeficiente,
                Preco_NF = produtoDto.Preco_NF
            };

            return ret;
        }
    }
}
