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
            {
                var imp = new Especificacao.Prepedido.PrepedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Prepedido.Prepedido.PrepedidoListaDependencias", imp,
                    "Especificacao.Pedido.Pedido.PedidoListaDependencias");
            }

            {
                var imp = new Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedido();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", imp,
                    "Especificacao.Pedido.Pedido.PedidoListaDependencias");
            }
            {
                var imp = new Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedidoListaDependencias", imp,
                    "Especificacao.Pedido.Pedido.PedidoListaDependencias");
            }
        }
        [Given(@"O pedido não tem nenhum passo individual, somente passos incluídos pela PedidoListaDependencias")]
        public void GivenOPedidoNaoTemNenhumPassoIndividualSomentePassosIncluidosPelaPedidoListaDependencias()
        {
            //nao fazemos nada, esta classe existe só para dizer as interfaces que usamos
        }

    }
}
