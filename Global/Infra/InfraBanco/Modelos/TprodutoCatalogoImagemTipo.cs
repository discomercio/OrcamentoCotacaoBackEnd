using ClassesBase;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO_IMAGEM_TIPO")]
    public class TprodutoCatalogoImagemTipo : IModel
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("descricao")]
        [MaxLength(500)]
        public string Descricao { get; set; }
    }
}
