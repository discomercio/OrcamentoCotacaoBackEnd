using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Utils.TesteExecucaoCruzada
{
    public class LogicaParteFinal
    {
        public LogicaParteFinal()
        {
            instanciacoes++;
        }
        internal static int instanciacoes = 0;
    }
    [Binding, Scope(Tag = "Implementacao/TesteExecucaoCruzada")]
    public class TesteExecucaoCruzadaFinalSteps
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ScenarioContext scenarioContext;
#pragma warning restore IDE0052 // Remove unread private members

        public TesteExecucaoCruzadaFinalSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Then(@"Garantir que executou")]
        public void ThenGarantirQueExecutou()
        {
            Assert.NotEqual(0, LogicaParteFinal.instanciacoes);
        }
    }
}
