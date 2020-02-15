using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioLogado
    {
        private readonly ClaimsPrincipal user;
        private readonly ISession httpContextSession;

        public UsuarioLogado(ClaimsPrincipal user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll)
        {
            this.user = user;
            this.httpContextSession = httpContextSession;

            if (!SessaoAtiva)
                CriarSessaoPorUser(user, httpContextSession, clienteBll, this).Wait();
        }

        private static async Task CriarSessaoPorUser(ClaimsPrincipal user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioLogado usuarioLogadoParaLAterarSessao)
        {
            string usuarioClaim = user?.Claims.Where(r => r.Type == ClaimTypes.Name).FirstOrDefault()?.Value ?? "Sem usuário";
            await CriarSessao(usuarioClaim, httpContextSession, clienteBll, usuarioLogadoParaLAterarSessao);
        }
        public static async Task CriarSessao(string usuario, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioLogado usuarioLogadoParaLAterarSessao = null)
        {
            //nao tem risco de dar recursão porque o construtor chama com uma instância
            if (usuarioLogadoParaLAterarSessao == null)
                usuarioLogadoParaLAterarSessao = new UsuarioLogado(null, httpContextSession, clienteBll);

            //tem que recriar
            usuario = usuario ?? "Sem usuário";
            usuario = usuario.Trim().ToUpper();
            usuarioLogadoParaLAterarSessao.Usuario = usuario;

            string lstOperacoesPermitidas = await clienteBll.BuscaListaOperacoesPermitidas(usuario);
            usuarioLogadoParaLAterarSessao.Lista_operacoes_permitidas = lstOperacoesPermitidas;

            usuarioLogadoParaLAterarSessao.SessaoAtiva = true;
        }


        public bool Operacao_permitida(int id_operacao)
        {
            var permitidas = Lista_operacoes_permitidas;
            var s = id_operacao.ToString();

            if (string.IsNullOrWhiteSpace(s))
                return false;
            s = "|" + s + "|";

            if (permitidas.Contains(s))
                return true;

            return false;
        }



        //o que colocamos na session
        private static class StringsSession
        {
            public static readonly string SessaoAtiva = "SessaoAtiva";
            public static readonly string NomeUsuario = "usuario";
            public static readonly string Lista_operacoes_permitidas = "lista_operacoes_permitidas";
        }

        public string Usuario
        {
            get => httpContextSession.GetString(StringsSession.NomeUsuario) ?? "Sem usuário";
            private set => httpContextSession.SetString(StringsSession.NomeUsuario, value);
        }
        private bool SessaoAtiva
        {
            get
            {
                if (httpContextSession.GetInt32(StringsSession.SessaoAtiva) == null)
                    return false;
                return true;
            }
            set => httpContextSession.SetInt32(StringsSession.SessaoAtiva, 1);
        }
        public string Lista_operacoes_permitidas
        {
            get => httpContextSession.GetString(StringsSession.Lista_operacoes_permitidas) ?? "";
            private set => httpContextSession.SetString(StringsSession.Lista_operacoes_permitidas, value);
        }

    }

}
