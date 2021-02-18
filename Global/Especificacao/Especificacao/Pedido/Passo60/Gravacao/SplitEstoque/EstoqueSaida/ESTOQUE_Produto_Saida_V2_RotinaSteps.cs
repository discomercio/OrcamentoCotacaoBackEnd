﻿using Especificacao.Testes.Utils.BancoTestes;
using InfraBanco;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;
using InfraBanco.Constantes;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.SplitEstoque
{
    [Binding, Scope(Tag = "Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.ESTOQUE_Produto_Saida_V2_RotinaSteps")]
    public class ESTOQUE_Produto_Saida_V2_RotinaSteps
    {
        private readonly ContextoBdProvider contextoBdProvider;
        public ESTOQUE_Produto_Saida_V2_RotinaSteps()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            this.contextoBdProvider = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        //os produtos podem ter nomes
        private class FabricanteProdutoDados
        {
            public class FabricanteProdutoItem
            {
                public readonly string Fabricante;
                public readonly string Produto;
                public FabricanteProdutoItem(string fabricante, string produto)
                {
                    Fabricante = fabricante ?? throw new ArgumentNullException(nameof(fabricante));
                    Produto = produto ?? throw new ArgumentNullException(nameof(produto));
                }
            }

            //um dicionário com os nomes
            public Dictionary<string, FabricanteProdutoItem> Produtos = new Dictionary<string, FabricanteProdutoItem>();
        }
        private FabricanteProdutoDados Produtos = new FabricanteProdutoDados();

        [Given(@"Usar produto ""(.*)"" como fabricante = ""(.*)"", produto = ""(.*)""")]
        public void GivenUsarProdutoComoFabricanteProduto(string nome, string fabricante, string produto)
        {
            Produtos.Produtos.Add(nome, new FabricanteProdutoDados.FabricanteProdutoItem(fabricante: fabricante, produto: produto));
        }

        #region variáveis de acesso ao banco
        private class InsercaoDados
        {
            public int Id_estoque = 0;
        }
        private InsercaoDados Insercao = new InsercaoDados();
        //qual CD usamos apra tstar
        private short Id_nfe_emitente = 4903;
        private string Id_usuario = "TESTE";
        private string Id_pedido = "222292N";
        #endregion

        #region ultimo acesso
        class UltimoAcessoDados
        {
            public bool Retorno = false;
            public short qtde_estoque_vendido;
            public short qtde_estoque_sem_presenca;
            public List<string> LstErros = new List<string>();
        }
        private UltimoAcessoDados UltimoAcesso = new UltimoAcessoDados();
        #endregion

        [Given(@"Zerar todo o estoque")]
        public void GivenZerarTodoOEstoque()
        {
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            GerenciamentoBancoSteps.LimparTabelaDbSet(db.TestoqueMovimentos);
            GerenciamentoBancoSteps.LimparTabelaDbSet(db.TestoqueItems);
            GerenciamentoBancoSteps.LimparTabelaDbSet(db.Testoques);
            db.SaveChanges();
            db.transacao.Commit();
        }
        [Given(@"Definir saldo de estoque = ""(\d*)"" para produto ""(.*)""")]
        public void GivenDefinirSaldoDeEstoqueParaProduto(int qde, string nomeProduto)
        {
            GivenDefinirSaldoDeEstoqueParaProdutoComValor(qde, nomeProduto, 987);
        }
        [Given(@"Definir2 saldo de estoque = ""(\d*)"" para produto ""(.*)"" com valor ""(\d*)""")]
        public void GivenDefinirSaldoDeEstoqueParaProdutoComValor(int qde, string nomeProduto, int valor)
        {
            var produto = Produtos.Produtos[nomeProduto];

            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            db.Testoques.Add(new InfraBanco.Modelos.Testoque()
            {
                Id_estoque = Insercao.Id_estoque.ToString(),
                Data_ult_movimento = DateTime.Now,
                Id_nfe_emitente = Id_nfe_emitente
            });
            db.TestoqueItems.Add(new InfraBanco.Modelos.TestoqueItem()
            {
                Fabricante = produto.Fabricante,
                Produto = produto.Produto,
                Qtde = (short)qde,
                Preco_fabricante = valor,
                Qtde_utilizada = 0,
                Id_estoque = Insercao.Id_estoque.ToString(),
                Data_ult_movimento = DateTime.Now
            }); ;


            Insercao.Id_estoque++;

            db.SaveChanges();
            db.transacao.Commit();
        }

        [When(@"Chamar ESTOQUE_PRODUTO_SAIDA_V2 com produto = ""(.*)"", qtde_a_sair = ""(.*)"", qtde_autorizada_sem_presenca = ""(.*)""")]
        public void WhenChamarESTOQUE_PRODUTO_SAIDA_VComProdutoQtde_A_SairQtde_Autorizada_Sem_Presenca(string nomeProduto, int qtde_a_sair, int qtde_autorizada_sem_presenca)
        {
            var produto = Produtos.Produtos[nomeProduto];

            Produto.Estoque.Estoque.QuantidadeEncapsulada qtde_estoque_vendido = new Produto.Estoque.Estoque.QuantidadeEncapsulada();
            qtde_estoque_vendido.Valor = UltimoAcesso.qtde_estoque_vendido;
            Produto.Estoque.Estoque.QuantidadeEncapsulada qtde_estoque_sem_presenca = new Produto.Estoque.Estoque.QuantidadeEncapsulada();
            qtde_estoque_sem_presenca.Valor = UltimoAcesso.qtde_estoque_sem_presenca;

            using var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            UltimoAcesso.Retorno = Produto.Estoque.Estoque.Estoque_produto_saida_v2(Id_usuario,
                id_pedido: Id_pedido,
                id_nfe_emitente: Id_nfe_emitente,
                id_fabricante: produto.Fabricante,
                id_produto: produto.Produto,
                qtde_a_sair: qtde_a_sair, qtde_autorizada_sem_presenca: qtde_autorizada_sem_presenca,
                qtde_estoque_vendido: qtde_estoque_vendido, qtde_estoque_sem_presenca: qtde_estoque_sem_presenca,
                lstErros: UltimoAcesso.LstErros,
                dbGravacao: db
                ).Result;

            UltimoAcesso.qtde_estoque_vendido = qtde_estoque_vendido.Valor;
            UltimoAcesso.qtde_estoque_sem_presenca = qtde_estoque_sem_presenca.Valor;

            db.SaveChanges();
            db.transacao.Commit();
        }

        [Then(@"Retorno sucesso, qtde_estoque_vendido = ""(.*)"", qtde_estoque_sem_presenca = ""(.*)""")]
        public void ThenRetornoSucessoQtde_Estoque_VendidoQtde_Estoque_Sem_Presenca(int qtde_estoque_vendido, int qtde_estoque_sem_presenca)
        {
            Assert.True(UltimoAcesso.Retorno);
            Assert.Empty(UltimoAcesso.LstErros);
            Assert.Equal(qtde_estoque_vendido, UltimoAcesso.qtde_estoque_vendido);
            Assert.Equal(qtde_estoque_sem_presenca, UltimoAcesso.qtde_estoque_sem_presenca);
        }

        [Then(@"msg_erro ""(.*)""")]
        public void ThenMsg_Erro(string msg)
        {
            Assert.False(UltimoAcesso.Retorno);
            Assert.Contains(msg, UltimoAcesso.LstErros[0]);
        }

        [Then(@"Saldo2 de estoque = ""(.*)"" para produto ""(.*)"" com valor ""(.*)""")]
        public void ThenSaldoDeEstoqueParaProdutoComValor(int saldo, string nomeProduto, int valor)
        {
            var produto = Produtos.Produtos[nomeProduto];
            var db = contextoBdProvider.GetContextoLeitura();
            var estoque = (from ei in db.TestoqueItems
                           where ei.Produto == produto.Produto && ei.Fabricante == produto.Fabricante
                            && ei.Preco_fabricante == valor
                           select ei.Qtde - ei.Qtde_utilizada).Sum();
            Assert.Equal(saldo, estoque);
        }

        [Then(@"Saldo de estoque = ""(.*)"" para produto ""(.*)""")]
        public void ThenSaldoDeEstoqueParaProduto(int saldo, string nomeProduto)
        {
            var produto = Produtos.Produtos[nomeProduto];
            var db = contextoBdProvider.GetContextoLeitura();
            var estoque = (from ei in db.TestoqueItems
                           where ei.Produto == produto.Produto && ei.Fabricante == produto.Fabricante
                           select ei.Qtde - ei.Qtde_utilizada).Sum();
            Assert.Equal(saldo, estoque);
        }

        [Then(@"Movimento de estoque = ""(.*)"" para produto ""(.*)""")]
        public void ThenMovimentoDeEstoqueParaProduto(int movimento, string nomeProduto)
        {
            var produto = Produtos.Produtos[nomeProduto];
            var db = contextoBdProvider.GetContextoLeitura();
            var estoque = (from ei in db.TestoqueMovimentos
                           join e in db.Testoques on ei.Id_Estoque equals e.Id_estoque
                           where ei.Produto == produto.Produto && ei.Fabricante == produto.Fabricante
                               && ei.Operacao == Constantes.OP_ESTOQUE_VENDA
                               && ei.Estoque == Constantes.ID_ESTOQUE_VENDIDO
                               && e.Id_nfe_emitente == Id_nfe_emitente
                               && ei.Anulado_Status == 0
                           select (int)(ei.Qtde ?? 0)).Sum();
            Assert.Equal(movimento, estoque);
        }
        [Then(@"Movimento ID_ESTOQUE_SEM_PRESENCA = ""(.*)"" para produto ""(.*)""")]
        public void ThenMovimentoID_ESTOQUE_SEM_PRESENCAParaProduto(int movimento, string nomeProduto)
        {
            var produto = Produtos.Produtos[nomeProduto];
            var db = contextoBdProvider.GetContextoLeitura();
            var estoque = (from ei in db.TestoqueMovimentos
                           where ei.Produto == produto.Produto && ei.Fabricante == produto.Fabricante
                               && ei.Operacao == Constantes.OP_ESTOQUE_VENDA
                               && ei.Estoque == Constantes.ID_ESTOQUE_SEM_PRESENCA
                               && ei.Anulado_Status == 0
                           select (int)(ei.Qtde ?? 0)).Sum();
            Assert.Equal(movimento, estoque);
        }
    }
}