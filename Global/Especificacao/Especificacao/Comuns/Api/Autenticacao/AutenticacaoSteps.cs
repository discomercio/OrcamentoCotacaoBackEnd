using Especificacao.Testes.Utils.ListaDependencias;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Especificacao.Comuns.Api.Autenticacao
{
    [Binding, Scope(Tag = "Especificacao.Comuns.Api.Autenticacao.Autenticacao")]
    public class AutenticacaoSteps : ListaImplementacoes<IAutenticacaoSteps>, IAutenticacaoSteps
    {
        public AutenticacaoSteps()
        {
            {
                var imp = new Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Autenticacao();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedidoListaDependencias", imp,
                    "Especificacao.Comuns.Api.Autenticacao");
            }

            {
                var imp = new Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.Autenticacao();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.BuscarQtdeParcCartaoVisaListaDependencias", imp,
                    "Especificacao.Comuns.Api.Autenticacao");
            }

            {
                var imp = new Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.Autenticacao();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplaceListaDependencias", imp,
                    "Especificacao.Comuns.Api.Autenticacao");
            }

            {
                var imp = new Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.Autenticacao();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CadastrarPedidoListaDependencias", imp,
                    "Especificacao.Comuns.Api.Autenticacao");
            }
            {
                var imp = new Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido.Autenticacao();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido.BuscarStatusPrepedidoListaDependencias", imp,
                    "Especificacao.Comuns.Api.Autenticacao");
            }
        }

        [Given(@"Dado base")]
        public void GivenDadoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            base.Executar(i => i.GivenDadoBase());
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            base.Executar(i => i.WhenInformo(p0, p1));
        }

        [Then(@"Erro status code ""(.*)""")]
        public void ThenErroStatusCode(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(p0, this);
            base.Executar(i => i.ThenErroStatusCode(p0));
        }
    }
}
