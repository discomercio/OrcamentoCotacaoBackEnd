using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_FABRICANTE")]
    public class Tfabricante
    {
        [Key]
        [Column("fabricante")]
        [MaxLength(4)]
        [Required]
        public string Fabricante { get; set; }

        [Column("nome")]
        [MaxLength(30)]
        public string Nome { get; set; }

        [Column("markup")]
        public float Markup { get; set; }

        public List<TprodutoCatalogo> TprodutoCatalogos{ get; set; }
    }
}
