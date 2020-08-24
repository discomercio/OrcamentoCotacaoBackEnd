using System;
using System.Reflection;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Utils.TesteListaDependencias
{
    public class LogicaParteFinal
    {
        public LogicaParteFinal()
        {
            LogTestes.Log("LogicaParteFinal instanciacoes++");
            instanciacoes++;
        }
        internal static int instanciacoes = 0;
    }
    [Binding, Scope(Tag = "Testes.TesteListaDependencias.Final")]
    public class TesteListaDependenciasFinalSteps
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ScenarioContext scenarioContext;
#pragma warning restore IDE0052 // Remove unread private members

        public TesteListaDependenciasFinalSteps(ScenarioContext scenarioContext)
        {
            LogTestes.Log("TesteListaDependenciasFinalSteps");
            this.scenarioContext = scenarioContext;
        }

        [Then(@"Garantir que executou")]
        public void ThenGarantirQueExecutou()
        {
            Assert.NotEqual(0, LogicaParteFinal.instanciacoes);
        }
    }
}
