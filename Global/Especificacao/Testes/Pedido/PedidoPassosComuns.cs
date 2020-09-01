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
            mensagem = Utils.MapeamentoMensagens.MapearMensagem(this.GetType().FullName, mensagem);
            logTestes.LogMensagem($"PedidoPassosComuns {this.GetType().FullName} ThenSemErro({mensagem})");
            base.Executar(i => i.ThenSemErro(mensagem));
        }

        public void ThenErro(string mensagem)
        {
            if (ignorarFeature) return;
            mensagem = Utils.MapeamentoMensagens.MapearMensagem(this.GetType().FullName, mensagem);
            logTestes.LogMensagem($"PedidoPassosComuns {this.GetType().FullName} ThenErro({mensagem})");
            base.Executar(i => i.ThenErro(mensagem));
        }

        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            logTestes.LogMensagem($"PedidoPassosComuns {this.GetType().FullName} ThenSemNenhumErro()");
            base.Executar(i => i.ThenSemNenhumErro());
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

        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            base.Executar(i => i.GivenPedidoBaseComEnderecoDeEntrega());
        }
    }
}
