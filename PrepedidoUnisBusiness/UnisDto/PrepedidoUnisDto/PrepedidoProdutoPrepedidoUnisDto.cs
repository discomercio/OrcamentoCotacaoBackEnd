using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class PrePedidoProdutoPrePedidoUnisDto
    {
        //TODO: alterar o nomes das variaveis para os mesmos nomes da tabela
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        //[Required]
        //[MaxLength(8)]
        //public string NumProduto { get; set; }
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; } //  = NumProduto

        public short? Qtde { get; set; }

        //public float? Desconto { get; set; }
        public float? Desc_Dado { get; set; }// = Desconto

        //[Required]
        //public decimal VlUnitario { get; set; }
        [Required]
        public decimal Preco_Venda { get; set; }// = VlUnitario

        //public decimal? Preco { get; set; }
        public decimal? Preco_Fabricante { get; set; }

        //[Required]
        //public decimal VlLista { get; set; }
        [Required]
        public decimal Preco_Lista { get; set; }// = VlLista

        public decimal? Preco_NF { get; set; } // se permite RA = Preco_Lista / senão VlUnitario

        [Required]
        public float CustoFinancFornecCoeficiente { get; set; }

        public decimal CustoFinancFornecPrecoListaBase { get; set; } //recebe Preco_Lista

        //[Required]
        //public decimal? VlTotalItem { get; set; }

        //[Required]
        //public decimal? TotalItemRA { get; set; }

        //[Required]
        //public decimal? TotalItem { get; set; }


    }
}
