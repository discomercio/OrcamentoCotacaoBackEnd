﻿using Especificacao.Testes.Utils.BancoTestes;
using InfraBanco;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;
using InfraBanco.Constantes;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida
{
    public class SplitEstoqueRotinas
    {
        public readonly ContextoBdProvider contextoBdProvider;
        public SplitEstoqueRotinas()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            this.contextoBdProvider = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        //os produtos podem ter nomes
        public class FabricanteProdutoDados
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
        public readonly FabricanteProdutoDados Produtos = new FabricanteProdutoDados();

        public void UsarProdutoComoFabricanteProduto(string nome, string fabricante, string produto)
        {
            if (Produtos.Produtos.ContainsKey(nome))
                Produtos.Produtos.Remove(nome);
            Produtos.Produtos.Add(nome, new FabricanteProdutoDados.FabricanteProdutoItem(fabricante: fabricante, produto: produto));
        }

        #region variáveis de acesso ao banco
        private class InsercaoDados
        {
            public int Id_estoque = 0;
        }
        private readonly InsercaoDados Insercao = new InsercaoDados();
        //qual CD usamos apra tstar
        public readonly short Id_nfe_emitente = 4903;
        public readonly string Id_usuario = "TESTE";
        #endregion

        public void ZerarTodoOEstoque()
        {
            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            Insercao.Id_estoque = int.Parse((from c in db.Testoque
                                             select c.Id_estoque).FirstOrDefault());
            GerenciamentoBancoSteps.LimparTabelaDbSet(db.TestoqueMovimento);
            GerenciamentoBancoSteps.LimparTabelaDbSet(db.TestoqueItem);
            GerenciamentoBancoSteps.LimparTabelaDbSet(db.Testoque);
            db.SaveChanges();
            db.transacao.Commit();
        }

        public void DefinirSaldoDeEstoqueParaProdutoComValor(int qde, string nomeProduto, int valor)
        {
            var produto = Produtos.Produtos[nomeProduto];

            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);
            //var idEstoque = (from c in db.Testoques
            //                 where c.Id_nfe_emitente == Id_nfe_emitente
            //                 select c.Id_estoque).FirstOrDefault();

            //GerenciamentoBancoSteps.LimparTabelaDbSet(db.Testoques);

            db.Testoque.Add(new InfraBanco.Modelos.Testoque()
            {
                Id_estoque = Insercao.Id_estoque.ToString(),
                Data_ult_movimento = DateTime.Now,
                Data_entrada = DateTime.Now,
                Id_nfe_emitente = Id_nfe_emitente
            });
            db.TestoqueItem.Add(new InfraBanco.Modelos.TestoqueItem()
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
        public void DefinirSaldoDeEstoqueParaProdutoComValorEIdNfeEmitente(int qde, string nomeProduto, int valor, short id_nfe_emitente)
        {
            var produto = Produtos.Produtos[nomeProduto];

            using var db = contextoBdProvider.GetContextoGravacaoParaUsing(ContextoBdGravacao.BloqueioTControle.NENHUM);

            db.Testoque.Add(new InfraBanco.Modelos.Testoque()
            {
                Id_estoque = Insercao.Id_estoque.ToString(),
                Data_ult_movimento = DateTime.Now,
                Data_entrada = DateTime.Now,
                Id_nfe_emitente = id_nfe_emitente
            });
            db.TestoqueItem.Add(new InfraBanco.Modelos.TestoqueItem()
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

        public void SaldoDeEstoqueParaProdutoComValor(int saldo, string nomeProduto, int valor)
        {
            var produto = Produtos.Produtos[nomeProduto];
            var db = contextoBdProvider.GetContextoLeitura();
            var estoque = (from ei in db.TestoqueItem
                           where ei.Produto == produto.Produto && ei.Fabricante == produto.Fabricante
                            && ei.Preco_fabricante == valor
                           select ei.Qtde - ei.Qtde_utilizada).Sum();
            Assert.Equal(saldo, estoque);
        }

        public void SaldoDeEstoqueParaProduto(int saldo, string nomeProduto)
        {
            var produto = Produtos.Produtos[nomeProduto];
            var db = contextoBdProvider.GetContextoLeitura();
            var estoque = (from ei in db.TestoqueItem
                           where ei.Produto == produto.Produto && ei.Fabricante == produto.Fabricante
                           select ei.Qtde - ei.Qtde_utilizada).Sum();
            Assert.Equal(saldo, estoque);
        }

        public void MovimentoDeEstoqueParaProduto(int movimento, string nomeProduto)
        {
            var produto = Produtos.Produtos[nomeProduto];
            var db = contextoBdProvider.GetContextoLeitura();
            var estoque = (from ei in db.TestoqueMovimento
                           join e in db.Testoque on ei.Id_Estoque equals e.Id_estoque
                           where ei.Produto == produto.Produto && ei.Fabricante == produto.Fabricante
                               && ei.Operacao == Constantes.OP_ESTOQUE_VENDA
                               && ei.Estoque == Constantes.ID_ESTOQUE_VENDIDO
                               && e.Id_nfe_emitente == Id_nfe_emitente
                               && ei.Anulado_Status == 0
                           select (int)(ei.Qtde ?? 0)).Sum();
            Assert.Equal(movimento, estoque);
        }

        public void MovimentoID_ESTOQUE_SEM_PRESENCAParaProduto(int movimento, string nomeProduto)
        {
            var produto = Produtos.Produtos[nomeProduto];
            var db = contextoBdProvider.GetContextoLeitura();
            var estoque = (from ei in db.TestoqueMovimento
                           where ei.Produto == produto.Produto && ei.Fabricante == produto.Fabricante
                               && ei.Operacao == Constantes.OP_ESTOQUE_VENDA
                               && ei.Estoque == Constantes.ID_ESTOQUE_SEM_PRESENCA
                               && ei.Anulado_Status == 0
                           select (int)(ei.Qtde ?? 0)).Sum();
            Assert.Equal(movimento, estoque);
        }
    }
}
