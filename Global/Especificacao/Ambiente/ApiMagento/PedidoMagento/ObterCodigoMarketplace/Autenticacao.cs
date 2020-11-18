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
            Testes.Utils.LogTestes.LogOperacoes.Informo(p0, p1, this.GetType());
            obterCodigoMarketplace.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.ErroStatusCode(p0, this.GetType());
            obterCodigoMarketplace.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes.DadoBase(this.GetType());
            obterCodigoMarketplace.GivenDadoBase();
        }
    }
}
