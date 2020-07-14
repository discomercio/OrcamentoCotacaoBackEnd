﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTISTA_E_INDICADOR")]
    public class TorcamentistaEindicador
    {
        [Key]
        [Column("apelido")]
        [MaxLength(20)]
        [Required]
        public string Apelido { get; set; }

        [Column("razao_social_nome")]
        [MaxLength(60)]
        [Required]
        public string Razao_Social_Nome { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("vendedor")]
        [MaxLength(10)]
        public string Vendedor { get; set; }

        [Column("hab_acesso_sistema")]
        public short? Hab_Acesso_Sistema { get; set; }

        [Column("status")]
        [MaxLength(1)]
        public string Status { get; set; }

        [Column("senha")]
        [MaxLength(10)]
        public string Senha { get; set; }

        [Column("datastamp")]
        [MaxLength(32)]
        public string Datastamp { get; set; }

        [Column("dt_ult_alteracao_senha")]
        public DateTime? Dt_Ult_Alteracao_Senha { get; set; }

        [Column("dt_ult_atualizacao")]
        public DateTime? Dt_Ult_Atualizacao { get; set; }

        [Column("dt_ult_acesso")]
        public DateTime? Dt_Ult_Acesso { get; set; }

        [Column("permite_RA_status")]
        [Required]
        public short Permite_RA_Status { get; set; }
    }
}