using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Loja.Testes.Automatizados.Especificacao.Pedido.SalvarPedido
{
    [Binding]
    public class PedidoMinimoSteps
    {
        private readonly ScenarioContext scenarioContext;
        public PedidoMinimoSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Existe ""(.*)"" = ""(.*)""")]
        public void GivenExiste(string p0, string p1)
        {
            //scenarioContext.Pending();
        }

        [When(@"Fiz login como ""(.*)"" e escolhi a loja ""(.*)""")]
        public void WhenFizLoginComoEEscolhiALoja(string p0, string p1)
        {
            //this.scenarioContext.Pending();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            //scenarioContext.Pending();
        }

        [When(@"informo todos os outros campos")]
        public void WhenInformoTodosOsOutrosCampos()
        {
            //scenarioContext.Pending();
        }

        [When(@"Salvo o pedido")]
        public void WhenSalvoOPedido()
        {
            //scenarioContext.Pending();
        }

        [Then(@"O pedido é criado")]
        public void ThenOPedidoECriado()
        {
            //scenarioContext.Pending();
            //Assert.Equal(1, 0);
        }
        [Given(@"Existe cliente ""(.*)"" = ""(.*)"" como PF")]
        public void GivenExisteClienteComoPF(string p0, string p1)
        {
            //this.scenarioContext.Pending();
        }

        [Given(@"Existe produto ""(.*)"" = ""(.*)"", ""(.*)"" = ""(.*)"", ""(.*)"" = ""(.*)""")]
        public void GivenExisteProduto(string p0, string p1, string p2, string p3, string p4, string p5)
        {
            //this.scenarioContext.Pending();
        }
        [Given(@"Pedido base")]
        public void GivenPedidoBase()
        {
            //this.scenarioContext.Pending();
        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            //this.scenarioContext.Pending();
        }
    }
}
