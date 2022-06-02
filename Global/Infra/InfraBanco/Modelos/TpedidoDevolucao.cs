using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_PEDIDO_DEVOLUCAO")]
    public class TpedidoDevolucao
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("pedido")]
        [MaxLength(9)]
        public string Pedido { get; set; }

        [Required]
        [Column("status")]
        public byte Status { get; set; }
    }
}
