using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace
{
    public class Autenticacao : Comuns.Api.Autenticacao.IAutenticacaoSteps
    {
        private readonly ObterCodigoMarketplace obterCodigoMarketplace= new ObterCodigoMarketplace();
        private readonly Testes.Utils.LogTestes logTestes = Testes.Utils.LogTestes.GetInstance();

        public void WhenInformo(string p0, string p1)
        {
            obterCodigoMarketplace.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            logTestes.LogMensagem("Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace Autenticacao");
            obterCodigoMarketplace.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            obterCodigoMarketplace.GivenDadoBase();
        }
    }
}
