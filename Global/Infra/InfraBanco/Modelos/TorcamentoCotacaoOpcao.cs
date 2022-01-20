using ClassesBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_ORCAMENTOCOTACAO_OPCAO")]
    public class TorcamentoCotacaoOpcao : IModel
    {
        [Key]
        [Column("Id")]
        [Required]
        public int Id { get; set; }

        [Column("IdOrcamentoCotacao")]
        [Required]
        public int IdOrcamentoCotacao { get; set; }

        [Column("VlTotal")]
        [Required]
        public decimal VlTotal { get; set; }

        [Column("ValorTotalComRA")]
        public decimal? ValorTotalComRA { get; set; }

        [Key]
        [Column("Observacoes")]
        [MaxLength(500)]
        public string Observacoes { get; set; }

        [Column("UsuarioCadastro")]
        [MaxLength(20)]
        [Required]
        public string UsuarioCadastro { get; set; }

        [Key]
        [Column("DataCadastro")]
        [Required]
        public DateTime DataCadastro { get; set; }
                
        [Column("UsuarioUltimaAlteracao")]
        [MaxLength(20)]
        public string UsuarioUltimaAlteracao { get; set; }
        
        [Column("DataUltimaAlteracao")]
        public DateTime? DataUltimaAlteracao { get; set; }

    }
}
