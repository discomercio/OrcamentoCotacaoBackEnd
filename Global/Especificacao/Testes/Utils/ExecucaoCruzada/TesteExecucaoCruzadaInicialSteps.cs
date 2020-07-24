using System;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Utils.ExecucaoCruzada
{
    [Binding, Scope(Tag = "TesteExecucaoCruzada")]
    public class TesteExecucaoCruzadaInicialSteps
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ScenarioContext scenarioContext;
        readonly TesteExecucaoCruzada.LogicaParteFinal logicaParteFinal = new TesteExecucaoCruzada.LogicaParteFinal();
#pragma warning restore IDE0052 // Remove unread private members

        public TesteExecucaoCruzadaInicialSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Passo(.*)")]
#pragma warning disable IDE0060 // Remove unused parameter
        public void GivenPasso(int p0)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }

        [When(@"Passo(.*)")]
#pragma warning disable IDE0060 // Remove unused parameter
        public void WhenPasso(int p0)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }

        [Then(@"Passo(.*)")]
#pragma warning disable IDE0060 // Remove unused parameter
        public void ThenPasso(int p0)
#pragma warning restore IDE0060 // Remove unused parameter
        {
        }
    }
}
