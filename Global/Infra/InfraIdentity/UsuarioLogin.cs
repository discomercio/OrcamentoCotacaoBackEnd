using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InfraIdentity
{
    public class UsuarioLogin
    {
        //esta é a chave 
        public string Apelido { get; set; }
        public string Nome { get; set; }
        public string Loja { get; set; }
        public string Unidade_negocio { get; set; }
        //esse campo esta sendo incluido para verificar 
        //se o usuário esta bloqueado ou se iremos redirecionar para alterar a senha
        public int IdErro { get; set; }
        public string Email { get; set; }
        public string TipoUsuario { get; set; }
        public string IdParceiro { get; set; }
        public string IdVendedor { get; set; }
    }
}
