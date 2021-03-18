using System;
using TechTalk.SpecFlow;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao
{
    [Binding, Scope(Tag = "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao.CpfCnpj")]
    public class CpfCnpjSteps
    {
        private readonly CadastrarPrepedido cadastrarPrepedido = new CadastrarPrepedido();

        [When(@"Prepedido base")]
        public void WhenPrepedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            cadastrarPrepedido.GivenPrepedidoBase();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            cadastrarPrepedido.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            cadastrarPrepedido.ThenErro(p0);
        }

        [Then(@"Sem erro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            cadastrarPrepedido.ThenSemErro(p0);
        }
    }
}
