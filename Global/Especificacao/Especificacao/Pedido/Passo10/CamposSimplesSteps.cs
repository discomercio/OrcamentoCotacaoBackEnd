using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using System;
using TechTalk.SpecFlow;

namespace Especificacao.Especificacao.Pedido.Passo10
{
    [Binding, Scope(Tag = "Especificacao.Pedido.Passo10.CamposSimples")]
    public class CamposSimplesSteps : PedidoPassosComuns
    {
        public CamposSimplesSteps(FeatureContext featureContext)
        {
            base.AdicionarImplementacao(new Especificacao.Pedido.PedidoSteps());
            RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", "Especificacao.Pedido.Passo10.CamposSimplesListaDependencias");
        }

        [When(@"Pedido base")]
        new public void WhenPedidoBase()
        {
            base.WhenPedidoBase();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        new public void WhenInformo(string p0, string p1)
        {
            base.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        new public void ThenErro(string p0)
        {
            base.ThenErro(p0);
        }

        [Then(@"Sem erro ""(.*)""")]
        new public void ThenSemErro(string p0)
        {
            base.ThenSemErro(p0);
        }

        [Given(@"No ambiente ""(.*)"" erro ""(.*)"" é ""(.*)""")]
        public void GivenNoAmbienteErroE(string p0, string p1, string p2)
        {
            Testes.Utils.MapeamentoMensagens.GivenNoAmbienteErroE(p0, p1, p2);
        }

        [Given(@"Ignorar feature no ambiente ""(.*)""")]
        new public void GivenIgnorarFeatureNoAmbiente(string p0)
        {
            base.GivenIgnorarFeatureNoAmbiente(p0);
        }
    }
}
