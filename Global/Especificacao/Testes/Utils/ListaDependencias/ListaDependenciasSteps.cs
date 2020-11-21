using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Utils.ListaDependencias
{
    [Binding, Scope(Tag = "ListaDependencias")]
    public class ListaDependenciasSteps
    {
        private string nomeItem = "";
        [Given(@"Nome deste item ""(.*)""")]
        public void GivenNomeDesteItem(string p0)
        {
            nomeItem = p0;
        }

        [Given(@"Especificado em ""(.*)""")]
        public void GivenEspecificadoEm(string p0)
        {
            RegistroDependencias.GivenEspecificadoEm(nomeItem, p0);
        }

        [Given(@"Implementado em ""(.*)""")]
        public void GivenImplementadoEm(string p0)
        {
            RegistroDependencias.GivenImplementadoEm(p0, nomeItem);
        }

        [Given(@"AdicionarDependencia ambiente = ""(.*)"", especificacao = ""(.*)""")]
        public void GivenAdicionarDependenciaAmbienteEspecificacao(string p0, string p1)
        {
            RegistroDependencias.AdicionarDependencia(p0, null, p1);
        }
    }
}
