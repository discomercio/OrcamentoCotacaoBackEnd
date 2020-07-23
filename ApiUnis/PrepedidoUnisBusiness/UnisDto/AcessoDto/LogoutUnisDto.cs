using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoApiUnisBusiness.UnisDto.AcessoDto
{
    public class LogoutUnisDto
    {
        [Required]
        public string TokenAcesso { get; set; }
    }
}
