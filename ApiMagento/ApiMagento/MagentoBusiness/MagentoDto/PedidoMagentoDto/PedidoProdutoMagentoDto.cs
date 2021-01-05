﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PedidoProdutoMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; } //  = NumProduto
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [Required]
        public short Qtde { get; set; }

        /// <summary>
        /// Preço de venda do item sem o rateio do frete
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_Venda { get; set; }

        /// <summary>
        /// Preco_NF preço que será impresso na nota fiscal, inclui o rateio do frete
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_NF { get; set; } // Caso RA = False,   "Preco_NF"  deve ser  = "Preco_Venda"

        /*
         * Os campos Preco_Fabricante, CustoFinancFornecCoeficiente, CustoFinancFornecPrecoListaBase e Preco_Fabricante vamos ler das tabelas
         * Os campos Preco_Lista e Desc_Dado serão preenchidos por nós e devemos calcular de forma que fiquem consistentes.
        */

        public static Pedido.Dados.Criacao.PedidoProdutoPedidoDados ProdutosDePedidoProdutoMagentoDto(
            PedidoProdutoMagentoDto produtoDto, Produto.Dados.ProdutoDados produtoDados, float coeficiente)
        {
            var ret = new Pedido.Dados.Criacao.PedidoProdutoPedidoDados()
            {
                Fabricante = produtoDto.Fabricante,
                Produto = produtoDto.Produto,
                Qtde = produtoDto.Qtde,
                CustoFinancFornecPrecoListaBase_Conferencia = produtoDados.Preco_lista ?? 0,
                Preco_Lista = Math.Round((decimal)(produtoDados.Preco_lista ?? 0 * (decimal)coeficiente), 2),
                Desc_Dado = 0, //produtoDto.Desc_Dado,
                Preco_Venda = produtoDto.Preco_Venda,
                Preco_NF = produtoDto.Preco_NF,
                CustoFinancFornecCoeficiente_Conferencia = coeficiente
            };

            return ret;
        }
    }
}
