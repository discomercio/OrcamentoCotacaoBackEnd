using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO_PROPRIEDADE")]
    public class TProdutoCatalogoPropriedade
    {       
        [Key]
        [Column("Id")]
        public int id { get; set; }
        [Required]
        public short IdCfgTipoPropriedade { get; set; }
        [Required]
        public short IdCfgTipoPermissaoEdicaoCadastro { get; set; }
        [Required]
        public short IdCfgDataType { get; set; }
        [Required]
        public string descricao { get; set; }
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
