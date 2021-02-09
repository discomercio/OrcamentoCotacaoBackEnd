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
    [Xunit.TraitAttribute("Category", "Especificacao.Pedido.FluxoCriacaoPedido")]
    public partial class FluxoGravacaoPedidoFeature : object, Xunit.IClassFixture<FluxoGravacaoPedidoFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Especificacao.Pedido.FluxoCriacaoPedido"};
        
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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FluxoGravacaoPedido", "arquivo loja/PedidoNovoConfirma.asp\r\n\r\nPasso01: Gerar o NSU do pedido (para bloqu" +
                    "ear transações concorrentes)\r\n\r\nPasso10: Fazer todas as validações (documentado " +
                    "em FluxoCriacaoPedido.feature e nos passos dele).\r\n\r\nPasso15: Verificar pedidos " +
                    "repetidos\r\n\r\nPasso17: Marcar t_DESCONTO que forem usados\r\n\tMonta a lista de prod" +
                    "utos para processamento, incluindo a autorização da t_DESCONTO\r\n\tMonta a lista d" +
                    "e registro da t_DESCONTO utilizadas\r\n\tloja/PedidoNovoConfirma.asp de linha 798 a" +
                    " 908 (VERIFICA CADA UM DOS PRODUTOS SELECIONADOS)\r\n\r\nPasso20: LER AS REGRAS DE C" +
                    "ONSUMO DO ESTOQUE\r\n\trotina obtemCtrlEstoqueProdutoRegra (arquivo bdd.asp)\r\n\t\ttip" +
                    "o_pessoa: especificado em Passo20/multi_cd_regra_determina_tipo_pessoa.feature\r\n" +
                    "\t\trotina obtemCtrlEstoqueProdutoRegra validações especificado em Passo20/obtemCt" +
                    "rlEstoqueProdutoRegra.feature\r\n\r\n\tTraduzindo: para cada produto:\r\n\t\tDado o produ" +
                    "to, UF, tipo_cliente, contribuinte_icms_status, produtor_rural_status           " +
                    "  \r\n\t\tDescobrir em t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD os CDs que atendem em ordem" +
                    " de prioridade\r\n\t\tLê as tabelas t_WMS_REGRA_CD, t_PRODUTO_X_WMS_REGRA_CD, t_WMS_" +
                    "REGRA_CD_X_UF, t_WMS_REGRA_CD_X_UF_X_PESSOA, t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD\r\n" +
                    "\r\n\r\nPasso25:  VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK - linha 101" +
                    "0\r\n\t\tVerifica se todos os produtos possuem regra ativa e não bloqueada e ao meno" +
                    "s um CD ativo.\r\n\t\'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO " +
                    "ESTÁ HABILITADO EM TODAS AS REGRAS - linha 1047\r\n\t\tNo caso de CD manual, verific" +
                    "a se o CD tem regra ativa\r\n\r\n\r\nPasso30: OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTO" +
                    "QUE - linha 1083\r\n\tPara todas as regras linha 1086\r\n\t\tSe o CD deve ser usado (ma" +
                    "nual ou auto)\r\n\t\tpara todos os CDs da regra linha 1088\r\n\t\t\tProcura esse produto " +
                    "na lista de produtos linha 1095\r\n\t\t\testoque_verifica_disponibilidade_integral_v2" +
                    " em estoque.asp, especificado em Passo30/estoque_verifica_disponibilidade_integr" +
                    "al_v2.feature\r\n\t\t\t\t\'Calcula quantidade em estoque no CD especificado\r\n\r\n\tTraduzi" +
                    "ndo:\r\n\t\tCalcula o estoque de cada produto em cada CD que pode ser usado\r\n\r\n\r\nPas" +
                    "so40: Verifica se a disponibilidade do estoque foi alterada - Linha 1159\r\n\tHÁ PR" +
                    "ODUTO C/ ESTOQUE INSUFICIENTE (SOMANDO-SE O ESTOQUE DE TODAS AS EMPRESAS CANDIDA" +
                    "TAS) - linha 1127\r\n\r\n\tPorque: avisamos o usuário que existem produtos sem presen" +
                    "ça no estoque e, no momento de salvar, os produtos sem presença no estoque foram" +
                    " alterados.\r\n\tNo caso da ApiMagento, não temos essa verificação\r\n\r\n\r\nPasso50: AN" +
                    "ALISA A QUANTIDADE DE PEDIDOS QUE SERÃO CADASTRADOS (AUTO-SPLIT) - linha 1184\r\n\t" +
                    "\t\t\'\tOS CD\'S ESTÃO ORDENADOS DE ACORDO C/ A PRIORIZAÇÃO DEFINIDA PELA REGRA DE CO" +
                    "NSUMO DO ESTOQUE\r\n\t\t\t\'\tSE O PRIMEIRO CD HABILITADO NÃO PUDER ATENDER INTEGRALMEN" +
                    "TE A QUANTIDADE SOLICITADA DO PRODUTO,\r\n\t\t\t\'\tA QUANTIDADE RESTANTE SERÁ CONSUMID" +
                    "A DOS DEMAIS CD\'S.\r\n\t\t\t\'\tSE HOUVER ALGUMA QUANTIDADE RESIDUAL P/ FICAR NA LISTA " +
                    "DE PRODUTOS SEM PRESENÇA NO ESTOQUE:\r\n\t\t\t\'\t\t1) SELEÇÃO AUTOMÁTICA DE CD: A QUANT" +
                    "IDADE PENDENTE FICARÁ ALOCADA NO CD DEFINIDO P/ TAL\r\n\t\t\t\'\t\t2) SELEÇÃO MANUAL DE " +
                    "CD: A QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD SELECIONADO MANUALMENTE\r\n\r\n\tPara " +
                    "cada produto:\r\n\t\tAloca a quantidade solicitada nos CDs ordenados até alocar todo" +
                    "s.\r\n\t\tSe não conseguir alocar todos, marca a quantidade residual no CD manual ou" +
                    " no CD de t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente\r\n\r\nPasso55: Contagem " +
                    "de pedidos a serem gravados - Linha 1286\r\n\t\'\tCONTAGEM DE EMPRESAS QUE SERÃO USAD" +
                    "AS NO AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, \r\n\tJÁ QU" +
                    "E CADA PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA\r\n\r\n\tConta todos os CDs que tem" +
                    " alguma quantidade solicitada.\r\n\r\n\r\nPasso60: criar pedidos - \'\tCADASTRA O PEDIDO" +
                    " E PROCESSA A MOVIMENTAÇÃO NO ESTOQUE\r\n\tLoop nos CDs a utilizar\r\n\t\tGerar o númer" +
                    "o do pedido: Passo60/Gerar_o_numero_do_pedido.feature\r\n\t\tAdiciona um novo pedido" +
                    "\r\n\t\tPreenche os campos do pedido: Passo60/Preenche_os_campos_do_pedido.feature\r\n" +
                    "\t\t\ta maioria no pai e filhotes, alguns só no pai, alguns só nos filhotes\r\n\t\tSalv" +
                    "a o registro em t_pedido\r\n\r\n\t\tLoop nas regras: \r\n\t\t\tEspecificado em Passo60/Iten" +
                    "s/Gerar_t_PEDIDO_ITEM.feature\r\n\t\t\t\tSe essa regra cobrir um dos itens do pedido, " +
                    "adicionar registro em t_PEDIDO_ITEM (linha 2090 até 2122)\r\n\t\t\t\tNote que a quanti" +
                    "dade rs(\"qtde\") é a que foi alocada para esse filhote pela regra, não a quantida" +
                    "de total do pedido inteiro\r\n\t\t\t\tA sequencia do t_PEDIDO_ITEM para esse pedido (b" +
                    "ase ou filhote) começa de 1 e é sequencial.\r\n\t\t\tSe qtde_solicitada > qtde_estoqu" +
                    "e, qtde_spe (quantidade_sen_presença_estoque) fica com o número de itens faltand" +
                    "o\r\n\t\t\tchama rotina ESTOQUE_produto_saida_v2, em Passo60/Itens/ESTOQUE_produto_sa" +
                    "ida_v2.feature\r\n\t\t\t\tA quantidade deste item ou efetivamente sai do estoque (atua" +
                    "lizando t_ESTOQUE_ITEM)\r\n\t\t\t\tou entra como venda sem presença no estoque (novo r" +
                    "egistro na tabela t_ESTOQUE_MOVIMENTO, operacao = OP_ESTOQUE_VENDA, estoque = ID" +
                    "_ESTOQUE_SEM_PRESENCA)\r\n\t\t\tMonta o log do item - Passo60/Itens/Log.feature\r\n\t\t\t\r" +
                    "\n\t\tDetermina o status st_entrega deste pedido (Passo60/st_entrega.feature)\r\n\r\nPa" +
                    "sso70: ajustes adicionais no pedido pai\r\n\tNo pai e nos filhotes, atualiza campos" +
                    " de RA (Passo70/calcula_total_RA_liquido_BD.feture)\r\n\r\n\tCaso tenha usado algum d" +
                    "esconto superior ao limite, liberado pela t_DESCONTO, marca como usado (Passo70/" +
                    "Senhas_de_autorizacao_para_desconto_superior.feature)\r\n\r\n\tINDICADOR: SE ESTE PED" +
                    "IDO É COM INDICADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, ENTÃO CA" +
                    "DASTRA ESTE. (Passo70/CadastroIndicador.feature)\r\n\r\n\r\nPasso80: VERIFICA SE O END" +
                    "EREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)\r\n\tPasso80/F" +
                    "luxoVerificacaoEndereco.feature\r\n\r\n\r\nPasso90: log (Passo90/Log.feature)", ProgrammingLanguage.CSharp, new string[] {
                        "Especificacao.Pedido.FluxoCriacaoPedido"});
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
        
        [Xunit.SkippableFactAttribute(DisplayName="Fluxo da gravação do pedido")]
        [Xunit.TraitAttribute("FeatureTitle", "FluxoGravacaoPedido")]
        [Xunit.TraitAttribute("Description", "Fluxo da gravação do pedido")]
        public virtual void FluxoDaGravacaoDoPedido()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fluxo da gravação do pedido", null, tagsOfScenario, argumentsOfScenario);
#line 110
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
#line 111
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 112
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
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
