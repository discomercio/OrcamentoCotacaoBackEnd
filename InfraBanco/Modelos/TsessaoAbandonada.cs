using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_SESSAO_ABANDONADA")]
    public class TsessaoAbandonada
    {
        [Column("usuario")]
        [MaxLength(20)]
        [Required]
        public string Usuario { get; set; }

        public DateTime SessaoAbandonadaDtHrInicio { get; set; }

        [MaxLength(3)]
        public string SessaoAbandonadaLoja { get; set; }

        [MaxLength(5)]
        [Required]
        public string SessaoAbandonadaModulo { get; set; }

        public DateTime SessaoSeguinteDtHrInicio { get; set; }

        [MaxLength(3)]
        public string SessaoSeguinteLoja { get; set; }

        [MaxLength(5)]
        [Required]
        public string SessaoSeguinteModulo { get; set; }
    }
}
