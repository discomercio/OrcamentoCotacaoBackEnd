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
namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.ESTOQUE_Produto_Saida_V2_Rotin" +
        "aSteps")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class ESTOQUE_Produto_Saida_V2_RotinaFeature : object, Xunit.IClassFixture<ESTOQUE_Produto_Saida_V2_RotinaFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.ESTOQUE_Produto_Saida_V2_Rotin" +
                    "aSteps",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ESTOQUE_produto_saida_v2_rotina.feature"
#line hidden
        
        public ESTOQUE_Produto_Saida_V2_RotinaFeature(ESTOQUE_Produto_Saida_V2_RotinaFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ESTOQUE_produto_saida_v2_rotina", null, ProgrammingLanguage.CSharp, new string[] {
                        "Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.ESTOQUE_Produto_Saida_V2_Rotin" +
                            "aSteps",
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
#line 6
#line hidden
#line 7
 testRunner.Given("Reiniciar banco ao terminar cenário", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 8
 testRunner.Given("Usar produto \"um\" como fabricante = \"003\", produto = \"003220\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 9
 testRunner.And("Usar produto \"dois\" como fabricante = \"003\", produto = \"003221\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 10
 testRunner.And("Zerar todo o estoque", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ESTOQUE_produto_saida_v2_rotina normal")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "ESTOQUE_produto_saida_v2_rotina normal")]
        public virtual void ESTOQUE_Produto_Saida_V2_RotinaNormal()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ESTOQUE_produto_saida_v2_rotina normal", null, tagsOfScenario, argumentsOfScenario);
#line 31
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
#line 6
this.FeatureBackground();
#line hidden
#line 32
 testRunner.Given("Definir saldo de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 33
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"10\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 34
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"10\", qtde_estoque_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 35
 testRunner.And("Saldo de estoque = \"30\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 36
 testRunner.And("Movimento de estoque = \"10\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 37
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ESTOQUE_produto_saida_v2_rotina para estoque sem presença")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "ESTOQUE_produto_saida_v2_rotina para estoque sem presença")]
        public virtual void ESTOQUE_Produto_Saida_V2_RotinaParaEstoqueSemPresenca()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ESTOQUE_produto_saida_v2_rotina para estoque sem presença", null, tagsOfScenario, argumentsOfScenario);
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
#line 6
this.FeatureBackground();
#line hidden
#line 40
 testRunner.Given("Definir saldo de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 41
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"100\", qtde_aut" +
                        "orizada_sem_presenca = \"60\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 42
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"40\", qtde_estoque_sem_presenca = \"60\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 43
 testRunner.And("Saldo de estoque = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 44
 testRunner.And("Movimento de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 45
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"60\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Produtos spe")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "Produtos spe")]
        public virtual void ProdutosSpe()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Produtos spe", null, tagsOfScenario, argumentsOfScenario);
#line 47
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
#line 6
this.FeatureBackground();
#line hidden
#line 48
 testRunner.Given("Definir saldo de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 49
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"50\", qtde_auto" +
                        "rizada_sem_presenca = \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 50
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"40\", qtde_estoque_sem_presenca = \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 51
 testRunner.And("Saldo de estoque = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 52
 testRunner.And("Movimento de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 53
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"10\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="muitos produtos sem presenca")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "muitos produtos sem presenca")]
        public virtual void MuitosProdutosSemPresenca()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("muitos produtos sem presenca", null, tagsOfScenario, argumentsOfScenario);
#line 55
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
#line 6
this.FeatureBackground();
#line hidden
#line 57
 testRunner.Given("Definir saldo de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 58
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"50\", qtde_auto" +
                        "rizada_sem_presenca = \"20\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 59
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"40\", qtde_estoque_sem_presenca = \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 60
 testRunner.And("Saldo de estoque = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 61
 testRunner.And("Movimento de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 62
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"10\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Sem produtos")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "Sem produtos")]
        public virtual void SemProdutos()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Sem produtos", null, tagsOfScenario, argumentsOfScenario);
#line 64
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
#line 6
this.FeatureBackground();
#line hidden
#line 66
 testRunner.Given("Definir saldo de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 67
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"50\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 68
 testRunner.Then("msg_erro \"Produto 003220 do fabricante 003: faltam 10 unidades\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Sem produtos - 2")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "Sem produtos - 2")]
        public virtual void SemProdutos_2()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Sem produtos - 2", null, tagsOfScenario, argumentsOfScenario);
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
#line 6
this.FeatureBackground();
#line hidden
#line 71
 testRunner.Given("Definir saldo de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 72
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"50\", qtde_auto" +
                        "rizada_sem_presenca = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 73
 testRunner.Then("msg_erro \"Produto 003220 do fabricante 003: faltam 5 unidades\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="chamadas acumuladas - com erro")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "chamadas acumuladas - com erro")]
        public virtual void ChamadasAcumuladas_ComErro()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("chamadas acumuladas - com erro", null, tagsOfScenario, argumentsOfScenario);
#line 76
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
#line 6
this.FeatureBackground();
#line hidden
#line 77
 testRunner.Given("Definir saldo de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 78
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"20\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 79
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"20\", qtde_estoque_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 80
 testRunner.And("Saldo de estoque = \"20\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 81
 testRunner.And("Movimento de estoque = \"20\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 82
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 83
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"15\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 84
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"15\", qtde_estoque_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 85
 testRunner.And("Saldo de estoque = \"5\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 86
 testRunner.And("Movimento de estoque = \"35\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 87
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 88
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"10\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 91
 testRunner.Then("msg_erro \"Produto 003220 do fabricante 003: faltam 5 unidades\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="chamadas acumuladas")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "chamadas acumuladas")]
        public virtual void ChamadasAcumuladas()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("chamadas acumuladas", null, tagsOfScenario, argumentsOfScenario);
#line 94
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
#line 6
this.FeatureBackground();
#line hidden
#line 95
 testRunner.Given("Definir saldo de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 96
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"20\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 97
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"20\", qtde_estoque_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 98
 testRunner.And("Saldo de estoque = \"20\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 99
 testRunner.And("Movimento de estoque = \"20\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 100
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 101
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"15\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 102
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"15\", qtde_estoque_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 103
 testRunner.And("Saldo de estoque = \"5\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 104
 testRunner.And("Movimento de estoque = \"35\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 105
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 106
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"11\", qtde_auto" +
                        "rizada_sem_presenca = \"8\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 107
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"5\", qtde_estoque_sem_presenca = \"6\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 108
 testRunner.And("Saldo de estoque = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 109
 testRunner.And("Movimento de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 110
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"6\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 112
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"12\", qtde_auto" +
                        "rizada_sem_presenca = \"12\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 113
 testRunner.Then("Retorno sucesso, qtde_estoque_vendido = \"0\", qtde_estoque_sem_presenca = \"12\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 114
 testRunner.And("Saldo de estoque = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 115
 testRunner.And("Movimento de estoque = \"40\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 116
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"18\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Respeita ordem FIFO")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "Respeita ordem FIFO")]
        public virtual void RespeitaOrdemFIFO()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Respeita ordem FIFO", null, tagsOfScenario, argumentsOfScenario);
#line 118
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
#line 6
this.FeatureBackground();
#line hidden
#line 119
 testRunner.Given("Definir2 saldo de estoque = \"12\" para produto \"um\" com valor \"100\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 120
 testRunner.Given("Definir2 saldo de estoque = \"14\" para produto \"um\" com valor \"222\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 121
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"15\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 122
 testRunner.Then("Saldo2 de estoque = \"0\" para produto \"um\" com valor \"100\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 123
 testRunner.Then("Saldo2 de estoque = \"11\" para produto \"um\" com valor \"222\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 124
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="FIFO com mais de um produto")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "FIFO com mais de um produto")]
        public virtual void FIFOComMaisDeUmProduto()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("FIFO com mais de um produto", null, tagsOfScenario, argumentsOfScenario);
#line 127
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
#line 6
this.FeatureBackground();
#line hidden
#line 128
 testRunner.Given("Definir2 saldo de estoque = \"10\" para produto \"um\" com valor \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 129
 testRunner.Given("Definir2 saldo de estoque = \"14\" para produto \"um\" com valor \"222\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 130
 testRunner.Given("Definir2 saldo de estoque = \"21\" para produto \"dois\" com valor \"333\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 131
 testRunner.Given("Definir2 saldo de estoque = \"24\" para produto \"dois\" com valor \"444\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 132
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"dois\", qtde_a_sair = \"44\", qtde_au" +
                        "torizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 133
 testRunner.Then("Saldo2 de estoque = \"0\" para produto \"dois\" com valor \"333\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 134
 testRunner.Then("Saldo2 de estoque = \"1\" para produto \"dois\" com valor \"444\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 135
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"0\" para produto \"dois\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 136
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"15\", qtde_auto" +
                        "rizada_sem_presenca = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 137
 testRunner.Then("Saldo2 de estoque = \"0\" para produto \"um\" com valor \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 138
 testRunner.Then("Saldo2 de estoque = \"9\" para produto \"um\" com valor \"222\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 139
 testRunner.And("Movimento ID_ESTOQUE_SEM_PRESENCA = \"0\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Não conseguiu movimentar qtde suficiente")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2_rotina")]
        [Xunit.TraitAttribute("Description", "Não conseguiu movimentar qtde suficiente")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void NaoConseguiuMovimentarQtdeSuficiente()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Não conseguiu movimentar qtde suficiente", @"esta duplicado pois estou tendo entrar no erro
Produto "" + id_produto + "" do fabricante "" + id_fabricante + 
"": faltam "" + ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) + 
"" unidades no estoque para poder atender ao pedido.
Não consigo entrar na mensagem de erro que quero", tagsOfScenario, argumentsOfScenario);
#line 142
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
#line 6
this.FeatureBackground();
#line hidden
#line 148
 testRunner.Given("Definir saldo de estoque = \"50\" para produto \"um\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 149
 testRunner.When("Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = \"um\", qtde_a_sair = \"51\", qtde_auto" +
                        "rizada_sem_presenca = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 150
 testRunner.Then("msg_erro \"Ajustar\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                ESTOQUE_Produto_Saida_V2_RotinaFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ESTOQUE_Produto_Saida_V2_RotinaFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
