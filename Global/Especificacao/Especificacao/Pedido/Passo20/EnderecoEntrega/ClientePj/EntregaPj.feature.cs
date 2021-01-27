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
namespace Especificacao.Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj")]
    public partial class PedidoDeClientePJComEnderecoDeEntregaPJFeature : object, Xunit.IClassFixture<PedidoDeClientePJComEnderecoDeEntregaPJFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "EntregaPj.feature"
#line hidden
        
        public PedidoDeClientePJComEnderecoDeEntregaPJFeature(PedidoDeClientePJComEnderecoDeEntregaPJFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Pedido de cliente PJ com endereço de entrega PJ", null, ProgrammingLanguage.CSharp, new string[] {
                        "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj"});
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
#line 12
#line hidden
#line 13
 testRunner.Given("Ignorar cenário no ambiente \"Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.Ca" +
                    "dastrarPedido\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 14
 testRunner.Given("Pedido base cliente PJ com endereço de entrega PJ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 15
 testRunner.Given(@"No ambiente ""Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido.CadastrarPrepedido"" erro ""Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter valor no campo de Inscrição Estadual!!"" é ""Endereço de entrega: se o Contribuinte ICMS é isento, o campo IE deve ser vazio!""", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 16
 testRunner.Given(@"No ambiente ""Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.CadastrarPedido.CadastrarPedido"" erro ""Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter valor no campo de Inscrição Estadual!!"" é ""Endereço de entrega: se o Contribuinte ICMS é isento, o campo IE deve ser vazio!""", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 17
 testRunner.Given(@"No ambiente ""Ambiente.PrepedidoApi.PrepedidoBusiness.Bll.PrepedidoApiBll.CadastrarPrepedido.CadastrarPrepedidoPrepedidoApi"" mapear erro ""Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter valor no campo de Inscrição Estadual!!"" para ""Endereço de entrega: se o Contribuinte ICMS é isento, o campo IE deve ser vazio!""", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_cnpj_cpf_PJ")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_cnpj_cpf_PJ")]
        public virtual void EndEtg_Cnpj_Cpf_PJ()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_cnpj_cpf_PJ", null, tagsOfScenario, argumentsOfScenario);
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
#line 12
this.FeatureBackground();
#line hidden
#line 20
 testRunner.Given("Pedido base cliente PJ com endereço de entrega PJ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 21
 testRunner.When("Informo \"EndEtg_cnpj_cpf\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 22
 testRunner.Then("Erro \"Endereço de entrega: CNPJ inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 24
 testRunner.Given("Pedido base cliente PJ com endereço de entrega PJ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 25
 testRunner.When("Informo \"EndEtg_cnpj_cpf\" = \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 26
 testRunner.Then("Erro \"Endereço de entrega: CNPJ inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 28
 testRunner.Given("Pedido base cliente PJ com endereço de entrega PJ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 29
 testRunner.When("Informo \"EndEtg_cnpj_cpf\" = \"435.434.870-51\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 30
 testRunner.Then("Erro \"Endereço de entrega: CNPJ inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 32
 testRunner.Given("Pedido base cliente PJ com endereço de entrega PJ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 33
 testRunner.When("Informo \"EndEtg_cnpj_cpf\" = \"40.745.075/0001-00\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 34
 testRunner.Then("Erro \"Endereço de entrega: CNPJ inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 36
 testRunner.Given("Pedido base cliente PJ com endereço de entrega PJ", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 37
 testRunner.When("Informo \"EndEtg_cnpj_cpf\" = \"40.745.075/0001-16\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 38
 testRunner.Then("Sem erro \"Endereço de entrega: CNPJ inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_contribuinte_icms_status_PJ")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_contribuinte_icms_status_PJ")]
        public virtual void EndEtg_Contribuinte_Icms_Status_PJ()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_contribuinte_icms_status_PJ", null, tagsOfScenario, argumentsOfScenario);
#line 40
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
#line 12
this.FeatureBackground();
#line hidden
#line 41
 testRunner.When("Informo \"EndEtg_contribuinte_icms_status\" = \"10\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 42
 testRunner.Then("Erro \"Endereço de entrega: valor de contribuinte do ICMS inválido!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM")]
        public virtual void EndEtg_Contribuinte_Icms_Status_PJCOD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM", null, tagsOfScenario, argumentsOfScenario);
#line 48
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
#line 12
this.FeatureBackground();
#line hidden
#line 49
 testRunner.When("Informo \"EndEtg_contribuinte_icms_status\" = \"COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM" +
                        "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 50
 testRunner.And("Informo \"EndEtg_ie\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 51
 testRunner.Then("Erro \"Endereço de entrega: se o cliente é contribuinte do ICMS a inscrição estadu" +
                        "al deve ser preenchida!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM ISEN")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM ISEN")]
        public virtual void EndEtg_Contribuinte_Icms_Status_PJCOD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIMISEN()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM ISEN", null, tagsOfScenario, argumentsOfScenario);
#line 53
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
#line 12
this.FeatureBackground();
#line hidden
#line 54
 testRunner.When("Informo \"EndEtg_contribuinte_icms_status\" = \"COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM" +
                        "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 55
 testRunner.And("Informo \"EndEtg_ie\" = \"ISEN\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 56
 testRunner.Then("Erro \"Endereço de entrega: se cliente é contribuinte do ICMS, não pode ter o valo" +
                        "r ISENTO no campo de Inscrição Estadual!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO")]
        public virtual void EndEtg_Contribuinte_Icms_Status_PJCOD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO", null, tagsOfScenario, argumentsOfScenario);
#line 58
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
#line 12
this.FeatureBackground();
#line hidden
#line 59
 testRunner.When("Informo \"EndEtg_contribuinte_icms_status\" = \"COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO" +
                        "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 60
 testRunner.And("Informo \"EndEtg_ie\" = \"ISEN\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 61
 testRunner.Then("Erro \"Endereço de entrega: se cliente é não contribuinte do ICMS, não pode ter o " +
                        "valor ISENTO no campo de Inscrição Estadual!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO 2")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO 2")]
        public virtual void EndEtg_Contribuinte_Icms_Status_PJCOD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO2()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO 2", null, tagsOfScenario, argumentsOfScenario);
#line 63
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
#line 12
this.FeatureBackground();
#line hidden
#line 64
 testRunner.When("Informo \"EndEtg_contribuinte_icms_status\" = \"COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO" +
                        "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 66
 testRunner.And("Informo \"EndEtg_ie\" = \"715255502973\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 67
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO opcional")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO opcional")]
        public virtual void EndEtg_Contribuinte_Icms_Status_PJCOD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAOOpcional()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO opcional", null, tagsOfScenario, argumentsOfScenario);
#line 69
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
#line 12
this.FeatureBackground();
#line hidden
#line 70
 testRunner.When("Informo \"EndEtg_contribuinte_icms_status\" = \"COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO" +
                        "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 71
 testRunner.And("Informo \"EndEtg_ie\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 72
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO 1")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO 1")]
        public virtual void EndEtg_Contribuinte_Icms_Status_PJCOD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO1()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO 1", null, tagsOfScenario, argumentsOfScenario);
#line 74
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
#line 12
this.FeatureBackground();
#line hidden
#line 75
 testRunner.When("Informo \"EndEtg_contribuinte_icms_status\" = \"COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISE" +
                        "NTO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 76
 testRunner.And("Informo \"EndEtg_ie\" = \"ISEN\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 77
 testRunner.Then("Erro \"Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter" +
                        " valor no campo de Inscrição Estadual!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO 2")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO 2")]
        public virtual void EndEtg_Contribuinte_Icms_Status_PJCOD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO2()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_contribuinte_icms_status_PJ COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO 2", null, tagsOfScenario, argumentsOfScenario);
#line 79
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
#line 12
this.FeatureBackground();
#line hidden
#line 80
 testRunner.When("Informo \"EndEtg_contribuinte_icms_status\" = \"COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISE" +
                        "NTO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 82
 testRunner.And("Informo \"EndEtg_ie\" = \"715255502973\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 83
 testRunner.Then("Erro \"Endereço de entrega: se cliente é contribuinte do ICMS isento, não pode ter" +
                        " valor no campo de Inscrição Estadual!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="EndEtg_nome")]
        [Xunit.TraitAttribute("FeatureTitle", "Pedido de cliente PJ com endereço de entrega PJ")]
        [Xunit.TraitAttribute("Description", "EndEtg_nome")]
        public virtual void EndEtg_Nome()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("EndEtg_nome", null, tagsOfScenario, argumentsOfScenario);
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
#line 12
this.FeatureBackground();
#line hidden
#line 86
 testRunner.When("Informo \"EndEtg_nome\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 87
 testRunner.Then("Erro \"Endereço de Entrega: Preencha o nome/razão social no endereço de entrega!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                PedidoDeClientePJComEnderecoDeEntregaPJFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                PedidoDeClientePJComEnderecoDeEntregaPJFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
