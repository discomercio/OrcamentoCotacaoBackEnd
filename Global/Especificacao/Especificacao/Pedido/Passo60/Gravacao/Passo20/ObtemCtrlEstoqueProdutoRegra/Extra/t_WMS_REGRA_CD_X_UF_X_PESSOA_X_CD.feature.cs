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
namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo20.ObtemCtrlEstoqueProdutoRegra.Extra
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo60")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CDFeature : object, Xunit.IClassFixture<T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CDFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.Passo60",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD.feature"
#line hidden
        
        public T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CDFeature(T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CDFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD", null, ProgrammingLanguage.CSharp, new string[] {
                        "Especificacao.Pedido.Passo60",
                        "GerenciamentoBanco"});
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
        
        public virtual void FeatureBackground()
        {
#line 5
#line hidden
#line 6
 testRunner.Given("Reiniciar banco ao terminar cenário", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD - sem registro")]
        [Xunit.TraitAttribute("FeatureTitle", "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD")]
        [Xunit.TraitAttribute("Description", "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD - sem registro")]
        public virtual void T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD_SemRegistro()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD - sem registro", null, tagsOfScenario, argumentsOfScenario);
#line 17
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
#line 5
this.FeatureBackground();
#line hidden
#line 18
 testRunner.Given("Limpar tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 19
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 20
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': regra associada ao produto (003)003220 não especifica nenhum CD para cons" +
                        "umo do estoque (Id=5)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD - considerar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD." +
            "st_inativo = 1")]
        [Xunit.TraitAttribute("FeatureTitle", "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD")]
        [Xunit.TraitAttribute("Description", "t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD - considerar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD." +
            "st_inativo = 1")]
        [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo60.Gravacao.Passo20.ObtemCtrlEstoqueProdutoRegra.t_WMS_" +
            "REGRA_CD_X_UF_X_PESSOA_CD")]
        public virtual void T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD_ConsiderarT_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD_St_Inativo1()
        {
            string[] tagsOfScenario = new string[] {
                    "Especificacao.Pedido.Passo60.Gravacao.Passo20.ObtemCtrlEstoqueProdutoRegra.t_WMS_" +
                        "REGRA_CD_X_UF_X_PESSOA_CD"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD - considerar t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD." +
                    "st_inativo = 1", null, tagsOfScenario, argumentsOfScenario);
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
#line 5
this.FeatureBackground();
#line hidden
#line 24
 testRunner.Given("Tabela \"t_NFe_EMITENTE\" registro tipo de pessoa = \"PR\" e id_wms_regra_cd_x_uf = \"" +
                        "26\", alterar campo \"st_ativo\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 25
 testRunner.Given("Chamar ObtemCtrlEstoqueProdutoRegra e verificar regra do produto = \"003220\" e id_" +
                        "nfe_emitente = \"4001\", campo st_inativo = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 26
 testRunner.Given("Tabela \"t_NFe_EMITENTE\" verificar registro tipo de pessoa = \"PR\" e id_wms_regra_c" +
                        "d_x_uf = \"26\",campo \"st_ativo\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
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
                T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CDFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                T_WMS_REGRA_CD_X_UF_X_PESSOA_X_CDFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion