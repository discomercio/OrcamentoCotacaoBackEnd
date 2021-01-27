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
            {
                var imp = new Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedidoListaDependencias", imp,
                    "Especificacao.Prepedido.Prepedido.PrepedidoListaDependencias");
            }
            {
                var imp = new Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApiListaDependencias", imp,
                    "Especificacao.Prepedido.Prepedido.PrepedidoListaDependencias");
            }
        }
        [Given(@"O prepedido não tem nenhum passo individual, somente passos incluídos pela PedidoListaDependencias")]
        public void GivenOPrepedidoNaoTemNenhumPassoIndividualSomentePassosIncluidosPelaPedidoListaDependencias()
        {
        }
    }
}
