using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InfraBanco.Modelos
{
    [Table("t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD")]
    public class TwmsRegraCdXUfXPessoaXCd
    {
        [Key]
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("id_wms_regra_cd_x_uf_x_pessoa")]
        [Required]
        public int Id_wms_regra_cd_x_uf_x_pessoa { get; set; }

        [Column("id_nfe_emitente")]
        [Required]
        public int Id_nfe_emitente { get; set; }

        [Column("ordem_prioridade")]
        [Required]
        public int Ordem_prioridade { get; set; }

        [Column("st_inativo")]
        [Required]
        public byte St_inativo { get; set; }
    }
}
