using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class DetalhesPrePedidoUnisDto
    {
        /// <summary>
        /// EntregaImediata: 
        ///     COD_ETG_IMEDIATA_ST_INICIAL = 0,
        ///     COD_ETG_IMEDIATA_NAO = 1,
        ///     COD_ETG_IMEDIATA_SIM = 2
        /// </summary>
        [Required]
        public short St_Entrega_Imediata { get; set; }
        
        public DateTime? PrevisaoEntregaData { get; set; }

        /// <summary>
        /// BemDeUso_Consumo : COD_ST_BEM_USO_CONSUMO_NAO = 0, COD_ST_BEM_USO_CONSUMO_SIM = 1
        /// </summary>
        [Required]
        public short BemDeUso_Consumo { get; set; }

        /// <summary>
        /// InstaladorInstala: COD_INSTALADOR_INSTALA_NAO_DEFINIDO = 0, COD_INSTALADOR_INSTALA_NAO = 1, COD_INSTALADOR_INSTALA_SIM = 2
        /// </summary>
        [Required]
        public short InstaladorInstala { get; set; }

        [MaxLength(500)]
        public string Obs_1 { get; set; }
    }
}
