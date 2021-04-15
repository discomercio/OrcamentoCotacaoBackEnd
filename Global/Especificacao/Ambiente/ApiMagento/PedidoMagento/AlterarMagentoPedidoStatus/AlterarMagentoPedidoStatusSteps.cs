using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus
{
    [Binding, Scope(Tag = "Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus")]
    public class AlterarMagentoPedidoStatusSteps
    {
        private readonly string tokenAcesso = Ambiente.ApiMagento.InjecaoDependencias.TokenAcessoApiMagento();
    }
}
