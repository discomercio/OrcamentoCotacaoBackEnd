﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("NFE_UF")]
    public class NfeUf
    {
        [Key]
        [Required]
        [MaxLength(2)]
        [Column("CodUF")]
        public string CodUF { get; set; }

        [Column("SiglaUF")]
        [Required]
        [MaxLength(2)]
        public string SiglaUF { get; set; }
    }
}
