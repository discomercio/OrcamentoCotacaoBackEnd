using System;
using TechTalk.SpecFlow;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    [Binding /*, Scope(Tag = "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido") */]
    public class CadastrarPrepedidoVerificarQueExecutouSteps
    {
        private readonly FeatureContext featureContext;
        private readonly ScenarioContext scenarioContext;

        public CadastrarPrepedidoVerificarQueExecutouSteps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;
        }

        [Then(@"Verificar que executou ""(.*)""")]
        public void ThenVerificarQueExecutou(string especificacao)
        {
            switch (especificacao)
            {
                case "Especificacao.Comuns.Api.Autenticacao":
                    Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Autenticacao.ThenVerificarQueExecutou(especificacao);
                    break;
                case "Especificacao.Pedido":
                    Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Pedido.ThenVerificarQueExecutou(especificacao);
                    break;
                default:
                    throw new ArgumentException($"especificacao {especificacao} desconhecida");
            }
        }
    }
}
