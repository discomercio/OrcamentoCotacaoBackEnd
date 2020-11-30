using Especificacao.Testes.Utils.ListaDependencias;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Pedido
{
    public class PedidoPassosComuns : ListaImplementacoes<IPedidoPassosComuns>, IPedidoPassosComuns
    {
        public void WhenPedidoBase()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBase(this);
            base.Executar(i => i.WhenPedidoBase());
        }

        public void WhenPedidoBaseClientePF()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBaseClientePF(this);
            base.Executar(i => i.WhenPedidoBaseClientePF());
        }

        public void WhenPedidoBaseClientePJ()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            base.Executar(i => i.WhenPedidoBaseClientePJ());
        }

        public void WhenInformo(string p0, string p1)
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            base.Executar(i => i.WhenInformo(p0, p1));
        }

        public void ThenSemErro(string mensagem)
        {
            if (ignorarFeature) return;
            mensagem = Utils.MapeamentoMensagens.MapearMensagem(this.GetType().FullName, mensagem);
            Utils.LogTestes.LogOperacoes2.SemErro(mensagem, this);
            base.Executar(i => i.ThenSemErro(mensagem));
        }

        public void ThenErro(string mensagem)
        {
            if (ignorarFeature) return;
            mensagem = Utils.MapeamentoMensagens.MapearMensagem(this.GetType().FullName, mensagem);
            Utils.LogTestes.LogOperacoes2.Erro(mensagem, this);
            base.Executar(i => i.ThenErro(mensagem));
        }

        public void ThenSemNenhumErro()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            base.Executar(i => i.ThenSemNenhumErro());
        }

        private bool ignorarFeature = false;
        public void GivenIgnorarFeatureNoAmbiente2(string p0)
        {
            IgnorarFeatureNoAmbiente(p0, ref ignorarFeature, this.GetType());
            base.Executar(i => i.GivenIgnorarFeatureNoAmbiente2(p0));
        }

        public static void IgnorarFeatureNoAmbiente(string p0, ref bool ignorarFeature, Type getType)
        {
            var typeFullName = getType.FullName;
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
        }

        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            base.Executar(i => i.GivenPedidoBaseComEnderecoDeEntrega());
        }
    }
}
