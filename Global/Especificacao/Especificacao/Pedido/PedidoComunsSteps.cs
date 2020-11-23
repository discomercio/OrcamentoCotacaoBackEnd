using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using System;
using System.Linq;
using TechTalk.SpecFlow;

/*
 * esta classe serve para implementar muitos dos features.
 * Os steps são bem parecidos.
 * Ele usa a tag para saber quem faz a omplementação efetiva
 * */

namespace Especificacao.Especificacao.Pedido
{
    [Binding]
    [Scope(Tag = "Especificacao.Pedido.Passo10.CamposSimples")]
    [Scope(Tag = "Especificacao.Pedido.Passo20.EnderecoEntrega")]
    [Scope(Tag = "Especificacao.Pedido.FluxoCriacaoPedido")]
    public class PedidoComunsSteps : PedidoPassosComuns
    {
        public PedidoComunsSteps(FeatureContext featureContext)
        {
            var tags = featureContext.FeatureInfo.Tags.ToList();

            if (tags.Contains("Especificacao.Pedido.Passo10.CamposSimples"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo10.CamposSimplesPfListaDependencias");
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo10.CamposSimplesPjListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo20.EnderecoEntrega"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo20.EnderecoEntrega.EnderecoEntregaListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.FluxoCriacaoPedido"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                //este nao tem registro de dependencias
                //RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                //    "Especificacao.Pedido.FluxoCriacaoPedidoDependencias");
                return;
            }
        }

        [When(@"Pedido base cliente PF")]
        new public void WhenPedidoBaseClientePF()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePF(this);
            base.WhenPedidoBaseClientePF();
        }

        [When(@"Pedido base cliente PJ")]
        new public void WhenPedidoBaseClientePJ()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            base.WhenPedidoBaseClientePJ();
        }


        [Given(@"Pedido base")]
        [When(@"Pedido base")]
        new public void WhenPedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            base.WhenPedidoBase();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        new public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            base.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        new public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            base.ThenErro(p0);
        }

        [Then(@"Sem [Ee]rro ""(.*)""")]
        new public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            base.ThenSemErro(p0);
        }

        [Given(@"No ambiente ""(.*)"" erro ""(.*)"" é ""(.*)""")]
        public void GivenNoAmbienteErroE(string p0, string p1, string p2)
        {
            Testes.Utils.MapeamentoMensagens.GivenNoAmbienteErroE(p0, p1, p2);
        }

        [Given(@"Ignorar feature no ambiente ""(.*)""")]
        new public void GivenIgnorarFeatureNoAmbiente2(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.IgnorarFeatureNoAmbiente(p0, this);
            base.GivenIgnorarFeatureNoAmbiente2(p0);
        }

        [Given(@"Pedido base com endereço de entrega")]
        new public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            base.GivenPedidoBaseComEnderecoDeEntrega();
        }

        [Then(@"Sem nenhum erro")]
        new public void ThenSemNenhumErro()
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            base.ThenSemNenhumErro();
        }

    }
}
