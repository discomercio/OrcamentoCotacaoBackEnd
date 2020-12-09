using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.BuscarStatusPrepedido
{
    public class Autenticacao : Comuns.Api.Autenticacao.IAutenticacaoSteps
    {
        private readonly BuscarStatusPrepedidoImplementacao buscarStatusPrepedidoImplementacao = new BuscarStatusPrepedidoImplementacao();

        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            buscarStatusPrepedidoImplementacao.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(p0, this);
            buscarStatusPrepedidoImplementacao.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            buscarStatusPrepedidoImplementacao.GivenDadoBase();
        }
    }
}
