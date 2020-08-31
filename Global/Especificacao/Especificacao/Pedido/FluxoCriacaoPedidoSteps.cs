using System;
using TechTalk.SpecFlow;

namespace Especificacao.Especificacao.Pedido
{
    [Binding, Scope(Tag = "@Especificacao.Pedido.FluxoCriacaoPedido")]
    public class FluxoCriacaoPedidoSteps
    {
        [When(@"Tudo certo, só para aparecer na lista")]
        public void WhenTudoCertoSoParaAparecerNaLista()
        {
        }
    }
}
