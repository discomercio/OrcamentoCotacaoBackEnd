using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using System;
using TechTalk.SpecFlow;

namespace Especificacao.Especificacao.Pedido
{
    //todo: precisa fazer os testes que usem esta classe
    [Binding, Scope(Tag = "@Especificacao.Pedido.PedidoFaltandoImplementarSteps")]
    public class PedidoFaltandoImplementarSteps
    {
        public PedidoFaltandoImplementarSteps()
        {
        }
        [Given(@"fazer esta validação, no pedido e prepedido")]
        public void GivenFazerEstaValidacaoNoPedidoEPrepedido()
        {
            
        }
        [Given(@"Pedido base com endereco de entrega")]
        public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            
        }

        [Then(@"Sem nehum erro")]
        public void ThenSemNehumErro()
        {
            
        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            
        }

        [Given(@"Pedido base cliente PJ com endereço de entrega PF")]
        public void GivenPedidoBaseClientePJComEnderecoDeEntregaPF()
        {
            
        }

        [Then(@"Sem nenhum erro")]
        public void ThenSemNenhumErro()
        {
            
        }

        [Given(@"Pedido base cliente PJ com endereço de entrega PJ")]
        public void GivenPedidoBaseClientePJComEnderecoDeEntregaPJ()
        {
            
        }
    }
}
