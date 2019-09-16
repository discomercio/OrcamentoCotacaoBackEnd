using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_PRODUTO_X_WMS_REGRA_CD")]
    public class TprodutoXwmsRegraCd
    {
        [Key]
        [Column("fabricante")]
        [MaxLength(4)]
        [Required]
        public string Fabricante { get; set; }

        [Column("produto")]
        [MaxLength(8)]
        [Required]
        public string Produto { get; set; }

        [Column("id_wms_regra_cd")]
        [Required]
        public int Id_wms_regra_cd { get; set; }

        [Column("usuario_cadastro")]
        [MaxLength(10)]
        [Required]
        public string Usuario_cadastro { get; set; }

        [Column("dt_cadastro")]
        [Required]
        public DateTime Dt_cadastro { get; set; }

        [Column("dt_hr_cadastro")]
        [Required]
        public DateTime Dt_hr_cadastro { get; set; }

        [Column("usuario_ult_atualizacao")]
        [MaxLength(10)]
        [Required]
        public string Usuario_ult_atualizacao { get; set; }

        [Column("dt_ult_atualizacao")]
        [Required]
        public DateTime Dt_ult_atualizacao { get; set; }

        [Column("dt_hr_ult_atualizacao")]
        [Required]
        public DateTime Dt_hr_ult_atualizacao { get; set; }
    }
}
