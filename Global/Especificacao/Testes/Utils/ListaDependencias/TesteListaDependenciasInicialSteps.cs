using System;
using System.Reflection;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Utils.ListaDependencias
{
    [Binding, Scope(Tag = "Testes.TesteListaDependencias")]
    public class TesteListaDependenciasInicialSteps
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ScenarioContext scenarioContext;
        readonly TesteListaDependencias.LogicaParteFinal logicaParteFinal = new TesteListaDependencias.LogicaParteFinal();
#pragma warning restore IDE0052 // Remove unread private members

        public TesteListaDependenciasInicialSteps(ScenarioContext scenarioContext)
        {
            LogTestes.Log("TesteListaDependenciasInicialSteps");
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
