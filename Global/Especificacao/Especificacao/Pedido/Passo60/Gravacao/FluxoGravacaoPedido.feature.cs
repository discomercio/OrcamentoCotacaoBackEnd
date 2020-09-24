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
namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "ignore")]
    public partial class FluxoGravacaoPedidoFeature : object, Xunit.IClassFixture<FluxoGravacaoPedidoFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "ignore"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "FluxoGravacaoPedido.feature"
#line hidden
        
        public FluxoGravacaoPedidoFeature(FluxoGravacaoPedidoFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FluxoGravacaoPedido", "Fazer todas as validações (não documentado aqui)\r\n\r\n10: \r\nLER AS REGRAS DE CONSUM" +
                    "O DO ESTOQUE\r\n\tPara cada produto no pedido:\r\n\t\tRECUPERA AS REGRAS DE CONSUMO DO " +
                    "ESTOQUE ASSOCIADAS AOS PRODUTOS\r\n\t\tVERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS" +
                    " ESTÃO OK\r\n\t\tverifica se algum CD das regras está ativo (variável qtde_CD_ativo " +
                    "no ASP)\r\n\t\t\r\n\r\n20:\r\n\'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONA" +
                    "DO ESTÁ HABILITADO EM TODAS AS REGRAS\r\n\r\n\r\nINDICADOR: SE ESTE PEDIDO É COM INDIC" +
                    "ADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, ENTÃO CADASTRA ESTE.\r\n\r" +
                    "\nLoop nos CDs a utilizar (vEmpresaAutoSplit)\r\n\tGerar o número do pedido: caso ma" +
                    "ior que 1, colocar letras como sufixo\r\n\t\t\t\'\tControla a quantidade de pedidos no " +
                    "auto-split\r\n\t\t\t\'\tpedido-base: indice_pedido=1\r\n\t\t\t\'\tpedido-filhote \'A\' => indice" +
                    "_pedido=2\r\n\t\t\t\'\tpedido-filhote \'B\' => indice_pedido=3\r\n\t\t\t\'\tetc\r\n\tAdiciona um no" +
                    "vo pedido\r\n\tCampos que existem em todos os pedidos: pedido, loja, data, hora\r\n\tC" +
                    "ampos que existem somente no pedido base, não nos filhotes:\r\n\t\tst_auto_split se " +
                    "tiver filhotes\r\n\t\tCampos transferidos: de linha 1800 rs(\"dt_st_pagto\") = Date at" +
                    "é 1887 rs(\"perc_limite_RA_sem_desagio\") = perc_limite_RA_sem_desagio\r\n\tCampos qu" +
                    "e existem somente nos pedidos filhotes, não no base:\r\n\t\tlinha 1892 rs(\"st_auto_s" +
                    "plit\") = 1 até 1903 rs(\"forma_pagto\")=\"\"\r\n\tTransfere mais campos: linha 1907 até" +
                    " 2055\r\n\tSalva o registro em t_pedido\r\n\r\n\tLoop nas regras (vProdRegra)\r\n\t\tSe essa" +
                    " regra cobrir um dos itens do pedido, adicionar registro em t_PEDIDO_ITEM (linha" +
                    " 2090 até 2122)\r\n\t\tNote que a quantidade rs(\"qtde\") é a que foi alocada para ess" +
                    "e filhote pela regra, não a quantidade total do pedido inteiro\r\n\t\tA sequencia do" +
                    " t_PEDIDO_ITEM para esse pedido (base ou filhote) começa de 1 e é sequencial.\r\n\t" +
                    "\tSE qtde_solicitada > qtde_estoque, qtde_spe quantiade_estoque_sem_presença fica" +
                    " com o número de itens faltando\r\n\t\tchama rotina ESTOQUE_produto_saida_v2\r\n\t\tAtua" +
                    "lzia alguma coisa do estoque desse item?????? linha 2145\r\n\t\tMarca se tem algum p" +
                    "roduto deste pedido sem presença no estoque (para mudar o status deste pedido)\r\n" +
                    "\t\tMonta o log\r\n\t\t\t\r\n\tDetermina o st_entrega deste pedido (pai ou filhote)\r\n\r\n\tPa" +
                    "ra o pedido pai: chama a rotina calcula_total_RA_liquido_BD linha 2225 e atualiz" +
                    "a vl_total_RA_liquido qtde_parcelas_desagio_RA st_tem_desagio_RA\r\n\r\n\tPara o pedi" +
                    "do pai: linha 2251 \r\n\tSENHAS DE AUTORIZAÇÃO PARA DESCONTO SUPERIOR\r\n\tCaso tenha " +
                    "usado algum desconto superior ao limite, liberado pela t_DESCONTO, marca como us" +
                    "ado\r\n\r\n\tVERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POS" +
                    "SÍVEL FRAUDE)\r\n\t\tlinha 2289 - 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO\r\n\t" +
                    "\tlinha 2348 - 2)VERIFICA PEDIDOS DE OUTROS CLIENTES\r\n\t\tlinha 2488 - ENDEREÇO DE " +
                    "ENTREGA (SE HOUVER) 1) VERIFICA SE O ENDEREÇO USADO É O DO PARCEIRO\r\n\t\tlinha 254" +
                    "4 - ENDEREÇO DE ENTREGA (SE HOUVER) 2)VERIFICA PEDIDOS DE OUTROS CLIENTES\r\n\t\tlin" +
                    "ha 2685 - Se for o caso, marca analise_endereco_tratar_status no pedido \r\n\r\nMont" +
                    "a o log do pedido (variável s_log)\r\nMonta o log dos itens do pedido \r\nMonta o lo" +
                    "g do auto-slipt (incluindo log dos filhotes)", ProgrammingLanguage.CSharp, new string[] {
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
        
        [Xunit.SkippableFactAttribute(DisplayName="Add two numbers")]
        [Xunit.TraitAttribute("FeatureTitle", "FluxoGravacaoPedido")]
        [Xunit.TraitAttribute("Description", "Add two numbers")]
        public virtual void AddTwoNumbers()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add two numbers", null, tagsOfScenario, argumentsOfScenario);
#line 70
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
#line 71
 testRunner.Given("the first number is 50", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 72
 testRunner.And("the second number is 70", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 73
 testRunner.When("the two numbers are added", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 74
 testRunner.Then("the result should be 120", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
                FluxoGravacaoPedidoFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                FluxoGravacaoPedidoFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
