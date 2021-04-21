using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    /// <summary>
    /// Os campo estão com os mesmos nomes exibidos no Painel Admin na consulta do Pedido
    /// </summary>
    public class PedidoProdutoMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        /// <summary>
        /// SKU: código do produto
        /// <hr />
        /// </summary>
        [Required]
        [MaxLength(6)]
        public string Sku { get; set; } //tamanho mínimo de 6, vamos normalizar os zeros à esquerda
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        [Required]
        public short Quantidade { get; set; }

        /// <summary>
        /// Subtotal: valor total da linha (todos os produtos da linha) sem desconto aplicado
        /// <hr />
        /// </summary>
        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public float TaxAmount { get; set; }

        /// <summary>
        /// DiscountAmount: valor de desconto
        /// <hr />
        /// </summary>
        [Required]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// RowTotal: valor total da linha (todos os produtos da linha) com desconto aplicado
        /// <hr />
        /// </summary>
        [Required]
        public decimal RowTotal { get; set; }

        /*
         * Os campos Preco_Fabricante, CustoFinancFornecCoeficiente, CustoFinancFornecPrecoListaBase e Preco_Fabricante vamos ler das tabelas
         * Os campos Preco_Lista e Desc_Dado serão preenchidos por nós e devemos calcular de forma que fiquem consistentes.
        */
        /* DETALHES DE COMO FAZER OS CÁLCULOS
         * ==================================
         * preco_nf = RowTotal / Quantidade
         * preco_lista = Subtotal / Quantidade
         * desc_dado = 100 * (preco_lista - preco_venda) / preco_lista
         */
        public static Pedido.Dados.Criacao.PedidoCriacaoProdutoDados PedidoCriacaoProdutoDados_De_PedidoProdutoMagentoDto(
            PedidoProdutoMagentoDto produtoDto, Produto.Dados.ProdutoDados produtoDados, float coeficiente)
        {
            /* o valor de produtoDados.Preco_lista não esta calculado com o coeficiente e para fazer o cálculo 
             * de desconto corretamente deve estar calculado com o coeficiente 
             * por isso, criei essa váriavel precoListaBase
             */
            decimal? precoListaBase = Math.Round((produtoDados.Preco_lista ?? 0) * (decimal)coeficiente, 2);
            decimal preco_venda = produtoDto.Subtotal / (produtoDto.Quantidade == 0 ? 1 : produtoDto.Quantidade);
            decimal preco_nf = produtoDto.RowTotal / (produtoDto.Quantidade == 0 ? 1 : produtoDto.Quantidade);

            var ret = new Pedido.Dados.Criacao.PedidoCriacaoProdutoDados(
                fabricante: produtoDados.Fabricante,
                produto: produtoDados.Produto,
                qtde: produtoDto.Quantidade,
                custoFinancFornecPrecoListaBase_Conferencia: produtoDados.Preco_lista ?? 0,
                preco_Lista: Math.Round((produtoDados.Preco_lista ?? 0) * (decimal)coeficiente, 2),//tinha um erro aqui - não estava calculando corretamente
                desc_Dado: (float)(100 * ((precoListaBase ?? 0) - preco_venda) / (precoListaBase ?? 0)),
                preco_Venda: preco_venda,
                preco_NF: preco_nf,
                custoFinancFornecCoeficiente_Conferencia: coeficiente,
                //no magento não validamos se o estoque mudou
                qtde_spe_usuario_aceitou: null
            );

            return ret;
        }
    }
}
