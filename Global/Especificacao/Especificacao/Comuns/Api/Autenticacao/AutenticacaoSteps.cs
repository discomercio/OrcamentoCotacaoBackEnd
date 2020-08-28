﻿using Especificacao.Testes.Utils.ListaDependencias;
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
            base.AdicionarImplementacao(new Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Autenticacao());
            RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.ListaDependencias", "Especificacao.Comuns.Api.Autenticacao");

            base.AdicionarImplementacao(new Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.Autenticacao());
            RegistroDependencias.AdicionarDependencia("Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.ListaDependencias", "Especificacao.Comuns.Api.Autenticacao");
        }

        [Given(@"Dado base")]
        public void GivenDadoBase()
        {
            base.Executar(i => i.GivenDadoBase());
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            base.Executar(i => i.WhenInformo(p0, p1));
        }

        [Then(@"Erro status code ""(.*)""")]
        public void ThenErroStatusCode(int p0)
        {
            base.Executar(i => i.ThenErroStatusCode(p0));
        }
    }
}