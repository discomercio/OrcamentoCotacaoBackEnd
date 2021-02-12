using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using System;
using TechTalk.SpecFlow;

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

        [Then(@"Sem erro ""(.*)""")]
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


        [Given(@"No ambiente ""(.*)"" erro ""(.*)"" é ""(.*)""")]
        [Given(@"No ambiente ""(.*)"" mapear erro ""(.*)"" para ""(.*)""")]
        public void GivenNoAmbienteErroE(string p0, string p1, string p2)
        {
            
        }
    }
}
