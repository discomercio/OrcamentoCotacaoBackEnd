using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto
{
    public class CancelarPrepedidoUnisDto
    {
        [Required]
        public string TokenAcesso { get; set; }

        [Required]
        [MaxLength(20)]
        public string Indicador_Orcamentista { get; set; }

        [MaxLength(9)]
        [Required]
        public string NumeroPrepedido { get; set; }
    }
}
