using System;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Pedido
{
    /*
	Está aqui somente por documentação, não é efetivamente testado
     * */

    [Binding, Scope(Tag = "Implementacao.PedidoCaminhoFelizAsp")]
    public class CaminhoFelizAspSteps
    {
        [Given(@"Fiz login")]
        public void GivenFizLogin()
        {
        }

        [Given(@"Estou na página ""(.*)""")]
        public void GivenEstouNaPagina(string p0)
        {
        }

        [When(@"Seleciono a opção ""(.*)"" e clico em ""(.*)""")]
        public void WhenSelecionoAOpcaoEClicoEm(string p0, string p1)
        {
        }

        [When(@"Seleciono o produto ""(.*)"" e quantidade ""(.*)"" e clico em ""(.*)""")]
#pragma warning disable IDE0060 // Remove unused parameter
        public void WhenSelecionoOProdutoEQuantidadeEClicoEm(int p0, int p1, string p2)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }

        [When(@"Seleciono a opção ""(.*)"" como ""(.*)""")]
#pragma warning disable IDE0060 // Remove unused parameter
        public void WhenSelecionoAOpcaoComo(string p0, string p1)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }

        [When(@"Clico em ""(.*)""")]
        public void WhenClicoEm(string p0)
        {
        }

        [Then(@"Vou para página ""(.*)""")]
#pragma warning disable IDE0060 // Remove unused parameter
        public void ThenVouParaPagina(string p0)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }

        [Then(@"O pedido é criado")]
        public void ThenOPedidoECriado()
        {
        }
        [When(@"No bloco ""(.*)"" digito o CPF/CNPJ ""(.*)"" e clico em ""(.*)""")]
        public void WhenNoBlocoDigitoOCPFCNPJEClicoEm(string p0, string p1, string p2)
        {
        }
    }
}
