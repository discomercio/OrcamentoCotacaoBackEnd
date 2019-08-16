using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_CLIENTE_REF_BANCARIA")]
    public class TclienteRefBancaria
    {

        [Column("id_cliente")]
        [Required]
        [MaxLength(12)]
        //[ForeignKey("Id")]
        public string Id_Cliente { get; set; }

        [Column("banco")]
        [Required]
        [MaxLength(4)]
        public string Banco { get; set; }

        [Column("agencia")]
        [Required]
        [MaxLength(8)]
        public string Agencia { get; set; }

        [Column("conta")]
        [Required]
        [MaxLength(12)]
        public string Conta { get; set; }

        [Column("ddd")]
        [MaxLength(2)]
        public string Ddd { get; set; }

        [Column("telefone")]
        [MaxLength(9)]
        public string Telefone { get; set; }

        [Column("contato")]
        [MaxLength(40)]
        public string Contato { get; set; }

        [Column("ordem")]
        public short? Ordem { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_Cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(20)]
        public string Usuario_Cadastro { get; set; }

        [Column("excluido_status")]
        public short? Excluido_Status { get; set; }

    }
}
