using Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("T_ORCAMENTO_COTACAO_ARQUIVOS")]
    public class TorcamentoCotacaoArquivos : IModel
    {
        [Key]
        [Column("id")]
        [Required]
        public Guid Id { get; set; }

        [Column("nome")]
        [MaxLength(100)]
        [Required]
        public string Nome { get; set; }
        
        [Column("tamanho")]
        [MaxLength(10)]
        public string Tamanho { get; set; }
        
        [Required]
        [Column("tipo")]
        [MaxLength(10)]
        public string Tipo { get; set; }

        [Column("pai")]
        public Guid? Pai { get; set; }

        [Column("descricao")]
        [MaxLength(500)]
        public string Descricao { get; set; }
    }
}
