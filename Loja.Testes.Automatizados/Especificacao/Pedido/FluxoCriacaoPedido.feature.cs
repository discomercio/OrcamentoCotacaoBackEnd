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
namespace Loja.Testes.Automatizados.Especificacao.Pedido
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class FluxoDaCriacaoDoPedidoFeature : object, Xunit.IClassFixture<FluxoDaCriacaoDoPedidoFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "FluxoCriacaoPedido.feature"
#line hidden
        
        public FluxoDaCriacaoDoPedidoFeature(FluxoDaCriacaoDoPedidoFeature.FixtureData fixtureData, Loja_Testes_Automatizados_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Fluxo da criação do pedido", @"---
Fluxo no ERP/loja:
1 - Escolher cliente já cadastrado
2 - Confirmar (ou editar) dados cadastrais e informar endereço de entrega
3 - Escolher produtos, quantidades
4 - Escolher indicador e RA
5 - Alterar valores e forma de pagamento e observações (entrega imediata, instalador instala, etc) 
6 - Salvar o pedido
--- 
Fluxo no módulo loja:
1 - Escolher cliente já cadastrado
2 - Confirmar (ou editar) dados cadastrais e informar endereço de entrega
3 - Escolher indicador e RA e Modo de Seleção do CD 
4 - Escolher produtos, quantidades e alterar valores e forma de pagamento
5 - Informar observações (entrega imediata, instalador instala, etc) 
6 - Salvar o pedido
--- 
Fluxo na API:
Salvar o pedido
	Enviar todos os dados para cadastrar o pedido", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        
        [Xunit.SkippableFactAttribute(DisplayName="Cadastrar Pedido com o mínimo de informação possível")]
        [Xunit.TraitAttribute("FeatureTitle", "Fluxo da criação do pedido")]
        [Xunit.TraitAttribute("Description", "Cadastrar Pedido com o mínimo de informação possível")]
        public virtual void CadastrarPedidoComOMinimoDeInformacaoPossivel()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Cadastrar Pedido com o mínimo de informação possível", null, tagsOfScenario, argumentsOfScenario);
#line 23
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
#line 24
 testRunner.Given("Existe \"login\" = \"usuario_sistema\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 25
 testRunner.And("Existe \"loja\" = \"202\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
