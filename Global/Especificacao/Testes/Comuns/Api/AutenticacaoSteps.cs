using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Especificacao.Testes.Comuns.Api
{
    [Binding, Scope(Tag = "Especificacao.Comuns.Api.Autenticacao")]
    public class AutenticacaoSteps : IAutenticacaoSteps
    {
        private List<IAutenticacaoSteps> implementacoes = new List<IAutenticacaoSteps>();
        private bool usado = false;

        [Given(@"Implementando em ""(.*)""")]
        public void GivenImplementandoEm(string ambiente)
        {
            //todos os Implementando precisam acontecer antes do resto
            if (usado)
                throw new ArgumentException($"Inicializando ambiente {ambiente} depois de algum passo");

            switch (ambiente)
            {
                case "Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido":
                    implementacoes.Add(new Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.Autenticacao("Especificacao.Comuns.Api.Autenticacao"));
                    break;
                default:
                    throw new ArgumentException($"Ambiente desconhecido: {ambiente}");
            }
        }

        //nao pode usar se não tiver um ambiente
        private void VerificarInicializado()
        {
            usado = true;
            if (implementacoes.Count == 0)
                throw new ArgumentException($"Sem nenhnum ambiente.");
        }

        [Given(@"Dado base")]
        public void GivenDadoBase()
        {
            VerificarInicializado();
            foreach (var i in implementacoes)
                i.GivenDadoBase();
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {
            VerificarInicializado();
            foreach (var i in implementacoes)
                i.WhenInformo(p0, p1);
        }

        [Then(@"Erro status code ""(.*)""")]
        public void ThenErroStatusCode(int p0)
        {
            VerificarInicializado();
            foreach (var i in implementacoes)
                i.ThenErroStatusCode(p0);
        }
    }
}
