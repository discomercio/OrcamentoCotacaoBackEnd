using Especificacao.Testes.Utils.ListaDependencias;
using Microsoft.Extensions.DependencyInjection;
using System;
using TechTalk.SpecFlow;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.ValidacaoCampos
{
    [Binding, Scope(Tag = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.ValidacaoCampos")]
    public class ValidacaoCampos
    {
        private readonly CadastrarPedido cadastrarPedido = new CadastrarPedido();

        public ValidacaoCampos()
        {
        }

        [Given(@"Pedido base")]
        public void GivenPedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            cadastrarPedido.GivenDadoBase();
        }

        [Given(@"Informo ""(.*)"" = ""(.*)""")]
        public void GivenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0,p1,this);
            cadastrarPedido.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0,this);
            cadastrarPedido.ThenErro(p0);
        }

        [Then(@"Sem erro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0,this);
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
