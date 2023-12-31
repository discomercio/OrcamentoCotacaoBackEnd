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
namespace Especificacao.Especificacao.Pedido.Passo30
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class CamposMagentoExigidosFeature : object, Xunit.IClassFixture<CamposMagentoExigidosFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "CamposMagentoExigidos.feature"
#line hidden
        
        public CamposMagentoExigidosFeature(CamposMagentoExigidosFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CamposMagentoExigidos", null, ProgrammingLanguage.CSharp, new string[] {
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
        
        public virtual void FeatureBackground()
        {
#line 5
#line hidden
#line 6
 testRunner.Given("Reiniciar appsettings", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 7
 testRunner.Given("Reiniciar banco ao terminar cenário", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Pedido_bs_x_marketplace - vazio")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Pedido_bs_x_marketplace - vazio")]
        public virtual void Pedido_Bs_X_Marketplace_Vazio()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Pedido_bs_x_marketplace - vazio", null, tagsOfScenario, argumentsOfScenario);
#line 82
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
#line 83
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 84
 testRunner.When("Informo \"Tipo_Parcelamento\" = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 85
 testRunner.When("Informo \"C_pu_valor\" = \"3132.90\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 86
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_marketplace\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 87
 testRunner.And("Informo \"InfCriacaoPedido.Marketplace_codigo_origem\" = \"010\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 88
 testRunner.Then("Erro \"Informe o nº do pedido do marketplace (Mercado Livre)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Pedido_bs_x_marketplace - somente digitos")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Pedido_bs_x_marketplace - somente digitos")]
        public virtual void Pedido_Bs_X_Marketplace_SomenteDigitos()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Pedido_bs_x_marketplace - somente digitos", null, tagsOfScenario, argumentsOfScenario);
#line 90
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
#line 91
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 92
 testRunner.When("Informo \"Tipo_Parcelamento\" = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 93
 testRunner.When("Informo \"C_pu_valor\" = \"3132.90\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 94
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"123456789\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 95
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_marketplace\" = \"123AA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 96
 testRunner.And("Informo \"InfCriacaoPedido.Marketplace_codigo_origem\" = \"010\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 97
 testRunner.Then("Erro \"O número Marketplace deve conter apenas dígitos e hífen\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 98
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 99
 testRunner.When("Informo \"Tipo_Parcelamento\" = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 100
 testRunner.When("Informo \"C_pu_valor\" = \"3132.90\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 101
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"123456789\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 102
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_marketplace\" = \"12-3AA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 103
 testRunner.And("Informo \"InfCriacaoPedido.Marketplace_codigo_origem\" = \"010\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 104
 testRunner.Then("Erro \"O número Marketplace deve conter apenas dígitos e hífen\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Marketplace_codigo_origem - vazio")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Marketplace_codigo_origem - vazio")]
        public virtual void Marketplace_Codigo_Origem_Vazio()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Marketplace_codigo_origem - vazio", null, tagsOfScenario, argumentsOfScenario);
#line 106
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
#line 107
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 108
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_marketplace\" = \"126\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 109
 testRunner.And("Informo \"InfCriacaoPedido.Marketplace_codigo_origem\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 110
 testRunner.Then("Erro \"Informe o Marketplace_codigo_origem.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Marketplace_codigo_origem - não existe")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Marketplace_codigo_origem - não existe")]
        public virtual void Marketplace_Codigo_Origem_NaoExiste()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Marketplace_codigo_origem - não existe", null, tagsOfScenario, argumentsOfScenario);
#line 112
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
#line 113
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 114
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_marketplace\" = \"127\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 115
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"123456789\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 116
 testRunner.And("Informo \"InfCriacaoPedido.Marketplace_codigo_origem\" = \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 117
 testRunner.Then("Erro \"Código Marketplace não encontrado.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Pedido_bs_x_ac - vazio")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Pedido_bs_x_ac - vazio")]
        public virtual void Pedido_Bs_X_Ac_Vazio()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Pedido_bs_x_ac - vazio", null, tagsOfScenario, argumentsOfScenario);
#line 119
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
#line 120
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 121
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 122
 testRunner.Then("Erro \"Favor informar o número do pedido Magento(Pedido_bs_x_ac)!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Pedido_bs_x_ac - formato inválido")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Pedido_bs_x_ac - formato inválido")]
        public virtual void Pedido_Bs_X_Ac_FormatoInvalido()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Pedido_bs_x_ac - formato inválido", null, tagsOfScenario, argumentsOfScenario);
#line 124
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
#line 125
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 126
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"12345678\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 127
 testRunner.Then("Erro \"Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 128
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 129
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"1234567890\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 130
 testRunner.Then("Erro \"Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Pedido_bs_x_ac - somente digitos")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Pedido_bs_x_ac - somente digitos")]
        public virtual void Pedido_Bs_X_Ac_SomenteDigitos()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Pedido_bs_x_ac - somente digitos", null, tagsOfScenario, argumentsOfScenario);
#line 132
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
#line 133
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 134
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"1234567AA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 135
 testRunner.Then("Erro \"O número Magento deve conter apenas dígitos!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Pedido_bs_x_ac - inicia com dígito inválido loja 201")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Pedido_bs_x_ac - inicia com dígito inválido loja 201")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void Pedido_Bs_X_Ac_IniciaComDigitoInvalidoLoja201()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Pedido_bs_x_ac - inicia com dígito inválido loja 201", null, tagsOfScenario, argumentsOfScenario);
#line 138
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
#line 139
 testRunner.And("verificar diigito inicial conforme a t_loja", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 141
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 142
 testRunner.And("Informo \"t_loja\" de \"201\", campo \"digito_magento\" = \"4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 143
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"223456778\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 144
 testRunner.Then("Erro \"regex .*O número do pedido Magento inicia com dígito inválido para a loja\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 145
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"423456778\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 146
 testRunner.Then("Sem erro \"regex .*O número do pedido Magento inicia com dígito inválido para a lo" +
                        "ja\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Pedido_bs_x_ac - inicia com dígito inválido loja 202")]
        [Xunit.TraitAttribute("FeatureTitle", "CamposMagentoExigidos")]
        [Xunit.TraitAttribute("Description", "Pedido_bs_x_ac - inicia com dígito inválido loja 202")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void Pedido_Bs_X_Ac_IniciaComDigitoInvalidoLoja202()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Pedido_bs_x_ac - inicia com dígito inválido loja 202", null, tagsOfScenario, argumentsOfScenario);
#line 149
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
#line 150
 testRunner.And("verificar diigito inicial conforme a t_loja", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 151
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 152
 testRunner.And("Informo \"appsettings.Loja\" = \"202\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 153
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"123456778\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 154
 testRunner.Then("Erro \"regex .*O número do pedido Magento inicia com dígito inválido para a loja\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                CamposMagentoExigidosFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                CamposMagentoExigidosFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
