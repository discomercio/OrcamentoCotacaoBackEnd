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

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        public string Usuario_cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_ult_atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(10)]
        public string Usuario_ult_atualizacao { get; set; }

        [Column("timestamp")]
        public byte? Timestamp { get; set; }

        [Column("hab_parcela_unica")]
        [Required]
        public short Hab_parcela_unica { get; set; }
    }
}
