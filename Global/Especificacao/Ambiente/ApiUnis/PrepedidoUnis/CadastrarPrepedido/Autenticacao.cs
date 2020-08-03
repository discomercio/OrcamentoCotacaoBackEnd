using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    public class Autenticacao : Testes.Comuns.Api.IAutenticacaoSteps
    {
        private readonly CadastrarPrepedido cadastrarPrepedido = new CadastrarPrepedido();
        private static readonly List<string> especificacoes = new List<string>();

        public Autenticacao(string especificacao)
        {
            especificacoes.Add(especificacao);
        }
        public static void ThenVerificarQueExecutou(string especificacao)
        {
            //este teste somente passa se executar todos os testes
            Assert.Contains(especificacao, especificacoes);
        }

        public void WhenInformo(string p0, string p1)
        {
            cadastrarPrepedido.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            cadastrarPrepedido.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            cadastrarPrepedido.GivenPrepedidoBase();
        }
    }
}
