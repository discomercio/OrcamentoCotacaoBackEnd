﻿using Especificacao.Testes.Pedido;
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
    [Scope(Tag = "@Especificacao.Pedido.Passo10.Permissoes")]
    public class PedidoComunsSteps : PedidoPassosComuns
    {
        public PedidoComunsSteps(FeatureContext featureContext)
        {
            var tags = featureContext.FeatureInfo.Tags.ToList();

            if (tags.Contains("Especificacao.Pedido.Passo10.CamposSimples"))
            {
                base.AdicionarImplementacao(new Especificacao.Pedido.PedidoSteps());
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias",
                    "Especificacao.Pedido.Passo10.CamposSimplesPfListaDependencias");
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias",
                    "Especificacao.Pedido.Passo10.CamposSimplesPjListaDependencias");
            }
            if (tags.Contains("Especificacao.Pedido.Passo20.EnderecoEntrega"))
            {
                base.AdicionarImplementacao(new Especificacao.Pedido.PedidoSteps());
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias",
                    "Especificacao.Pedido.Passo20.EnderecoEntrega.EnderecoEntregaListaDependencias");
            }
        }

        [Given(@"Pedido base")]
        [When(@"Pedido base")]
        new public void WhenPedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes.DadoBase(this.GetType());
            base.WhenPedidoBase();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        new public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes.Informo(p0, p1, this.GetType());
            base.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        new public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.Erro(p0, this.GetType());
            base.ThenErro(p0);
        }

        [Then(@"Sem [Ee]rro ""(.*)""")]
        new public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.SemErro(p0, this.GetType());
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
            Testes.Utils.LogTestes.LogOperacoes.IgnorarFeatureNoAmbiente(p0, this.GetType());
            base.GivenIgnorarFeatureNoAmbiente2(p0);
        }

        [Given(@"Pedido base com endereço de entrega")]
        new public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes.DadoBaseComEnderecoDeEntrega(this.GetType());
            base.GivenPedidoBaseComEnderecoDeEntrega();
        }

        [Then(@"Sem nenhum erro")]
        new public void ThenSemNenhumErro()
        {
            Testes.Utils.LogTestes.LogOperacoes.SemNenhumErro(this.GetType());
            base.ThenSemNenhumErro();
        }

    }
}