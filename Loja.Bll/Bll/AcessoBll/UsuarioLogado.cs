using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioLogado
    {
        //somente usando os métodos de construtor
        private UsuarioLogado() { }

        //o que colocamos na session
        private static class StringsSession
        {
            public static readonly string SessaoAtiva = "SessaoAtiva";
            public static readonly string NomeUsuario = "usuario";
        }

        public string Nome;
        public static UsuarioLogado ObterUsuarioLogado(ClaimsPrincipal user, ISession httpContextSession)
        {
            var ret = new UsuarioLogado()
            {
                Nome = "Sem usuário"
            };

            if (user == null)
                return ret;
            if (httpContextSession == null)
                return ret;
            if (httpContextSession.GetString(StringsSession.SessaoAtiva) == null)
            {
                //tem que recriar
            }


            return ret;
        }
    }

}
