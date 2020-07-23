using System;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Utils.ExecucaoCruzada
{
    [Binding, Scope(Tag = "TesteExecucaoCruzada")]
    public class TesteExecucaoCruzadaInicialSteps
    {
        private readonly ScenarioContext scenarioContext;
        TesteExecucaoCruzada.LogicaParteFinal logicaParteFinal = new TesteExecucaoCruzada.LogicaParteFinal();

        public TesteExecucaoCruzadaInicialSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Passo(.*)")]
        public void GivenPasso(int p0)
        {
        }

        [When(@"Passo(.*)")]
        public void WhenPasso(int p0)
        {
        }

        [Then(@"Passo(.*)")]
        public void ThenPasso(int p0)
        {
        }
    }
}
