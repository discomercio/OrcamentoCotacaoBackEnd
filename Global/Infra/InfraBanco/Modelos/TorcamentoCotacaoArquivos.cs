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

        [Column("Nome")]
        [MaxLength(50)]
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
        [MaxLength(100)]
        public string Descricao { get; set; }

        //[id]
        //[uniqueidentifier]
        //NOT NULL,

        //[nome] [varchar] (50) NOT NULL,

        //[tamanho] [varchar] (10) NULL,
        //[tipo] [varchar] (10) NOT NULL,

        //[pai] [uniqueidentifier] NULL,
        //[descricao] [varchar] (100) NULL
    }
}
