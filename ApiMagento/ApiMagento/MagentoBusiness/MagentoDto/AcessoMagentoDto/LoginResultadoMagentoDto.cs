using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.MagentoDto
{
    public class LoginResultadoMagentoDto
    {
        public string TokenAcesso { get; set; }
        public List<string> ListaErros { get; set; } = new List<string>();
    }
}
