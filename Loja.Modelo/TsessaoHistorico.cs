using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_SESSAO_HISTORICO")]
    public class TsessaoHistorico
    {
        [Column("Usuario")]
        [MaxLength(20)]
        [Required]
        public string Usuario { get; set; }

        [Column("SessionCtrlTicket")]
        [Required]
        [MaxLength(64)]
        public string SessionCtrlTicket { get; set; }

        [Column("DtHrInicio")]
        [Required]
        public DateTime DtHrInicio { get; set; }

        [Column("DtHrTermino")]
        public DateTime? DtHrTermino { get; set; }

        [Column("Loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("Modulo")]
        [MaxLength(5)]
        [Required]
        public string Modulo { get; set; }

        [Column("IP")]
        [MaxLength(50)]
        [Required]
        public string IP { get; set; }

        [Column("UserAgent")]
        [MaxLength]
        public string UserAgent { get; set; }

    }
}
