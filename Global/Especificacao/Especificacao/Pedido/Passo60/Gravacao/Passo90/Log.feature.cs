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
namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo90
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.Passo60")]
    [Xunit.TraitAttribute("Category", "GerenciamentoBanco")]
    public partial class EspecificacaoPedidoPasso60GravacaoPasso90LogFeature : object, Xunit.IClassFixture<EspecificacaoPedidoPasso60GravacaoPasso90LogFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.Passo60",
                "GerenciamentoBanco"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Log.feature"
#line hidden
        
        public EspecificacaoPedidoPasso60GravacaoPasso90LogFeature(EspecificacaoPedidoPasso60GravacaoPasso90LogFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Especificacao\\Pedido\\Passo60\\Gravacao\\Passo90\\Log", null, ProgrammingLanguage.CSharp, new string[] {
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
#line 5
#line hidden
#line 6
 testRunner.Given("Ignorar cenário no ambiente \"Especificacao.Prepedido.PrepedidoSteps\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 7
 testRunner.Given("Reiniciar banco ao terminar cenário", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Log - pedido magento")]
        [Xunit.TraitAttribute("FeatureTitle", "Especificacao\\Pedido\\Passo60\\Gravacao\\Passo90\\Log")]
        [Xunit.TraitAttribute("Description", "Log - pedido magento")]
        public virtual void Log_PedidoMagento()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Log - pedido magento", null, tagsOfScenario, argumentsOfScenario);
#line 14
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
this.FeatureBackground();
#line hidden
#line 16
 testRunner.Given("Ignorar cenário no ambiente \"Ambiente.Loja.Loja_Bll.Bll.PedidoBll.PedidoBll.Cadas" +
                        "trarPedido.CadastrarPedido\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 17
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 18
 testRunner.When("Informo \"InfCriacaoPedido.Pedido_bs_x_ac\" = \"123457092\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 19
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 20
 testRunner.And("Tabela \"t_LOG\" pedido gerado e operacao = \"OP_LOG_PEDIDO_NOVO\", verificar campo \"" +
                        "complemento\" = \"vl total=3.132,90; ra=\"\"; indicador=\"\"; vl_total_nf=3.132,90; vl" +
                        "_total_ra=0,00; perc_rt=0; qtde_parcelas=1; st_etg_imediata=2; stbemusoconsumo=1" +
                        "; instaladorinstalastatus=1; obs_1=teste magento; pedido_bs_x_ac = 123457092; ma" +
                        "rketplace_codigo_origem = 001; status da análise crédito: 8 - pendente vendas; t" +
                        "ipo_parcelamento=1; av_forma_pagto=6; custofinancfornectipoparcelamento=av; cust" +
                        "ofinancfornecqtdeparcelas=0; endereço cobrança=rua professor fábio fanucchi, 97 " +
                        "- jardim são paulo(zona norte) - são paulo - sp - 0204-080 (email=testecad@gabri" +
                        "el.com, email_xml=, nome=vivian, ddd_res=11, tel_res=11111111, ddd_com=11, tel_c" +
                        "om=12345678, ramal_com=, ddd_cel=11, tel_cel=981603313, ddd_com_2=, tel_com_2=, " +
                        "ramal_com_2=, tipo_pessoa=pf, cnpj_cpf=14039603052, contribuinte_icms_status=0, " +
                        "produtor_rural_status=1, ie=, rg=, contato=); endereço entrega=mesmo do cadastro" +
                        "; escolha automática de transportadora=n; garantiaindicadorstatus=0; perc_desagi" +
                        "o_ra_liquida=0; pedido_bs_x_at=; cod_origem_pedido=001; operação de origem: cada" +
                        "stramento semi-automático de pedido do e-commerce (nº magento pedido_bs_x_ac=123" +
                        "457092;\\r 2x003220(003); preco_lista=626,58; desc_dado=0; preco_venda=626,58; pr" +
                        "eco_nf=626,58; custofinancforneccoeficiente=1; custofinancfornecprecolistabase=6" +
                        "26,58; estoque_vendido=2;\\r 2x003221(003); preco_lista=939,87; desc_dado=0; prec" +
                        "o_venda=939,87; preco_nf=939,87; custofinancforneccoeficiente=1; custofinancforn" +
                        "ecprecolistabase=939,87; estoque_vendido=2\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
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
                EspecificacaoPedidoPasso60GravacaoPasso90LogFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                EspecificacaoPedidoPasso60GravacaoPasso90LogFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
