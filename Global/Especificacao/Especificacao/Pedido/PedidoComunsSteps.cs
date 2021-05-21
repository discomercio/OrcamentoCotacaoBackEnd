using Especificacao.Testes.Pedido;
using Especificacao.Testes.Utils.ListaDependencias;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

/*
 * esta classe serve para implementar muitos dos features.
 * Os steps são bem parecidos.
 * Ele usa a tag para saber quem faz a omplementação efetiva
 * */

namespace Especificacao.Especificacao.Pedido
{
    [Binding]
    [Scope(Tag = "Especificacao.Pedido.Passo10.CamposSimples")]
    [Scope(Tag = "Especificacao.Pedido.Passo20.EnderecoEntrega")]
    [Scope(Tag = "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj")]
    [Scope(Tag = "Especificacao.Pedido.FluxoCriacaoPedido")]
    [Scope(Tag = "Especificacao.Pedido.Passo30")]
    [Scope(Tag = "Especificacao.Pedido.Passo40")]
    [Scope(Tag = "Especificacao.Pedido.Passo60")]
    public class PedidoComunsSteps : PedidoPassosComuns
    {
        public PedidoComunsSteps(FeatureContext featureContext)
        {
            var tags = featureContext.FeatureInfo.Tags.ToList();

            if (tags.Contains("Especificacao.Pedido.Passo10.CamposSimples"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo10.PermissoesListaDependencias");
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo10.CamposSimplesPfListaDependencias");
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo10.CamposSimplesPjListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo20.EnderecoEntrega.ClientePj.ClientePjListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo20.EnderecoEntrega"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo20.EnderecoEntrega.EnderecoEntregaListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.FluxoCriacaoPedido"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                //este nao tem registro de dependencias
                //RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                //    "Especificacao.Pedido.FluxoCriacaoPedidoDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo30"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                //paramos de fazer o registro de dependencias
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo40"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo40.Passo40ListaDependencias");
                return;
            }
            if (tags.Contains("Especificacao.Pedido.Passo60"))
            {
                var imp = new Especificacao.Pedido.PedidoSteps();
                base.AdicionarImplementacao(imp);
                RegistroDependencias.AdicionarDependencia("Especificacao.Pedido.Pedido.PedidoListaDependencias", imp,
                    "Especificacao.Pedido.Passo60.Passo60ListaDependencias");
                return;
            }
        }

        [Given(@"Pedido base cliente PF")]
        [When(@"Pedido base cliente PF")]
        new public void GivenPedidoBaseClientePF()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePF(this);
            base.GivenPedidoBaseClientePF();
        }

        [Given(@"Pedido base cliente PJ")]
        [When(@"Pedido base cliente PJ")]
        new public void GivenPedidoBaseClientePJ()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            base.GivenPedidoBaseClientePJ();
        }


        [Given(@"Pedido base")]
        [When(@"Pedido base")]
        [Then(@"Pedido base")]
        new public void GivenPedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            base.GivenPedidoBase();
        }

        [Given(@"Pedido base cliente PJ com endereço de entrega")]
        new public void GivenPedidoBaseClientePJComEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJComEnderecoDeEntrega(this);
            base.GivenPedidoBaseClientePJComEnderecoDeEntrega();
        }

        [Given(@"Pedido base cliente PJ com endereço de entrega PJ")]
        public void GivenPedidoBaseClientePJComEnderecoDeEntregaPJ()
        {
            GivenPedidoBaseClientePJComEnderecoDeEntrega();
            WhenInformo("EndEtg_tipo_pessoa", "PJ");
            WhenInformo("EndEtg_cnpj_cpf", "76297703000195");
            WhenInformo("EndEtg_produtor_rural_status", "COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL");
            WhenInformo("EndEtg_contribuinte_icms_status", "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL");

            WhenInformo("EndEtg_ddd_res", "");
            WhenInformo("EndEtg_tel_res", "");
            WhenInformo("EndEtg_ddd_cel", "");
            WhenInformo("EndEtg_tel_cel", "");
            WhenInformo("EndEtg_ddd_com", "11");
            WhenInformo("EndEtg_tel_com", "12345678");
            WhenInformo("EndEtg_rg", "");
        }
        [Given(@"Pedido base cliente PJ com endereço de entrega PF")]
        public void GivenPedidoBaseClientePJComEnderecoDeEntregaPF()
        {
            GivenPedidoBaseClientePJComEnderecoDeEntrega();
            WhenInformo("EndEtg_tipo_pessoa", "PF");
            WhenInformo("EndEtg_cnpj_cpf", "35270445824");
            WhenInformo("EndEtg_produtor_rural_status", "COD_ST_CLIENTE_PRODUTOR_RURAL_NAO");
            WhenInformo("EndEtg_contribuinte_icms_status", "COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO");

            WhenInformo("EndEtg_ddd_res", "11");
            WhenInformo("EndEtg_tel_res", "12345678");
            WhenInformo("EndEtg_ddd_com", "");
            WhenInformo("EndEtg_tel_com", "");
            WhenInformo("EndEtg_rg", "12345678");
        }

        [Given(@"Pedido base COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA")]
        public void GivenPedidoBaseCOD_FORMA_PAGTO_PARCELADO_COM_ENTRADA()
        {
            GivenPedidoBaseClientePJ();
            WhenInformo("FormaPagtoCriacao.Tipo_Parcelamento", "COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA");
            WhenInformo("FormaPagtoCriacao.Op_pce_entrada_forma_pagto", "1");
            WhenInformo("FormaPagtoCriacao.Op_pce_prestacao_forma_pagto", "1");
            WhenInformo("FormaPagtoCriacao.C_pce_prestacao_valor", ((3470.24 - 1) / 3).ToString());
            WhenInformo("FormaPagtoCriacao.C_pce_entrada_valor", "1");
            WhenInformo("FormaPagtoCriacao.C_pce_prestacao_periodo", "1");
            WhenInformo("FormaPagtoCriacao.CustoFinancFornecTipoParcelamento", "COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA");
            WhenInformo("FormaPagtoCriacao.CustoFinancFornecQtdeParcelas", "3");
            WhenInformo("FormaPagtoCriacao.C_pce_prestacao_qtde", "3");
        }

        [When(@"Endereço de entrega do estado ""(.*)""")]
        public void WhenEnderecoDeEntregaDoEstado(string p0)
        {
            //somente SP ou BA
            Testes.Utils.LogTestes.LogOperacoes2.EnderecoDeEntregaDoEstado(p0, this);
            string[] permitidos = { "SP", "BA" };
            Assert.Contains(p0, permitidos);
            if (p0 == "SP")
                EnderecoDeEntregaDoEstado("02408150", "São Paulo", "Água Fria", "Rua Francisco Pecoraro", "SP");
            if (p0 == "BA")
                EnderecoDeEntregaDoEstado("40290050", "Salvador", "Acupe de Brotas", "Rua Prado Moraes", "BA");
        }
        private void EnderecoDeEntregaDoEstado(string EndEtg_cep, string EndEtg_cidade, string EndEtg_bairro, string EndEtg_endereco, string EndEtg_uf)
        {
            WhenInformo("EndEtg_cep", EndEtg_cep);
            WhenInformo("EndEtg_cidade", EndEtg_cidade);
            WhenInformo("EndEtg_bairro", EndEtg_bairro);
            WhenInformo("EndEtg_endereco", EndEtg_endereco);
            WhenInformo("EndEtg_uf", EndEtg_uf);
        }

        [When(@"Informo ""(.*)"" = ""(.*)""")]
        new public void WhenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            base.WhenInformo(p0, p1);
        }

        [Then(@"Erro ""(.*)""")]
        new public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            base.ThenErro(p0);
        }

        [Then(@"Sem [Ee]rro ""(.*)""")]
        new public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            base.ThenSemErro(p0);
        }

        [Then(@"Exceção ""(.*)""")]
        public void ThenExcecao(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Excecao(p0, this);
            bool lancouExcecao = false;
            try
            {
                base.ThenSemNenhumErro();
            }
            catch (Exception e)
            {
                if (e.Message.Contains(p0))
                    lancouExcecao = true;
            }
            Assert.True(lancouExcecao);
        }

        [Given(@"No ambiente ""(.*)"" erro ""(.*)"" é ""(.*)""")]
        [Given(@"No ambiente ""(.*)"" mapear erro ""(.*)"" para ""(.*)""")]
        public void GivenNoAmbienteErroE(string p0, string p1, string p2)
        {
            Testes.Utils.MapeamentoMensagens.GivenNoAmbienteErroE(p0, p1, p2);
        }

        //o parâmetro é o nome da classe que efetivamente implementa
        [Given(@"Ignorar cenário no ambiente ""(.*)""")]
        new public void GivenIgnorarCenarioNoAmbiente(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.IgnorarCenarioNoAmbiente(p0, this);
            base.GivenIgnorarCenarioNoAmbiente(p0);
        }

        [Given(@"Pedido base com endereço de entrega")]
        new public void GivenPedidoBaseComEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseComEnderecoDeEntrega(this);
            base.GivenPedidoBaseComEnderecoDeEntrega();
        }

        [Then(@"Sem nenhum erro")]
        new public void ThenSemNenhumErro()
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            base.ThenSemNenhumErro();
        }

        [When(@"Lista de itens ""(.*)"" informo ""(.*)"" = ""(.*)""")]
        public void WhenListaDeItensInformo(int numeroItem, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensInformo(numeroItem, campo, valor, this);
            base.ListaDeItensInformo(numeroItem, campo, valor);
        }

        [When(@"Recalcular totais do pedido")]
        public void WhenRecalcularTotaisDoPedido()
        {
            Testes.Utils.LogTestes.LogOperacoes2.RecalcularTotaisDoPedido(this);
            base.RecalcularTotaisDoPedido();
        }
        [When(@"Deixar forma de pagamento consistente")]
        public void WhenDeixarFormaDePagamentoConsistente()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DeixarFormaDePagamentoConsistente(this);
            base.DeixarFormaDePagamentoConsistente();
        }
        [When(@"Lista de itens com ""(.*)"" itens")]
        public void WhenListaDeItensComXitens(int p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.ListaDeItensComXitens(p0, this);
            base.ListaDeItensComXitens(p0);
        }

        [Then(@"Tabela ""t_PEDIDO"" registro pai criado, verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDORegistroPaiCriadoVerificarCampo(string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO", "pedido", "registro pai criado", campo, valor, this);
            base.TabelaT_PEDIDORegistroPaiCriadoVerificarCampo(campo, valor);
        }
        [Then(@"Tabela ""t_PEDIDO_ITEM"" registro criado, verificar item ""(.*)"" campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(int item, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO_ITEM", "pedido", "registro pai criado", campo, valor, this);
            base.TabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(item, campo, valor);
        }

        [Then(@"Tabela ""t_PEDIDO"" registros filhotes criados, verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo(string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO", "pedido", "registros filhotes criados", campo, valor, this);
            base.TabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo(campo, valor);
        }

        private readonly Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida.SplitEstoqueRotinas SplitEstoqueRotinas = new Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida.SplitEstoqueRotinas();
        [Given(@"Zerar todo o estoque")]
        public void GivenZerarTodoOEstoque()
        {
            SplitEstoqueRotinas.ZerarTodoOEstoque();
        }

        [Given(@"Definir saldo de estoque = ""(\d*)"" para produto ""(.*)""")]
        public void GivenDefinirSaldoDeEstoqueParaProduto(int qde, string nomeProduto)
        {
            SplitEstoqueRotinas.DefinirSaldoDeEstoqueParaProdutoComValor(qde, nomeProduto, 987);
        }


        [Given(@"Definir saldo estoque = ""(\d*)"" para produto = ""(.*)"" e id_nfe_emitente = ""(.*)""")]
        public void GivenDefinirSaldoEstoqueEId_Nfe_Emitente(int qtde, string nomeProduto, short id_nfe_emitente)
        {
            SplitEstoqueRotinas.DefinirSaldoDeEstoqueParaProdutoComValorEIdNfeEmitente(qtde, nomeProduto, 666, id_nfe_emitente);
        }

        [Given(@"Usar produto ""(.*)"" como fabricante = ""(.*)"", produto = ""(.*)""")]
        public void GivenUsarProdutoComoFabricanteProduto(string nome, string fabricante, string produto)
        {
            SplitEstoqueRotinas.UsarProdutoComoFabricanteProduto(nome, fabricante, produto);
        }

        #region ultimo acesso
        class UltimoAcessoDados
        {
            public bool Retorno = false;
            public short qtde_estoque_vendido;
            public short qtde_estoque_sem_presenca;
            public List<string> LstErros = new List<string>();
        }
        private readonly UltimoAcessoDados UltimoAcesso = new UltimoAcessoDados();
        #endregion
        [When(@"Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = ""(.*)"", qtde_a_sair = ""(.*)"", qtde_autorizada_sem_presenca = ""(.*)""")]
        public void WhenChamarESTOQUE_PRODUTO_SAIDA_VComProdutoQtde_A_SairQtde_Autorizada_Sem_Presenca(string nomeProduto, int qtde_a_sair, int qtde_autorizada_sem_presenca)
        {
            var id_pedido = "222292N";
            var produto = SplitEstoqueRotinas.Produtos.Produtos[nomeProduto];

            Produto.Estoque.Estoque.QuantidadeEncapsulada qtde_estoque_vendido = new Produto.Estoque.Estoque.QuantidadeEncapsulada
            {
                Valor = UltimoAcesso.qtde_estoque_vendido
            };
            Produto.Estoque.Estoque.QuantidadeEncapsulada qtde_estoque_sem_presenca = new Produto.Estoque.Estoque.QuantidadeEncapsulada
            {
                Valor = UltimoAcesso.qtde_estoque_sem_presenca
            };

            UltimoAcesso.LstErros = new List<string>();
            using var db = SplitEstoqueRotinas.contextoBdProvider.GetContextoGravacaoParaUsing();
            UltimoAcesso.Retorno = Produto.Estoque.Estoque.Estoque_produto_saida_v2(SplitEstoqueRotinas.Id_usuario,
                id_pedido: id_pedido,
                id_nfe_emitente: SplitEstoqueRotinas.Id_nfe_emitente,
                id_fabricante: produto.Fabricante,
                id_produto: produto.Produto,
                qtde_a_sair: qtde_a_sair, qtde_autorizada_sem_presenca: qtde_autorizada_sem_presenca,
                qtde_estoque_vendido: qtde_estoque_vendido, qtde_estoque_sem_presenca: qtde_estoque_sem_presenca,
                lstErros: UltimoAcesso.LstErros,
                dbGravacao: db
                ).Result;

            UltimoAcesso.qtde_estoque_vendido = qtde_estoque_vendido.Valor;
            UltimoAcesso.qtde_estoque_sem_presenca = qtde_estoque_sem_presenca.Valor;

            if (UltimoAcesso.LstErros.Any())
            {
                db.transacao.Rollback();
            }
            else
            {
                db.SaveChanges();
                db.transacao.Commit();
            }

        }


        [Then(@"Tabela ""t_ESTOQUE_MOVIMENTO"" registro pai e produto = ""(.*)"" e estoque = ""(.*)"", verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaRegistroPaiEProdutoVerificarCampo(string produto, string tipo_estoque, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_MOVIMENTO", $"produto {produto} e estoque{tipo_estoque}", "verificar campos", campo, valor, this);
            base.TabelaT_ESTOQUE_MOVIMENTORegistroPaiEProdutoVerificarCampo(produto, tipo_estoque, campo, valor);
        }

        [Then(@"Tabela ""t_ESTOQUE_ITEM"" registro pai e produto = ""(.*)"", verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(string produto, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_ITEM", "produto", "verificar campos", campo, valor, this);
            base.TabelaT_ESTOQUE_ITEMRegistroPaiEProdutoVerificarCampo(produto, campo, valor);
        }

        [Then(@"Tabela ""t_ESTOQUE"" registro pai, verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_ESTOQUERegistroPaiVerificarCampo(string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE", "pedido", "verificar campos", campo, valor, this);
            base.TabelaT_ESTOQUERegistroPaiVerificarCampo(campo, valor);
        }

        [Then(@"Tabela ""t_ESTOQUE_LOG"" pedido gerado e produto = ""(.*)"" e operacao = ""(.*)"", verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(string produto, string operacao, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_ESTOQUE_LOG", "pedido", "verificar campos", campo, valor, this);
            base.TabelaT_ESTOQUE_LOGPedidoGeradoVerificarCampo(produto, operacao, campo, valor);
        }

        [Then(@"Tabela ""t_LOG"" pedido gerado e operacao = ""(.*)"", verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(string operacao, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_LOG", "pedido", "verificar campos", campo, valor, this);
            base.TabelaT_LOGPedidoGeradoEOperacaoVerificarCampo(operacao, campo, valor);
        }

        [Then(@"Tabela t_PRODUTO_X_WMS_REGRA_CD fabricante = ""(.*)"" e produto = ""(.*)"", verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(string fabricante, string produto, string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PRODUTO_X_WMS_REGRA_CD", "fabricante e produto", "verificar campos", campo, valor, this);
            base.TabelaT_PRODUTO_X_WMS_REGRA_CDFabricanteEProdutoVerificarCampo(fabricante, produto, campo, valor);
        }

        [Then(@"Tabela ""t_PEDIDO_ANALISE_ENDERECO"" registro criado, verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDO_ANALISE_ENDERECORegistroCriadoVerificarCampo(string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO_ANALISE_ENDERECO", "campo", "valor", campo, valor, this);
            base.TabelaT_PEDIDO_ANALISE_ENDERECORegistroCriadoVerificarCampo(campo, valor);
        }

        [Then(@"Tabela ""t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO"" registro criado, verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAORegistroCriadoVerificarCampo(string campo, string valor)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.TabelaRegistroComCampoVerificarCampo("t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO", "campo", "valor", campo, valor, this);
            base.TabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAORegistroCriadoVerificarCampo(campo, valor);
        }
        [Given(@"Cadastra 50 pedidos com o mesmo endereco")]
        public void GivenCadastraPedidosComOMesmoEndereco()
        {
            int qtdePedido = 50;
            for (int i = 0; i <= qtdePedido; i++)
            {
                GivenPedidoBase();
                WhenInformo("cnpj_cpf", global::Especificacao.Testes.Utils.CpfCnpj.GerarCpf());
                ThenSemNenhumErro();
            }
        }

        [Then(@"Tabela ""t_PEDIDO"" verificar qtde de pedidos salvos ""(.*)""")]
        public void ThenTabelaT_PEDIDOVerificarQtdeDePedidosSalvos(int qtde)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.Verificacao("Verificar quantidade de pedidos salvos: ", qtde);
            base.VerificarQtdePedidosSalvos(qtde);

        }


        [Then(@"Gerado (.*) pedidos")]
        public void ThenGeradoPedidos(int qtde_pedidos)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.Verificacao("Verificar quantidade de pedidos gerados: ", qtde_pedidos);
            base.GeradoPedidos(qtde_pedidos);
        }

        [Then(@"Pedido gerado ""(.*)"", verificar st_entrega = ""(.*)""")]
        public void ThenPedidoGeradoVerificarSt_EntregaSEP(int indicePedido, string st_entrega)
        {
            //se indicePedido for 0 é pedido pai
            if (indicePedido == 0)
                ThenTabelaT_PEDIDORegistroPaiCriadoVerificarCampo("st_entrega", st_entrega);
            if (indicePedido == 1)
                ThenTabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo("st_entrega", st_entrega);
        }

        [Then(@"Pedido gerado (.*), verificar id_nfe_emitente = (.*) e qde = (.*)")]
        public void ThenPedidoGeradoVerificarId_Nfe_EmitenteEQde(int indicePedido, string id_nfe_emitente, string qtde)
        {
            if (indicePedido == 0)
            {
                ThenTabelaT_PEDIDORegistroPaiCriadoVerificarCampo("id_nfe_emitente", id_nfe_emitente);
                base.TabelaT_PEDIDO_ITEMRegistroCriadoVerificarCampo(1, "qtde", qtde);

            }
            if (indicePedido == 1)
            {
                ThenTabelaT_PEDIDORegistrosFilhotesCriadosVerificarCampo("id_nfe_emitente", id_nfe_emitente);
                base.TabelaT_PEDIDO_ITEMFilhoteRegistroCriadoVerificarCampo(1, "qtde", qtde);
            }
        }

        [Then(@"Verificar estoque id_nfe_emitente = (.*), saldo de estoque = (.*)")]
        public void ThenVerificarEstoqueId_Nfe_EmitenteSaldoDeEstoque(string id_nfe_emitente, int saldo)
        {
            base.TabelaT_ESTOQUE_ITEMVerificarSaldo(id_nfe_emitente, saldo);
        }

        [Then(@"Verificar pedido gerado ""(.*)"", saldo de ID_ESTOQUE_SEM_PRESENCA = ""(.*)""")]
        public void ThenVerificarPedidoGeradoSaldoDeID_ESTOQUE_SEM_PRESENCA(int indicePedido, int qtde)
        {
            base.VerificarPedidoGeradoSaldoDeID_ESTOQUE_SEM_PRESENCA(indicePedido, qtde);
        }

        #region Reiniciar appsettings e AfterScenario
        [Given(@"Reiniciar appsettings")]
        public void GivenReiniciarAppsettings()
        {
            //so pra lembrar que a gente faz isso
            AfterScenario();
        }
        [AfterScenario]
        public void AfterScenario()
        {
            var configuracaoApiMagento = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos().GetRequiredService<global::MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento>();
            Ambiente.ApiMagento.InjecaoDependencias.InicializarConfiguracaoApiMagento(configuracaoApiMagento);
        }
        #endregion

        [Then(@"Tabela ""t_PEDIDO_ANALISE_ENDERECO"" verificar qtde de itens salvos ""(.*)""")]
        public void ThenTabelaT_PEDIDO_ANALISE_ENDERECOVerificarQtdeDeItensSalvos(int qtde)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.Verificacao("Verificar quantidade de itens salvos: ", qtde);
            base.TabelaT_PEDIDO_ANALISE_ENDERECOVerificarQtdeDeItensSalvos(qtde);
        }

        [Then(@"Tabela ""t_PEDIDO_ANALISE_ENDERECO_CONFRONTACAO"" verificar qtde de itens salvos ""(.*)""")]
        public void ThenTabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAOVerificarQtdeDeItensSalvos(int qtde)
        {
            Testes.Utils.LogTestes.LogOperacoes2.BancoDados.Verificacao("Verificar quantidade de itens salvos: ", qtde);
            base.TabelaT_PEDIDO_ANALISE_ENDERECO_CONFRONTACAOVerificarQtdeDeItensSalvos(qtde);
        }



    }
}
