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
    }
}
