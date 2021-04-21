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
namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "ignore")]
    [Xunit.TraitAttribute("Category", "Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus")]
    public partial class AlterarMagentoPedidoStatusFeature : object, Xunit.IClassFixture<AlterarMagentoPedidoStatusFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "ignore",
                "Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "AlterarMagentoPedidoStatus.feature"
#line hidden
        
        public AlterarMagentoPedidoStatusFeature(AlterarMagentoPedidoStatusFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "AlterarMagentoPedidoStatus", @"	- statuspedido:
	  gravar campo na t_pedido
	  na transição de status, incluir um bloco de notas no pedido
	  e também um log
	  campo controlado somente pelo pedido pai
	- status: o magento pode passar um pedido de aprovado para rejeitado?
	  resposta: não
	- Status do pedido: quais os status possíveis?
	  (inicialmente: pedido aprovado ou não aprovado)
      se o pedido estiver com status = cancelado ou entregue, não podemos mexer no pedido. Se acontecer, 
		mandar email para karina e retornar erro.
      possíveis:
      - aprovação pendente -> analise_credito = esperando aprovação pelo magento (o novo status)
      - aprovado -> quando for para aprovado, passa para analise_credito = credito_ok se o marketplace permitir (ver o fluxo de criação do pedido)
      - rejeitado -> cancelamos automaticamente o pedido, ter um flag por marketplace para habilitar o 
	     cancelamento automático (definr o flag em t_codigo_descricao)
      Talvez: colocar um bloco na tela inicial do verdinho listando os pedidos parados em aprovação pendente há mais de X dias.
      ter mais um status de analise de crédito: esperando aprovação pelo magento.", ProgrammingLanguage.CSharp, new string[] {
                        "ignore",
                        "Ambiente.ApiMagento.PedidoMagento.AlterarMagentoPedidoStatus"});
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
        
        [Xunit.SkippableFactAttribute(DisplayName="AlterarMagentoPedidoStatus - aprovacao pendente")]
        [Xunit.TraitAttribute("FeatureTitle", "AlterarMagentoPedidoStatus")]
        [Xunit.TraitAttribute("Description", "AlterarMagentoPedidoStatus - aprovacao pendente")]
        public virtual void AlterarMagentoPedidoStatus_AprovacaoPendente()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AlterarMagentoPedidoStatus - aprovacao pendente", "\taprovação pendente -> analise_credito = esperando aprovação pelo magento (o novo" +
                    " status)", tagsOfScenario, argumentsOfScenario);
#line 23
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
 testRunner.Then("Sem nenhum Erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="AlterarMagentoPedidoStatus - aprovado")]
        [Xunit.TraitAttribute("FeatureTitle", "AlterarMagentoPedidoStatus")]
        [Xunit.TraitAttribute("Description", "AlterarMagentoPedidoStatus - aprovado")]
        public virtual void AlterarMagentoPedidoStatus_Aprovado()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AlterarMagentoPedidoStatus - aprovado", "\taprovado -> quando for para aprovado, \r\n\tpassa para analise_credito = credito_ok" +
                    " se o marketplace permitir (ver o fluxo de criação do pedido)", tagsOfScenario, argumentsOfScenario);
#line 27
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
#line 30
 testRunner.Then("Sem nenhum Erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="AlterarMagentoPedidoStatus - rejeitado")]
        [Xunit.TraitAttribute("FeatureTitle", "AlterarMagentoPedidoStatus")]
        [Xunit.TraitAttribute("Description", "AlterarMagentoPedidoStatus - rejeitado")]
        public virtual void AlterarMagentoPedidoStatus_Rejeitado()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AlterarMagentoPedidoStatus - rejeitado", "\trejeitado -> cancelamos automaticamente o pedido, ter um flag por marketplace pa" +
                    "ra habilitar o \r\n\tcancelamento automático (definr o flag em t_codigo_descricao)", tagsOfScenario, argumentsOfScenario);
#line 32
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
#line 35
 testRunner.Then("Sem nenhum Erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="AlterarMagentoPedidoStatus - pedido cancelado")]
        [Xunit.TraitAttribute("FeatureTitle", "AlterarMagentoPedidoStatus")]
        [Xunit.TraitAttribute("Description", "AlterarMagentoPedidoStatus - pedido cancelado")]
        public virtual void AlterarMagentoPedidoStatus_PedidoCancelado()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AlterarMagentoPedidoStatus - pedido cancelado", "\tse o pedido estiver com status = cancelado, não podemos mexer no pedido. Se acon" +
                    "tecer, \r\n\tmandar email para karina e retornar erro.", tagsOfScenario, argumentsOfScenario);
#line 37
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
#line 40
 testRunner.Then("Sem nenhum Erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="AlterarMagentoPedidoStatus - pedido entregue")]
        [Xunit.TraitAttribute("FeatureTitle", "AlterarMagentoPedidoStatus")]
        [Xunit.TraitAttribute("Description", "AlterarMagentoPedidoStatus - pedido entregue")]
        public virtual void AlterarMagentoPedidoStatus_PedidoEntregue()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AlterarMagentoPedidoStatus - pedido entregue", "\tse o pedido estiver com status = entregue, não podemos mexer no pedido. Se acont" +
                    "ecer, \r\n\tmandar email para karina e retornar erro.", tagsOfScenario, argumentsOfScenario);
#line 42
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
#line 45
 testRunner.Then("Sem nenhum Erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="AlterarMagentoPedidoStatus - verificar bloco de notas no pedido")]
        [Xunit.TraitAttribute("FeatureTitle", "AlterarMagentoPedidoStatus")]
        [Xunit.TraitAttribute("Description", "AlterarMagentoPedidoStatus - verificar bloco de notas no pedido")]
        public virtual void AlterarMagentoPedidoStatus_VerificarBlocoDeNotasNoPedido()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AlterarMagentoPedidoStatus - verificar bloco de notas no pedido", "\tna transição de status, incluir um bloco de notas no pedido", tagsOfScenario, argumentsOfScenario);
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
#line 49
 testRunner.Then("Sem nenhum Erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="AlterarMagentoPedidoStatus - verificar log")]
        [Xunit.TraitAttribute("FeatureTitle", "AlterarMagentoPedidoStatus")]
        [Xunit.TraitAttribute("Description", "AlterarMagentoPedidoStatus - verificar log")]
        public virtual void AlterarMagentoPedidoStatus_VerificarLog()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("AlterarMagentoPedidoStatus - verificar log", "\tna transição de status, insere log", tagsOfScenario, argumentsOfScenario);
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
#line 54
 testRunner.Then("Sem nenhum Erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                AlterarMagentoPedidoStatusFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                AlterarMagentoPedidoStatusFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
