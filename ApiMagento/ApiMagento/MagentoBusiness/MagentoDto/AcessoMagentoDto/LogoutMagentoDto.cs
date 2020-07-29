using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto
{
    public class LogoutMagentoDto
    {
        [Required]
        public string TokenAcesso { get; set; }
    }
}
