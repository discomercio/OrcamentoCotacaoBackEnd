using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CONTROLE")]
    public class Tcontrole
    {
        [Key]
        [Required]
        [MaxLength(60)]
        [Column("id_nsu")]
        public string Id_Nsu { get; set; }

        [Required]
        [MaxLength(12)]
        [Column("nsu")]
        public string Nsu { get; set; }

        [Column("seq_anual")]
        public short? Seq_Anual { get; set; }

        [Column("dt_ult_atualizacao")]
        public DateTime Dt_Ult_Atualizacao { get; set; }

        [Column("ano_letra_seq")]
        [MaxLength(1)]
        public string Ano_Letra_Seq { get; set; }

        [Column("ano_letra_step")]
        public short Ano_Letra_Step { get; set; }

        [Column("dummy")]
        public Boolean Dummy { get; set; }
    }
}
