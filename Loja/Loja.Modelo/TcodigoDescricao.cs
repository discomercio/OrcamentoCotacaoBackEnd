using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Loja.Modelos
{
    [Table("t_CODIGO_DESCRICAO")]
    public class TcodigoDescricao
    {
        [Key]
        [Required]
        [MaxLength(60)]
        [Column("grupo")]
        public string Grupo { get; set; }

        [Column("codigo")]
        [Required]
        [MaxLength(20)]
        public string Codigo { get; set; }

        [Column("ordenacao")]
        [Required]
        public short Ordenacao { get; set; }

        [Column("st_inativo")]
        [Required]
        public byte St_Inativo { get; set; }

        [Column("descricao")]
        [Required]
        [MaxLength(60)]
        public string Descricao { get; set; }

        [Column("dt_hr_cadastro")]
        public DateTime? Dt_Hr_Cadastro { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        public string Usuario_Cadastro { get; set; }

        [Column("dt_hr_ult_atualizacao")]
        public DateTime? Dt_Hr_Ult_Atualizacao { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(10)]
        public string Usuario_Ult_Atualizacao { get; set; }

        [Column("st_possui_sub_codigo")]
        [Required]
        public byte St_Possui_Sub_Codigo { get; set; }

        [Column("st_eh_sub_codigo")]
        public byte? St_Eh_Sub_Codigo { get; set; }

        [Column("grupo_pai")]
        [MaxLength(60)]
        public string Grupo_Pai { get; set; }

        [Column("codigo_pai")]
        [MaxLength(20)]
        public string Codigo_Pai { get; set; }

        [Column("lojas_habilitadas")]
        [MaxLength]
        public string Lojas_Habilitadas { get; set; }

        [Column("parametro_1_campo_flag")]
        [Required]
        public byte Parametro_1_Campo_Flag { get; set; }

        [Column("parametro_2_campo_flag")]
        [Required]
        public byte Parametro_2_Campo_Flag { get; set; }

        [Column("parametro_3_campo_flag")]
        [Required]
        public byte Parametro_3_Campo_Flag { get; set; }

        [Column("parametro_4_campo_flag")]
        [Required]
        public byte Parametro_4_Campo_Flag { get; set; }

        [Column("parametro_5_campo_flag")]
        [Required]
        public byte Parametro_5_Campo_Flag { get; set; }

        [Column("parametro_campo_inteiro")]
        [Required]
        public int Parametro_Campo_Inteiro { get; set; }

        [Column("parametro_campo_monetario")]
        [Required]
        public decimal Parametro_Campo_Monetario { get; set; }

        [Column("parametro_campo_real")]
        [Required]
        public float Parametro_Campo_Real { get; set; }

        [Column("parametro_campo_data")]
        public DateTime? Parametro_Campo_Data { get; set; }

        [Column("parametro_campo_texto")]
        [MaxLength(1024)]
        public string Parametro_Campo_Texto { get; set; }

        [Column("descricao_parametro")]
        [MaxLength(800)]
        public string Descricao_Parametro { get; set; }

        [Column("parametro_2_campo_texto")]
        [MaxLength(1024)]
        public string Parametro_2_Campo_Texto { get; set; }
    }
}
