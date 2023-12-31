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
namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.P40_Produtos
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "ignore")]
    [Xunit.TraitAttribute("Category", "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class ProdutosCompostosFeature : object, Xunit.IClassFixture<ProdutosCompostosFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "ignore",
                "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ProdutosCompostosComDesconto.feature"
#line hidden
        
        public ProdutosCompostosFeature(ProdutosCompostosFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ProdutosCompostos", @"	Verifica se a Api Magento está aceitando o pedido com produtos compostos.
	O Magento pode enviar os produtos compostos para que a ApiMagento faça a separação dos produtos compostos antes de inserir o pedido

SKU:	001090	
É composto por:	
001 / 001000	x 1
001 / 001001	x 1", ProgrammingLanguage.CSharp, new string[] {
                        "ignore",
                        "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido",
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
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ProdutosCompostos - Sucesso")]
        [Xunit.TraitAttribute("FeatureTitle", "ProdutosCompostos")]
        [Xunit.TraitAttribute("Description", "ProdutosCompostos - Sucesso")]
        public virtual void ProdutosCompostos_Sucesso()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ProdutosCompostos - Sucesso", null, tagsOfScenario, argumentsOfScenario);
#line 13
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
#line 15
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 16
 testRunner.When("Lista de itens com \"1\" itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 18
 testRunner.When("Lista de itens \"0\" informo \"Sku\" = \"001090\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 19
 testRunner.When("Lista de itens \"0\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 20
 testRunner.When("Lista de itens \"0\" informo \"Subtotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 21
 testRunner.When("Lista de itens \"0\" informo \"RowTotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 22
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 23
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 24
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"produto\" = \"001" +
                        "000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 25
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"qtde\" = \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 26
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 27
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 28
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 29
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"produto\" = \"001" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 30
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"qtde\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 31
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 32
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ProdutosCompostos - produtos repetidos")]
        [Xunit.TraitAttribute("FeatureTitle", "ProdutosCompostos")]
        [Xunit.TraitAttribute("Description", "ProdutosCompostos - produtos repetidos")]
        public virtual void ProdutosCompostos_ProdutosRepetidos()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ProdutosCompostos - produtos repetidos", null, tagsOfScenario, argumentsOfScenario);
#line 34
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
#line 37
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 38
 testRunner.When("Lista de itens com \"2\" itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 40
 testRunner.When("Lista de itens \"0\" informo \"Sku\" = \"001090\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 41
 testRunner.When("Lista de itens \"0\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 42
 testRunner.When("Lista de itens \"0\" informo \"Subtotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 43
 testRunner.When("Lista de itens \"0\" informo \"RowTotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 45
 testRunner.When("Lista de itens \"1\" informo \"Sku\" = \"001000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 46
 testRunner.When("Lista de itens \"1\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 47
 testRunner.When("Lista de itens \"1\" informo \"Subtotal\" = \"340.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 48
 testRunner.When("Lista de itens \"1\" informo \"RowTotal\" = \"340.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 49
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 50
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 51
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"produto\" = \"001" +
                        "000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 52
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"qtde\" = \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 53
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_venda\" = " +
                        "\"680.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 54
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_nf\" = \"68" +
                        "0.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 55
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 56
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"produto\" = \"001" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 57
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"qtde\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 58
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 59
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ProdutosCompostos - com desconto")]
        [Xunit.TraitAttribute("FeatureTitle", "ProdutosCompostos")]
        [Xunit.TraitAttribute("Description", "ProdutosCompostos - com desconto")]
        public virtual void ProdutosCompostos_ComDesconto()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ProdutosCompostos - com desconto", null, tagsOfScenario, argumentsOfScenario);
#line 61
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
#line 63
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 64
 testRunner.When("Lista de itens com \"1\" itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 66
 testRunner.When("Lista de itens \"0\" informo \"Sku\" = \"001090\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 67
 testRunner.When("Lista de itens \"0\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 68
 testRunner.When("Lista de itens \"0\" informo \"Subtotal\" = \"1300.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 69
 testRunner.When("Lista de itens \"0\" informo \"DiscountAmount\" = \"200.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 70
 testRunner.When("Lista de itens \"0\" informo \"RowTotal\" = \"1100.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 71
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 72
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 73
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"produto\" = \"001" +
                        "000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 74
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"qtde\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 75
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"desc_dado\" = \"a" +
                        "justar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 76
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 77
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 78
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 79
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"produto\" = \"001" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 80
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"qtde\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 81
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"desc_dado\" = \"a" +
                        "justar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 82
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 83
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ProdutosCompostos - produtos repetidos com desconto")]
        [Xunit.TraitAttribute("FeatureTitle", "ProdutosCompostos")]
        [Xunit.TraitAttribute("Description", "ProdutosCompostos - produtos repetidos com desconto")]
        public virtual void ProdutosCompostos_ProdutosRepetidosComDesconto()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ProdutosCompostos - produtos repetidos com desconto", null, tagsOfScenario, argumentsOfScenario);
#line 85
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
#line 87
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 88
 testRunner.When("Lista de itens com \"2\" itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 90
 testRunner.When("Lista de itens \"0\" informo \"Sku\" = \"001090\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 91
 testRunner.When("Lista de itens \"0\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 92
 testRunner.When("Lista de itens \"0\" informo \"Subtotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 93
 testRunner.When("Lista de itens \"0\" informo \"DiscountAmount\" = \"200.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 94
 testRunner.When("Lista de itens \"0\" informo \"RowTotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 96
 testRunner.When("Lista de itens \"1\" informo \"Sku\" = \"001000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 97
 testRunner.When("Lista de itens \"1\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 98
 testRunner.When("Lista de itens \"1\" informo \"Subtotal\" = \"340.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 99
 testRunner.When("Lista de itens \"0\" informo \"DiscountAmount\" = \"100.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 100
 testRunner.When("Lista de itens \"1\" informo \"RowTotal\" = \"340.00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 101
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 102
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 103
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"produto\" = \"001" +
                        "000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 104
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"qtde\" = \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 105
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"desc_dado\" = \"a" +
                        "justar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 106
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 107
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 108
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 109
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"produto\" = \"001" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 110
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"qtde\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 111
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"desc_dado\" = \"a" +
                        "justar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 112
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 113
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ProdutosCompostos - diluir frete")]
        [Xunit.TraitAttribute("FeatureTitle", "ProdutosCompostos")]
        [Xunit.TraitAttribute("Description", "ProdutosCompostos - diluir frete")]
        public virtual void ProdutosCompostos_DiluirFrete()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ProdutosCompostos - diluir frete", null, tagsOfScenario, argumentsOfScenario);
#line 115
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
#line 117
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 118
 testRunner.When("Informo \"PedidoTotaisMagentoDto.FreteBruto\" = \"69.90\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 119
 testRunner.And("Lista de itens com \"2\" itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 121
 testRunner.When("Lista de itens \"0\" informo \"Sku\" = \"001090\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 122
 testRunner.When("Lista de itens \"0\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 123
 testRunner.When("Lista de itens \"0\" informo \"Subtotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 124
 testRunner.When("Lista de itens \"0\" informo \"RowTotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 126
 testRunner.When("Lista de itens \"1\" informo \"Sku\" = \"001091\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 127
 testRunner.When("Lista de itens \"1\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 128
 testRunner.When("Lista de itens \"1\" informo \"Subtotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 129
 testRunner.When("Lista de itens \"1\" informo \"RowTotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 130
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 132
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 133
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"produto\" = \"001" +
                        "000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 134
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"qtde\" = \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 135
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"desc_dado\" = \"a" +
                        "justar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 136
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 137
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"1\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 139
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 140
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"produto\" = \"001" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 141
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"qtde\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 142
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"desc_dado\" = \"a" +
                        "justar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 143
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 144
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"2\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 146
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"3\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 147
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"3\" campo \"produto\" = \"001" +
                        "000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 148
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"3\" campo \"qtde\" = \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 149
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"3\" campo \"desc_dado\" = \"a" +
                        "justar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 150
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"3\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 151
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"3\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 153
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"4\" campo \"fabricante\" = \"" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 154
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"4\" campo \"produto\" = \"001" +
                        "001\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 155
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"4\" campo \"qtde\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 156
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"4\" campo \"desc_dado\" = \"a" +
                        "justar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 157
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"4\" campo \"preco_venda\" = " +
                        "\"ajustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 158
 testRunner.And("Tabela \"t_PEDIDO_ITEM\" registro criado, verificar item \"4\" campo \"preco_nf\" = \"aj" +
                        "ustar valor\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ProdutosCompostos - com desconto e diluir frete")]
        [Xunit.TraitAttribute("FeatureTitle", "ProdutosCompostos")]
        [Xunit.TraitAttribute("Description", "ProdutosCompostos - com desconto e diluir frete")]
        public virtual void ProdutosCompostos_ComDescontoEDiluirFrete()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ProdutosCompostos - com desconto e diluir frete", null, tagsOfScenario, argumentsOfScenario);
#line 160
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
#line 162
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ProdutosCompostos - não existe")]
        [Xunit.TraitAttribute("FeatureTitle", "ProdutosCompostos")]
        [Xunit.TraitAttribute("Description", "ProdutosCompostos - não existe")]
        public virtual void ProdutosCompostos_NaoExiste()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ProdutosCompostos - não existe", null, tagsOfScenario, argumentsOfScenario);
#line 164
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
#line 165
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 166
 testRunner.When("Lista de itens com \"1\" itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 167
 testRunner.When("Lista de itens \"0\" informo \"Sku\" = \"001111\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 168
 testRunner.When("Lista de itens \"0\" informo \"Quantidade\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 169
 testRunner.When("Lista de itens \"0\" informo \"Subtotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 170
 testRunner.When("Lista de itens \"0\" informo \"RowTotal\" = \"ajustar valores\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 171
 testRunner.Then("Erro \"ajustar msg\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                ProdutosCompostosFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ProdutosCompostosFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
