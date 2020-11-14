using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa
{
    public class Autenticacao : Comuns.Api.Autenticacao.IAutenticacaoSteps
    {
        private readonly BuscarQtdeParcCartaoVisa buscarQtdeParcCartaoVisa = new BuscarQtdeParcCartaoVisa();
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();

        public void WhenInformo(string p0, string p1)
        {
            buscarQtdeParcCartaoVisa.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            logTestes.LogMensagem("Especificacao.Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa Autenticacao");
            buscarQtdeParcCartaoVisa.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            buscarQtdeParcCartaoVisa.GivenDadoBase();
        }
    }
}
