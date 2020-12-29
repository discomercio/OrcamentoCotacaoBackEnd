using Especificacao.Testes.Utils.InjecaoDependencia;
using Especificacao.Testes.Utils.ListaDependencias;
using InfraBanco;
using InfraBanco.Constantes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PrepedidoUnisBusiness.UnisDto.FormaPagtoUnisDto;
using PrepedidoUnisBusiness.UnisDto.PrepedidoUnisDto;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Testes.Utils.BancoTestes
{
    [Binding, Scope(Tag = "GerenciamentoBanco")]
    public class GerenciamentoBancoSteps
    {
        public GerenciamentoBancoSteps()
        {
        }

        [Given(@"Limpar tabela ""(.*)""")]
        public void GivenLimparTabela(string tabela)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.LimparTabela(tabela, this);
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(servicos.GetRequiredService<InfraBanco.ContextoBdProvider>(), servicos.GetRequiredService<InfraBanco.ContextoCepProvider>());
            bd.LimparTabela(tabela);
        }

        private bool reiniciarBancoAoTerminarCenario = false;
        [Given(@"Reiniciar banco ao terminar cenário")]
        public void GivenReiniciarBancoAoTerminarCenario()
        {
            reiniciarBancoAoTerminarCenario = true;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (!reiniciarBancoAoTerminarCenario)
                return;
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            //precisa restaurar o banco
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(servicos.GetRequiredService<InfraBanco.ContextoBdProvider>(), servicos.GetRequiredService<InfraBanco.ContextoCepProvider>());
            bd.InicializarForcado();
        }
    }
}

