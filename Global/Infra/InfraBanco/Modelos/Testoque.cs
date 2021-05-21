using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ESTOQUE")]
    public class Testoque
    {
        [Key]
        [Required]
        [Column("id_estoque")]
        [MaxLength(12)]
        public string Id_estoque { get; set; }

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
        [Column("data_entrada")]
        [Required]
        public DateTime Data_entrada { get; set; }

        [Column("data_ult_movimento")]
        [Required]
        public DateTime Data_ult_movimento { get; set; }
#endif

        [Column("id_nfe_emitente")]
        [Required]
        public short Id_nfe_emitente { get; set; }

        public ICollection<TestoqueItem> TestoqueItem { get; set; }

    }
}
