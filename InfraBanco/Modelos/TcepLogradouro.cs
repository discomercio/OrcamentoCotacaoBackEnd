using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_CEP_LOGRADOURO")]
    public class TcepLogradouro
    {
        [Key]
        [Column("CHVLOCAL_LOG")]
        [MaxLength(6)]
        public string Chvlocal_log { get; set; }

        [Column("NOME_LOCAL")]
        [MaxLength(60)]
        public string Nome_local { get; set; }

        //[Column("CHAVE_LOG")]
        //[MaxLength(6)]
        //public string Chave_log { get; set; }

        //[Column("CHVTIPO_LOG")]
        //[MaxLength(3)]
        //public string Chvtipo_log { get; set; }

        [Column("ABREV_TIPO")]
        [MaxLength(10)]
        public string Abrev_tipo { get; set; }

        [Column("NOME_LOG")]
        [MaxLength(70)]
        public string Nome_log { get; set; }

        [Column("COMPLE_LOG")]
        [MaxLength(100)]
        public string Comple_log { get; set; }

        //[Column("CHVBAI1_LOG")]
        //[MaxLength(5)]
        //public string Chvbai1_log { get; set; }

        [Column("EXTENSO_BAI")]
        [MaxLength(72)]
        public string Extenso_bai { get; set; }

        //[Column("ABREV_BAI")]
        //[MaxLength(36)]
        //public string Abrev_bai { get; set; }

        //[Column("CHVBAI2_LOG")]
        //[MaxLength(5)]
        //public string Chvbai2_log { get; set; }

        [Column("UF_LOG")]
        [MaxLength(2)]
        public string Uf_log { get; set; }

        [Column("CEP8_LOG")]
        [MaxLength(8)]
        public string Cep8_log { get; set; }
    }
}
