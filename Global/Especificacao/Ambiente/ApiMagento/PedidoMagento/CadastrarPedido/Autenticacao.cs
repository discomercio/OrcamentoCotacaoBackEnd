using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
{
    public class Autenticacao : Comuns.Api.Autenticacao.IAutenticacaoSteps
    {
        private readonly CadastrarPedido cadastrarPedido = new CadastrarPedido();

        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes.Informo(p0, p1, this.GetType());
            cadastrarPedido.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes.ErroStatusCode(p0, this.GetType());
            cadastrarPedido.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes.DadoBase(this.GetType());
            cadastrarPedido.GivenDadoBase();
        }
    }
}
