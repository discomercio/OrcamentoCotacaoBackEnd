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

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        [Column("sequencia")]
        public short? Sequencia { get; set; }
#endif
    }
}