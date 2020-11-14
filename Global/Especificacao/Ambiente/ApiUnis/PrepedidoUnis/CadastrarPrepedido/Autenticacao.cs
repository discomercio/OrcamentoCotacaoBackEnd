using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    public class Autenticacao : Comuns.Api.Autenticacao.IAutenticacaoSteps
    {
        private readonly CadastrarPrepedido cadastrarPrepedido = new CadastrarPrepedido();
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();

        public void WhenInformo(string p0, string p1)
        {
            cadastrarPrepedido.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            logTestes.LogMensagem("Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido Autenticacao");
            cadastrarPrepedido.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            cadastrarPrepedido.GivenPrepedidoBase();
        }
    }
}
