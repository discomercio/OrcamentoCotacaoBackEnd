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
            VerificarFimConfiguracao(false);
            //se der erro não queremos dar o erro de falta de fim da confiugração
            RegistroDependencias.VerificarQueUsou(nomeItem, p0, ref fimConfiguracao);
        }

        [Given(@"Implementado em ""(.*)""")]
        public void GivenImplementadoEm(string p0)
        {
            VerificarFimConfiguracao(false);
            //se der erro não queremos dar o erro de falta de fim da confiugração
            RegistroDependencias.VerificarQueUsou(p0, nomeItem, ref fimConfiguracao);
        }

        private bool fimConfiguracao = false;
        [Given(@"Fim da configuração")]
        public void GivenFimDaConfiguracao()
        {
            ///todo: verificar se cruzaram todos os lugares onde este item foi usado
            fimConfiguracao = true;
        }

        [AfterScenario]
        public void AfterScenario() => VerificarFimConfiguracao(true);

        private void VerificarFimConfiguracao(bool exigir)
        {
            if (!fimConfiguracao && exigir)
                Assert.Equal("", "Fim da configuração deve ser o último item do cenário");
            if (fimConfiguracao && !exigir)
                Assert.Equal("", "Fim da configuração deve ser o último item do cenário");
        }
    }
}
