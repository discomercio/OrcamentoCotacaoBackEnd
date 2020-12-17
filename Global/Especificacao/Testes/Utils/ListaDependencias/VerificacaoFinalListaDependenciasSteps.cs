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

        [Given(@"ApagarMapaComChamadas\.txt")]
        public void GivenApagarMapaComChamadas_Txt()
        {
            LogTestes.LogOperacoes2.MensagemEspecial("GivenApagarMapaComChamadas_Txt",this);
            RegistroDependencias.ApagarMapaComChamadas_Txt();
        }

        [Given(@"SalvarMapaComChamadas\.txt")]
        public void GivenSalvarMapaComChamadas_Txt()
        {
            LogTestes.LogOperacoes2.MensagemEspecial("GivenSalvarMapaComChamadas_Txt", this);
            RegistroDependencias.SalvarMapaComChamadas_Txt();
        }

    }
}
