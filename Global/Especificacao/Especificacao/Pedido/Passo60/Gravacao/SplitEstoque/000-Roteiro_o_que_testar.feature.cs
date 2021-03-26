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
    public partial class _000_Roteiro_O_Que_TestarFeature : object, Xunit.IClassFixture<_000_Roteiro_O_Que_TestarFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "ignore"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "000-Roteiro_o_que_testar.feature"
#line hidden
        
        public _000_Roteiro_O_Que_TestarFeature(_000_Roteiro_O_Que_TestarFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "000-Roteiro_o_que_testar", "Roteiro de cada teste:\r\n\r\n1 - Escrever regra de consumo de estoque\r\n2 - Definir e" +
                    "stoque\r\n3 - Cadastrar pedido\r\n4 - Verificar slipt e quantidades dos pedidos cria" +
                    "dos\r\n5 - Verificar posicao e movimento de estoque\r\n\r\nVamos colocar as tabelas co" +
                    "mpletas de homologacao, as tabelas *WMS*, para ter massa de dados.\r\n\r\nTestes:\r\n\r" +
                    "\n\r\n100 - Um único produto com um único CD\r\n\tPedido sem slipt\r\n\tPedido com slipt " +
                    "totalmente atendido\r\n\tPedido com slipt faltando produto\r\n\r\n200 - Um único produt" +
                    "o com um mais de um CD\r\n\tResumo dos tests:\r\n\r\n\t\t210 - Definição de regras\r\n\t\t\t.." +
                    "....\r\n\t\t\tRegra de consumo para esperar mercadoria CD1 para tipo_pessoa para esta" +
                    "do UF\r\n\t\t\t......\r\n\t\t\r\n\t\t220 - ser atendido pelo CD 1 e pelo CD 2 = gera 01 filho" +
                    "te\r\n\t\t230 - atender pelo CD2 e o resto sem presença = gera 01 filhote\r\n\t\t240 - f" +
                    "icar com todos os produtos sem presença = somente pedido pai\r\n\r\n\t\t250 - Novas re" +
                    "gras\r\n\t\t\t......\r\n\t\t\tRegra de consumo para esperar mercadoria CD2 para tipo_pesso" +
                    "a para estado UF\r\n\t\t\t......\r\n\r\n\t\t260 - ser atendido pelo CD 1 e pelo CD 2, esper" +
                    "ando no CD2 = gera 02 filhote\r\n\t\t270 - atender pelo CD2 e o resto sem presença\r\n" +
                    "\t\t280 - ficar com todos os produtos sem presença\r\n\r\n\r\n400 - Três produtos atendi" +
                    "dos por CDs com a mesma prioridade\r\n\tOs três produtos possuem a mesma regra de c" +
                    "onsumo de estoque\r\n\t410 - O primeiro CD tem estoque para todos\r\n\t420 - O primeir" +
                    "o CD tem estoque para o produto 1 e o CD 2 para o produto 2\r\n\t430 - Idem mas não" +
                    " atende completamente; o produto 1 é vendido sem presença\r\n\t440 - Idem mas não a" +
                    "tende completamente; o produto 1 e 2 são vendidos sem presença\r\n500 - Três produ" +
                    "tos atendidos por CDs com duas regras com prioridades diferentes\r\n600 - Testar s" +
                    "tatus de ativo das regras\r\n\r\n\r\nTestes não atuomatizados:\r\nFazer um roteiro manua" +
                    "l para criar pedidos, dar entrada no estoque pelo ASP\r\ne verficiar que a rotina " +
                    "que processa os produtos pendentes atende esses pedidos\r\ne que o estoque ficou c" +
                    "onsistente.", ProgrammingLanguage.CSharp, new string[] {
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
        
        [Xunit.SkippableFactAttribute(DisplayName="fazer")]
        [Xunit.TraitAttribute("FeatureTitle", "000-Roteiro_o_que_testar")]
        [Xunit.TraitAttribute("Description", "fazer")]
        public virtual void Fazer()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("fazer", null, tagsOfScenario, argumentsOfScenario);
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
#line 62
 testRunner.Given("falta fazer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
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
                _000_Roteiro_O_Que_TestarFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                _000_Roteiro_O_Que_TestarFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
