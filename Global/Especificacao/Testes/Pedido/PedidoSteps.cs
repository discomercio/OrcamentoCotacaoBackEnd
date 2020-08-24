using Especificacao.Testes.Utils.ExecucaoCruzada;
using System;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Pedido
{
    [Binding, Scope(Tag = "@Especificacao.Pedido")]
    public class PedidoSteps : ListaAmbientes<IPedidoSteps>, IPedidoSteps
    {
        [Given(@"Implementado em ""(.*)""")]
        public void GivenImplementadoEm(string ambiente)
        {
            var implementacao = ambiente switch
            {
                "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido" =>
                    new Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Pedido("Especificacao.Pedido", "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido"),
                _ => throw new ArgumentException($"Ambiente desconhecido: {ambiente}"),
            };
            base.GivenImplementadoEm(ambiente, implementacao);
        }

        [When(@"Pedido base")]
        public void WhenPedidoBase()
        {
            base.ExecutarTodos(i => i.WhenPedidoBase());
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            base.ExecutarTodos(i => i.WhenInformo(p0, p1));
        }

        [Then(@"No ambiente ""(.*)"" erro ""(.*)""")]
        public void ThenNoAmbienteErro(string ambiente, string erro)
        {
            base.ExecutarTodos(i => i.ThenNoAmbienteErro(ambiente, erro));
        }

        [Then(@"No ambiente ""(.*)"" sem erro ""(.*)""")]
        public void ThenNoAmbienteSemErro(string ambiente, string erro)
        {
            base.ExecutarTodos(i => i.ThenNoAmbienteSemErro(ambiente, erro));
        }
    }
}
