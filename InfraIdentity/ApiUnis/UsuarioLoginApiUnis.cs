using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraIdentity.ApiUnis
{
    public class UsuarioLoginApiUnis
    {
        public string Usuario { get; set; }
        public string Nome { get; set; }
        //esse campo esta sendo incluido para verificar 
        //se o usuário esta bloqueado ou se iremos redirecionar para alterar a senha
        public int IdErro { get; set; }
    }
}
