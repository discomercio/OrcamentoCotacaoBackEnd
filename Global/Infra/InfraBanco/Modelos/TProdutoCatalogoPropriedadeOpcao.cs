using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO_PROPRIEDADE_OPCAO")]
    public class TProdutoCatalogoPropriedadeOpcao
    {       
        [Key]
        [Column("Id")]
        public int id { get; set; }
        [Required]
        public int id_produto_catalogo_propriedade { get; set; }
        public string valor { get; set; }
        [Required]
        public bool oculto { get; set; }
        [Required]
        public int ordem { get; set; }
        [Required]
        public DateTime dt_cadastro { get; set; }
        [Required]
        public string usuario_cadastro { get; set; }
    }
}
