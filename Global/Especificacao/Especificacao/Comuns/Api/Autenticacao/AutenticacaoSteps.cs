using Especificacao.Testes.Utils.ListaDependencias;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Especificacao.Comuns.Api.Autenticacao
{
    [Binding, Scope(Tag = "Especificacao.Comuns.Api.Autenticacao.Autenticacao")]
    public class AutenticacaoSteps : ListaImplementacoes<IAutenticacaoSteps>, IAutenticacaoSteps
    {
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();

        public AutenticacaoSteps()
        {
            base.AdicionarImplementacao(new Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Autenticacao());
            RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedidoListaDependencias",
                "Especificacao.Comuns.Api.Autenticacao");

            base.AdicionarImplementacao(new Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.Autenticacao());
            RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.BuscarQtdeParcCartaoVisaListaDependencias",
                "Especificacao.Comuns.Api.Autenticacao");

            base.AdicionarImplementacao(new Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.Autenticacao());
            RegistroDependencias.AdicionarDependencia("Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplaceListaDependencias",
                "Especificacao.Comuns.Api.Autenticacao");

            base.AdicionarImplementacao(new Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.Autenticacao());
            RegistroDependencias.AdicionarDependencia("Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias",
                "Especificacao.Comuns.Api.Autenticacao");
        }

        [Given(@"Dado base")]
        public void GivenDadoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes.DadoBase(this.GetType());
            base.Executar(i => i.GivenDadoBase());
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes.Informo(p0, p1, this.GetType());
            base.Executar(i => i.WhenInformo(p0, p1));
        }

        [Then(@"Erro status code ""(.*)""")]
        public void ThenErroStatusCode(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.ErroStatusCode(p0, this.GetType());
            base.Executar(i => i.ThenErroStatusCode(p0));
        }
    }
}
