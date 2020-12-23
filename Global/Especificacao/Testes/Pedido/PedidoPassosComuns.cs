using Especificacao.Testes.Utils.ListaDependencias;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Pedido
{
    public class PedidoPassosComuns : ListaImplementacoes<IPedidoPassosComuns>, IPedidoPassosComuns
    {
        public void GivenPedidoBase()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBase(this);
            base.Executar(i => i.GivenPedidoBase());
        }

        public void GivenPedidoBaseClientePF()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBaseClientePF(this);
            base.Executar(i => i.GivenPedidoBaseClientePF());
        }

        public void GivenPedidoBaseClientePJ()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            base.Executar(i => i.GivenPedidoBaseClientePJ());
        }

        public void GivenPedidoBaseClientePJComEnderecoDeEntrega()
        {
            if (ignorarFeature) return;
            Utils.LogTestes.LogOperacoes2.DadoBaseClientePJComEnderecoDeEntrega(this);
            base.Executar(i => i.GivenPedidoBaseClientePJComEnderecoDeEntrega());
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
        public void ListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            base.Executar(i => i.ListaDeItensInformo(numeroItem, campo, valor));
        }

        public void RecalcularTotaisDoPedido()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.RecalcularTotaisDoPedido(this);
            base.Executar(i => i.RecalcularTotaisDoPedido());
        }
        public void DeixarFormaDePagamentoConsistente()
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.DeixarFormaDePagamentoConsistente(this);
            base.Executar(i => i.DeixarFormaDePagamentoConsistente());
        }
        public void ListaDeItensComXitens(int p0)
        {
            if (ignorarFeature) return;
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(p0, this);
            base.Executar(i => i.ListaDeItensComXitens(p0));
        }

        private bool ignorarFeature = false;
        public void GivenIgnorarCenarioNoAmbiente(string p0)
        {
            IgnorarCenarioNoAmbiente(p0, ref ignorarFeature, this.GetType());
            base.Executar(i => i.GivenIgnorarCenarioNoAmbiente(p0));
        }

        public static void IgnorarCenarioNoAmbiente(string p0, ref bool ignorarFeature, Type getType)
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
