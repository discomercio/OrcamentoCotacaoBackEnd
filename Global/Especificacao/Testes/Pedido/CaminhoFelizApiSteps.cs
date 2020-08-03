using System;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Pedido
{
    [Binding, Scope(Tag = "Especificacao/Pedido/CaminhoFelizApi")]
    public class CaminhoFelizApiSteps
    {
        private readonly ScenarioContext scenarioContext;

        public CaminhoFelizApiSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }
        [Given(@"Existe ""(.*)"" = ""(.*)""")]
#pragma warning disable IDE0060 //unused parameter
        public void GivenExiste(string p0, string p1)
        {
            scenarioContext.Pending();
        }

        [Given(@"Existe cliente ""(.*)"" = ""(.*)"" como PF")]
        public void GivenExisteClienteComoPF(string p0, int p1)
        {
            scenarioContext.Pending();
        }

        [Given(@"Existe produto ""(.*)"" = ""(.*)"", ""(.*)"" = ""(.*)"", ""(.*)"" = ""(.*)""")]
        public void GivenExisteProduto(string p0, int p1, string p2, int p3, string p4, int p5)
        {
            scenarioContext.Pending();
        }

        [When(@"Fiz login como ""(.*)"" e escolhi a loja ""(.*)""")]
        public void WhenFizLoginComoEEscolhiALoja(string p0, int p1)
        {
            scenarioContext.Pending();
        }

        [When(@"Pedido vazio")]
        public void WhenPedidoVazio()
        {
            scenarioContext.Pending();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            scenarioContext.Pending();
        }

        [When(@"Salvo o pedido")]
        public void WhenSalvoOPedido()
        {
            scenarioContext.Pending();
        }

        [Then(@"O pedido é criado")]
        public void ThenOPedidoECriado()
        {
            scenarioContext.Pending();
        }

        [Then(@"Campo ""(.*)"" = ""(.*)""")]
        public void ThenCampo(string p0, int p1)
        {
            scenarioContext.Pending();
        }
    }
}
