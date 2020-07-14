using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_FORMA_PAGTO")]
    public class TformaPagto
    {
        [Key]
        [Column("id")]
        [Required]
        public short Id { get; set; }

        [Column("descricao")]
        [Required]
        [MaxLength(20)]
        public string Descricao { get; set; }

        [Column("hab_a_vista")]
        public short? Hab_a_vista { get; set; }

        [Column("hab_entrada")]
        public short? Hab_entrada { get; set; }

        [Column("hab_prestacao")]
        public short? Hab_prestacao { get; set; }

        [Column("ordenacao")]
        public int? Ordenacao { get; set; }

        [Column("hab_parcela_unica")]
        [Required]
        public short Hab_parcela_unica { get; set; }
    }
}
