using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace
{
    public class Autenticacao : Comuns.Api.Autenticacao.IAutenticacaoSteps
    {
        private readonly CadastrarPedido obterCodigoMarketplace = new CadastrarPedido();
        private readonly Testes.Utils.LogTestes.LogTestes logTestes = Testes.Utils.LogTestes.LogTestes.GetInstance();

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
