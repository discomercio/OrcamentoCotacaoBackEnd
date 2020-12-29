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
    public partial class MagentoCriacaoCliente_PfFeature : object, Xunit.IClassFixture<MagentoCriacaoCliente_PfFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "CriacaoCliente_Pf.feature"
#line hidden
        
        public MagentoCriacaoCliente_PfFeature(MagentoCriacaoCliente_PfFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Magento CriacaoCliente_Pf", null, ProgrammingLanguage.CSharp, new string[] {
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
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
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
                        "Magento.CadastrarPedido.CriacaoCliente.CriacaoCliente_Pf\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Caminho feliz")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "Caminho feliz")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void CaminhoFeliz()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Caminho feliz", null, tagsOfScenario, argumentsOfScenario);
#line 46
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
#line 47
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 48
 testRunner.And("Limpar endereço de entrega", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 49
 testRunner.When("Informo \"OutroEndereco\" = \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 50
 testRunner.When("Informo \"EndEtg_endereco\" = \"Rua Professor Fábio Fanucchi\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 51
 testRunner.When("Informo \"EndEtg_endereco_numero\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 52
 testRunner.When("Informo \"EndEtg_bairro\" = \"Jardim São Paulo(Zona Norte)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 53
 testRunner.When("Informo \"EndEtg_cidade\" = \"São Paulo\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 54
 testRunner.When("Informo \"EndEtg_uf\" = \"SP\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 55
 testRunner.When("Informo \"EndEtg_cep\" = \"02045080\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 56
 testRunner.When("Informo \"EndEtg_nome\" = \"Vivian\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 57
 testRunner.When("Informo \"EndEtg_tipo_pessoa\" = \"PF\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 58
 testRunner.When("Informo \"EndEtg_cnpj_cpf\" = \"29756194804\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 59
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 60
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"endereco\" = \"Rua Professor Fábio Fanucchi\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 61
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"endereco_numero\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 62
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"bairro\" = \"Jardim São Paulo(Zona Norte)\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 63
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"cidade\" = \"São Paulo\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 64
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"uf\" = \"SP\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 65
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"cep\" = \"02045080\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 66
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"nome\" = \"Vivian\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 67
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tipo_pessoa\" = \"PF\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 68
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"cnpj_cpf\" = \"29756194804\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 69
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"contribuinte_icms_status\" = \"COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 70
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"produtor_rural_status\" = \"COD_ST_CLIENTE_PRODUTOR_RURAL_NAO\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Exigimos endereço de entrega para PF")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "Exigimos endereço de entrega para PF")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ExigimosEnderecoDeEntregaParaPF()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Exigimos endereço de entrega para PF", null, tagsOfScenario, argumentsOfScenario);
#line 73
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
#line 76
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 77
 testRunner.When("Informo \"OutroEndereco\" = \"false\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 78
 testRunner.Then("Erro \"Obrigatório informar um endereço de entrega na API Magento para cliente PF." +
                        "\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RUR" +
            "AL_NAO e Endereco_contribuinte_icms_status = NAO")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RUR" +
            "AL_NAO e Endereco_contribuinte_icms_status = NAO")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void SeForPFAssumimosEndereco_Produtor_Rural_StatusCOD_ST_CLIENTE_PRODUTOR_RURAL_NAOEEndereco_Contribuinte_Icms_StatusNAO()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("se for PF, assumimos Endereco_produtor_rural_status = COD_ST_CLIENTE_PRODUTOR_RUR" +
                    "AL_NAO e Endereco_contribuinte_icms_status = NAO", null, tagsOfScenario, argumentsOfScenario);
#line 81
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
#line 82
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 83
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 85
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"produtor_rural_status\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 87
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"contribuinte_icms_status\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 88
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ie\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="usar o flag para indicar que esse t_cliente foi criado pelo magento (o sistema_re" +
            "sponsavel_cadastro)")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "usar o flag para indicar que esse t_cliente foi criado pelo magento (o sistema_re" +
            "sponsavel_cadastro)")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void UsarOFlagParaIndicarQueEsseT_ClienteFoiCriadoPeloMagentoOSistema_Responsavel_Cadastro()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("usar o flag para indicar que esse t_cliente foi criado pelo magento (o sistema_re" +
                    "sponsavel_cadastro)", null, tagsOfScenario, argumentsOfScenario);
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
#line 36
this.FeatureBackground();
#line hidden
#line 93
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 94
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 96
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"sistema_responsavel_cadastro\" = \"4\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="falta: sexo e data de nascimento - vao ficar em branco")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "falta: sexo e data de nascimento - vao ficar em branco")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void FaltaSexoEDataDeNascimento_VaoFicarEmBranco()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("falta: sexo e data de nascimento - vao ficar em branco", null, tagsOfScenario, argumentsOfScenario);
#line 99
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
#line 100
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 101
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 103
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"nascimento\" = \"null\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 104
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"sexo\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="comprovar que salva todos os campos")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "comprovar que salva todos os campos")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ComprovarQueSalvaTodosOsCampos()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("comprovar que salva todos os campos", null, tagsOfScenario, argumentsOfScenario);
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
#line 36
this.FeatureBackground();
#line hidden
#line 108
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 109
 testRunner.When("Informo \"OutroEndereco\" = \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 111
 testRunner.When("Informo \"EndEtg_tipo_pessoa\" = \"PF\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 112
 testRunner.When("Informo \"EndEtg_cnpj_cpf\" = \"29756194804\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 116
 testRunner.When("Informo \"EndEtg_endereco_complemento\" = \"compl\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 117
 testRunner.When("Informo \"EndEtg_email\" = \"email@teste.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 118
 testRunner.When("Informo \"EndEtg_email_xml\" = \"emailxml@teste.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 119
 testRunner.When("Informo \"EndEtg_ddd_res\" = \"12\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 120
 testRunner.When("Informo \"EndEtg_tel_res\" = \"12345678\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 121
 testRunner.When("Informo \"EndEtg_ddd_com\" = \"34\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 122
 testRunner.When("Informo \"EndEtg_tel_com\" = \"34567890\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 123
 testRunner.When("Informo \"EndEtg_ramal_com\" = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 124
 testRunner.When("Informo \"EndEtg_ddd_cel\" = \"45\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 125
 testRunner.When("Informo \"EndEtg_tel_cel\" = \"56789012\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 126
 testRunner.When("Informo \"EndEtg_ddd_com_2\" = \"56\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 127
 testRunner.When("Informo \"EndEtg_tel_com_2\" = \"56789012\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 128
 testRunner.When("Informo \"EndEtg_ramal_com_2\" = \"7\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 130
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 132
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"endereco_complemento\" = \"compl\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 133
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"email\" = \"email@teste.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 134
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"email_xml\" = \"emailxml@teste.com\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 135
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ddd_res\" = \"12\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 136
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tel_res\" = \"12345678\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 137
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ddd_com\" = \"34\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 138
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tel_com\" = \"34567890\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 139
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ramal_com\" = \"5\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 140
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ddd_cel\" = \"45\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 141
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tel_cel\" = \"56789012\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 142
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ddd_com_2\" = \"56\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 143
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"tel_com_2\" = \"56789012\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 144
 testRunner.And("Tabela \"t_CLIENTE\" registro com campo \"cnpj_cpf\" = \"29756194804\", verificar campo" +
                        " \"ramal_com_2\" = \"7\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="validar emails")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "validar emails")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ValidarEmails()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("validar emails", null, tagsOfScenario, argumentsOfScenario);
#line 148
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
#line 149
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 150
 testRunner.When("Informo \"OutroEndereco\" = \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 151
 testRunner.When("Informo \"EndEtg_email\" = \"email sem arroba\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 152
 testRunner.Then("Erro \"e-mail inválido\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 154
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 155
 testRunner.When("Informo \"EndEtg_email_xml\" = \"email sem arroba\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 156
 testRunner.Then("Erro \"e-mail inválido\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="validar telefones")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "validar telefones")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void ValidarTelefones()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("validar telefones", null, tagsOfScenario, argumentsOfScenario);
#line 159
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
#line 160
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 161
 testRunner.When("Informo \"OutroEndereco\" = \"true\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 162
 testRunner.Given("fazer este cenário", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Não atualizar dados se o cliente já existir")]
        [Xunit.TraitAttribute("FeatureTitle", "Magento CriacaoCliente_Pf")]
        [Xunit.TraitAttribute("Description", "Não atualizar dados se o cliente já existir")]
        [Xunit.TraitAttribute("Category", "ignore")]
        public virtual void NaoAtualizarDadosSeOClienteJaExistir()
        {
            string[] tagsOfScenario = new string[] {
                    "ignore"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Não atualizar dados se o cliente já existir", null, tagsOfScenario, argumentsOfScenario);
#line 165
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
#line 166
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
                MagentoCriacaoCliente_PfFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                MagentoCriacaoCliente_PfFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
