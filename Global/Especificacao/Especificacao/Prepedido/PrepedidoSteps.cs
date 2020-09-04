using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using System;
using TechTalk.SpecFlow;

namespace Especificacao.Especificacao.Prepedido
{
    [Binding, Scope(Tag = "@Especificacao.Prepedido.Prepedido")]
    public class PrepedidoSteps : PedidoPassosComuns
    {
        public PrepedidoSteps()
        {
            base.AdicionarImplementacao(new Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido());
            RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedidoListaDependencias", 
                "Especificacao.Prepedido.Prepedido.PrepedidoListaDependencias");
        }
        [Given(@"O prepedido não tem nenhum passo individual, somente passos incluídos pela PedidoListaDependencias")]
        public void GivenOPrepedidoNaoTemNenhumPassoIndividualSomentePassosIncluidosPelaPedidoListaDependencias()
        {
        }
    }
}
