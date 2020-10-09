using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
{
    public class Autenticacao : Comuns.Api.Autenticacao.IAutenticacaoSteps
    {
        private readonly CadastrarPedido cadastrarPedido= new CadastrarPedido();
        private readonly Testes.Utils.LogTestes logTestes = Testes.Utils.LogTestes.GetInstance();

        public void WhenInformo(string p0, string p1)
        {
            cadastrarPedido.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            cadastrarPedido.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            cadastrarPedido.GivenDadoBase();
        }
    }
}
