using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PedidoServicoMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Required]
        [MaxLength(8)]
        public string Sku_produto { get; set; } //  = NumProduto

        [Required]
        [MaxLength(120)]
        public string Descricao { get; set; }

        [Required]
        public short Qtde { get; set; }

        /// <summary>
        /// Preco_bruto: preço de venda do item, gravado em preco_lista
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_bruto { get; set; }

        /// <summary>
        /// Preco_liquido: preço que seria impresso na nota fiscal, gravado em preco_venda e preco_nf 
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_liquido { get; set; }

#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
