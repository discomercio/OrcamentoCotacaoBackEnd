using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_WMS_REGRA_CD_X_UF")]
    public class TwmsRegraCdXUf
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("id_wms_regra_cd")]
        [Required]
        public int Id_wms_regra_cd { get; set; }

        [Column("uf")]
        [MaxLength(2)]
        [Required]
        public string Uf { get; set; }

        [Column("st_inativo")]
        [Required]
        public byte St_inativo { get; set; }
    }
}
