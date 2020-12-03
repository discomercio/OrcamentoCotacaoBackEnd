using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_LOJA")]
    public class TprodutoLoja
    {
        [Column("fabricante")]
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Column("produto")]
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Column("loja")]
        [Required]
        [MaxLength(3)]
        public string Loja { get; set; }

        public Tproduto Tproduto { get; set; }

        [Column("preco_lista", TypeName = "money(19,4)")]
        public decimal? Preco_Lista { get; set; }

        [Column("margem")]
        public float? Margem { get; set; }

        [Column("desc_max")]
        public float? Desc_Max { get; set; }

        [Column("comissao")]
        public float? Comissao { get; set; }

        [Column("vendavel")]
        [MaxLength(1)]
        public string Vendavel { get; set; }

        [Column("qtde_max_venda")]
        public short? Qtde_Max_Venda { get; set; }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        public Tfabricante Tfabricante { get; set; }
#endif
    }
}