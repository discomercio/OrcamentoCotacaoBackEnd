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

#nullable enable

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioLogado
    {
        private readonly ISession httpContextSession;


        public UsuarioLogado(ClaimsPrincipal? user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao)
        {
            this.httpContextSession = httpContextSession;
            if (SessaoAtiva)
            {
                //sem autenticacao, sem sessão!
                if (user?.Identity == null)
                    SessaoAtiva = false;
                if (!(user?.Identity?.IsAuthenticated ?? false) || string.IsNullOrEmpty(user?.Identity?.Name))
                    SessaoAtiva = false;

                //verificamos se o nome bate
                if (user?.Identity != null)
                    if (user.Identity.IsAuthenticated && !string.IsNullOrEmpty(user.Identity.Name))
                        if (user.Identity.Name != Usuario)
                            SessaoAtiva = false;
            }

            if (configuracao.PermitirManterConectado)
                if (!SessaoAtiva && clienteBll != null && usuarioAcessoBll != null)
                    CriarSessaoPorUser(user, httpContextSession, clienteBll, usuarioAcessoBll, configuracao, this);

            //verificar se devemos renovar as permissões do usuário
            if (Lista_operacoes_permitidas_data_atualizacao.AddMinutes(configuracao.RecarregarPermissoesUsuarioMinutos) < DateTimeOffset.UtcNow
                && clienteBll != null && usuarioAcessoBll != null)
            {
                CriarSessao_Lista_operacoes_permitidas(clienteBll, this, usuarioAcessoBll);
            }
        }

        private void CriarSessao_Lista_operacoes_permitidas(ClienteBll.ClienteBll clienteBll, UsuarioLogado usuarioLogadoParaLAterarSessao,
            UsuarioAcessoBll usuarioAcessoBll)
        {
            usuarioLogadoParaLAterarSessao.LojasDisponiveis =
                usuarioAcessoBll.Loja_troca_rapida_monta_itens_select_a_partir_banco(Usuario, null).Result;

            string lstOperacoesPermitidas = clienteBll.BuscaListaOperacoesPermitidas(Usuario).Result;
            Lista_operacoes_permitidas = lstOperacoesPermitidas;
            Lista_operacoes_permitidas_data_atualizacao = DateTimeOffset.UtcNow;
        }

        private void CriarSessaoPorUser(ClaimsPrincipal? user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            UsuarioLogado usuarioLogadoParaLAterarSessao)
        {
            string? usuarioClaim = user?.Claims.Where(r => r.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            CriarSessao(usuarioClaim, httpContextSession, clienteBll, usuarioAcessoBll, configuracao, usuarioLogadoParaLAterarSessao);
        }
        public static void CriarSessao(string? usuario, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            UsuarioLogado? usuarioLogadoParaLAterarSessao = null)
        {
            //nao tem risco de dar recursão porque o construtor chama com uma instância
            if (usuarioLogadoParaLAterarSessao == null)
                usuarioLogadoParaLAterarSessao = new UsuarioLogado(null, httpContextSession, clienteBll, usuarioAcessoBll, configuracao);

            //tem que recriar
            if (string.IsNullOrWhiteSpace(usuario))
                return;
            usuario = usuario.Trim().ToUpper();
            usuarioLogadoParaLAterarSessao.Usuario = usuario;

            usuarioLogadoParaLAterarSessao.CriarSessao_Lista_operacoes_permitidas(clienteBll, usuarioLogadoParaLAterarSessao, usuarioAcessoBll);
            usuarioLogadoParaLAterarSessao.SessaoAtiva = true;
        }

        private static DateTimeOffset Lista_operacoes_permitidas_data_atualizacao_bem_antiga { get => new DateTimeOffset(2000, 01, 01, 0, 0, 0, DateTimeOffset.UtcNow.Offset); }

        public static class ClaimsUsuario
        {
            public static readonly string ClaimEmissao = "http://arclube.itssolucoes.com.br/claims/emissao";
            public static List<Claim> CriarClaims(string apelido)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, apelido));
                //emissao como ticks
                claims.Add(new Claim(ClaimEmissao, DateTimeOffset.UtcNow.Ticks.ToString()));
                return claims;
            }
            public static bool ConfirmarSenhaPorGet(IEnumerable<Claim> claims, Configuracao configuracao)
            {
                var emissao = claims.Where(r => r.Type == ClaimEmissao).FirstOrDefault()?.Value;
                if (emissao == null)
                    return true;
                //vemos se já passou o tempo da emissao
                if (!long.TryParse(emissao, out long emissaoticks))
                    return true;
                DateTimeOffset emissaodt = new DateTimeOffset(emissaoticks, DateTimeOffset.UtcNow.Offset);
                if (emissaodt.AddMinutes(configuracao.ForcarLoginPorGetMinutos) < DateTimeOffset.UtcNow)
                    return true;
                return false;
            }
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
            public static readonly string NomeUsuario = "usuario";
            public static readonly string Lista_operacoes_permitidas = "lista_operacoes_permitidas";
            public static readonly string Lista_operacoes_permitidas_data_atualizacao = "Lista_operacoes_permitidas_data_atualizacao";
            public static readonly string LojasDisponiveis = "LojasDisponiveis";
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
        public string Lista_operacoes_permitidas
        {
            get => httpContextSession.GetString(StringsSession.Lista_operacoes_permitidas) ?? "";
            private set => httpContextSession.SetString(StringsSession.Lista_operacoes_permitidas, value);
        }
        public DateTimeOffset Lista_operacoes_permitidas_data_atualizacao
        {
            get
            {
                var emissao = httpContextSession.GetString(StringsSession.Lista_operacoes_permitidas_data_atualizacao) ?? "";
                if (string.IsNullOrWhiteSpace(emissao))
                    return Lista_operacoes_permitidas_data_atualizacao_bem_antiga;
                if (!long.TryParse(emissao, out long emissaoticks))
                    return Lista_operacoes_permitidas_data_atualizacao_bem_antiga;
                DateTimeOffset emissaodt = new DateTimeOffset(emissaoticks, DateTimeOffset.UtcNow.Offset);
                return emissaodt;
            }
            private set
            {
                httpContextSession.SetString(StringsSession.Lista_operacoes_permitidas_data_atualizacao, value.Ticks.ToString());
            }
        }

        public string LojaAtiva
        {
            get => httpContextSession.GetString(StringsSession.LojaAtiva) ?? "";
            private set => httpContextSession.SetString(StringsSession.LojaAtiva, value);
        }

        public bool LojaAtivaAlterar(string novaloja)
        {
            //verifica se pode ir para essa loja
            if (!LojasDisponiveis.Any(r => r.Id == novaloja))
                return false;
            LojaAtiva = novaloja;
            return true;
        }

        public List<UsuarioAcessoBll.LojaPermtidaUsuario> LojasDisponiveis
        {
            get
            {
                var sessao = httpContextSession.GetString(StringsSession.LojasDisponiveis);
                //ao invés de dar exceção, retornamos uma lsita vazia. Para dimiuir o número de erros imprevistos;
                if (sessao == null)
                    return new List<UsuarioAcessoBll.LojaPermtidaUsuario>();
                return JsonConvert.DeserializeObject<List<UsuarioAcessoBll.LojaPermtidaUsuario>>(sessao);
            }
            private set => httpContextSession.SetString(StringsSession.LojasDisponiveis, JsonConvert.SerializeObject(value));
        }

    }

}
