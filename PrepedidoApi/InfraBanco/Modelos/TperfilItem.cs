using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PERFIL_ITEM")]
    public class TperfilItem
    {
        [Required]
        [MaxLength(12)]
        [Column("id")]
        public string Id { get; set; }

        public Tperfil Tperfil { get; set; }
        public Toperacao Toperacao { get; set; }

        [Required]
        [MaxLength(12)]
        [Column("id_perfil")]
        [ForeignKey("Tperfil")]
        public string Id_perfil { get; set; }
        
        [Required]
        [Column("id_operacao")]
        [ForeignKey("Toperacao")]
        public int Id_operacao { get; set; }

        [Required]
        [Column("dt_cadastro")]
        public DateTime Dt_cadastro { get; set; }

        [MaxLength(10)]
        [Column("usuario_cadastro")]
        public string Usuario_cadastro { get; set; }

        [Column("excluido_status")]
        public short? Excluido_status { get; set; }

        [Column("timestamp")]
        public byte Timestamp { get; }

    }
}
