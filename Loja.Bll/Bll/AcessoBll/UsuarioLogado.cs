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
using Loja.Modelos;
using Microsoft.Extensions.Logging;
using Loja.Bll.Dto.ClienteDto;
using Loja.Bll.Dto.PedidoDto.DetalhesPedido;

#nullable enable

namespace Loja.Bll.Bll.AcessoBll
{
    public class UsuarioLogado
    {
        private readonly ISession httpContextSession;


        public UsuarioLogado(ILogger<UsuarioLogado> logger, ClaimsPrincipal? user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao)
        {
            logger.LogTrace($"UsuarioLogado inicio");

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
                        if (user.Identity.Name != Usuario_atual)
                            SessaoAtiva = false;
            }

            logger.LogTrace($"UsuarioLogado {SessaoAtiva}");

            if (configuracao.PermitirManterConectado)
                if (!SessaoAtiva && clienteBll != null && usuarioAcessoBll != null)
                    CriarSessaoPorUser(logger, user, httpContextSession, clienteBll, usuarioAcessoBll, configuracao, this);

            //verificar se devemos renovar as permissões do usuário
            if (Lista_operacoes_permitidas_data_atualizacao.AddMinutes(configuracao.RecarregarPermissoesUsuarioMinutos) < DateTimeOffset.UtcNow
                && clienteBll != null && usuarioAcessoBll != null)
            {
                CriarSessao_carregar_permissoes_banco(logger, clienteBll, this, usuarioAcessoBll, null, null, null).Wait();
            }
        }

        private static async Task CriarSessao_carregar_permissoes_banco(ILogger<UsuarioLogado> logger, ClienteBll.ClienteBll clienteBll, UsuarioLogado usuarioLogadoParaLAterarSessao,
            UsuarioAcessoBll usuarioAcessoBll, Tusuario? tusuario, string? loja, string? loja_nome)
        {
            if (!usuarioLogadoParaLAterarSessao.Usuario_atual_existe)
                return;

            logger.LogTrace($"CriarSessao_carregar_permissoes_banco {usuarioLogadoParaLAterarSessao.Usuario_atual}");

            usuarioLogadoParaLAterarSessao.LojasDisponiveis =
                usuarioAcessoBll.Loja_troca_rapida_monta_itens_select_a_partir_banco(usuarioLogadoParaLAterarSessao.Usuario_atual, null).Result;

            var Lista_operacoes_permitidas_task = clienteBll.BuscaListaOperacoesPermitidas(usuarioLogadoParaLAterarSessao.Usuario_atual);
            var Nivel_acesso_chamado_task = clienteBll.NivelAcessoChamadoPedido(usuarioLogadoParaLAterarSessao.Usuario_atual);
            var Nivel_acesso_bloco_notas_task = clienteBll.NivelAcessoBlocoNotasPedido(usuarioLogadoParaLAterarSessao.Usuario_atual);

            usuarioLogadoParaLAterarSessao.S_lista_operacoes_permitidas = await Lista_operacoes_permitidas_task;
            usuarioLogadoParaLAterarSessao.Nivel_acesso_chamado = await Nivel_acesso_chamado_task;
            usuarioLogadoParaLAterarSessao.Nivel_acesso_bloco_notas = await Nivel_acesso_bloco_notas_task;

            //mais dados na session
            if (tusuario == null)
                tusuario = await usuarioAcessoBll.UsuarioCarregar(usuarioLogadoParaLAterarSessao.Usuario_atual);
			if (tusuario == null)
            {
                var msg = $"Erro: usuarioAcessoBll.UsuarioCarregar {usuarioLogadoParaLAterarSessao.Usuario_atual} não encontrou o usuário";
                logger.LogError(msg);
                //voltamos e permitimos o acesso. No fundo, não tem autorização nenhuma.
                return;
            }
            if (loja == null)
                loja = tusuario.Loja;
            if (loja_nome == null)
                loja_nome = await usuarioAcessoBll.Loja_nome(loja);

            usuarioLogadoParaLAterarSessao.Loja_atual_id = loja;
            usuarioLogadoParaLAterarSessao.Usuario_nome_atual = tusuario.Nome;
            usuarioLogadoParaLAterarSessao.Loja_nome_atual = loja_nome ?? "";
            usuarioLogadoParaLAterarSessao.Vendedor_loja = tusuario.Vendedor_Loja != 0;
            usuarioLogadoParaLAterarSessao.Vendedor_externo = tusuario.Vendedor_Externo != 0;

            usuarioLogadoParaLAterarSessao.Lista_operacoes_permitidas_data_atualizacao = DateTimeOffset.UtcNow;
        }

        private void CriarSessaoPorUser(ILogger<UsuarioLogado> logger, ClaimsPrincipal? user, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            UsuarioLogado usuarioLogadoParaLAterarSessao)
        {
            string? usuarioClaim = user?.Claims.Where(r => r.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            logger.LogTrace($"CriarSessaoPorUser {usuarioClaim}");
            CriarSessao(logger, usuarioClaim, httpContextSession, clienteBll, usuarioAcessoBll, configuracao, usuarioLogadoParaLAterarSessao);
        }
        public static void CriarSessao(ILogger<UsuarioLogado> logger, string? usuario, ISession httpContextSession, ClienteBll.ClienteBll clienteBll,
            UsuarioAcessoBll usuarioAcessoBll, Configuracao configuracao,
            UsuarioLogado? usuarioLogadoParaLAterarSessao = null)
        {
            //nao tem risco de dar recursão porque o construtor chama com uma instância
            if (usuarioLogadoParaLAterarSessao == null)
                usuarioLogadoParaLAterarSessao = new UsuarioLogado(logger, null, httpContextSession, clienteBll, usuarioAcessoBll, configuracao);

            logger.LogTrace($"CriarSessao {usuario}");

            //tem que recriar
            if (string.IsNullOrWhiteSpace(usuario))
                return;
            usuario = usuario.Trim().ToUpper();
            usuarioLogadoParaLAterarSessao.Usuario_atual = usuario;
            usuarioLogadoParaLAterarSessao.Verificou_quadro_avisos = false;

            CriarSessao_carregar_permissoes_banco(logger, clienteBll, usuarioLogadoParaLAterarSessao, usuarioAcessoBll,
                null, null, null).Wait();
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

        public static bool Operacao_permitida_estatica(int id_operacao, string Lista_operacoes_permitidas)
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
        public bool Operacao_permitida(int id_operacao)
        {
            return Operacao_permitida_estatica(id_operacao, S_lista_operacoes_permitidas);
        }



        //o que colocamos na session
        private static class StringsSession
        {
            public static readonly string SessaoAtiva = "SessaoAtiva";
            public static readonly string Usuario_atual = "Usuario_atual";
            public static readonly string S_lista_operacoes_permitidas = "S_lista_operacoes_permitidas";
            public static readonly string Lista_operacoes_permitidas_data_atualizacao = "Lista_operacoes_permitidas_data_atualizacao";
            public static readonly string LojasDisponiveis = "LojasDisponiveis";
            public static readonly string Loja_atual_id = "Loja_atual_id";
            public static readonly string Nivel_acesso_bloco_notas = "Nivel_acesso_bloco_notas";
            public static readonly string Nivel_acesso_chamado = "Nivel_acesso_chamado";
            public static readonly string Verificou_quadro_avisos = "Verificou_quadro_avisos";
            public static readonly string Loja_atual = "Loja_atual";
            public static readonly string Usuario_nome_atual = "Usuario_nome_atual";
            public static readonly string Loja_nome_atual = "Loja_nome_atual";
            public static readonly string Vendedor_loja = "Vendedor_loja";
            public static readonly string Vendedor_externo = "Vendedor_externo";
            public static readonly string Cliente_Selecionado = "Cliente_Selecionado";
            public static readonly string PedidoDto = "PedidoDto";
        }

        public string Usuario_atual
        {
            get => httpContextSession.GetString(StringsSession.Usuario_atual) ?? "Sem usuário";
            private set => httpContextSession.SetString(StringsSession.Usuario_atual, value);
        }
        public bool Usuario_atual_existe
        {
            get => !string.IsNullOrWhiteSpace(httpContextSession.GetString(StringsSession.Usuario_atual) ?? "");
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
        public string S_lista_operacoes_permitidas
        {
            get => httpContextSession.GetString(StringsSession.S_lista_operacoes_permitidas) ?? "";
            private set => httpContextSession.SetString(StringsSession.S_lista_operacoes_permitidas, value);
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

        public bool LojaAtivaAlterar(string novaloja)
        {
            //verifica se pode ir para essa loja
            if (!LojasDisponiveis.Any(r => r.Id == novaloja))
                return false;
            Loja_atual_id = novaloja;
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

        public int Nivel_acesso_chamado
        {
            get => httpContextSession.GetInt32(StringsSession.Nivel_acesso_chamado) ?? Constantes.Constantes.COD_NIVEL_ACESSO_CHAMADO_PEDIDO__NAO_DEFINIDO;
            private set => httpContextSession.SetInt32(StringsSession.Nivel_acesso_chamado, value);
        }
        public int Nivel_acesso_bloco_notas
        {
            get => httpContextSession.GetInt32(StringsSession.Nivel_acesso_bloco_notas) ?? Constantes.Constantes.COD_NIVEL_ACESSO_BLOCO_NOTAS_PEDIDO__NAO_DEFINIDO;
            private set => httpContextSession.SetInt32(StringsSession.Nivel_acesso_bloco_notas, value);
        }
        public bool Verificou_quadro_avisos
        {
            get => (httpContextSession.GetInt32(StringsSession.Verificou_quadro_avisos) ?? 0) == 1;
            private set => httpContextSession.SetInt32(StringsSession.Verificou_quadro_avisos, value ? 1 : 0);
        }
        public bool Vendedor_externo
        {
            get => (httpContextSession.GetInt32(StringsSession.Vendedor_externo) ?? 0) == 1;
            private set => httpContextSession.SetInt32(StringsSession.Vendedor_externo, value ? 1 : 0);
        }
        public bool Vendedor_loja
        {
            get => (httpContextSession.GetInt32(StringsSession.Vendedor_loja) ?? 0) == 1;
            private set => httpContextSession.SetInt32(StringsSession.Vendedor_loja, value ? 1 : 0);
        }


        public string Loja_atual_id
        {
            get => httpContextSession.GetString(StringsSession.Loja_atual_id) ?? "";
            private set => httpContextSession.SetString(StringsSession.Loja_atual_id, value);
        }
        public string Usuario_nome_atual
        {
            get => httpContextSession.GetString(StringsSession.Usuario_nome_atual) ?? "";
            private set => httpContextSession.SetString(StringsSession.Usuario_nome_atual, value);
        }
        public string Loja_nome_atual
        {
            get => httpContextSession.GetString(StringsSession.Loja_nome_atual) ?? "";
            private set => httpContextSession.SetString(StringsSession.Loja_nome_atual, value);
        }

        public ClienteCadastroDto Cliente_Selecionado
        {
            get
            {
                var sessao = httpContextSession.GetString(StringsSession.Cliente_Selecionado);
                if (sessao == null)
                    return new ClienteCadastroDto();
                return JsonConvert.DeserializeObject<ClienteCadastroDto>(sessao);
            }
            set => httpContextSession.SetString(StringsSession.Cliente_Selecionado, JsonConvert.SerializeObject(value));

        }

        public PedidoDto PedidoDto
        {
            get
            {
                var sessao = httpContextSession.GetString(StringsSession.PedidoDto);
                if (sessao == null)
                    return new PedidoDto();
                return JsonConvert.DeserializeObject<PedidoDto>(sessao);
            }
            set => httpContextSession.SetString(StringsSession.PedidoDto, JsonConvert.SerializeObject(value));
        }
    }

}
