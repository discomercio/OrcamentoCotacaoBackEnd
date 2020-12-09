using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.MagentoDto
{
    public class LoginResultadoMagentoDto
    {
        public LoginResultadoMagentoDto(string TokenAcesso, List<string> ListaErros)
        {
            this.TokenAcesso = TokenAcesso;
            this.ListaErros = ListaErros;
        }
        public string TokenAcesso { get; set; }
        public List<string> ListaErros { get; set; } = new List<string>();
    }
}
