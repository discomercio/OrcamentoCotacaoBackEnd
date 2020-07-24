using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfraBanco.Modelos
{
    [Table("t_WMS_REGRA_CD_X_UF_X_PESSOA")]
    public class TwmsRegraCdXUfPessoa
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("id_wms_regra_cd_x_uf")]
        [Required]
        public int Id_wms_regra_cd_x_uf { get; set; }

        [Column("tipo_pessoa")]
        [MaxLength(6)]
        [Required]
        public string Tipo_pessoa { get; set; }

        [Column("st_inativo")]
        [Required]
        public byte St_inativo { get; set; }

        [Column("spe_id_nfe_emitente")]
        [Required]
        public int Spe_id_nfe_emitente { get; set; }

    }
}
