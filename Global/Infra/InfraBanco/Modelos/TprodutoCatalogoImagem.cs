using ClassesBase;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO_IMAGEM")]
    public class TprodutoCatalogoImagem : IModel
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("id_produto_catalogo")]
        [MaxLength(8)]
        [Required]
        public string IdProdutoCatalogo { get; set; }
        
        [Column("id_tipo_imagem")]
        public int IdTipoImagem { get; set; }
        
        [Required]
        [Column("caminho")]
        [MaxLength(500)]
        public string Caminho { get; set; }

        [Column("ordem")]
        public int Ordem { get; set; }
    }
}

