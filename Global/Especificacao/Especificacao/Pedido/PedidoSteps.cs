using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using System;
using TechTalk.SpecFlow;

namespace Especificacao.Especificacao.Pedido
{
    [Binding, Scope(Tag = "@Especificacao.Pedido.Pedido")]
    public class PedidoSteps : PedidoPassosComuns
    {
        public PedidoSteps()
        {
            base.AdicionarImplementacao(new Especificacao.Prepedido.PrepedidoSteps());
            RegistroDependencias.AdicionarDependencia("Especificacao.Prepedido.Prepedido.PrepedidoListaDependencias",
                "Especificacao.Pedido.Pedido.PedidoListaDependencias");

            base.AdicionarImplementacao(new Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido());
            RegistroDependencias.AdicionarDependencia("Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias",
                "Especificacao.Pedido.Pedido.PedidoListaDependencias");
        }
        [Given(@"O pedido não tem nenhum passo individual, somente passos incluídos pela PedidoListaDependencias")]
        public void GivenOPedidoNaoTemNenhumPassoIndividualSomentePassosIncluidosPelaPedidoListaDependencias()
        {
            //nao fazemos nada, esta classe existe só para dizer as interfaces que usamos
        }

    }
}
