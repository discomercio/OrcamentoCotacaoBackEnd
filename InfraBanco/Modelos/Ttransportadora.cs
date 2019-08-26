using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_TRANSPORTADORA")]
    public class Ttransportadora
    {
        [Key]
        [Required]
        [Column("id")]
        [MaxLength(10)]
        public string Id { get; set; }


    }
}
