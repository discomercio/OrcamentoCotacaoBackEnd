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
namespace Especificacao.Especificacao.Pedido.Passo60.Validacao.CreditoAutomatico
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional")]
    public partial class CreditoAutomaticoECommerceFeature : object, Xunit.IClassFixture<CreditoAutomaticoECommerceFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "CreditoAutomaticoECommerce.feature"
#line hidden
        
        public CreditoAutomaticoECommerceFeature(CreditoAutomaticoECommerceFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "CreditoAutomaticoECommerce", null, ProgrammingLanguage.CSharp, new string[] {
                        "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.EspecificacaoAdicional"});
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
        
        [Xunit.SkippableFactAttribute(DisplayName="CreditoAutomaticoECommerce - NUMERO_LOJA_ECOMMERCE_AR_CLUBE")]
        [Xunit.TraitAttribute("FeatureTitle", "CreditoAutomaticoECommerce")]
        [Xunit.TraitAttribute("Description", "CreditoAutomaticoECommerce - NUMERO_LOJA_ECOMMERCE_AR_CLUBE")]
        public virtual void CreditoAutomaticoECommerce_NUMERO_LOJA_ECOMMERCE_AR_CLUBE()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CreditoAutomaticoECommerce - NUMERO_LOJA_ECOMMERCE_AR_CLUBE", null, tagsOfScenario, argumentsOfScenario);
#line 92
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
#line 94
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 95
 testRunner.When("Informo \"Tipo_Parcelamento\" = \"COD_FORMA_PAGTO_PARCELA_UNICA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 96
 testRunner.When("Informo \"C_pu_valor\" = \"3132.90\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 97
 testRunner.When("Informo \"InfCriacaoPedido.Marketplace_codigo_origem\" = \"010\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 98
 testRunner.And("Informo \"InfCriacaoPedido.Pedido_bs_x_marketplace\" = \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 99
 testRunner.When("Informo \"appsettings.Loja\" = \"201\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 100
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 101
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"tipo_parcelamento\" = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 102
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"vendedor\" = \"USRMAG\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 103
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_credito\" = \"2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 104
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_credito_usuario\" = \"A" +
                        "UTOMÁTICO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 105
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_endereco_tratar_statu" +
                        "s\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="CreditoAutomaticoECommerce - NUMERO_LOJA_ECOMMERCE_AR_CLUBE á vista")]
        [Xunit.TraitAttribute("FeatureTitle", "CreditoAutomaticoECommerce")]
        [Xunit.TraitAttribute("Description", "CreditoAutomaticoECommerce - NUMERO_LOJA_ECOMMERCE_AR_CLUBE á vista")]
        public virtual void CreditoAutomaticoECommerce_NUMERO_LOJA_ECOMMERCE_AR_CLUBEAVista()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CreditoAutomaticoECommerce - NUMERO_LOJA_ECOMMERCE_AR_CLUBE á vista", null, tagsOfScenario, argumentsOfScenario);
#line 107
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
#line 113
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 114
 testRunner.When("Informo \"appsettings.Loja\" = \"201\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 115
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 116
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"tipo_parcelamento\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 117
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"av_forma_pagto\" = \"6\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 118
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"vendedor\" = \"USRMAG\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 119
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_credito\" = \"8\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 120
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_credito_usuario\" = \"A" +
                        "UTOMÁTICO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 121
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_endereco_tratar_statu" +
                        "s\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="CreditoAutomaticoECommerce - Loja diferente de NUMERO_LOJA_ECOMMERCE_AR_CLUBE")]
        [Xunit.TraitAttribute("FeatureTitle", "CreditoAutomaticoECommerce")]
        [Xunit.TraitAttribute("Description", "CreditoAutomaticoECommerce - Loja diferente de NUMERO_LOJA_ECOMMERCE_AR_CLUBE")]
        public virtual void CreditoAutomaticoECommerce_LojaDiferenteDeNUMERO_LOJA_ECOMMERCE_AR_CLUBE()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CreditoAutomaticoECommerce - Loja diferente de NUMERO_LOJA_ECOMMERCE_AR_CLUBE", null, tagsOfScenario, argumentsOfScenario);
#line 123
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
#line 126
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 127
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 128
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"tipo_parcelamento\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 129
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"vendedor\" = \"USRMAG\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 130
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_credito\" = \"8\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 131
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_credito_usuario\" = \"A" +
                        "UTOMÁTICO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 132
 testRunner.And("Tabela \"t_PEDIDO\" registro criado, verificar campo \"analise_endereco_tratar_statu" +
                        "s\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="configurações da t_CODIGO_DESCRICAO para PedidoECommerce_Origem_Grupo (testar o b" +
            "lnPedidoECommerceCreditoOkAutomatico e a exigência de nº do pedido do marketplac" +
            "e)")]
        [Xunit.TraitAttribute("FeatureTitle", "CreditoAutomaticoECommerce")]
        [Xunit.TraitAttribute("Description", "configurações da t_CODIGO_DESCRICAO para PedidoECommerce_Origem_Grupo (testar o b" +
            "lnPedidoECommerceCreditoOkAutomatico e a exigência de nº do pedido do marketplac" +
            "e)")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ConfiguracoesDaT_CODIGO_DESCRICAOParaPedidoECommerce_Origem_GrupoTestarOBlnPedidoECommerceCreditoOkAutomaticoEAExigenciaDeNºDoPedidoDoMarketplace()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("configurações da t_CODIGO_DESCRICAO para PedidoECommerce_Origem_Grupo (testar o b" +
                    "lnPedidoECommerceCreditoOkAutomatico e a exigência de nº do pedido do marketplac" +
                    "e)", null, tagsOfScenario, argumentsOfScenario);
#line 135
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
 testRunner.Given("Fazer este teste", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
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
                CreditoAutomaticoECommerceFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                CreditoAutomaticoECommerceFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
