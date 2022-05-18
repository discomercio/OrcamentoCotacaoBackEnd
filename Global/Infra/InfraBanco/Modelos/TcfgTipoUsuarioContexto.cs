using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CFG_TIPO_USUARIO_CONTEXTO")]
    public class TcfgTipoUsuarioContexto
    {
        [Key]
        [Column("Id")]
        public short Id { get; set; }

        [Column("Descricao")]
        [MaxLength(30)]
        [Required]
        public string Descricao { get; set; }

        [Column("TabelaAutenticacao")]
        [MaxLength(255)]
        public string TabelaAutenticacao { get; set; }

        [Column("Ordenacao")]
        public short Ordenacao { get; set; }

        public TorcamentoCotacaoOpcaoPagto TorcamentoCotacaoOpcaoPagto { get; set; }

    }
}
