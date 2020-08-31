using Especificacao.Testes.Utils.ListaDependencias;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Pedido
{
    public class PedidoPassosComuns : ListaImplementacoes<IPedidoPassosComuns>, IPedidoPassosComuns
    {
        private readonly Testes.Utils.LogTestes logTestes = Testes.Utils.LogTestes.GetInstance();

        public void WhenPedidoBase()
        {
            if (ignorarFeature) return;
            base.Executar(i => i.WhenPedidoBase());
        }

        public void WhenInformo(string p0, string p1)
        {
            if (ignorarFeature) return;
            logTestes.LogMensagem($"PedidoPassosComuns {this.GetType().FullName} WhenInformo({p0}, {p1})");
            base.Executar(i => i.WhenInformo(p0, p1));
        }

        public void ThenSemErro(string mensagem)
        {
            if (ignorarFeature) return;
            mensagem = MapearMensagem(this.GetType().FullName, mensagem);
            logTestes.LogMensagem($"PedidoPassosComuns {this.GetType().FullName} ThenSemErro({mensagem})");
            base.Executar(i => i.ThenSemErro(mensagem));
        }

        public void ThenErro(string mensagem)
        {
            if (ignorarFeature) return;
            mensagem = MapearMensagem(this.GetType().FullName, mensagem);
            logTestes.LogMensagem($"PedidoPassosComuns {this.GetType().FullName} ThenErro({mensagem})");
            base.Executar(i => i.ThenErro(mensagem));
        }

        private bool ignorarFeature = false;
        public void GivenIgnorarFeatureNoAmbiente(string p0)
        {
            var typeFullName = this.GetType().FullName;
            if (typeFullName == null)
            {
                Assert.Equal("", "sem this.GetType().FullName");
                return;
            }

            //mal resolvido: temos um Especificacao na frente.... bom, tiramos!
            typeFullName = typeFullName.Replace("Especificacao.Ambiente.", "Ambiente.");
            typeFullName = typeFullName.Replace("Especificacao.Especificacao.", "Especificacao.");

            if (typeFullName == p0)
                ignorarFeature = true;
            base.Executar(i => i.GivenIgnorarFeatureNoAmbiente(p0));
        }

        //fazemos como static para facilitar MUITO vida
        private static Dictionary<string, Dictionary<string, string>> dicionarioMensagens = new Dictionary<string, Dictionary<string, string>>();
        public static void GivenNoAmbienteErroE(string ambiente, string msgOriginal, string msgSubstituta)
        {
            if (!dicionarioMensagens.ContainsKey(ambiente))
                dicionarioMensagens.Add(ambiente, new Dictionary<string, string>());
            var mensagens = dicionarioMensagens[ambiente];
            if (!mensagens.ContainsKey(msgOriginal))
                mensagens.Add(msgOriginal, msgSubstituta);

            //se tentarem colocar mensagens diferentes damos erro
            Assert.Equal(mensagens[msgOriginal], msgSubstituta);
        }
        public static string MapearMensagem(string? typeFullName, string msgOriginal)
        {
            if (typeFullName == null)
            {
                Assert.Equal("", "sem this.GetType().FullName");
                return msgOriginal;
            }

            //mal resolvido: temos um Especificacao na frente.... bom, tiramos!
            typeFullName = typeFullName.Replace("Especificacao.Ambiente.", "Ambiente.");
            typeFullName = typeFullName.Replace("Especificacao.Especificacao.", "Especificacao.");

            if (!dicionarioMensagens.ContainsKey(typeFullName))
                return msgOriginal;
            var mensagens = dicionarioMensagens[typeFullName];
            if (!mensagens.ContainsKey(msgOriginal))
                return msgOriginal;
            return mensagens[msgOriginal];
        }


    }
}
