using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_PRAZO_PAGTO_VISANET")]
    public class TprazoPagtoVisanet
    {
        [Column("tipo")]
        [Key]
        [Required]
        [MaxLength(12)]
        public string Tipo { get; set; }

        [Column("descricao")]
        [MaxLength(60)]
        [Required]
        public string Descricao { get; set; }

        [Column("qtde_parcelas")]
        [Required]
        public short Qtde_parcelas { get; set; }

        [Column("vl_min_parcela")]
        [Required]
        public decimal Vl_min_parcela { get; set; }

        [Column("atualizacao_data")]
        public DateTime? Atualizacao_data { get; set; }

        [Column("atualizacao_hora")]
        [MaxLength(6)]
        public string Atualizacao_hora { get; set; }

        [Column("atualizacao_usuario")]
        [MaxLength(10)]
        public string Atualizacao_usuario { get; set; }

        [Column("timestamp")]
        public byte? Timestamp { get; }
    }
}
