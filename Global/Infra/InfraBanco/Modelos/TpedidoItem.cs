using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_ITEM")]
    public class TpedidoItem
    {
        [Column("pedido")]
        [Required]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Column("fabricante")]
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Column("produto")]
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Column("qtde")]
        public short? Qtde { get; set; }

        [Column("desc_dado")]
        public Single? Desc_Dado { get; set; }

        [Column("preco_venda", TypeName = "money(19,4)")]
        [Required]
        public decimal Preco_Venda { get; set; }

        [Column("preco_lista", TypeName = "money")]
        [Required]
        public decimal Preco_Lista { get; set; }

        [Column("comissao")]
        public Single? Comissao { get; set; }

        [Column("preco_NF", TypeName = "money")]
        public decimal? Preco_NF { get; set; }

        [Column("descricao_html")]
        [MaxLength(400)]
        public string Descricao_Html { get; set; }

        [Column("subgrupo")]
        [MaxLength(10)]
        public string Subgrupo { get; set; }

        public Tpedido Tpedido { get; set; }


        [Column("sequencia")]
        public short? Sequencia { get; set; }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG


        [Column("preco_fabricante", TypeName = "money")]
        public decimal? Preco_Fabricante { get; set; }

        [Column("vl_custo2", TypeName = "money")]
        [Required]
        public decimal Vl_Custo2 { get; set; }

        [Column("margem")]
        public Single? Margem { get; set; }

        [Column("desc_max")]
        public Single? Desc_Max { get; set; }

        [Column("descricao")]
        [MaxLength(120)]
        public string Descricao { get; set; }

        [Column("ean")]
        [MaxLength(14)]
        public string Ean { get; set; }

        [Column("grupo")]
        [MaxLength(2)]
        public string Grupo { get; set; }

        [Column("peso")]
        public Single? Peso { get; set; }

        [Column("qtde_volumes")]
        public short? Qtde_Volumes { get; set; }

        [Column("abaixo_min_status")]
        public short? Abaixo_Min_Status { get; set; }

        [Column("abaixo_min_autorizacao")]
        [MaxLength(12)]
        public string Abaixo_Min_Autorizacao { get; set; }

        [Column("abaixo_min_autorizador")]
        [MaxLength(10)]
        public string Abaixo_Min_Autorizador { get; set; }

        [Column("abaixo_min_superv_autorizador")]
        [MaxLength(10)]
        public string Abaixo_Min_Superv_Autorizador { get; set; }

        [Column("markup_fabricante")]
        public Single? Markup_Fabricante { get; set; }

        [Column("custoFinancFornecCoeficiente")]
        [Required]
        public Single CustoFinancFornecCoeficiente { get; set; }

        [Column("custoFinancFornecPrecoListaBase", TypeName = "money")]
        [Required]
        public decimal CustoFinancFornecPrecoListaBase { get; set; }

        [Column("cubagem")]
        [Required]
        public Single Cubagem { get; set; }

        [Column("ncm")]
        [MaxLength(8)]
        public string Ncm { get; set; }

        [Column("cst")]
        [MaxLength(3)]
        public string Cst { get; set; }

        [Column("descontinuado")]
        [MaxLength(1)]
        public string Descontinuado { get; set; }
#endif
    }
}