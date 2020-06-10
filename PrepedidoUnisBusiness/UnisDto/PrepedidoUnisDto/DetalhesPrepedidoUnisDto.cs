using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    public class DetalhesPrePedidoUnisDto
    {
        [Required]
        [MaxLength(1)]
        public string EntregaImediata { get; set; }
        
        public DateTime? EntregaImediataData { get; set; }

        [Required]
        [MaxLength(1)]
        public string BemDeUso_Consumo { get; set; }

        [Required]
        [MaxLength(1)]
        public string InstaladorInstala { get; set; }
    }
}
