using System;
using System.Collections.Generic;

namespace InfraIdentity
{
    public class UsuarioLogin
    {
        public int Id { get; set; }
        //esta é a chave 
        public string Apelido { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Loja { get; set; }
        public byte? IdUnidade_negocio { get; set; }
        public string Unidade_negocio { get; set; }
        //esse campo esta sendo incluido para verificar 
        //se o usuário esta bloqueado ou se iremos redirecionar para alterar a senha
        public int IdErro { get; set; }
        public string Email { get; set; }
        public int? TipoUsuario { get; set; }
        public string IdParceiro { get; set; }
        public string IdVendedor { get; set; }
        public string VendedorResponsavel { get; set; }
        public string Token { get; set; }
        public List<string> Permissoes { get; set; }
        public DateTime? Dt_Ult_Alteracao_Senha { get; set; }
        public bool Bloqueado { get; set; }
        public string Datastamp { get; set; }
        public bool AcessoHabilitado { get; set; }
        public int QtdeConsecutivaFalhaLogin { get; set; }
    }
}