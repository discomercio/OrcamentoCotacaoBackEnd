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
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FluxoGravacaoPedido", "arquivo loja/PedidoNovoConfirma.asp\r\n\r\n\r\nPasso10: Fazer todas as validações (docu" +
                    "mentado em FluxoCriacaoPedido.feature e nos passos dele).\r\n\r\nPasso20: LER AS REG" +
                    "RAS DE CONSUMO DO ESTOQUE\r\n\trotina obtemCtrlEstoqueProdutoRegra (arquivo bdd.asp" +
                    ")\r\n\t\ttipo_pessoa: especificado em Passo20/multi_cd_regra_determina_tipo_pessoa.f" +
                    "eature\r\n\t\trotina obtemCtrlEstoqueProdutoRegra validações especificado em Passo20" +
                    "/obtemCtrlEstoqueProdutoRegra.feature\r\n\r\n\tTraduzindo: para cada produto:\r\n\t\tDado" +
                    " o produto, UF, tipo_cliente, contribuinte_icms_status, produtor_rural_status   " +
                    "          \r\n\t\tDescobrir em t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD os CDs que atendem " +
                    "em ordem de prioridade\r\n\t\tLê as tabelas t_PRODUTO_X_WMS_REGRA_CD, t_WMS_REGRA_CD" +
                    "_X_UF, t_WMS_REGRA_CD_X_UF_X_PESSOA, t_WMS_REGRA_CD_X_UF_X_PESSOA_X_CD\r\n\r\n\r\nPass" +
                    "o25:  \r\n\tVERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK - linha 1010\r\n\t\t" +
                    "Verifica se todos os produtos possuem regra ativa e não bloqueada e ao menos um " +
                    "CD ativo.\r\n\t\'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ " +
                    "HABILITADO EM TODAS AS REGRAS - linha 1047\r\n\t\tNo caso de CD manual, verifica se " +
                    "o CD tem regra ativa\r\n\r\n\r\nPasso30: OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE -" +
                    " linha 1083\r\n\tPara todas as regras linha 1086\r\n\t\tSe o CD deve ser usado (manual " +
                    "ou auto)\r\n\t\tpara todos os CDs da regra linha 1088\r\n\t\t\tProcura esse produto na li" +
                    "sta de produtos linha 1095\r\n\t\t\testoque_verifica_disponibilidade_integral_v2 em e" +
                    "stoque.asp, especificado em Passo30/estoque_verifica_disponibilidade_integral_v2" +
                    ".feature\r\n\t\t\t\t\'Calcula quantidade em estoque no CD especificado\r\n\r\n\tTraduzindo:\r" +
                    "\n\t\tCalcula o estoque de cada produto em cada CD que pode ser usado\r\n\r\n\r\nPasso40:" +
                    " Verifica se a disponibilidade do estoque foi alterada - Linha 1159\r\n\tHÁ PRODUTO" +
                    " C/ ESTOQUE INSUFICIENTE (SOMANDO-SE O ESTOQUE DE TODAS AS EMPRESAS CANDIDATAS) " +
                    "- linha 1127\r\n\r\n\tPorque: avisamos o usuário que existem produtos sem presença no" +
                    " estoque e, no momento de salvar, os produtos sem presença no estoque foram alte" +
                    "rados.\r\n\tNo caso da ApiMagento, não temos essa verificação\r\n\r\n\r\nPasso50: ANALISA" +
                    " A QUANTIDADE DE PEDIDOS QUE SERÃO CADASTRADOS (AUTO-SPLIT) - linha 1184\r\n\t\t\t\'\tO" +
                    "S CD\'S ESTÃO ORDENADOS DE ACORDO C/ A PRIORIZAÇÃO DEFINIDA PELA REGRA DE CONSUMO" +
                    " DO ESTOQUE\r\n\t\t\t\'\tSE O PRIMEIRO CD HABILITADO NÃO PUDER ATENDER INTEGRALMENTE A " +
                    "QUANTIDADE SOLICITADA DO PRODUTO,\r\n\t\t\t\'\tA QUANTIDADE RESTANTE SERÁ CONSUMIDA DOS" +
                    " DEMAIS CD\'S.\r\n\t\t\t\'\tSE HOUVER ALGUMA QUANTIDADE RESIDUAL P/ FICAR NA LISTA DE PR" +
                    "ODUTOS SEM PRESENÇA NO ESTOQUE:\r\n\t\t\t\'\t\t1) SELEÇÃO AUTOMÁTICA DE CD: A QUANTIDADE" +
                    " PENDENTE FICARÁ ALOCADA NO CD DEFINIDO P/ TAL\r\n\t\t\t\'\t\t2) SELEÇÃO MANUAL DE CD: A" +
                    " QUANTIDADE PENDENTE FICARÁ ALOCADA NO CD SELECIONADO MANUALMENTE\r\n\r\n\tPara cada " +
                    "produto:\r\n\t\tAloca a quantidade solicitada nos CDs ordenados até alocar todos.\r\n\t" +
                    "\tSe não conseguir alocar todos, marca a quantidade residual no CD manual ou no C" +
                    "D de t_WMS_REGRA_CD_X_UF_X_PESSOA.spe_id_nfe_emitente\r\n\r\nPasso55: Contagem de pe" +
                    "didos a serem gravados - Linha 1286\r\n\t\'\tCONTAGEM DE EMPRESAS QUE SERÃO USADAS NO" +
                    " AUTO-SPLIT, OU SEJA, A QUANTIDADE DE PEDIDOS QUE SERÁ CADASTRADA, \r\n\tJÁ QUE CAD" +
                    "A PEDIDO SE REFERE AO ESTOQUE DE UMA EMPRESA\r\n\r\n\tConta todos os CDs que tem algu" +
                    "ma quantidade solicitada.\r\n\r\n\r\nPasso60: criar pedidos\r\n\tLoop nos CDs a utilizar\r" +
                    "\n\t\tGerar o número do pedido: Passo60/Gerar_o_numero_do_pedido.feature\r\n\t\tAdicion" +
                    "a um novo pedido\r\n\t\tPreenche os campos do pedido: Passo60/Preenche_os_campos_do_" +
                    "pedido.feature\r\n\t\t\ta maioria no pai e filhotes, alguns só no pai, alguns só nos " +
                    "filhotes\r\n\t\tSalva o registro em t_pedido\r\n\r\n\t\tLoop nas regras: \r\n\t\t\tEspecificado" +
                    " em Passo60/Itens/Gerar_t_PEDIDO_ITEM.feature\r\n\t\t\t\tSe essa regra cobrir um dos i" +
                    "tens do pedido, adicionar registro em t_PEDIDO_ITEM (linha 2090 até 2122)\r\n\t\t\t\tN" +
                    "ote que a quantidade rs(\"qtde\") é a que foi alocada para esse filhote pela regra" +
                    ", não a quantidade total do pedido inteiro\r\n\t\t\t\tA sequencia do t_PEDIDO_ITEM par" +
                    "a esse pedido (base ou filhote) começa de 1 e é sequencial.\r\n\t\t\tSe qtde_solicita" +
                    "da > qtde_estoque, qtde_spe (quantidade_sen_presença_estoque) fica com o número " +
                    "de itens faltando\r\n\t\t\tchama rotina ESTOQUE_produto_saida_v2, em Passo60/Itens/ES" +
                    "TOQUE_produto_saida_v2.feature\r\n\t\t\t\tA quantidade deste item ou efetivamente sai " +
                    "do estoque (atualizando t_ESTOQUE_ITEM)\r\n\t\t\t\tou entra como venda sem presença no" +
                    " estoque (novo registro na tabela t_ESTOQUE_MOVIMENTO, operacao = OP_ESTOQUE_VEN" +
                    "DA, estoque = ID_ESTOQUE_SEM_PRESENCA)\r\n\t\t\tMonta o log do item - Passo60/Itens/L" +
                    "og.feature\r\n\t\t\t\r\n\t\tDetermina o status st_entrega deste pedido (Passo60/st_entreg" +
                    "a.feature)\r\n\r\nPasso70: ajustes adicionais no pedido pai\r\n\tNo pedido pai atualiza" +
                    " campos de RA (Passo70/calcula_total_RA_liquido_BD.feture)\r\n\r\n\tCaso tenha usado " +
                    "algum desconto superior ao limite, liberado pela t_DESCONTO, marca como usado (P" +
                    "asso70/Senhas_de_autorizacao_para_desconto_superior.feature)\r\n\r\n\tINDICADOR: SE E" +
                    "STE PEDIDO É COM INDICADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, E" +
                    "NTÃO CADASTRA ESTE. (Passo70/CadastroIndicador.feature)\r\n\r\n\r\nPasso80: VERIFICA S" +
                    "E O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)\r\n\tPa" +
                    "sso80/FluxoVerificacaoEndereco.feature\r\n\r\n\r\nPasso90: log (Passo90/Log.feature)", ProgrammingLanguage.CSharp, new string[] {
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
#line 103
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
#line 104
 testRunner.When("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 105
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
