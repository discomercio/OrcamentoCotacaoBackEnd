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
    [Xunit.TraitAttribute("Category", "ignore")]
    public partial class ValidacoesDoPedidoNovoFeature : object, Xunit.IClassFixture<ValidacoesDoPedidoNovoFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "ignore"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "PedidoNovo.feature"
#line hidden
        
        public ValidacoesDoPedidoNovoFeature(ValidacoesDoPedidoNovoFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Validações do PedidoNovo", null, ProgrammingLanguage.CSharp, new string[] {
                        "ignore"});
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
        
        [Xunit.SkippableFactAttribute(DisplayName="Configuração")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "Configuração")]
        public virtual void Configuracao()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Configuração", null, tagsOfScenario, argumentsOfScenario);
#line 4
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
 testRunner.Given("Nome deste item \"Especificacao.Pedido.Passo30.PedidoNovo\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 6
 testRunner.Given("Implementado em \"Especificacao.Pedido.Pedido\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Validar CEP")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "Validar CEP")]
        public virtual void ValidarCEP()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Validar CEP", @"	if Trim("""" & r_cliente.cep) <> """" then
		if Len(retorna_so_digitos(Trim("""" & r_cliente.cep))) < 8 then
			alerta=texto_add_br(alerta)
			alerta=alerta & ""O CEP do cadastro do cliente está incompleto (CEP: "" & Trim("""" & r_cliente.cep) & "")""
			end if
		end if

	if rb_end_entrega = ""S"" then
		if EndEtg_cep <> """" then
			if Len(retorna_so_digitos(EndEtg_cep)) < 8 then
				alerta=texto_add_br(alerta)
				alerta=alerta & ""O CEP do endereço de entrega está incompleto (CEP: "" & EndEtg_cep & "")""
				end if
			end if
		end if", tagsOfScenario, argumentsOfScenario);
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
#line 25
testRunner.Given("fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ddd_res")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "ddd_res")]
        public virtual void Ddd_Res()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ddd_res", "\t\'\tDDD VÁLIDO?\r\n\t\tif Not ddd_ok(r_cliente.ddd_res) then\r\n\t\t\tif alerta <> \"\" then " +
                    "alerta = alerta & \"<br><br>\" & String(80,\"=\") & \"<br><br>\"\r\n\t\t\talerta = alerta &" +
                    " \"DDD do telefone residencial é inválido!!\"\r\n\t\t\tend if", tagsOfScenario, argumentsOfScenario);
#line 28
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
#line 34
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 35
 testRunner.And("Informo \"ddd_res\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 36
 testRunner.Then("Erro \"DDD do telefone residencial é inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 38
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 39
 testRunner.And("Informo \"ddd_res\" = \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 40
 testRunner.Then("Erro \"DDD do telefone residencial é inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 42
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 43
 testRunner.And("Informo \"ddd_res\" = \"12\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 44
 testRunner.Then("Sem erro \"DDD do telefone residencial é inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="ddd_com")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "ddd_com")]
        public virtual void Ddd_Com()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ddd_com", "\t\tif Not ddd_ok(r_cliente.ddd_com) then\r\n\t\t\tif alerta <> \"\" then alerta = alerta " +
                    "& \"<br><br>\" & String(80,\"=\") & \"<br><br>\"\r\n\t\t\talerta = alerta & \"DDD do telefon" +
                    "e comercial é inválido!!\"\r\n\t\t\tend if", tagsOfScenario, argumentsOfScenario);
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
#line 52
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 53
 testRunner.And("Informo \"ddd_com\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 54
 testRunner.Then("Erro \"DDD do telefone comercial é inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 56
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 57
 testRunner.And("Informo \"ddd_com\" = \"123\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 58
 testRunner.Then("Erro \"DDD do telefone comercial é inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 60
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 61
 testRunner.And("Informo \"ddd_com\" = \"12\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 62
 testRunner.Then("Sem erro \"DDD do telefone comercial é inválido!!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="I.E. É VÁLIDA?")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "I.E. É VÁLIDA?")]
        public virtual void I_E_EVALIDA()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I.E. É VÁLIDA?", @"		if (r_cliente.contribuinte_icms_status = COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) then
			if Not isInscricaoEstadualValida(r_cliente.ie, r_cliente.uf) then
				if alerta <> """" then alerta = alerta & ""<br><br>"" & String(80,""="") & ""<br><br>""
				alerta=alerta & ""Corrija a IE (Inscrição Estadual) com um número válido!!"" & _
						""<br>"" & ""Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE.""
				end if
			end if", tagsOfScenario, argumentsOfScenario);
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
#line 72
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?")]
        public virtual void MUNICIPIODEACORDOCTABELADOIBGE()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("MUNICÍPIO DE ACORDO C/ TABELA DO IBGE?", "\t\tif Not consiste_municipio_IBGE_ok(r_cliente.cidade, r_cliente.uf, s_lista_suger" +
                    "ida_municipios, msg_erro) then", tagsOfScenario, argumentsOfScenario);
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
#line 76
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="MUNICÍPIO DE ENTREGA DE ACORDO C/ TABELA DO IBGE?")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "MUNICÍPIO DE ENTREGA DE ACORDO C/ TABELA DO IBGE?")]
        public virtual void MUNICIPIODEENTREGADEACORDOCTABELADOIBGE()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("MUNICÍPIO DE ENTREGA DE ACORDO C/ TABELA DO IBGE?", "\t\tif rb_end_entrega = \"S\" then\r\n\t\t\'\tMUNICÍPIO DE ACORDO C/ TABELA DO IBGE?\r\n\t\t\tif" +
                    " Not consiste_municipio_IBGE_ok(EndEtg_cidade, EndEtg_uf, s_lista_sugerida_munic" +
                    "ipios, msg_erro) then", tagsOfScenario, argumentsOfScenario);
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
#line 83
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="\'VERIFICA SE O MESMO CÓDIGO FOI DIGITADO REPETIDO EM VÁRIAS LINHAS")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "\'VERIFICA SE O MESMO CÓDIGO FOI DIGITADO REPETIDO EM VÁRIAS LINHAS")]
        public virtual void VERIFICASEOMESMOCODIGOFOIDIGITADOREPETIDOEMVARIASLINHAS()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("\'VERIFICA SE O MESMO CÓDIGO FOI DIGITADO REPETIDO EM VÁRIAS LINHAS", "alerta=alerta & \"Produto \" & vDuplic(i).produto & \" do fabricante \" & vDuplic(i)." +
                    "fabricante & \": linha \" & renumera_com_base1(LBound(vDuplic),i) & \" repete o mes" +
                    "mo produto da linha \" & renumera_com_base1(LBound(vDuplic),j)", tagsOfScenario, argumentsOfScenario);
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
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="alert(\"Não há produtos na lista!!\");")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "alert(\"Não há produtos na lista!!\");")]
        public virtual void AlertNaoHaProdutosNaLista()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("alert(\"Não há produtos na lista!!\");", null, tagsOfScenario, argumentsOfScenario);
#line 89
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
#line 90
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Especifique a loja que fez a indicação")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "Especifique a loja que fez a indicação")]
        public virtual void EspecifiqueALojaQueFezAIndicacao()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Especifique a loja que fez a indicação", null, tagsOfScenario, argumentsOfScenario);
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
#line 99
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Selecione o \"indicador\"")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "Selecione o \"indicador\"")]
        public virtual void SelecioneOIndicador()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Selecione o \"indicador\"", null, tagsOfScenario, argumentsOfScenario);
#line 101
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
#line 102
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Somente na tela: avisar O indicador selecionado é diferente do indicador que cons" +
            "ta no cadastro deste cliente")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "Somente na tela: avisar O indicador selecionado é diferente do indicador que cons" +
            "ta no cadastro deste cliente")]
        public virtual void SomenteNaTelaAvisarOIndicadorSelecionadoEDiferenteDoIndicadorQueConstaNoCadastroDesteCliente()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Somente na tela: avisar O indicador selecionado é diferente do indicador que cons" +
                    "ta no cadastro deste cliente", null, tagsOfScenario, argumentsOfScenario);
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
#line 150
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Não foi informada a forma de pagamento!")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "Não foi informada a forma de pagamento!")]
        public virtual void NaoFoiInformadaAFormaDePagamento()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Não foi informada a forma de pagamento!", null, tagsOfScenario, argumentsOfScenario);
#line 152
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
#line 153
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 154
 testRunner.And("Informo \"custoFinancFornecTipoParcelamento\" = \"\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 155
 testRunner.Then("Erro \"Não foi informada a forma de pagamento!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 157
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 158
 testRunner.And("Informo \"custoFinancFornecTipoParcelamento\" = \"COD_CUSTO_FINANC_FORNEC_TIPO_PARCE" +
                        "LAMENTO__COM_ENTRADA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 159
 testRunner.Then("Sem Erro \"Não foi informada a forma de pagamento!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Não foi informada a quantidade de parcelas da forma de pagamento!")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "Não foi informada a quantidade de parcelas da forma de pagamento!")]
        public virtual void NaoFoiInformadaAQuantidadeDeParcelasDaFormaDePagamento()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Não foi informada a quantidade de parcelas da forma de pagamento!", null, tagsOfScenario, argumentsOfScenario);
#line 161
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
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 163
 testRunner.And("Informo \"custoFinancFornecTipoParcelamento\" = \"COD_CUSTO_FINANC_FORNEC_TIPO_PARCE" +
                        "LAMENTO__COM_ENTRADA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 164
 testRunner.And("Informo \"custoFinancFornecQtdeParcelas\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 165
 testRunner.Then("Erro \"Não foi informada a quantidade de parcelas da forma de pagamento!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 167
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 168
 testRunner.And("Informo \"custoFinancFornecTipoParcelamento\" = \"COD_CUSTO_FINANC_FORNEC_TIPO_PARCE" +
                        "LAMENTO__COM_ENTRADA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 169
 testRunner.And("Informo \"custoFinancFornecQtdeParcelas\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 170
 testRunner.Then("Sem Erro \"Não foi informada a quantidade de parcelas da forma de pagamento!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 172
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 173
 testRunner.And("Informo \"custoFinancFornecTipoParcelamento\" = \"COD_CUSTO_FINANC_FORNEC_TIPO_PARCE" +
                        "LAMENTO__SEM_ENTRADA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 174
 testRunner.And("Informo \"custoFinancFornecQtdeParcelas\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 175
 testRunner.Then("Erro \"Não foi informada a quantidade de parcelas da forma de pagamento!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 177
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 178
 testRunner.And("Informo \"custoFinancFornecTipoParcelamento\" = \"COD_CUSTO_FINANC_FORNEC_TIPO_PARCE" +
                        "LAMENTO__SEM_ENTRADA\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 179
 testRunner.And("Informo \"custoFinancFornecQtdeParcelas\" = \"1\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 180
 testRunner.Then("Sem Erro \"Não foi informada a quantidade de parcelas da forma de pagamento!\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="A forma de pagamento não está disponível para o(s) produto(s)")]
        [Xunit.TraitAttribute("FeatureTitle", "Validações do PedidoNovo")]
        [Xunit.TraitAttribute("Description", "A forma de pagamento não está disponível para o(s) produto(s)")]
        public virtual void AFormaDePagamentoNaoEstaDisponivelParaOSProdutoS()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("A forma de pagamento não está disponível para o(s) produto(s)", null, tagsOfScenario, argumentsOfScenario);
#line 182
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
#line 188
 testRunner.When("Fazer esta validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
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
                ValidacoesDoPedidoNovoFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ValidacoesDoPedidoNovoFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
