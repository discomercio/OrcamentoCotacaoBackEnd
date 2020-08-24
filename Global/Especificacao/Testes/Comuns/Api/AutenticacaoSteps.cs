using Especificacao.Testes.Utils.ExecucaoCruzada;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Comuns.Api
{
    [Binding, Scope(Tag = "Especificacao.Comuns.Api.Autenticacao")]
    public class AutenticacaoSteps : ListaAmbientes<IAutenticacaoSteps>, IAutenticacaoSteps
    {
        [Given(@"Implementado em ""(.*)""")]
        public void GivenImplementadoEm(string ambiente)
        {
            var implementacao = ambiente switch
            {
                "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido" =>
                    new Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Autenticacao("Especificacao.Comuns.Api.Autenticacao"),
                _ => throw new ArgumentException($"Ambiente desconhecido: {ambiente}"),
            };
            base.GivenImplementadoEm(ambiente, implementacao);
        }

        [Given(@"Dado base")]
        public void GivenDadoBase()
        {
            base.ExecutarTodos(i => i.GivenDadoBase());
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            base.ExecutarTodos(i => i.WhenInformo(p0, p1));
        }

        [Then(@"Erro status code ""(.*)""")]
        public void ThenErroStatusCode(int p0)
        {
            base.ExecutarTodos(i => i.ThenErroStatusCode(p0));
        }
    }
}
