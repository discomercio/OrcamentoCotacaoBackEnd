using System;
using TechTalk.SpecFlow;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao
{
    [Binding, Scope(Tag = "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao.CpfCnpj")]
    public class CpfCnpjSteps
    {
        private readonly CadastrarPrepedido cadastrarPrepedido = new CadastrarPrepedido();
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();

        [When(@"Prepedido base")]
        public void WhenPrepedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes.DadoBase(this.GetType());
            cadastrarPrepedido.GivenPrepedidoBase();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes.Informo(p0, p1, this.GetType());
            cadastrarPrepedido.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.Erro(p0, this.GetType());
            cadastrarPrepedido.ThenErro(p0);
        }

        [Then(@"Sem erro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.SemErro(p0, this.GetType());
            cadastrarPrepedido.ThenSemErro(p0);
        }
    }
}
