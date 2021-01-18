using Especificacao.Testes.Utils.ListaDependencias;
using Microsoft.Extensions.DependencyInjection;
using System;
using TechTalk.SpecFlow;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
{
    [Binding, Scope(Tag = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente")]
    public class CriacaoClienteSteps
    {
        private readonly CadastrarPedido cadastrarPedido = new CadastrarPedido();

        public CriacaoClienteSteps()
        {
        }

        [Given(@"Pedido base")]
        public void GivenPedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            cadastrarPedido.GivenDadoBase();
        }
        [Given(@"Pedido base cliente PJ")]
        [When(@"Pedido base cliente PJ")]
        public void GivenPedidoBaseClientePJ()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            cadastrarPedido.GivenPedidoBaseClientePJ();
        }

        [Given(@"Limpar endereço de entrega")]
        public void GivenLimparEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.LimparEnderecoDeEntrega(this);
            cadastrarPedido.LimparEnderecoDeEntrega();
        }

        [Given(@"Limpar dados cadastrais e endereço de entrega")]
        public void GivenLimparDadosCadastraisEEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.LimparDadosCadastraisEEnderecoDeEntrega(this);
            cadastrarPedido.LimparDadosCadastraisEEnderecoDeEntrega();
        }


        [Given(@"Informo ""(.*)"" = ""(.*)""")]
        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void GivenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            cadastrarPedido.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            cadastrarPedido.ThenErro(p0);
        }

        [Then(@"Sem erro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            cadastrarPedido.ThenSemErro(p0);
        }

        [Then(@"Sem nenhum erro")]
        public void ThenSemNenhumErro()
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            cadastrarPedido.ThenSemNenhumErro();
        }

    }
}
