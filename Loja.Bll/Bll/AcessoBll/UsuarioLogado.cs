using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Loja.Bll.Util;

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioLogado
    {
        private readonly ISession httpContextSession;


        public UsuarioLogado(ClaimsPrincipal user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao)
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

            if (configuracao.PermitirManterConectado)
                if (!SessaoAtiva && clienteBll != null && usuarioAcessoBll != null)
                    CriarSessaoPorUser(user, httpContextSession, clienteBll, usuarioAcessoBll, configuracao, this).Wait();
        }

        private static async Task CriarSessaoPorUser(ClaimsPrincipal user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            UsuarioLogado usuarioLogadoParaLAterarSessao)
        {
            string usuarioClaim = user?.Claims.Where(r => r.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            await CriarSessao(usuarioClaim, httpContextSession, clienteBll, usuarioAcessoBll, configuracao, usuarioLogadoParaLAterarSessao);
        }
        public static async Task CriarSessao(string usuario, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            UsuarioLogado usuarioLogadoParaLAterarSessao = null)
        {
            //nao tem risco de dar recursão porque o construtor chama com uma instância
            if (usuarioLogadoParaLAterarSessao == null)
                usuarioLogadoParaLAterarSessao = new UsuarioLogado(null, httpContextSession, clienteBll, usuarioAcessoBll, configuracao);

            //tem que recriar
            if (string.IsNullOrWhiteSpace(usuario))
                return;
            usuario = usuario.Trim().ToUpper();
            usuarioLogadoParaLAterarSessao.Usuario = usuario;

            string lstOperacoesPermitidas = await clienteBll.BuscaListaOperacoesPermitidas(usuario);
            usuarioLogadoParaLAterarSessao.Lista_operacoes_permitidas = lstOperacoesPermitidas;

            usuarioLogadoParaLAterarSessao.Loja_troca_rapida_monta_itens_select = await usuarioAcessoBll.Loja_troca_rapida_monta_itens_select_a_partir_banco(usuario, null);

            usuarioLogadoParaLAterarSessao.SessaoAtiva = true;
        }

        public void EncerrarSessao()
        {
            SessaoAtiva = false;
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
            public static readonly string SessaoDeslogadaForcado = "SessaoDeslogadaForcado";
            public static readonly string NomeUsuario = "usuario";
            public static readonly string Lista_operacoes_permitidas = "lista_operacoes_permitidas";
            public static readonly string Loja_troca_rapida_monta_itens_select = "Loja_troca_rapida_monta_itens_select";
            public static readonly string LojaAtiva = "LojaAtiva";
        }
        //esta aqui é usada para verificar outros logins
        public static string StringsSession_SessaoAtiva { get { return StringsSession.SessaoAtiva; } }
        public static string StringsSession_SessaoDeslogadaForcado { get { return StringsSession.SessaoDeslogadaForcado; } }

        public string Usuario
        {
            get => httpContextSession.GetString(StringsSession.NomeUsuario) ?? "Sem usuário";
            private set
            {
                httpContextSession.SetString(StringsSession.NomeUsuario, value);
                new UsuarioSessoes().RegistrarSessao(value, httpContextSession);
            }
        }
        public bool SessaoAtiva
        {
            get
            {
                var ret = httpContextSession.GetInt32(StringsSession.SessaoAtiva);
                if (ret.HasValue && ret.Value != 0)
                    return true;
                return false;
            }
            private set
            {
                if (value)
                    httpContextSession.SetInt32(StringsSession.SessaoAtiva, 1);
                else
                    httpContextSession.SetInt32(StringsSession.SessaoAtiva, 0);
            }
        }
        public bool SessaoDeslogadaForcado
        {
            get
            {
                var ret = httpContextSession.GetInt32(StringsSession.SessaoDeslogadaForcado);
                if (ret.HasValue && ret.Value != 0)
                    return true;
                return false;
            }
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
