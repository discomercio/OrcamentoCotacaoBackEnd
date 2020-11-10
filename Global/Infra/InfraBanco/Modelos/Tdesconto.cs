using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

#if RELEASE_BANCO_PEDIDO || DEBUG_BANCO_DEBUG

namespace InfraBanco.Modelos
{
    [Table("t_DESCONTO")]
    public class Tdesconto
    {
        [Key]
        [Required]
        [Column("id")]
        [MaxLength(12)]
        public string Id { get; set; }

        [Required]
        [Column("data")]
        public DateTime Data { get; set; }

        [Required]
        [Column("autorizador")]
        [MaxLength(10)]
        public string Autorizador { get; set; }

        [Column("id_cliente")]
        [MaxLength(12)]
        public string Id_cliente { get; set; }

        //public Tcliente Tcliente { get; set; }

        [Required]
        [Column("cnpj_cpf")]
        [MaxLength(14)]
        public string Cnpj_cpf { get; set; }

        [Required]
        [Column("fabricante")]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Required]
        [Column("produto")]
        [MaxLength(8)]
        public string Produto { get; set; }

        [Column("desc_max", TypeName = "real")]
        public decimal? Desc_max { get; set; }

        [Column("loja")]
        [MaxLength(3)]
        public string Loja { get; set; }

        [Column("vendedor")]
        [MaxLength(10)]
        public string Vendedor { get; set; }

        [Column("usado_status")]
        public short? Usado_status { get; set; }

        [Column("usado_data")]
        public DateTime? Usado_data { get; set; }

        [Column("cancelado_status")]
        public short? Cancelado_status { get; set; }

        [Column("cancelado_data")]
        public DateTime? Cancelado_data { get; set; }

        [Column("cancelado_hora")]
        [MaxLength(6)]
        public string Cancelado_hora { get; set; }

        [Column("cancelado_usuario")]
        [MaxLength(10)]
        public string Cancelado_usuario { get; set; }

        [Column("supervisor_autorizador")]
        [MaxLength(10)]
        public string Supervisor_autorizador { get; set; }

        [Column("usado_usuario")]
        [MaxLength(10)]
        public string Usado_usuario { get; set; }
    }
}
#endif