using System;
using System.Collections.Generic;
using System.Text;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus
{
    public class Autenticacao: Comuns.Api.Autenticacao.IAutenticacaoSteps
    {
        //todo: fazer testes do AlterarMagentoPedidoStatus
        private readonly AlterarMagentoPedidoStatusChamada cadastrarPedido = new AlterarMagentoPedidoStatusChamada();

        public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            cadastrarPedido.WhenInformo(p0, p1);
        }

        public void ThenErroStatusCode(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ErroStatusCode(p0, this);
            cadastrarPedido.ThenErroStatusCode(p0);
        }

        public void GivenDadoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            cadastrarPedido.GivenDadoBase();
        }
    }
}
