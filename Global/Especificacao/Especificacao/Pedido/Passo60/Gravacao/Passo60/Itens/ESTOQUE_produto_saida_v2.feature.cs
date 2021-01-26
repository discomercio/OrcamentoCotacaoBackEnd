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
namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo60.Itens
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "ignore")]
    public partial class ESTOQUE_Produto_Saida_V2Feature : object, Xunit.IClassFixture<ESTOQUE_Produto_Saida_V2Feature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "ignore"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "ESTOQUE_produto_saida_v2.feature"
#line hidden
        
        public ESTOQUE_Produto_Saida_V2Feature(ESTOQUE_Produto_Saida_V2Feature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ESTOQUE_produto_saida_v2", null, ProgrammingLanguage.CSharp, new string[] {
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
        
        [Xunit.SkippableFactAttribute(DisplayName="ESTOQUE_produto_saida_v2")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2")]
        [Xunit.TraitAttribute("Description", "ESTOQUE_produto_saida_v2")]
        public virtual void ESTOQUE_Produto_Saida_V2()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("ESTOQUE_produto_saida_v2", null, tagsOfScenario, argumentsOfScenario);
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
#line 16
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 17
 testRunner.When("precisa alterar a \"qtde_utilizada\" da tabela t_ESTOQUE_ITEM para retornar 0", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 18
 testRunner.Then(@"Erro ""Produto "" + id_produto + "" do fabricante "" + id_fabricante + "": faltam "" + ((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_disponivel) + "" unidades no estoque ("" + UtilsGlobais.Util.ObterApelidoEmpresaNfeEmitentesGravacao(id_nfe_emitente, dbGravacao) + "") para poder atender ao pedido.""", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 19
 testRunner.And("afazer - Ajustar a mensagem de erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 28
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 29
 testRunner.When("precisa alterar para que a qtde_movimentada seja menor que (qtde_a_sair - qtde_au" +
                        "torizada_sem_presenca)", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 30
 testRunner.Then("Erro \"Produto \" + id_produto + \" do fabricante \" + id_fabricante + \": faltam \" + " +
                        "((qtde_a_sair - qtde_autorizada_sem_presenca) - qtde_movimentada) + \" unidades n" +
                        "o estoque para poder atender ao pedido.\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 31
 testRunner.And("afazer - ajustar a mensagem de erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="Verificar estoque item")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2")]
        [Xunit.TraitAttribute("Description", "Verificar estoque item")]
        [Xunit.InlineDataAttribute("id_estoque", "1", "id_estoque", "000000119328", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "fabricante", "003", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "produto", "003220", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "qtde", "100", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "preco_fabricante", "670.8500", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "qtde_utilizada", "100", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "data_ult_movimento", "2021-01-20 00:00:00", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "sequencia", "1", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "[timestamp]", "[xS", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "vl_custo2", "670.8500", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "vl_BC_ICMS_ST", "0.0000", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "vl_ICMS_ST", "0.0000", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "ncm", "84151011", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "cst", "000", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "st_ncm_cst_herdado_tabela_produto", "0", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "ean", "null", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "aliq_ipi", "0.0", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "aliq_icms", "0.0", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "vl_ipi", "0.0000", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "preco_origem", "null", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "produto_xml", "null", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "1", "vl_frete", "null", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "id_estoque", "000000119328", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "fabricante", "003", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "produto", "003221", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "qtde", "100", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "preco_fabricante", "1006.2800", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "qtde_utilizada", "100", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "data_ult_movimento", "2021-01-20 00:00:00", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "sequencia", "2", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "[timestamp]", "[xZ", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "vl_custo2", "1006.2800", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "vl_BC_ICMS_ST", "0.0000", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "vl_ICMS_ST", "0.0000", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "ncm", "84151011", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "cst", "000", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "st_ncm_cst_herdado_tabela_produto", "0", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "ean", "null", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "aliq_ipi", "0.0", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "aliq_icms", "0.0", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "vl_ipi", "0.0000", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "preco_origem", "null", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "produto_xml", "null", new string[0])]
        [Xunit.InlineDataAttribute("id_estoque", "2", "vl_frete", "null", new string[0])]
        public virtual void VerificarEstoqueItem(string id_Estoque, string sequencia, string campo, string valor, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("id_estoque", id_Estoque);
            argumentsOfScenario.Add("sequencia", sequencia);
            argumentsOfScenario.Add("campo", campo);
            argumentsOfScenario.Add("valor", valor);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Verificar estoque item", null, tagsOfScenario, argumentsOfScenario);
#line 33
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
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 35
 testRunner.Then("Sem nehum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 36
 testRunner.And("pegar o número do pedido gerado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 37
 testRunner.And("pegar o id_estoque na tabela t_ESTOQUE_MOVIMENTO", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 38
 testRunner.And(string.Format("Tabela \"t_ESTOQUE_ITEM\" registro com campo \"id_estoque\" = \"id_estoque da t_ESTOQU" +
                            "E_MOVIMENTO\", verificar campo \"{0}\" = \"{1}\"", campo, valor), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="verfificar estoque movimento")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2")]
        [Xunit.TraitAttribute("Description", "verfificar estoque movimento")]
        [Xunit.InlineDataAttribute("pedido", "003221", "id_movimento", "000003020931", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "data", "2021-01-20 00:00:00", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "hora", "174738", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "usuario", "HAMILTON", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "id_estoque", "000000119328", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "fabricante", "003", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "produto", "003221", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "qtde", "2", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "operacao", "VDA", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "estoque", "VDO", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "pedido", "222266N", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "loja", "", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "anulado_status", "0", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "anulado_data", "null", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "anulado_hora", "null", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "anulado_usuario", "null", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "timestamp", "[x.", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "kit", "0", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "kit_id_estoque", "", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003221", "id_ordem_servico", "null", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "id_movimento", "000003020930", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "data", "2021-01-20 00:00:00", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "hora", "174737", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "usuario", "HAMILTON", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "id_estoque", "000000119328", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "fabricante", "003", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "produto", "003220", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "qtde", "2", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "operacao", "VDA", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "estoque", "VDO", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "pedido", "222266N", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "loja", "", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "anulado_status", "0", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "anulado_data", "null", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "anulado_hora", "null", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "anulado_usuario", "null", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "timestamp", "[x-", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "kit", "0", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "kit_id_estoque", "", new string[0])]
        [Xunit.InlineDataAttribute("pedido", "003220", "id_ordem_servico", "null", new string[0])]
        public virtual void VerfificarEstoqueMovimento(string pedido, string produto, string campo, string valor, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("pedido", pedido);
            argumentsOfScenario.Add("produto", produto);
            argumentsOfScenario.Add("campo", campo);
            argumentsOfScenario.Add("valor", valor);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("verfificar estoque movimento", null, tagsOfScenario, argumentsOfScenario);
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
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 91
 testRunner.Then("Sem nehum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 92
 testRunner.And("pegar o número do pedido gerado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 93
 testRunner.And(string.Format("Tabela \"t_ESTOQUE_MOVIMENTO\" registro com campo \"pedido\" = \"pedido gerado\", verif" +
                            "icar campo \"{0}\" = \"{1}\"", campo, valor), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="verificar log da movimentação")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2")]
        [Xunit.TraitAttribute("Description", "verificar log da movimentação")]
        public virtual void VerificarLogDaMovimentacao()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("verificar log da movimentação", null, tagsOfScenario, argumentsOfScenario);
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
#line 139
 testRunner.Then("afazer essa validação", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="verificar alteração de último movimento estoque")]
        [Xunit.TraitAttribute("FeatureTitle", "ESTOQUE_produto_saida_v2")]
        [Xunit.TraitAttribute("Description", "verificar alteração de último movimento estoque")]
        [Xunit.InlineDataAttribute("id_estoque", "000000119328", new string[0])]
        [Xunit.InlineDataAttribute("data_entrada", "2020-12-29 00:00:00", new string[0])]
        [Xunit.InlineDataAttribute("hora_entrada", "135628", new string[0])]
        [Xunit.InlineDataAttribute("fabricante", "003", new string[0])]
        [Xunit.InlineDataAttribute("documento", "tstenovo", new string[0])]
        [Xunit.InlineDataAttribute("usuario", "PRAGMATICA", new string[0])]
        [Xunit.InlineDataAttribute("data_ult_movimento", "2021-01-20 00:00:00", new string[0])]
        [Xunit.InlineDataAttribute("[timestamp]", "[x[", new string[0])]
        [Xunit.InlineDataAttribute("kit", "0", new string[0])]
        [Xunit.InlineDataAttribute("entrada_especial", "0", new string[0])]
        [Xunit.InlineDataAttribute("devolucao_status", "0", new string[0])]
        [Xunit.InlineDataAttribute("devolucao_data", "null", new string[0])]
        [Xunit.InlineDataAttribute("devolucao_hora", "null", new string[0])]
        [Xunit.InlineDataAttribute("devolucao_usuario", "null", new string[0])]
        [Xunit.InlineDataAttribute("devolucao_loja", "null", new string[0])]
        [Xunit.InlineDataAttribute("devolucao_pedido", "null", new string[0])]
        [Xunit.InlineDataAttribute("devolucao_id_item_devolvido", "null", new string[0])]
        [Xunit.InlineDataAttribute("devolucao_id_estoque", "null", new string[0])]
        [Xunit.InlineDataAttribute("obs", "teste", new string[0])]
        [Xunit.InlineDataAttribute("id_nfe_emitente", "4903", new string[0])]
        [Xunit.InlineDataAttribute("entrada_tipo", "0", new string[0])]
        [Xunit.InlineDataAttribute("perc_agio", "0.0", new string[0])]
        [Xunit.InlineDataAttribute("data_emissao_NF_entrada", "null", new string[0])]
        public virtual void VerificarAlteracaoDeUltimoMovimentoEstoque(string campo, string valor, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("campo", campo);
            argumentsOfScenario.Add("valor", valor);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("verificar alteração de último movimento estoque", null, tagsOfScenario, argumentsOfScenario);
#line 141
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
#line 142
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 143
 testRunner.Then("Sem nehum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 144
 testRunner.And("pegar o número do pedido gerado", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 145
 testRunner.And("pegar o id_estoque na tabela t_ESTOQUE_MOVIMENTO", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 146
 testRunner.And(string.Format("Tabela \"t_ESTOQUE\" registro com campo \"id_estoque\" = \"id_estoque\", verificar camp" +
                            "o \"{0}\" = \"{1}\"", campo, valor), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
                ESTOQUE_Produto_Saida_V2Feature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                ESTOQUE_Produto_Saida_V2Feature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
