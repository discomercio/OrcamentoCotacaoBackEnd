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
    [Binding, Scope(Tag = "TesteExecucaoCruzada")]
    public class TesteExecucaoCruzadaFinalSteps
    {
        private readonly ScenarioContext scenarioContext;

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
