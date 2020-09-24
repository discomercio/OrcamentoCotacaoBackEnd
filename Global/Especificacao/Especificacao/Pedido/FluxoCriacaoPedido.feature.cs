﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.3.0.0
//      SpecFlow Generator Version:3.1.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Especificacao.Especificacao.Pedido
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.FluxoCriacaoPedido")]
    public partial class FluxoDaCriacaoDoPedidoFeature : object, Xunit.IClassFixture<FluxoDaCriacaoDoPedidoFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.FluxoCriacaoPedido"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "FluxoCriacaoPedido.feature"
#line hidden
        
        public FluxoDaCriacaoDoPedidoFeature(FluxoDaCriacaoDoPedidoFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Fluxo da criação do pedido", "---\r\nFluxo no ERP/loja:\r\n1 - Escolher cliente já cadastrado (em \"loja/resumo.asp\"" +
                    ")\r\n\tenvia para ClientePesquisa.asp\r\n\tse existe somente um cliente, envia para cl" +
                    "ienteedita.asp com OP_CONSULTA\r\n2 - Confirmar (ou editar) dados cadastrais e inf" +
                    "ormar endereço de entrega (em \"loja/clienteedita.asp\")\r\n\tse editar dados cadastr" +
                    "ais, salva na t_cliente\r\n\tenvia para PedidoNovoProdCompostoMask.asp ou pedidonov" +
                    "o.asp\r\n3 - Escolher produtos, quantidades (em \"loja/PedidoNovoProdCompostoMask.a" +
                    "sp\")\r\n4 - Escolher indicador e RA e CD (somente se o indicador permitir RA) (em " +
                    "\"loja/PedidoNovo.asp\")\r\n5 - Alterar valores e forma de pagamento e observações (" +
                    "entrega imediata, instalador instala, etc) (em \"loja/PedidoNovoConsiste.asp\")\r\n\t" +
                    "envia para PedidoNovoConfirma.asp\r\n6 - Salvar o pedido (finaliza em \"loja/pedido" +
                    ".asp\")\r\n--- \r\nFluxo no módulo loja:\r\n1 - Passo10: Escolher cliente já cadastrado" +
                    "\r\n\tSe o cliente não existir, ele deve ser cadastrado primeiro. (arquivo CLiente/" +
                    "FLuxoCadastroCliente - criar esse arquivo)\r\n2 - Passo20: Confirmar (ou editar) d" +
                    "ados cadastrais e informar endereço de entrega\r\n\tse editar dados cadastrais, sal" +
                    "va na t_cliente\r\n2.5 - Passo 25: somente na API. Validar dados cadastrais. Não e" +
                    "xiste na tela porque sempre se usa o atual do cliente.\r\n3 - Passo30: Escolher in" +
                    "dicador e RA e Modo de Seleção do CD \r\n4 - Passo40: Escolher produtos, quantidad" +
                    "es e alterar valores e forma de pagamento\r\n5 - Passo50: Informar observações (en" +
                    "trega imediata, instalador instala, etc) \r\n6 - Passo60: Salvar o pedido\r\n\tInclui" +
                    "r:\r\n\t\t- validar preços\r\n\t\t- validar se o tipo de parcelamento é permitido para t" +
                    "odos os produtos\r\n--- \r\nFluxo na ApaiMagento:\r\n1 - Validar o pedido\r\n2 - se o cl" +
                    "iente não existir, cadastrar o cliente\r\n3 - salvar o pedido\r\n--- \r\nFluxo na API:" +
                    "\r\nSalvar o pedido\r\n\tEnviar todos os dados para cadastrar o pedido", ProgrammingLanguage.CSharp, new string[] {
                        "Especificacao.Pedido.FluxoCriacaoPedido"});
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Fluxo da criação do pedido")]
        [Xunit.TraitAttribute("FeatureTitle", "Fluxo da criação do pedido")]
        [Xunit.TraitAttribute("Description", "Fluxo da criação do pedido")]
        public virtual void FluxoDaCriacaoDoPedido()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fluxo da criação do pedido", null, tagsOfScenario, argumentsOfScenario);
#line 40
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 42
 testRunner.When("Tudo certo, só para aparecer na lista", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                FluxoDaCriacaoDoPedidoFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                FluxoDaCriacaoDoPedidoFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
