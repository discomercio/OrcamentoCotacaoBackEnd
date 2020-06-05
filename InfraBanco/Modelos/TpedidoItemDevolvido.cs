using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_ITEM_DEVOLVIDO")]
    public class TpedidoItemDevolvido
    {
        [Column("id")]
        [Key]
        [Required]
        [MaxLength(12)]
        public string Id { get; set; }

        [Column("devolucao_data")]
        public DateTime? Devolucao_Data { get; set; }

        [Column("devolucao_hora")]
        [MaxLength(6)]
        public string Devolucao_Hora { get; set; }

        [Column("pedido")]
        [Required]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Column("produto")]
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Column("qtde")]
        public short? Qtde { get; set; }

        [Column("motivo")]
        [MaxLength(80)]
        public string Motivo { get; set; }

        [Column("preco_NF", TypeName = "money")]
        public decimal? Preco_NF { get; set; }

        [Column("descricao_html")]
        [MaxLength(4000)]
        public string Descricao_Html { get; set; }

        [Column("NFe_numero_NF")]
        [Required]
        public int NFe_Numero_NF { get; set; }

        [Column("subgrupo")]
        [MaxLength(10)]
        public string Subgrupo { get; set; }
    }
}