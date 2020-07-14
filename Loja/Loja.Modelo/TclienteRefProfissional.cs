using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loja.Modelos
{
    [Table("t_CLIENTE_REF_PROFISSIONAL")]
    public class TclienteRefProfissional
    {
        [Key]
        [Column("id_cliente")]
        [Required]
        [MaxLength(12)]
        //[ForeignKey("Tcliente")]
        public string Id_Cliente { get; set; }

        [Key]
        [Required]
        [MaxLength(60)]
        [Column("nome_empresa")]
        public string Nome_Empresa { get; set; }

        [Key]
        [Required]
        [MaxLength(40)]
        [Column("cargo")]
        public string Cargo { get; set; }

        [Column("ddd")]
        [MaxLength(2)]
        public string Ddd { get; set; }

        [Column("telefone")]
        [MaxLength(9)]
        public string Telefone { get; set; }

        [Column("periodo_registro")]
        public DateTime? Periodo_Registro { get; set; }

        [Column("rendimentos")]
        public decimal? Rendimentos { get; set; }

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

        [Column("cnpj")]
        [MaxLength(14)]
        public string Cnpj { get; set; }

    }
}