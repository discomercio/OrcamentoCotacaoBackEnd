using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTISTA_E_INDICADOR_RESTRICAO_FORMA_PAGTO")]
    public class TorcamentistaEIndicadorRestricaoFormaPagto
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Column("id_orcamentista_e_indicador")]
        [Required]
        [MaxLength(20)]
        [ForeignKey("TorcamentistaEindicador")]
        public string Id_orcamentista_e_indicador { get; set; }

        public TorcamentistaEindicador TorcamentistaEindicador { get; set; }

        [Column("id_forma_pagto")]
        [Required]
        [ForeignKey("TformaPagto")]
        public short Id_forma_pagto { get; set; }

        public TformaPagto TformaPagto { get; set; }

        [Column("tipo_cliente")]
        [Required]
        [MaxLength(2)]
        public string Tipo_cliente { get; set; }

        [Column("st_restricao_ativa")]
        [Required]
        public byte St_restricao_ativa { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_cadastro { get; set; }

        [Column("dt_hr_cadastro")]
        [Required]
        public DateTime Dt_hr_cadastro { get; set; }

        [Column("usuario_cadastro")]
        [Required]
        [MaxLength(10)]
        //[ForeignKey("")]
        public string Usuario_cadastro { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_ult_atualizacao { get; set; }

        [Column("dt_hr_ult_atualizacao")]
        [Required]
        public DateTime Dt_hr_ult_atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [Required]
        [MaxLength(10)]
        //[ForeignKey("")]
        public string Usuario_ult_atualizacao { get; set; }
        
    }
}
