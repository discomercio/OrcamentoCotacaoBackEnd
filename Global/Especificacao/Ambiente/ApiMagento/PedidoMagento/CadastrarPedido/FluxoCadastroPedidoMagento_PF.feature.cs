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
namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [Xunit.TraitAttribute("Category", "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido")]
    public partial class FLuxoCadastroPedidoMagento_PFFeature : object, Xunit.IClassFixture<FLuxoCadastroPedidoMagento_PFFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = new string[] {
                "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido"};
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "FluxoCadastroPedidoMagento_PF.feature"
#line hidden
        
        public FLuxoCadastroPedidoMagento_PFFeature(FLuxoCadastroPedidoMagento_PFFeature.FixtureData fixtureData, Especificacao_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "FLuxoCadastroPedidoMagento - PF", "============================\r\nFluxo Magento:\r\nP10_Cliente: \r\n\t- Só aceitamos clie" +
                    "nte PF.\r\n\t- Mover endereço de entrega para Dados cadastrais\r\n\t- Cliente PF: Prod" +
                    "utor Rural = 1 (Não), Contribuinte ICMS = 0 (Inicial), IE = vazio.\r\n\t\tTeste em A" +
                    "mbiente\\ApiMagento\\PedidoMagento\\CadastrarPedido\\P10_Cliente\\Dados _Cliente\\Dado" +
                    "sPessoais.feature\r\n\t- Não exigimos telefones\r\n\t- Endereço: pedidos do magento va" +
                    "lidamos Cidade contra o IGBE e UF contra o CEP informado. Não validamos nenhum o" +
                    "utro campo do endereço. \r\n\t\tSe o CEP não existir, aceitamos o que veio e só vali" +
                    "dar a cidade contra o IBGE.\r\n\t\tTeste em Ambiente\\ApiMagento\\PedidoMagento\\Cadast" +
                    "rarPedido\\P10_Cliente\\Dados _Cliente\\Endereco\\ValidacaoCep.feature\r\n\t- Caso o cl" +
                    "iente não exista, cadastramos o cliente. \r\n\r\nP20_Indicador: Se tiver valor de fr" +
                    "ete significa que tem indicador.\r\n\t- Se tiver valor de frete, inserimos o indica" +
                    "dor do appsettings e validamos se o indicador existe na base de dados.\r\n\t- Valid" +
                    "amos se a loja que esta no appsettings existe na base de dados.\r\n\r\nP30_InfPedido" +
                    ": Validar pedido magento, código de origem e pedido marketplace e MagentoPedidoS" +
                    "tatus:\r\n\tPedido_magento obrigatório, contém somente números, quantidade de carac" +
                    "teres menor que Constantes.MAX_TAMANHO_ID_PEDIDO_MAGENTO\r\n\tMarketplace_codigo_or" +
                    "igem obrigatório e existe na base de dados, t_CODIGO_DESCRICAO Grupo == InfraBan" +
                    "co.Constantes.Constantes.GRUPO_T_CODIGO_DESCRICAO__PEDIDOECOMMERCE_ORIGEM \r\n\tExi" +
                    "ste validação adicional em Especificacao\\Pedido\\Passo30\\CamposMagentoExigidos.fe" +
                    "ature e Especificacao\\Pedido\\Passo30\\CamposMagentoNaoAceitos.feature\r\n\tMagentoPe" +
                    "didoStatus deve ser aprovado ou aprovação pendente\r\n\r\nP35_Totais: validações de " +
                    "PedidoTotaisMagentoDto\r\n\tCampos não validados: FreteBruto e DescontoFrete\r\n\tCamp" +
                    "os com sua feature: Subtotal, DiscountAmount, BSellerInterest, GrandTotal\r\n\r\nP39" +
                    "_Servicos: para cada linha, consistir Quantidade > 0, RowTotal = Subtotal - Disc" +
                    "ountAmount dentro do arredondamento\r\n\r\nP40_Produtos: transfromar produtos compos" +
                    "tos e lançar os descontos\r\n\tP05: para cada linha, consistir Quantidade > 0, RowT" +
                    "otal = Subtotal - DiscountAmount dentro do arredondamento e verificar se existem" +
                    " produtos ou serviços repetidos\r\n\tP10: Transformar produtos compostos em simples" +
                    "\r\n\t\tBuscamos na t_EC_PRODUTO_COMPOSTO e expandimos os produtos conforme t_EC_PRO" +
                    "DUTO_COMPOSTO_ITEM. Se não for composto, mantemos.\r\n\t\t\tt_EC_PRODUTO_COMPOSTO.pro" +
                    "duto_composto == Sku\r\n\t\t\t\tse achar registro, busca em t_EC_PRODUTO_COMPOSTO_ITEM" +
                    " com t_EC_PRODUTO_COMPOSTO_ITEM.produto_composto == Sku && t_EC_PRODUTO_COMPOSTO" +
                    "_ITEM.fabricante_composto == t_EC_PRODUTO_COMPOSTO.fabricante_composto\r\n\t\t\t\tse n" +
                    "ão achar busca em t_PRODUTO com t_PRODUTO.produto == Sku \r\n\t\tAgrupamos produtos " +
                    "iguais, mantendo a ordem original.\r\n\tP20: Carregar valores dos produtos do banco" +
                    "\r\n\t\tCarregar valores base (t_PRODUTO, t_PRODUTO_LOJA) e coeficientes (t_PERCENTU" +
                    "AL_CUSTO_FINANCEIRO_FORNECEDOR ou fixo) conforme forma de pagamento\r\n\t\tVerificam" +
                    "os se todos estão cadastrados em t_PRODUTO_LOJA\r\n\tP30_CalcularDesconto: Inserir " +
                    "os descontos de forma a chegar nos valores do magento com o frete diluído\r\n\t\tVer" +
                    " planilha, resumo das fórmulas:\r\n\t\t\tdesconto_preco_nf: (valor total do pedido no" +
                    " verdinho - valor total no magento) / valor total do pedido no verdinho\r\n\t\t\tdesc" +
                    "_dado = (total do verdinho - total do magento sem o frete) / total do verdinho\r\n" +
                    "\t\t\tpreco_lista = valor base\r\n\t\t\tpreco_nf = valor base * (1 - desconto_preco_nf)\r" +
                    "\n\t\t\tpreco_venda = valor base * (1 - desc_dado)\r\n\t\t\tTodos os valores são arredond" +
                    "ados para 2 casas decimais (não o desconto)\r\n\t\tGarantir o menor arredondamento p" +
                    "ossível\r\n\t\t\tDesejado: GrandTotal = soma (qde * preco_nf) + total de serviços\r\n\t\t" +
                    "\tajuste_arredondamento: GrandTotal - (soma (qde * preco_nf) + total de serviços)" +
                    " com todos com 2 casas decimais\r\n\t\t\tEscolher a linha com a menor resto de ( abs(" +
                    "ajuste_arredondamento) / qde) e, nesssas, com a menor qde \r\n\t\t\talterar preco_nf " +
                    "= preco_nf - ajuste_arredondamento / qde (arredondado para 2 casas decimais)\r\n\t\t" +
                    "\tIsso já faz o melhor ajuste possível para RA também\r\n\t\t\t* testar com ajustes po" +
                    "sitivos e negativos\r\n\t\tConsistências (todas com arredondamento): \r\n\t\t\tRA = soma " +
                    "(qde * preco_nf) - soma (qde * preco_venda)\r\n\t\t\tRA = FreteBruto - DescontoFrete\r" +
                    "\n\t\t\tGrandTotal = soma (qde * preco_nf) + total de serviços\r\n\tP80: Garatir que te" +
                    "m menos ou igual a 12 itens (conforme configuração)\r\nP50_Pedido: verificaçoes ad" +
                    "icionais e converter estruturas de dados\r\n\tTratar PontoReferencia, endereco_comp" +
                    "lemento e NFe_texto_constar: Colocar a informação do ponto de referência no camp" +
                    "o \'Constar na NF\'.\r\n\t\tTeste em Ambiente\\ApiMagento\\PedidoMagento\\CadastrarPedido" +
                    "\\P50_Pedido\\Endereco\\PontoReferencia.feature\r\n\tGarantiaIndicador = Constantes.CO" +
                    "D_GARANTIA_INDICADOR_STATUS__NAO\r\n\t\tTeste em Ambiente\\ApiMagento\\PedidoMagento\\C" +
                    "adastrarPedido\\P50_Pedido\\Detalhes\\Detalhes.feature\r\n\tSó aceitamos os pagamentos" +
                    " Á vista, Parcela Única, Parcelado no Cartão\r\n\t\tTeste em Ambiente\\ApiMagento\\Ped" +
                    "idoMagento\\CadastrarPedido\\P50_Pedido\\FormaPagto\\*.feature\r\n\r\n\tConverter pedido " +
                    "para PedidoCriacaoDados\r\n\tConverter Endereco Cadastral para DadosClienteCadastro" +
                    "Dados\r\n\tConverter EnderecoCadastralClienteMagentoDto para EnderecoCadastralClien" +
                    "tePrepedidoDados\r\n\tConverter EnderecoEntregaClienteMagentoDto para EnderecoEntre" +
                    "gaClienteCadastroDados\r\n\tConverter FormaPagtoCriacaoMagentoDto para FormaPagtoCr" +
                    "iacaoDados\r\n\r\nP60_Cadastrar: fazer o cadastro do pedido na rotina global, confor" +
                    "me fluxo Especificacao\\Pedido\\FluxoCriacaoPedido.feature\r\n\t- caso aconteça um pe" +
                    "dido igual com número diferente no magento, mandar aviso por e-mail \r\n\t\t(enviar " +
                    "uma notificação para a equipe da karina)\r\n\r\n\r\n============================", ProgrammingLanguage.CSharp, new string[] {
                        "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido"});
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
        
        [Xunit.SkippableFactAttribute(DisplayName="salvando o pedido base")]
        [Xunit.TraitAttribute("FeatureTitle", "FLuxoCadastroPedidoMagento - PF")]
        [Xunit.TraitAttribute("Description", "salvando o pedido base")]
        public virtual void SalvandoOPedidoBase()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("salvando o pedido base", null, tagsOfScenario, argumentsOfScenario);
#line 86
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
 testRunner.Given("Pedido base", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 88
 testRunner.Then("Sem nenhum erro", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Fluxo de cadastro do magento")]
        [Xunit.TraitAttribute("FeatureTitle", "FLuxoCadastroPedidoMagento - PF")]
        [Xunit.TraitAttribute("Description", "Fluxo de cadastro do magento")]
        public virtual void FluxoDeCadastroDoMagento()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Fluxo de cadastro do magento", null, tagsOfScenario, argumentsOfScenario);
#line 90
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
#line 91
 testRunner.Given("Esta é a especificação, está sendo testado em outros .feature", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="o que falta fazer")]
        [Xunit.TraitAttribute("FeatureTitle", "FLuxoCadastroPedidoMagento - PF")]
        [Xunit.TraitAttribute("Description", "o que falta fazer")]
        public virtual void OQueFaltaFazer()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("o que falta fazer", "\tAlterações a documentar no processo global: gravação dos serviços\r\n\tIncluir no g" +
                    "lobal: PedidoMagentoStatus: campo controlado somente pelo pedido pai", tagsOfScenario, argumentsOfScenario);
#line 93
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
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.3.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                FLuxoCadastroPedidoMagento_PFFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                FLuxoCadastroPedidoMagento_PFFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
