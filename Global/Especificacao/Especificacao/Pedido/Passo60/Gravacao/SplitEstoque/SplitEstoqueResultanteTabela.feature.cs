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
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo60")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class SplitEstoqueResultanteTabelaFeature : object, Xunit.IClassFixture<SplitEstoqueResultanteTabelaFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.Passo60",
                "GerenciamentoBanco"};
        
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
                        "Especificacao.Pedido.Passo60",
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
#line 8
#line hidden
#line 9
 testRunner.Given("Ignorar cenário no ambiente \"Especificacao.Prepedido.PrepedidoSteps\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 10
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 11
 testRunner.When("Lista de itens com \"1\" itens", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 12
 testRunner.Given("Usar produto \"um\" como fabricante = \"003\", produto = \"003220\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 13
 testRunner.And("Zerar todo o estoque", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="Teste de auto-slipt - magento")]
        [Xunit.TraitAttribute("FeatureTitle", "SplitEstoqueResultanteTabela")]
        [Xunit.TraitAttribute("Description", "Teste de auto-slipt - magento")]
        [Xunit.InlineDataAttribute("1", "10", "40", "40", "1", "0", "SEP", "4903", "10", "30", "0", "Pedido totalmente atendido pelo CD 1", new string[0])]
        [Xunit.InlineDataAttribute("2", "50", "40", "40", "2", "0", "SEP", "4903", "40", "0", "0", "Pedido totalmente atendido pelo CD 1 e 2", new string[0])]
        [Xunit.InlineDataAttribute("2", "50", "40", "40", "2", "1", "SEP", "4003", "10", "30", "0", "", new string[0])]
        [Xunit.InlineDataAttribute("3", "100", "40", "40", "2", "0", "SPL", "4903", "60", "0", "20", "Pedido atendido pelo CD 1 e 2 e sobram mais 20 para o CD 1", new string[0])]
        [Xunit.InlineDataAttribute("3", "100", "40", "40", "2", "1", "SEP", "4003", "40", "0", "0", "", new string[0])]
        [Xunit.InlineDataAttribute("4", "80", "40", "40", "2", "0", "SEP", "4903", "40", "0", "0", "Pedido atendido pelo CD 1 e 2 e zera todo o estoque", new string[0])]
        [Xunit.InlineDataAttribute("4", "80", "40", "40", "2", "1", "SEP", "4003", "40", "0", "0", "", new string[0])]
        [Xunit.InlineDataAttribute("5", "100", "0", "0", "1", "0", "ESP", "4903", "100", "0", "100", "Pedido atendido pelo CD 1, tudo sem presença no estoque", new string[0])]
        [Xunit.InlineDataAttribute("6", "15", "0", "40", "1", "0", "SEP", "4903", "15", "25", "0", "Pedido atendido pelo CD 2", new string[0])]
        [Xunit.InlineDataAttribute("7", "120", "10", "0", "2", "0", "ESP", "4903", "110", "0", "110", "Pedido atendido pelo CD 1 com sem presença no estoque", new string[0])]
        [Xunit.InlineDataAttribute("7", "120", "10", "0", "2", "1", "SEP", "4003", "10", "0", "110", "Pedido atendido pelo CD 1 com sem presença no estoque", new string[0])]
        public virtual void TesteDeAuto_Slipt_Magento(string caso, string qde, string inicial1, string inicial2, string nroPedidos, string pedido, string st_Entrega, string cD, string iqde, string estoque, string spe, string comentarios, string[] exampleTags)
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
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Teste de auto-slipt - magento", null, tagsOfScenario, argumentsOfScenario);
#line 22
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
#line 8
this.FeatureBackground();
#line hidden
#line 24
 testRunner.Given("Ignorar cenário no ambiente \"Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.Cadas" +
                        "trarPedido.CadastrarPedido\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 26
 testRunner.Given(string.Format("Definir saldo estoque = \"{0}\" para produto = \"um\" e id_nfe_emitente = \"4003\"", inicial1), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 28
 testRunner.Given("Tabela \"t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD\" alterar registro id_wms_regra_cd_x_uf_" +
                        "x_pessoa = \"666\" e id_nfe_emitente = \"4003\", campo \"st_inativo\" = \"0\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 29
 testRunner.Given(string.Format("Definir saldo estoque = \"{0}\" para produto = \"um\" e id_nfe_emitente = \"4903\"", inicial2), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 30
 testRunner.When(string.Format("Lista de itens \"0\" informo \"Qtde\" = \"{0}\"", qde), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 31
 testRunner.And("Recalcular totais do pedido", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 32
 testRunner.And("Deixar forma de pagamento consistente", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 33
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 34
 testRunner.Then(string.Format("Gerado {0} pedidos", nroPedidos), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 35
 testRunner.And(string.Format("Pedido gerado \"{0}\", verificar st_entrega = \"{1}\"", pedido, st_Entrega), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 36
 testRunner.And(string.Format("Pedido gerado {0}, verificar id_nfe_emitente = {1} e qde = {2}", pedido, cD, iqde), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 37
 testRunner.And(string.Format("Verificar estoque id_nfe_emitente = {0}, saldo de estoque = {1}", cD, estoque), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 38
 testRunner.And(string.Format("Verificar pedido gerado \"{0}\", saldo de ID_ESTOQUE_SEM_PRESENCA = \"{1}\"", pedido, spe), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
