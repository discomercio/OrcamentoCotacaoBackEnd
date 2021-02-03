using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ESTOQUE_ITEM")]
    public class TestoqueItem
    {
        [Required]
        [Column("id_estoque")]
        [MaxLength(12)]
        public string Id_estoque { get; set; }

        [Required]
        [Column("fabricante")]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Column("produto")]
        [Required]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Column("qtde")]
        public short? Qtde { get; set; }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        [Column("preco_fabricante", TypeName = "money")]
        [Required]
        public decimal? Preco_fabricante { get; set; }
#endif

        [Column("qtde_utilizada")]
        public short? Qtde_utilizada { get; set; }

        public Testoque Testoque { get; set; }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        [Column("data_ult_movimento")]
        [Required]
        public DateTime Data_ult_movimento { get; set; }
#endif
    }
}
