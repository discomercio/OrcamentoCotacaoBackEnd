using Especificacao.Testes.Utils.BancoTestes;
using InfraBanco;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;
using InfraBanco.Constantes;
using Especificacao.Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.EstoqueSaida;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.SplitEstoque
{
    [Binding, Scope(Tag = "Especificacao.Pedido.Passo60.Gravacao.SplitEstoque.ESTOQUE_Produto_Saida_V2_RotinaSteps")]
    public class ESTOQUE_Produto_Saida_V2_RotinaSteps : SplitEstoqueRotinas
    {
        private readonly SplitEstoqueRotinas SplitEstoqueRotinas = new SplitEstoqueRotinas();
        public ESTOQUE_Produto_Saida_V2_RotinaSteps()
        {
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
                id_pedido: SplitEstoqueRotinas.Id_pedido,
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

            //todo: tirar o MultipleActiveResultSets e ver como resolver. tudo funciona, acho que é só o teste que dá erro.
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
        [Given(@"Definir2 saldo de estoque = ""(\d*)"" para produto ""(.*)"" com valor ""(\d*)""")]
        public void GivenDefinirSaldoDeEstoqueParaProdutoComValor(int qde, string nomeProduto, int valor)
        {
            SplitEstoqueRotinas.DefinirSaldoDeEstoqueParaProdutoComValor(qde, nomeProduto, valor);
        }

        [Then(@"Saldo2 de estoque = ""(.*)"" para produto ""(.*)"" com valor ""(.*)""")]
        public void ThenSaldoDeEstoqueParaProdutoComValor(int saldo, string nomeProduto, int valor)
        {
            SplitEstoqueRotinas.SaldoDeEstoqueParaProdutoComValor(saldo, nomeProduto, valor);
        }

        [Then(@"Saldo de estoque = ""(.*)"" para produto ""(.*)""")]
        public void ThenSaldoDeEstoqueParaProduto(int saldo, string nomeProduto)
        {
            SplitEstoqueRotinas.SaldoDeEstoqueParaProduto(saldo, nomeProduto);
        }

        [Then(@"Movimento de estoque = ""(.*)"" para produto ""(.*)""")]
        public void ThenMovimentoDeEstoqueParaProduto(int movimento, string nomeProduto)
        {
            SplitEstoqueRotinas.MovimentoDeEstoqueParaProduto(movimento, nomeProduto);
        }
        [Then(@"Movimento ID_ESTOQUE_SEM_PRESENCA = ""(.*)"" para produto ""(.*)""")]
        public void ThenMovimentoID_ESTOQUE_SEM_PRESENCAParaProduto(int movimento, string nomeProduto)
        {
            SplitEstoqueRotinas.MovimentoID_ESTOQUE_SEM_PRESENCAParaProduto(movimento, nomeProduto);
        }
    }
}
