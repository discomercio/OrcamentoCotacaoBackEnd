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
namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class MagentoCriacaoCliente_Pf_OpcionaisFeature : object, Xunit.IClassFixture<MagentoCriacaoCliente_Pf_OpcionaisFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "CriacaoCliente_Pf_Opcionais.feature"
#line hidden
        
        public MagentoCriacaoCliente_Pf_OpcionaisFeature(MagentoCriacaoCliente_Pf_OpcionaisFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Magento CriacaoCliente_Pf_Opcionais", null, ProgrammingLanguage.CSharp, new string[] {
                        "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente",
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
#line 36
#line hidden
#line 37
 testRunner.Given("Reiniciar banco ao terminar cenário", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 38
 testRunner.And("Limpar tabela \"t_CLIENTE\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="AdicionarDependencia")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf_Opcionais")]
        [Xunit.TraitAttribute("Description", "AdicionarDependencia")]
        [Xunit.TraitAttribute("Category", "ListaDependencias")]
        public virtual void AdicionarDependencia()
        {
            string[] tagsOfScenario = new string[] {
                    "ListaDependencias"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AdicionarDependencia", null, tagsOfScenario, argumentsOfScenario);
#line 41
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
#line 36
this.FeatureBackground();
#line hidden
#line 42
 testRunner.Given("AdicionarDependencia ambiente = \"Ambiente.ApiMagento.PedidoMagento.CadastrarPedid" +
                        "o.CadastrarPedidoListaDependencias\", especificacao = \"Ambiente.ApiMagento.Pedido" +
                        "Magento.CadastrarPedido.CriacaoCliente.CriacaoCliente_Pf_Opcionais\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="comprovar que salva todos os campos")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf_Opcionais")]
        [Xunit.TraitAttribute("Description", "comprovar que salva todos os campos")]
        public virtual void ComprovarQueSalvaTodosOsCampos()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("comprovar que salva todos os campos", null, tagsOfScenario, argumentsOfScenario);
#line 45
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
#line 36
this.FeatureBackground();
#line hidden
#line 46
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 47
 testRunner.When("Informo \"OutroEndereco\" = \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 49
 testRunner.When("Informo \"EndEtg_tipo_pessoa\" = \"PF\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 50
 testRunner.When("Informo \"cnpj_cpf\" = \"29756194804\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 53
 testRunner.When("Informo \"EndEtg_endereco_complemento\" = \"compl\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 54
 testRunner.When("Informo \"EndEtg_email\" = \"email@teste.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 55
 testRunner.When("Informo \"EndEtg_email_xml\" = \"emailxml@teste.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 56
 testRunner.When("Informo \"EndEtg_ddd_res\" = \"12\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 57
 testRunner.When("Informo \"EndEtg_tel_res\" = \"12345678\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 58
 testRunner.When("Informo \"EndEtg_ddd_com\" = \"34\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 59
 testRunner.When("Informo \"EndEtg_tel_com\" = \"34567890\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 60
 testRunner.When("Informo \"EndEtg_ramal_com\" = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 61
 testRunner.When("Informo \"EndEtg_ddd_cel\" = \"45\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 62
 testRunner.When("Informo \"EndEtg_tel_cel\" = \"56789012\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 63
 testRunner.When("Informo \"EndEtg_ddd_com_2\" = \"56\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 64
 testRunner.When("Informo \"EndEtg_tel_com_2\" = \"56789012\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 65
 testRunner.When("Informo \"EndEtg_ramal_com_2\" = \"7\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 67
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 69
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"endereco_complemento\" = \"compl\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 70
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"email\" = \"email@teste.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 71
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"email_xml\" = \"emailxml@teste.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 72
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ddd_res\" = \"12\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 73
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tel_res\" = \"12345678\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 74
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ddd_com\" = \"34\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 75
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tel_com\" = \"34567890\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 76
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ramal_com\" = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 77
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ddd_cel\" = \"45\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 78
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tel_cel\" = \"56789012\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 79
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ddd_com_2\" = \"56\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 80
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tel_com_2\" = \"56789012\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 81
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ramal_com_2\" = \"7\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="validar emails")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf_Opcionais")]
        [Xunit.TraitAttribute("Description", "validar emails")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ValidarEmails()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("validar emails", null, tagsOfScenario, argumentsOfScenario);
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
#line 36
this.FeatureBackground();
#line hidden
#line 86
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 87
 testRunner.When("Informo \"OutroEndereco\" = \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 88
 testRunner.When("Informo \"EndEtg_email\" = \"email sem arroba\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 89
 testRunner.Then("Erro \"e-mail inválido\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 91
 testRunner.Given("Limpar tabela \"t_CLIENTE\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 92
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 93
 testRunner.When("Informo \"EndEtg_email_xml\" = \"email sem arroba\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 94
 testRunner.Then("Erro \"e-mail inválido\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="validar telefones - especificação")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf_Opcionais")]
        [Xunit.TraitAttribute("Description", "validar telefones - especificação")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ValidarTelefones_Especificacao()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("validar telefones - especificação", null, tagsOfScenario, argumentsOfScenario);
#line 97
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
#line 36
this.FeatureBackground();
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="validar telefones")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf_Opcionais")]
        [Xunit.TraitAttribute("Description", "validar telefones")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ValidarTelefones()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("validar telefones", null, tagsOfScenario, argumentsOfScenario);
#line 113
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
#line 36
this.FeatureBackground();
#line hidden
#line 114
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 115
 testRunner.Given("fazer este cenário", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
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
                MagentoCriacaoCliente_Pf_OpcionaisFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                MagentoCriacaoCliente_Pf_OpcionaisFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
