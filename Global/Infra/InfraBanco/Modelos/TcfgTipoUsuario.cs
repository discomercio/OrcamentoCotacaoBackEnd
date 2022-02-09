using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_TIPO_USUARIO")]
    public class TcfgTipoUsuario
    {
        [Key]
        [Column("Id")]
        public short Id { get; set; }

        [Column("Descricao")]
        [MaxLength(30)]
        [Required]
        public string Descricao { get; set; }

        [Column("Ordenacao")]
        public short Ordenacao { get; set; }

        public ICollection<TcfgPagtoFormaStatus> TcfgPagtoFormaStatus { get; set; }
        public ICollection<TcfgPagtoMeioStatus> TcfgPagtoMeioStatus { get; set; }
    }
}
