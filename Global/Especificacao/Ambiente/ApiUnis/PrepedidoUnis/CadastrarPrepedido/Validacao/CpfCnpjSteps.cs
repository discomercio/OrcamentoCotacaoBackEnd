using System;
using TechTalk.SpecFlow;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao
{
    [Binding, Scope(Tag = "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao.CpfCnpj")]
    public class CpfCnpjSteps
    {
        private readonly CadastrarPrepedido cadastrarPrepedido = new CadastrarPrepedido();
        private readonly Testes.Utils.LogTestes logTestes = Testes.Utils.LogTestes.GetInstance();

        [When(@"Prepedido base")]
        public void WhenPrepedidoBase()
        {
            logTestes.LogMensagem("Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao.CpfCnpj WhenPrepedidoBase");
            cadastrarPrepedido.GivenPrepedidoBase();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            logTestes.LogMensagem($"Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao.CpfCnpj WhenInformo({p0}, {p1})");
            cadastrarPrepedido.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            logTestes.LogMensagem("Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao.CpfCnpj");
            cadastrarPrepedido.ThenErro(p0);
        }
        
        [Then(@"Sem erro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            logTestes.LogMensagem("Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Validacao.CpfCnpj");
            cadastrarPrepedido.ThenSemErro(p0);
        }
    }
}
