using Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_CATALOGO_ITENS")]
    public class TprodutoCatalogoItens : IModel
    {
        //[Key]
        //[Column("id")]
        //[Required]
        public int Id { get; set; }

        //[Column("valor")]
        //[MaxLength(100)]
        //[Required]
        public string Valor { get; set; }

        //[Column("ordem")]
        //[Required]
        public int Ordem { get; set; }

        //[NotMapped]
        //[Column("codigo")]
        public string Codigo { get; set; }

        //[NotMapped]
        //[Column("chave")]
        public string Chave { get; set; }
    }
}
