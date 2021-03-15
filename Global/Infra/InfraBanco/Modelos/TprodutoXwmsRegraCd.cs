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
    }
}
