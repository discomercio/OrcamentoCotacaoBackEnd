using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Loja.Bll.Bll.AcessoBll
{
    /*
     * esta classe gerencia todas as sessões ativas
     * quando o usuário fizer o login, este cara verifica se tem outra sessão ativa desse mesmo usuário
     * */
    public class UsuarioSessoes
    {
        public void RegistrarSessao(string usuario, ISession sessao)
        {
            UsuarioSessoesEstatico.RegistrarSessao(usuario, sessao);
        }

        public bool DeslogarLoginAnterior(string usuario, ISession sessionAtual)
        {
            return UsuarioSessoesEstatico.DeslogarLoginAnterior(usuario, sessionAtual);
        }


        private static class UsuarioSessoesEstatico
        {
            private static object __lockObject = new object();

            private static readonly Dictionary<string, List<ISession>> dicionarioUsuarioSessao = new Dictionary<string, List<ISession>>();

            public static void RegistrarSessao(string usuario, ISession sessao)
            {
                lock (__lockObject)
                {
                    //forçamos a manter a sessão
                    sessao.SetInt32("__RegistrarSessao", 0);
                    if (!dicionarioUsuarioSessao.ContainsKey(usuario))
                        dicionarioUsuarioSessao.Add(usuario, new List<ISession>());
                    dicionarioUsuarioSessao[usuario].Add(sessao);
                }
            }

            public static bool DeslogarLoginAnterior(string usuario, ISession sessionAtual)
            {
                lock (__lockObject)
                {
                    sessionAtual.SetInt32(UsuarioLogado.StringsSession_SessaoDeslogadaForcado, 0);

                    if (!dicionarioUsuarioSessao.ContainsKey(usuario))
                        return false;
                    var sessionList = dicionarioUsuarioSessao[usuario];
                    var outraSession = sessionList.Where(r => r.Id != sessionAtual.Id
                        && r.IsAvailable
                        && r.Keys.Contains(UsuarioLogado.StringsSession_SessaoAtiva)
                        && r.GetInt32(UsuarioLogado.StringsSession_SessaoAtiva) == 1
                        );
                    if (outraSession.Count() == 0)
                        return false;

                    //anulamos as sessoes anteriores
                    foreach (var session in outraSession)
                    {
                        try
                        {
                            session.SetInt32(UsuarioLogado.StringsSession_SessaoAtiva, 0);
                            session.SetInt32(UsuarioLogado.StringsSession_SessaoDeslogadaForcado, 1);
                        }
                        catch(ObjectDisposedException)
                        {
                            //sim, ignoramos
                        }
                    }

                    return true;
                }
            }
        }
    }
}


