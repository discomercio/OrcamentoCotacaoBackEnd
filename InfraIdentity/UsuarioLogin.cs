using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfraIdentity
{
    public class UsuarioLogin
    {
        //esta é a chave 
        public string Apelido { get; set; }
        public string Nome { get; set; }
        public string Loja { get; set; }
        //esse campo esta sendo incluido para verificar 
        //se o usuário esta bloqueado ou se iremos redirecionar para alterar a senha
        public int IdErro { get; set; }
    }
}
