using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.UI.Models.Acesso
{
    public class LoginViewModel
    {
        public string Loja { get; set; }
        public string Apelido { get; set; }
        public string Senha { get; set; }
        public string ReturnUrl { get; set; }
        public bool ManterConectado { get; set; }
        public bool PermitirManterConectado { get; set; }
        public bool AcessoNegado { get; set; }
        public bool ErroUsuarioSenha { get; set; }
    }
}
