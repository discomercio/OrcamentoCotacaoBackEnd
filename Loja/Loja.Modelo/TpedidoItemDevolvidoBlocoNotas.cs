using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace Loja.Modelos
{
    [Table("t_PEDIDO_ITEM_DEVOLVIDO_BLOCO_NOTAS")]
    public class TpedidoItemDevolvidoBlocoNotas
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("id_item_devolvido")]
        [Required]
        [MaxLength(12)]
        [ForeignKey("TpedidoItemDevolvido")]
        public string Id_Item_Devolvido { get; set; }

        [Column("usuario")]
        [Required]
        [MaxLength(10)]
        //[ForeignKey("Tusuario")]
        public string Usuario { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("dt_hr_cadastro")]
        [Required]
        public DateTime Dt_Hr_Cadastro { get; set; }

        [Column("mensagem")]
        [Required]
        [MaxLength(400)]
        public string Mensagem { get; set; }

        [Column("anulado_status")]
        [Required]
        public short Anulado_Status { get; set; }

        [Column("anulado_usuario")]
        [MaxLength(10)]
        public string Anulado_Usuario { get; set; }

        [Column("anulado_data")]
        public DateTime? Anulado_Data { get; set; }

        [Column("anulado_data_hora")]
        public DateTime? Anulado_Data_Hora { get; set; }

        public TpedidoItemDevolvido TpedidoItemDevolvido { get; set; }

    }
}