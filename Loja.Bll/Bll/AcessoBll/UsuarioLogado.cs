using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioLogado
    {
        private readonly ISession httpContextSession;


        public UsuarioLogado(ClaimsPrincipal user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll)
        {
            this.httpContextSession = httpContextSession;
            if (SessaoAtiva)
            {
                //verificamos se o nome bate
                if (user?.Identity != null)
                    if (user.Identity.IsAuthenticated && !string.IsNullOrEmpty(user.Identity.Name))
                        if (user.Identity.Name != Usuario)
                            SessaoAtiva = false;
            }

            //nao criamos automaticamente não!
            //a pensar melhor se a gente mudar o esquema do ciclo de vida do login e da sessão
            //if (!SessaoAtiva && clienteBll != null && usuarioAcessoBll != null)
            //    CriarSessaoPorUser(user, httpContextSession, clienteBll, usuarioAcessoBll, this).Wait();
        }

        private static async Task CriarSessaoPorUser(ClaimsPrincipal user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll,
            UsuarioLogado usuarioLogadoParaLAterarSessao)
        {
            string usuarioClaim = user?.Claims.Where(r => r.Type == ClaimTypes.Name).FirstOrDefault()?.Value ?? "Sem usuário";
            await CriarSessao(usuarioClaim, httpContextSession, clienteBll, usuarioAcessoBll, usuarioLogadoParaLAterarSessao);
        }
        public static async Task CriarSessao(string usuario, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll,
            UsuarioLogado usuarioLogadoParaLAterarSessao = null)
        {
            //nao tem risco de dar recursão porque o construtor chama com uma instância
            if (usuarioLogadoParaLAterarSessao == null)
                usuarioLogadoParaLAterarSessao = new UsuarioLogado(null, httpContextSession, clienteBll, usuarioAcessoBll);

            //tem que recriar
            usuario = usuario ?? "Sem usuário";
            usuario = usuario.Trim().ToUpper();
            usuarioLogadoParaLAterarSessao.Usuario = usuario;

            string lstOperacoesPermitidas = await clienteBll.BuscaListaOperacoesPermitidas(usuario);
            usuarioLogadoParaLAterarSessao.Lista_operacoes_permitidas = lstOperacoesPermitidas;

            usuarioLogadoParaLAterarSessao.Loja_troca_rapida_monta_itens_select = await usuarioAcessoBll.Loja_troca_rapida_monta_itens_select_a_partir_banco(usuario, null);

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
            public static readonly string Loja_troca_rapida_monta_itens_select = "Loja_troca_rapida_monta_itens_select";
            public static readonly string LojaAtiva = "LojaAtiva";
        }

        public string Usuario
        {
            get => httpContextSession.GetString(StringsSession.NomeUsuario) ?? "Sem usuário";
            private set => httpContextSession.SetString(StringsSession.NomeUsuario, value);
        }
        public bool SessaoAtiva
        {
            get
            {
                if (httpContextSession.GetInt32(StringsSession.SessaoAtiva) == null)
                    return false;
                return true;
            }
            private set => httpContextSession.SetInt32(StringsSession.SessaoAtiva, 1);
        }
        public string Lista_operacoes_permitidas
        {
            get => httpContextSession.GetString(StringsSession.Lista_operacoes_permitidas) ?? "";
            private set => httpContextSession.SetString(StringsSession.Lista_operacoes_permitidas, value);
        }
        public string LojaAtiva
        {
            get => httpContextSession.GetString(StringsSession.LojaAtiva) ?? "";
            private set => httpContextSession.SetString(StringsSession.LojaAtiva, value);
        }

        public bool LojaAtivaAlterar(string novaloja)
        {
            //verifica se pode ir para essa loja
            if (!Loja_troca_rapida_monta_itens_select.Any(r => r.Id == novaloja))
                return false;
            LojaAtiva = novaloja;
            return true;
        }

        public List<UsuarioAcessoBll.LojaPermtidaUsuario> Loja_troca_rapida_monta_itens_select
        {
            get
            {
                var sessao = httpContextSession.GetString(StringsSession.Loja_troca_rapida_monta_itens_select);
                //ao invés de dar exceção, retornamos uma lsita vazia. Para dimiuir o número de erros imprevistos;
                if (sessao == null)
                    return new List<UsuarioAcessoBll.LojaPermtidaUsuario>();
                return JsonConvert.DeserializeObject<List<UsuarioAcessoBll.LojaPermtidaUsuario>>(sessao);
            }
            private set => httpContextSession.SetString(StringsSession.Loja_troca_rapida_monta_itens_select, JsonConvert.SerializeObject(value));
        }

    }

}
