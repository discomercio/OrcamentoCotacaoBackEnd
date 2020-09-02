using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto
{
    public class MarketplaceMagentoDto
    {
        [Required]
        [MaxLength(60)]
        public string Grupo { get; set; }

        [Required]
        [MaxLength(60)]
        public string Descricao { get; set; }

        [Required]
        public short Parametro_1_campo_flag { get; set; }

        [Required]
        public short Parametro_2_campo_flag { get; set; }

        [Required]
        public short Parametro_3_campo_flag { get; set; }

        [Required]
        public short Parametro_4_campo_flag { get; set; }

        [Required]
        public short Parametro_5_campo_flag { get; set; }

        [MaxLength(1024)]
        public string Parametro_campo_texto { get; set; }

        [MaxLength(2048)]
        public string Descricao_parametro { get; set; }

        [MaxLength(1024)]
        public string Parametro_2_campo_texto { get; set; }

        [MaxLength(1024)]
        public string Parametro_3_campo_texto { get; set; }

    }
}
