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
            Testes.Utils.LogTestes.LogOperacoes.DadoBase(this.GetType());
            cadastrarPedido.GivenDadoBase();
        }

        [Given(@"Informo ""(.*)"" = ""(.*)""")]
        public void GivenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes.Informo(p0,p1,this.GetType());
            cadastrarPedido.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.Erro(p0,this.GetType());
            cadastrarPedido.ThenErro(p0);
        }

        [Then(@"Sem erro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.SemErro(p0,this.GetType());
            cadastrarPedido.ThenSemErro(p0);
        }

        [Then(@"Sem nenhum erro")]
        public void ThenSemNenhumErro()
        {
            Testes.Utils.LogTestes.LogOperacoes.SemNenhumErro(this.GetType());
            cadastrarPedido.ThenSemNenhumErro();
        }


    }
}
