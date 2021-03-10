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
namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo20.ObtemCtrlEstoqueProdutoRegra
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo60")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class T_WMS_REGRA_CD_X_UF_X_PESSOAFeature : object, Xunit.IClassFixture<T_WMS_REGRA_CD_X_UF_X_PESSOAFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.Passo60",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "t_WMS_REGRA_CD_X_UF_X_PESSOA.feature"
#line hidden
        
        public T_WMS_REGRA_CD_X_UF_X_PESSOAFeature(T_WMS_REGRA_CD_X_UF_X_PESSOAFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "t_WMS_REGRA_CD_X_UF_X_PESSOA", null, ProgrammingLanguage.CSharp, new string[] {
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
        
        [Xunit.SkippableFactAttribute(DisplayName="t_WMS_REGRA_CD_X_UF_X_PESSOA - sem regra")]
        [Xunit.TraitAttribute("FeatureTitle", "t_WMS_REGRA_CD_X_UF_X_PESSOA")]
        [Xunit.TraitAttribute("Description", "t_WMS_REGRA_CD_X_UF_X_PESSOA - sem regra")]
        public virtual void T_WMS_REGRA_CD_X_UF_X_PESSOA_SemRegra()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("t_WMS_REGRA_CD_X_UF_X_PESSOA - sem regra", null, tagsOfScenario, argumentsOfScenario);
#line 12
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
#line 13
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA\" apagar registro id = \"134\" e tipo de pessoa" +
                        " = \"PF\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 15
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA\" apagar registro id = \"134\" e tipo de pessoa" +
                        " = \"PR\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 16
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 17
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': regra associada ao produto (003)003220 não está cadastrada para a UF \'SP\'" +
                        " (Id=5)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="t_WMS_REGRA_CD_X_UF_X_PESSOA - duplicado")]
        [Xunit.TraitAttribute("FeatureTitle", "t_WMS_REGRA_CD_X_UF_X_PESSOA")]
        [Xunit.TraitAttribute("Description", "t_WMS_REGRA_CD_X_UF_X_PESSOA - duplicado")]
        public virtual void T_WMS_REGRA_CD_X_UF_X_PESSOA_Duplicado()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("t_WMS_REGRA_CD_X_UF_X_PESSOA - duplicado", null, tagsOfScenario, argumentsOfScenario);
#line 19
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
#line 20
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA\" duplicar registro id_wms_regra_cd_x_uf = \"1" +
                        "34\" e tipo de pessoa = \"PF\" com id = \"811\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 21
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA\" duplicar registro id_wms_regra_cd_x_uf = \"1" +
                        "34\" e tipo de pessoa = \"PR\" com id = \"812\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 22
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 23
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': regra associada ao produto (003)003220 não está cadastrada para a UF \'SP\'" +
                        " (Id=5)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="t_WMS_REGRA_CD_X_UF_X_PESSOA - spe_id_nfe_emitente = 0")]
        [Xunit.TraitAttribute("FeatureTitle", "t_WMS_REGRA_CD_X_UF_X_PESSOA")]
        [Xunit.TraitAttribute("Description", "t_WMS_REGRA_CD_X_UF_X_PESSOA - spe_id_nfe_emitente = 0")]
        public virtual void T_WMS_REGRA_CD_X_UF_X_PESSOA_Spe_Id_Nfe_Emitente0()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("t_WMS_REGRA_CD_X_UF_X_PESSOA - spe_id_nfe_emitente = 0", null, tagsOfScenario, argumentsOfScenario);
#line 25
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
#line 26
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA\" registro id_wms_regra_cd_x_uf = \"134\" e tip" +
                        "o de pessoa = \"PF\", alterar campo \"spe_id_nfe_emitente\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 27
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA\" registro id_wms_regra_cd_x_uf = \"134\" e tip" +
                        "o de pessoa = \"PR\", alterar campo \"spe_id_nfe_emitente\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 28
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 29
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': regra associada ao produto (003)003220 não especifica nenhum CD para agua" +
                        "rdar produtos sem presença no estoque (Id=5)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                T_WMS_REGRA_CD_X_UF_X_PESSOAFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                T_WMS_REGRA_CD_X_UF_X_PESSOAFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
