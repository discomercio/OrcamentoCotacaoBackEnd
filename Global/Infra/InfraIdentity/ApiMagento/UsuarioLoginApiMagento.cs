using System;
using System.Collections.Generic;
using System.Text;

namespace InfraIdentity.ApiMagento
{
    public class UsuarioLoginApiMagento
    {
        public string Usuario { get; set; }
        public string Nome { get; set; }
        //esse campo esta sendo incluido para verificar 
        //se o usuário esta bloqueado ou se iremos redirecionar para alterar a senha
        public List<string> ListaErros { get; set; } = new List<string>();
    }
}
