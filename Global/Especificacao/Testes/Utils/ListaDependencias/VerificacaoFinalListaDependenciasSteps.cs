using System;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Utils.ListaDependencias
{
    [Binding, Scope(Tag = "Especificacao.Testes.Utils.ListaDependencias")]
    public class VerificacaoFinalListaDependenciasSteps
    {
        [Given(@"VerificacaoFinalListaDependencias")]
        public void GivenVerificacaoFinalListaDependencias()
        {
            LogTestes.LogOperacoes2.VerificacaoFinalListaDependencias(this);
            RegistroDependencias.TodosVerificados();
        }
    }
}
