using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Acesso
{
    public class AlterarSenhaDto
    {
        public string Apelido { get; set; }
        public string Senha { get; set; }
        public string SenhaNova { get; set; }
        public string SenhaNovaConfirma { get; set; }
    }
}
