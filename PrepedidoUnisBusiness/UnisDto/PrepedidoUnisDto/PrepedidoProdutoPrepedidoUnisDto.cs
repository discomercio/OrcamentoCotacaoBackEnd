﻿using InfraBanco.Constantes;
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

        public short? Qtde { get; set; }

        public float? Desc_Dado { get; set; }// = Desconto

        [Required]
        public decimal Preco_Venda { get; set; }// = VlUnitario

        public decimal? Preco_Fabricante { get; set; }

        [Required]
        public decimal Preco_Lista { get; set; }// = VlLista

        public decimal? Preco_NF { get; set; } // se permite RA = Preco_Lista / senão VlUnitario

        [Required]
        public float CustoFinancFornecCoeficiente { get; set; }

        public decimal CustoFinancFornecPrecoListaBase { get; set; } //recebe Preco_Lista

        public static PrePedidoProdutoPrePedidoUnisDto PrePedidoProdutoPrePedidoUnisDtoDePrepedidoProdutoDtoPrepedido(PrepedidoProdutoDtoPrepedido produtoDto,
            float CustoFinancFornecCoeficiente)
        {
            var ret = new PrePedidoProdutoPrePedidoUnisDto()
            {
                Fabricante = produtoDto.Fabricante,
                Produto = produtoDto.NumProduto,
                Qtde = produtoDto.Qtde,
                Desc_Dado = produtoDto.Desconto,
                Preco_Venda = produtoDto.VlUnitario,
                Preco_Fabricante = produtoDto.Preco,
                Preco_Lista = produtoDto.VlLista,
                Preco_NF = produtoDto.Permite_Ra_Status == 1 ? produtoDto.Preco_Lista : produtoDto.VlUnitario,
                CustoFinancFornecCoeficiente = CustoFinancFornecCoeficiente,
                CustoFinancFornecPrecoListaBase = (decimal)produtoDto.Preco_Lista
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
                Preco = produtoDto.Preco_Fabricante,
                Preco_Lista = produtoDto.CustoFinancFornecPrecoListaBase,
                VlLista = produtoDto.Preco_Lista,
                Desconto = produtoDto.Desc_Dado,
                VlUnitario = produtoDto.Preco_Venda
            };

            return ret;
        }

    }
}
