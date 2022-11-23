using Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTISTA_E_INDICADOR_VENDEDOR")]
    public class TorcamentistaEIndicadorVendedor : IModel
    {
        [Required]
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(255)]
        [Column("Nome")]
        public string Nome { get; set; }

        [MaxLength(255)]
        [Column("Email")]
        public string Email { get; set; }

        [MaxLength(100)]
        [Column("Senha")]
        public string Senha { get; set; }

        [Column("datastamp")]
        [MaxLength(32)]
        public string Datastamp { get; set; }

        [Required]
        [Column("IdIndicador")]
        public int IdIndicador { get; set; }

        [MaxLength(15)]
        [Column("Telefone")]
        public string Telefone { get; set; }

        [MaxLength(15)]
        [Column("Celular")]
        public string Celular { get; set; }

        [Column("Ativo")]
        public bool Ativo { get; set; }

        [MaxLength(20)]
        [Column("UsuarioCadastro")]
        public string UsuarioCadastro { get; set; }

        [MaxLength(20)]
        [Column("UsuarioUltimaAlteracao")]
        public string UsuarioUltimaAlteracao { get; set; }

        [Column("DataCadastro")]
        public DateTime? DataCadastro { get; set; }

        [Column("DataUltimaAlteracao")]
        public DateTime? DataUltimaAlteracao { get; set; }

        [Column("DataUltimaAlteracaoSenha")]
        public DateTime? DataUltimaAlteracaoSenha { get; set; }

        [NotMapped]
        public string Loja { get; set; }

        [NotMapped]
        public string VendedorResponsavel { get; set; }

        [NotMapped]
        public string Parceiro { get; set; }

        /*
Column_name	Type	Length	Nullable
Id	int	4	no
Nome	varchar	255	yes
Email	varchar	255	yes
Senha	varchar	100	yes
IdIndicador	varchar	20	yes
Telefone	varchar	15	yes
Celular	varchar	15	yes
Ativo	bit	1	yes
UsuarioCadastro	varchar	20	yes
UsuarioUltimaAlteracao	varchar	20	yes
DataCadastro	datetime	8	yes
DataUltimaAlteracao	datetime	8	yes
         */
    }
}
