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
namespace Especificacao.Testes.Utils.ListaDependencias
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Especificacao.Testes.Utils.ListaDependencias")]
    public partial class VerificacaoFinalListaDependenciasFeature : object, Xunit.IClassFixture<VerificacaoFinalListaDependenciasFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Testes.Utils.ListaDependencias"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "VerificacaoFinalListaDependencias.feature"
#line hidden
        
        public VerificacaoFinalListaDependenciasFeature(VerificacaoFinalListaDependenciasFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "VerificacaoFinalListaDependencias", @"Temos um sistema de ListaDependencias: um arquivo .feature pode ser ""repetido"" por outros.

Por exemplo, a autenticação da API: está esepcificada em 
Especificacao\Comuns\Api\Autenticacao\Autenticacao.feature
e a lsita de dependências está em 
Especificacao\Comuns\Api\Autenticacao\AutenticacaoListaDependencias.feature

Quer dizer, Especificacao\Comuns\Api\Autenticacao\Autenticacao.feature lista como testamos a autenticação.
Esse teste é feito em vários lugares; por exemplo, em:
	And Implementado em ""Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedidoListaDependencias""
	And Implementado em ""Ambiente.ApiUnis.PrepedidoUnis.BuscarQtdeParcCartaoVisa.BuscarQtdeParcCartaoVisaListaDependencias""	
	And Implementado em ""Ambiente.ApiMagento.PedidoMagento.ObterCodigoMarketplace.ObterCodigoMarketplaceListaDependencias""	

Com esses registros, a gente amrca exatamente onde cada teste é feito.", ProgrammingLanguage.CSharp, new string[] {
                        "Especificacao.Testes.Utils.ListaDependencias"});
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
        
        [Xunit.SkippableFactAttribute(DisplayName="VerificacaoFinalListaDependencias")]
        [Xunit.TraitAttribute("FeatureTitle", "VerificacaoFinalListaDependencias")]
        [Xunit.TraitAttribute("Description", "VerificacaoFinalListaDependencias")]
        public virtual void VerificacaoFinalListaDependencias()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("VerificacaoFinalListaDependencias", null, tagsOfScenario, argumentsOfScenario);
#line 20
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
#line 23
 testRunner.Given("VerificacaoFinalListaDependencias", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
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
                VerificacaoFinalListaDependenciasFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                VerificacaoFinalListaDependenciasFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
