﻿using Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_CODIGO_DESCRICAO")]
    public class TcodigoDescricao : IModel
    {
        [Required]
        [MaxLength(60)]
        [Column("grupo")]
        public string Grupo { get; set; }

        [Column("codigo")]
        [Required]
        [MaxLength(20)]
        public string Codigo { get; set; }

        [Column("ordenacao")]
        public short Ordenacao { get; set; }

        [Column("st_inativo")]
        [Required]
        public byte St_Inativo { get; set; }

        [Column("descricao")]
        [Required]
        [MaxLength(60)]
        public string Descricao { get; set; }

        [Column("grupo_pai")]
        [MaxLength(60)]
        public string Grupo_pai { get; set; }

        [Column("codigo_pai")]
        [MaxLength(20)]
        public string Codigo_pai { get; set; }

        [Column("lojas_habilitadas")]
        [MaxLength]
        public string Lojas_Habilitadas { get; set; }

        [Column("parametro_1_campo_flag")]
        [Required]
        public byte Parametro_1_campo_flag { get; set; }

        [Column("parametro_2_campo_flag")]
        [Required]
        public byte Parametro_2_campo_flag { get; set; }

        [Column("parametro_3_campo_flag")]
        [Required]
        public byte Parametro_3_campo_flag { get; set; }

        [Column("parametro_4_campo_flag")]
        [Required]
        public byte Parametro_4_campo_flag { get; set; }

        [Column("parametro_5_campo_flag")]
        [Required]
        public byte Parametro_5_campo_flag { get; set; }

        [Column("parametro_campo_real")]
        public Single Parametro_campo_real { get; set; }

        [Column("parametro_campo_texto")]
        [MaxLength(1024)]
        public string Parametro_campo_texto { get; set; }

        [Column("descricao_parametro")]
        [MaxLength(2048)]
        public string Descricao_parametro { get; set; }

        [Column("parametro_2_campo_texto")]
        [MaxLength(1024)]
        public string Parametro_2_campo_texto { get; set; }

        [Column("parametro_3_campo_texto")]
        [MaxLength(1024)]
        public string Parametro_3_campo_texto { get; set; }
    }
}
