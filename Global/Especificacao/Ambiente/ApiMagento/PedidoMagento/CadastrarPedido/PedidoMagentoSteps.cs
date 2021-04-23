using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
{
    /*
todo: fluxo do cancelamento
todo: ver os pendentes K:\desenvolvimento\Ar Clube\reunioes\210322 calculo valores api magento\pendentes
todo: executar os cripts de banco
*/

    [Binding, Scope(Tag = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido")]
    public sealed class PedidoMagentoSteps
    {
        private readonly CadastrarPedido cadastrarPedido = new CadastrarPedido();
        private readonly Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();
        private readonly Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida.SplitEstoqueRotinas SplitEstoqueRotinas = new Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida.SplitEstoqueRotinas();

        public PedidoMagentoSteps(ScenarioContext scenarioContext)
        {
            
        }
        [Given(@"Esta é a especificação, está sendo testado em outros \.feature")]
        public void GivenEstaEAEspecificacaoEstaSendoTestadoEmOutros_Feature()
        {
            //nao fazemos nada mesmo
        }

        [Given(@"Pedido base")]
        public void GivenPedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            cadastrarPedido.GivenDadoBase();
        }

        [Given(@"Pedido base PF com endereço de entrega")]
        public void GivenPedidoBasePFComEnderecoDeEntrega()
        {
            //o pedido base sempre é PF com endereço de entrega
            GivenPedidoBase();
        }

        [Given(@"Pedido base cliente PJ")]
        [When(@"Pedido base cliente PJ")]
        public void GivenPedidoBaseClientePJ()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            cadastrarPedido.GivenPedidoBaseClientePJ();
        }


        [Given(@"Informo ""(.*)"" = ""(.*)""")]
        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void GivenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            cadastrarPedido.WhenInformo(p0, p1);
        }

        [When(@"Lista de itens ""(.*)"" informo ""(.*)"" = ""(.*)""")]
        public void WhenListaDeItensInformo(int indice, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(indice, campo, valor, this);
            cadastrarPedido.MagentoListaDeItensInformo(indice, campo, valor);
        }

        [When(@"Lista de itens com ""(.*)"" itens")]
        public void WhenListaDeItensComItens(int numeroItens)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(numeroItens, this);
            cadastrarPedido.MagentoListaDeItensComXitens(numeroItens);
        }


        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            cadastrarPedido.ThenErro(p0);
        }

        [Then(@"Sem erro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            cadastrarPedido.ThenSemErro(p0);
        }

        [Then(@"Sem nenhum erro")]
        public void ThenSemNenhumErro()
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            cadastrarPedido.ThenSemNenhumErro();
        }

        [Then(@"Tabela ""t_PEDIDO"" registro criado, verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDORegistroCriadoVerificarCampo(string campo, string valor)
        {
            var dto = cadastrarPedido.UltimoPedidoResultadoMagentoDto();
            if (dto == null)
                throw new Exception("erro");
            Assert.Empty(dto.ListaErros);

            List<string> somentePai = new List<string>()
                { dto.IdPedidoCadastrado??"" };

            gerenciamentoBanco.TabelaT_PEDIDORegistroVerificarCampo(somentePai, campo, valor);
        }

        [Then(@"Tabela ""t_PEDIDO_ITEM"" registro criado, verificar item ""(.*)"" campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(int item, string campo, string valor)
        {
            var dto = cadastrarPedido.UltimoPedidoResultadoMagentoDto();
            if (dto == null)
                throw new Exception("erro");
            Assert.Empty(dto.ListaErros);

            string pedido = dto.IdPedidoCadastrado ?? "";

            gerenciamentoBanco.TabelaT_PEDIDO_ITEMRegistroVerificarCampo(item, pedido, campo, valor);
        }


        [Given(@"Reiniciar appsettings")]
        public void GivenReiniciarAppsettings()
        {
            //so pra lembrar que a gente faz isso
            AfterScenario();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var configuracaoApiMagento = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento>();
            Ambiente.ApiMagento.InjecaoDependencias.InicializarConfiguracaoApiMagento(configuracaoApiMagento);
        }

        [Given(@"Zerar todo o estoque")]
        public void GivenZerarTodoOEstoque()
        {
            SplitEstoqueRotinas.ZerarTodoOEstoque();
        }

        [When(@"Recalcular totais do pedido")]
        public void WhenRecalcularTotaisDoPedido()
        {
            Testes.Utils.LogTestes.LogOperacoes2.RecalcularTotaisDoPedido(this);
            cadastrarPedido.RecalcularTotaisDoPedido();
        }

        [When(@"Deixar forma de pagamento consistente")]
        public void WhenDeixarFormaDePagamentoConsistente()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DeixarFormaDePagamentoConsistente(this);
            cadastrarPedido.DeixarFormaDePagamentoConsistente();
        }

    }
}
