using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG
namespace InfraBanco.Modelos
{
    [Table("t_TRANSPORTADORA_CEP")]
    public class TtransportadoraCep
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("transportadora_id")]
        [MaxLength(10)]
        public string Transportadora_id { get; set; }

        [Required]
        [Column("tipo_range")]
        public short Tipo_range { get; set; }

        [Column("cep_unico")]
        [MaxLength(8)]
        public string Cep_unico { get; set; }

        [Column("cep_faixa_inicial")]
        [MaxLength(8)]
        public string Cep_faixa_inicial { get; set; }

        [Column("cep_faixa_final")]
        [MaxLength(8)]
        public string Cep_faixa_final { get; set; }

        [Column("dt_cadastro")]
        public DateTime? Dt_cadastro { get; set; }

        [Column("dt_hr_cadastro")]
        public DateTime? Dt_hr_cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        public string Usuario_cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        public DateTime? Dt_ult_atualizacao { get; set; }

        [Column("dt_hr_ult_atualizacao")]
        public DateTime? Dt_hr_ult_atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(10)]
        public string Usuario_ult_atualizacao { get; set; }

    }
}
#endif
