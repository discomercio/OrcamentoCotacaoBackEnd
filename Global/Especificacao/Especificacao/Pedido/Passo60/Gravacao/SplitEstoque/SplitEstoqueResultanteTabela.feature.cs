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
namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.SplitEstoque
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "ignore")]
    public partial class SplitEstoqueResultanteTabelaFeature : object, Xunit.IClassFixture<SplitEstoqueResultanteTabelaFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "ignore"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "SplitEstoqueResultanteTabela.feature"
#line hidden
        
        public SplitEstoqueResultanteTabelaFeature(SplitEstoqueResultanteTabelaFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "SplitEstoqueResultanteTabela", null, ProgrammingLanguage.CSharp, new string[] {
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
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
#line 7
 testRunner.Given("Pedido base sem itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 8
 testRunner.And("Usando fabricante = \"001\", produto = \"001000\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 9
 testRunner.And("Zerar todo o estoque", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="Teste de auto-slipt")]
        [Xunit.TraitAttribute("FeatureTitle", "SplitEstoqueResultanteTabela")]
        [Xunit.TraitAttribute("Description", "Teste de auto-slipt")]
        [Xunit.InlineDataAttribute("1", "10", "40", "40", "1", "0", "SEP", "4003", "10", "30", "0", "Pedido totalmente atendido pelo CD 1", new string[0])]
        [Xunit.InlineDataAttribute("2", "50", "40", "40", "2", "0", "SEP", "4003", "40", "0", "0", "Pedido totalmente atendido pelo CD 1 e 2", new string[0])]
        [Xunit.InlineDataAttribute("2", "50", "40", "40", "2", "1", "SEP", "4903", "10", "30", "0", "", new string[0])]
        [Xunit.InlineDataAttribute("3", "100", "40", "40", "2", "0", "SPL", "4003", "60", "0", "20", "Pedido atendido pelo CD 1 e 2 e sobram mais 20 para o CD 1", new string[0])]
        [Xunit.InlineDataAttribute("3", "100", "40", "40", "2", "1", "SEP", "4903", "40", "0", "0", "", new string[0])]
        [Xunit.InlineDataAttribute("4", "80", "40", "40", "2", "0", "SEP", "4003", "40", "0", "0", "Pedido atendido pelo CD 1 e 2 e zera todo o estoque", new string[0])]
        [Xunit.InlineDataAttribute("4", "80", "40", "40", "2", "1", "SEP", "4903", "40", "0", "0", "", new string[0])]
        [Xunit.InlineDataAttribute("5", "100", "0", "0", "1", "0", "ESP", "4003", "100", "0", "100", "Pedido atendido pelo CD 1, tudo sem presença no estoque", new string[0])]
        [Xunit.InlineDataAttribute("6", "15", "0", "40", "1", "0", "SEP", "4903", "15", "25", "0", "Pedido atendido pelo CD 2", new string[0])]
        [Xunit.InlineDataAttribute("7", "120", "10", "0", "1", "0", "SPL", "4003", "120", "0", "110", "Pedido atendido pelo CD 1 com sem presença no estoque", new string[0])]
        public virtual void TesteDeAuto_Slipt(string caso, string qde, string inicial1, string inicial2, string nroPedidos, string pedido, string st_Entrega, string cD, string iqde, string estoque, string spe, string comentarios, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("Caso", caso);
            argumentsOfScenario.Add("qde", qde);
            argumentsOfScenario.Add("inicial1", inicial1);
            argumentsOfScenario.Add("inicial2", inicial2);
            argumentsOfScenario.Add("NroPedidos", nroPedidos);
            argumentsOfScenario.Add("pedido", pedido);
            argumentsOfScenario.Add("st_entrega", st_Entrega);
            argumentsOfScenario.Add("CD", cD);
            argumentsOfScenario.Add("iqde", iqde);
            argumentsOfScenario.Add("estoque", estoque);
            argumentsOfScenario.Add("spe", spe);
            argumentsOfScenario.Add("Comentários", comentarios);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teste de auto-slipt", null, tagsOfScenario, argumentsOfScenario);
#line 11
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
#line 14
 testRunner.Given(string.Format("Definir estoque id_nfe_emitente = \"4003\", saldo de estoque = {0}", inicial1), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 15
 testRunner.And(string.Format("Definir estoque id_nfe_emitente = \"4903\", saldo de estoque = {0}", inicial2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 17
 testRunner.And("Pedido base sem itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 18
 testRunner.And(string.Format("Criar novo item com qde = {0}", qde), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 20
 testRunner.When("Cadastrar pedido", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 21
 testRunner.Then(string.Format("Gerado {0} pedidos", nroPedidos), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 23
 testRunner.And(string.Format("Pedido gerado {0}, verificar st_entrega = {1}", pedido, st_Entrega), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 24
 testRunner.And(string.Format("Pedido gerado {0}, verificar id_nfe_emitente = {1} e qde = {2}", pedido, cD, iqde), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 26
 testRunner.And(string.Format("Verificar estoque id_nfe_emitente = {0}, saldo de estoque = {1}", cD, estoque), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 27
 testRunner.And(string.Format("Verificar estoque id_nfe_emitente = {0}, saldo de ID_ESTOQUE_SEM_PRESENCA = {1}", cD, spe), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
                SplitEstoqueResultanteTabelaFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                SplitEstoqueResultanteTabelaFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion