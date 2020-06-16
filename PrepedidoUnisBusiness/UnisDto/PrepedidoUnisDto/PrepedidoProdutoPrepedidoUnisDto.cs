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
        public string NumProduto { get; set; }
        
        public short? Qtde { get; set; }
        
        public decimal? Preco { get; set; }

        public decimal? Preco_Lista { get; set; }

        [Required]
        public decimal VlLista { get; set; }
        
        public float? Desconto { get; set; }

        [Required]
        public decimal VlUnitario { get; set; }

        [Required]
        public decimal? VlTotalItem { get; set; }

        [Required]
        public decimal? TotalItemRA { get; set; }

        [Required]
        public decimal? TotalItem { get; set; }

        [Required]
        public float CustoFinancFornecCoeficiente { get; set; }
    }
}
