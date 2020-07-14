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
        [ForeignKey("TestoqueItem")]
        public string Id_estoque { get; set; }

        [Column("id_nfe_emitente")]
        [Required]
        public short Id_nfe_emitente { get; set; }

        public TestoqueItem TestoqueItem { get; set; }
    }
}
