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

        [Column("id_forma_pagto")]
        [Required]
        [ForeignKey("TformaPagto")]
        public short Id_forma_pagto { get; set; }

        [Column("tipo_cliente")]
        [Required]
        [MaxLength(2)]
        public string Tipo_cliente { get; set; }

        [Column("st_restricao_ativa")]
        [Required]
        public byte St_restricao_ativa { get; set; }
    }
}
