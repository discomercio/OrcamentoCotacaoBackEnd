using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_EC_PRODUTO_COMPOSTO_ITEM")]
    public class TecProdutoCompostoItem
    {
        [Key]
        [Column("fabricante_composto")]
        [Required]
        [MaxLength(4)]
        public string Fabricante_composto { get; set; }

        [Column("produto_composto")]
        [Required]
        [MaxLength(8)]
        public string Produto_composto { get; set; }

        [Column("produto_item")]
        [Required]
        [MaxLength(8)]
        public string Produto_item { get; set; }

        [Column("qtde")]
        [Required]
        public short Qtde { get; set; }

        [Column("excluido_status")]
        public short Excluido_status { get; set; }
    }
}
