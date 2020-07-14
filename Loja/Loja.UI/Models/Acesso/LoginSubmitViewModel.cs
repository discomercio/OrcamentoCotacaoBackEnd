using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.UI.Models.Acesso
{
    public class LoginSubmitViewModel
    {
        public string Loja { get; set; }
        public string Apelido { get; set; }
        public string Senha { get; set; }
        public string ReturnUrl { get; set; }
    }
}
