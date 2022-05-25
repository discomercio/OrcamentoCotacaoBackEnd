using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("T_CFG_UNIDADE_NEGOCIO")]
    public class TcfgUnidadeNegocio : IModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(5)]
        public string Sigla { get; set; }
        [Required]
        [MaxLength(30)]
        public string NomeCurto { get; set; }
        [Required]
        [MaxLength(60)]
        public string NomeCompleto { get; set; }        
        [MaxLength(4000)]
        public string Obs { get; set; }
        public Int16 Ordenacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataHoraCadastro { get; set; }
        public DateTime? DataHoraUltAtualizacao { get; set; }
    }
}
