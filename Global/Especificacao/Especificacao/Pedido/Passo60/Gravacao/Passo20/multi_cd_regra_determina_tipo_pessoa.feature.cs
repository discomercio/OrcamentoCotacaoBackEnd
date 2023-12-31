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
namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo20
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo60.Gravacao.Passo20.multi_cd_regra_determina_tipo_pesso" +
        "a")]
    public partial class Multi_Cd_Regra_Determina_Tipo_PessoaFeature : object, Xunit.IClassFixture<Multi_Cd_Regra_Determina_Tipo_PessoaFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.Passo60.Gravacao.Passo20.multi_cd_regra_determina_tipo_pesso" +
                    "a"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "multi_cd_regra_determina_tipo_pessoa.feature"
#line hidden
        
        public Multi_Cd_Regra_Determina_Tipo_PessoaFeature(Multi_Cd_Regra_Determina_Tipo_PessoaFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "multi_cd_regra_determina_tipo_pessoa", null, ProgrammingLanguage.CSharp, new string[] {
                        "Especificacao.Pedido.Passo60.Gravacao.Passo20.multi_cd_regra_determina_tipo_pesso" +
                            "a"});
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
        
        [Xunit.SkippableTheoryAttribute(DisplayName="Testar multi_cd_regra_determina_tipo_pessoa")]
        [Xunit.TraitAttribute("FeatureTitle", "multi_cd_regra_determina_tipo_pessoa")]
        [Xunit.TraitAttribute("Description", "Testar multi_cd_regra_determina_tipo_pessoa")]
        [Xunit.InlineDataAttribute("ID_PF", "", "COD_ST_CLIENTE_PRODUTOR_RURAL_SIM", "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PRODUTOR_RURAL", new string[0])]
        [Xunit.InlineDataAttribute("ID_PF", "", "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO", "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_FISICA", new string[0])]
        [Xunit.InlineDataAttribute("ID_PJ", "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM", "", "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_CONTRIBUINTE", new string[0])]
        [Xunit.InlineDataAttribute("ID_PJ", "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO", "", "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_NAO_CONTRIBUINTE", new string[0])]
        [Xunit.InlineDataAttribute("ID_PJ", "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO", "", "COD_WMS_MULTI_CD_REGRA__TIPO_PESSOA__PESSOA_JURIDICA_ISENTO", new string[0])]
        public virtual void TestarMulti_Cd_Regra_Determina_Tipo_Pessoa(string tipo_Cliente, string contribuinte_Icms_Status, string produtor_Rural_Status, string resultado, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("tipo_cliente", tipo_Cliente);
            argumentsOfScenario.Add("contribuinte_icms_status", contribuinte_Icms_Status);
            argumentsOfScenario.Add("produtor_rural_status", produtor_Rural_Status);
            argumentsOfScenario.Add("resultado", resultado);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Testar multi_cd_regra_determina_tipo_pessoa", null, tagsOfScenario, argumentsOfScenario);
#line 29
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
#line 30
 testRunner.Then(string.Format("Chamar rotina MULTI_CD_REGRA_DETERMINA_TIPO_PESSOA tipo cliente = \"{0}\", contribu" +
                            "inte = \"{1}\", produtor rural = \"{2}\" e resultado = \"{3}\"", tipo_Cliente, contribuinte_Icms_Status, produtor_Rural_Status, resultado), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                Multi_Cd_Regra_Determina_Tipo_PessoaFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                Multi_Cd_Regra_Determina_Tipo_PessoaFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
