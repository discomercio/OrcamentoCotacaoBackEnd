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
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo60")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class ObtemCtrlEstoqueProdutoRegraFeature : object, Xunit.IClassFixture<ObtemCtrlEstoqueProdutoRegraFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.Passo60",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "obtemCtrlEstoqueProdutoRegra.feature"
#line hidden
        
        public ObtemCtrlEstoqueProdutoRegraFeature(ObtemCtrlEstoqueProdutoRegraFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "obtemCtrlEstoqueProdutoRegra", null, ProgrammingLanguage.CSharp, new string[] {
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
        
        [Xunit.SkippableFactAttribute(DisplayName="obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("FeatureTitle", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Description", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ObtemCtrlEstoqueProdutoRegra()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("obtemCtrlEstoqueProdutoRegra", null, tagsOfScenario, argumentsOfScenario);
#line 9
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
#line 37
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD")]
        [Xunit.TraitAttribute("FeatureTitle", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Description", "obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD")]
        public virtual void ObtemCtrlEstoqueProdutoRegra_T_PRODUTO_X_WMS_REGRA_CD()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD", null, tagsOfScenario, argumentsOfScenario);
#line 39
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
#line 45
 testRunner.Given("Tabela \"t_PRODUTO_X_WMS_REGRA_CD\" fabricante = \"003\" e produto = \"003220\", altera" +
                        "r registro do campo \"id_wms_regra_cd\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 46
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 47
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': produto (003)003220 não está associado a nenhuma regra\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 48
 testRunner.And("Tabela t_PRODUTO_X_WMS_REGRA_CD fabricante = \"003\" e produto = \"003220\", verifica" +
                        "r campo \"id_wms_regra_cd\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD produto duplicadoScenario" +
            ": obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD produto duplicado")]
        [Xunit.TraitAttribute("FeatureTitle", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Description", "obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD produto duplicadoScenario" +
            ": obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD produto duplicado")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ObtemCtrlEstoqueProdutoRegra_T_PRODUTO_X_WMS_REGRA_CDProdutoDuplicadoScenarioObtemCtrlEstoqueProdutoRegra_T_PRODUTO_X_WMS_REGRA_CDProdutoDuplicado()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD produto duplicadoScenario" +
                    ": obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD produto duplicado", null, tagsOfScenario, argumentsOfScenario);
#line 51
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
#line 53
 testRunner.Given("Tabela \"t_PRODUTO_X_WMS_REGRA_CD\" duplicar regra para fabricante = \"003\" e produt" +
                        "o = \"003221\" com id_wms_regra_cd = \"6\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 54
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 55
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': produto (003)003221 não possui regra associada\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD sem regra")]
        [Xunit.TraitAttribute("FeatureTitle", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Description", "obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD sem regra")]
        public virtual void ObtemCtrlEstoqueProdutoRegra_T_PRODUTO_X_WMS_REGRA_CDSemRegra()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("obtemCtrlEstoqueProdutoRegra - t_PRODUTO_X_WMS_REGRA_CD sem regra", null, tagsOfScenario, argumentsOfScenario);
#line 57
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
#line 58
 testRunner.Given("Tabela \"t_PRODUTO_X_WMS_REGRA_CD\" apagar registro do fabricante = \"003\" e produto" +
                        " = \"003220\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 59
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 60
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': produto (003)003220 não possui regra associada\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD")]
        [Xunit.TraitAttribute("FeatureTitle", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Description", "obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD")]
        public virtual void ObtemCtrlEstoqueProdutoRegra_T_WMS_REGRA_CD()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD", null, tagsOfScenario, argumentsOfScenario);
#line 62
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
#line 66
 testRunner.Given("Tabela \"t_WMS_REGRA_CD\" apagar registro do fabricante = \"003\" e produto = \"003220" +
                        "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 67
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 68
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': regra associada ao produto (003)003220 não foi localizada no banco de dad" +
                        "os (Id=5)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF")]
        [Xunit.TraitAttribute("FeatureTitle", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Description", "obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF")]
        public virtual void ObtemCtrlEstoqueProdutoRegra_T_WMS_REGRA_CD_X_UF()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF", null, tagsOfScenario, argumentsOfScenario);
#line 70
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
#line 74
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF\" apagar registro do id_wms_regra_cd = \"5\" da UF = \"SP" +
                        "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 75
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 76
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': regra associada ao produto (003)003220 não está cadastrada para a UF \'SP\'" +
                        " (Id=5)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF duplicado")]
        [Xunit.TraitAttribute("FeatureTitle", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Description", "obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF duplicado")]
        public virtual void ObtemCtrlEstoqueProdutoRegra_T_WMS_REGRA_CD_X_UFDuplicado()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF duplicado", null, tagsOfScenario, argumentsOfScenario);
#line 78
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
#line 79
 testRunner.Given(@"No ambiente ""Especificacao.Pedido.PedidoSteps"" mapear erro ""Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não foi localizada no banco de dados (Id=5)"" para ""Falha na leitura da regra de consumo do estoque para a UF 'SP' e 'Pessoa Física': regra associada ao produto (003)003220 não está cadastrada para a UF 'SP' (Id=5)""", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 80
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF\" duplicar registro do id_wms_regra_cd = \"5\" da UF = \"" +
                        "SP\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 81
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 82
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': regra associada ao produto (003)003220 não foi localizada no banco de dad" +
                        "os (Id=5)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF_X_PESSOA")]
        [Xunit.TraitAttribute("FeatureTitle", "obtemCtrlEstoqueProdutoRegra")]
        [Xunit.TraitAttribute("Description", "obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF_X_PESSOA")]
        public virtual void ObtemCtrlEstoqueProdutoRegra_T_WMS_REGRA_CD_X_UF_X_PESSOA()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("obtemCtrlEstoqueProdutoRegra - t_WMS_REGRA_CD_X_UF_X_PESSOA", null, tagsOfScenario, argumentsOfScenario);
#line 84
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
#line 89
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA\" apagar registro id = \"134\" e tipo de pessoa" +
                        " = \"PF\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 91
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA\" apagar registro id = \"134\" e tipo de pessoa" +
                        " = \"PR\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 92
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 93
 testRunner.Then("Erro \"Falha na leitura da regra de consumo do estoque para a UF \'SP\' e \'Pessoa Fí" +
                        "sica\': regra associada ao produto (003)003220 não está cadastrada para a UF \'SP\'" +
                        " (Id=5)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                ObtemCtrlEstoqueProdutoRegraFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ObtemCtrlEstoqueProdutoRegraFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
