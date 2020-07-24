using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PRAZO_PAGTO_VISANET")]
    public class TprazoPagtoVisanet
    {
        [Column("tipo")]
        [Key]
        [Required]
        [MaxLength(12)]
        public string Tipo { get; set; }

        [Column("qtde_parcelas")]
        [Required]
        public short Qtde_parcelas { get; set; }
    }
}
