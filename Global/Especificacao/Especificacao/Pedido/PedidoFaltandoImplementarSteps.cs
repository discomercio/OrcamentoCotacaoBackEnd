using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using System;
using TechTalk.SpecFlow;

#pragma warning disable IDE0060 // Remove unued parameter
namespace Especificacao.Especificacao.Pedido
{
    //todo: precisa fazer os testes que usem esta classe
    [Binding, Scope(Tag = "@Especificacao.Pedido.PedidoFaltandoImplementarSteps")]
    public class PedidoFaltandoImplementarSteps
    {
        public PedidoFaltandoImplementarSteps()
        {
        }
        [Given(@"fazer esta validação, no pedido e prepedido")]
        public void GivenFazerEstaValidacaoNoPedidoEPrepedido()
        {

        }
        [Given(@"Pedido base")]
        public void GivenPedidoBase()
        {

        }
        [Given(@"Pedido base com endereco de entrega")]
        public void GivenPedidoBaseComEnderecoDeEntrega()
        {

        }

        [Given(@"Pedido base cliente PF")]
        [When(@"Pedido base cliente PF")]
        public void GivenPedidoBaseClientePF()
        {
            
        }

        [Given(@"Pedido base cliente PJ")]
        [When(@"Pedido base cliente PJ")]
        public void GivenPedidoBaseClientePJ()
        {
            
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void WhenInformo(string p0, string p1)
        {

        }

        [Then(@"Sem nehum erro")]
        public void ThenSemNehumErro()
        {

        }

        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {

        }

        [Given(@"Pedido base cliente PJ com endereço de entrega PF")]
        public void GivenPedidoBaseClientePJComEnderecoDeEntregaPF()
        {

        }

        [Then(@"Sem nenhum erro")]
        public void ThenSemNenhumErro()
        {

        }

        [Given(@"Pedido base cliente PJ com endereço de entrega PJ")]
        public void GivenPedidoBaseClientePJComEnderecoDeEntregaPJ()
        {

        }

        [Given(@"Ignorar cenário no ambiente ""(.*)""")]
        public void GivenIgnorarCenarioNoAmbiente(string p0)
        {

        }

        [Given(@"Reiniciar appsettings")]
        public void GivenReiniciarAppsettings()
        {
            
        }

        [Given(@"Reiniciar banco ao terminar cenário")]
        public void GivenReiniciarBancoAoTerminarCenario()
        {

        }

        [Given(@"Limpar tabela ""(.*)""")]
        public void GivenLimparTabela(string tabela)
        {
            
        }

        [Given(@"Novo registro na tabela ""(.*)""")]
        public void GivenNovoRegistroNaTabela(string p0)
        {
            
        }

        [Given(@"Novo registro em ""(.*)"", campo ""(.*)"" = ""(.*)""")]
        public void GivenNovoRegistroEmCampo(string tabela, string p0, string p1)
        {
            
        }

        [Given(@"Gravar registro em ""(.*)""")]
        public void GivenGravarRegistroEm(string p0)
        {
            
        }

        [Given(@"Reiniciar permissões do usuário quando terminar o cenário")]
        public void GivenReiniciarPermissoesDoUsuarioQuandoTerminarOCenario()
        {

        }

        [Given(@"Usuário com permissão ""(.*)""")]
        public void GivenUsuarioComPermissao(string p0)
        {
            
        }

        [Given(@"Usuário sem permissão ""(.*)""")]
        public void GivenUsuarioSemPermissao(string p0)
        {
            
        }

        [When(@"Lista de itens ""(.*)"" informo ""(.*)"" = ""(.*)""")]
        public void WhenListaDeItensInformo(int indice, string campo, string valor)
        {
            
        }

        [Then(@"Sem [Ee]rro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            
        }

        [When(@"Lista de itens com ""(.*)"" itens")]
        public void WhenListaDeItensComItens(int numeroItens)
        {
            
        }

        [Given(@"Cadastro do cliente ""(.*)"" = ""(.*)""")]
        public void GivenCadastroDoCliente(string p0, string p1)
        {

        }

        [Then(@"Tabela ""t_PEDIDO"" registro pai criado, verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDORegistroPaiCriadoVerificarCampo(string campo, string valor)
        {
            
        }

        [Given(@"Definir appsettings limite pedido igual = ""(.*)""")]
        public void GivenDefinirAppsettingsLimitePedidoIgual(int p0)
        {
            
        }

        [Given(@"Reinciar appsettings no final da feature")]
        public void GivenReinciarAppsettingsNoFinalDaFeature()
        {
            
        }
        [Given(@"Definir appsettings limite pedido por cpf = ""(.*)""")]
        public void GivenDefinirAppsettingsLimitePedidoPorCpf(int p0)
        {
            
        }

        [Given(@"Existe ""(.*)"" = ""(.*)""")]
        public void GivenExiste(string p0, string p1)
        {
            
        }

        [Given(@"Existe cliente ""(.*)"" = ""(.*)"" como PF")]
        public void GivenExisteClienteComoPF(string p0, int p1)
        {
            
        }

        [Given(@"Existe produto ""(.*)"" = ""(.*)"", ""(.*)"" = ""(.*)"", ""(.*)"" = ""(.*)""")]
        public void GivenExisteProduto(string p0, int p1, string p2, int p3, string p4, int p5)
        {
            
        }

        [When(@"Fiz login como ""(.*)"" e escolhi a loja ""(.*)""")]
        public void WhenFizLoginComoEEscolhiALoja(string p0, int p1)
        {
            
        }

        [When(@"Pedido vazio")]
        public void WhenPedidoVazio()
        {
            
        }

        [When(@"Fazer esta validação")]
        public void WhenFazerEstaValidacao()
        {
        }

        [Given(@"No ambiente ""(.*)"" erro ""(.*)"" é ""(.*)""")]
        [Given(@"No ambiente ""(.*)"" mapear erro ""(.*)"" para ""(.*)""")]
        public void GivenNoAmbienteErroE(string p0, string p1, string p2)
        {
            
        }

        [Given(@"Usuário sem permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD")]
        public void GivenUsuarioSemPermissaoOP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD()
        {
            
        }

        [Given(@"Usuário com permissão OP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD")]
        public void GivenUsuarioComPermissaoOP_LJA_CADASTRA_NOVO_PEDIDO_SELECAO_MANUAL_CD()
        {

        }

        [Given(@"Alterar pedido colocar RA de (.*) real")]
        public void GivenAlterarPedidoColocarRADeReal(int p0)
        {
            
        }

        [When(@"Recalcular totais do pedido")]
        public void WhenRecalcularTotaisDoPedido()
        {
           
        }

        [When(@"Deixar forma de pagamento consistente")]
        public void WhenDeixarFormaDePagamentoConsistente()
        {
            
        }

        [Given(@"Pedido base COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA")]
        public void GivenPedidoBaseCOD_FORMA_PAGTO_PARCELADO_COM_ENTRADA()
        {
            
        }

        [Given(@"Reinciar banco ao terminar cenário")]
        public void GivenReinciarBancoAoTerminarCenario()
        {
            
        }

        [Given(@"Alterar registro em ""(.*)"", busca ""(.*)"" = ""(.*)"", campo ""(.*)"" = ""(.*)""")]
        public void GivenAlterarRegistroEmBuscaCampo(string p0, string p1, string p2, string p3, int p4)
        {
            
        }

        [Given(@"Modificar pedido para ""(.*)"" por cento de desconto")]
        public void GivenModificarPedidoParaPorCentoDeDesconto(int p0)
        {
            
        }

        [Given(@"Pedido base PJ")]
        public void GivenPedidoBasePJ()
        {
            
        }

        [Given(@"Pedido base PF")]
        public void GivenPedidoBasePF()
        {
            
        }

        [Given(@"Pedido base ""(.*)"" com forma pagamento ""(.*)"" com percentual de desconto ""(.*)"" de primeira prestação")]
        public void GivenPedidoBaseComFormaPagamentoComPercentualDeDescontoDePrimeiraPrestacao(string p0, string p1, int p2)
        {
            
        }

        [Given(@"Pedido base ""(.*)"" com forma pagamento ""(.*)""")]
        public void GivenPedidoBaseComFormaPagamento(string p0, string p1)
        {
            
        }

        [Given(@"Pedido base ""(.*)"" com forma pagamento ""(.*)"" com percentual de desconto ""(.*)"" de entrada")]
        public void GivenPedidoBaseComFormaPagamentoComPercentualDeDescontoDeEntrada(string p0, string p1, int p2)
        {
            
        }

       

    }
}
#pragma warning restore IDE0060 // Remove unued parameter
