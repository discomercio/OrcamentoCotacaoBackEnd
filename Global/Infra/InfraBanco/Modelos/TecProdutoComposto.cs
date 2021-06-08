using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_EC_PRODUTO_COMPOSTO")]
    public class TecProdutoComposto
    {
        [Column("fabricante_composto")]
        [MaxLength(4)]
        [Required]
        public string Fabricante_Composto { get; set; }

        [Column("produto_composto")]
        [MaxLength(8)]
        [Required]
        public string Produto_Composto { get; set; }
    }
}
