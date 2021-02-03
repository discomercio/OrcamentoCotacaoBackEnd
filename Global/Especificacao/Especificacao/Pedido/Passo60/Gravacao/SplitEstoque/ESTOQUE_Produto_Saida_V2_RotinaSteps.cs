using Especificacao.Testes.Utils.BancoTestes;
using InfraBanco;
using Microsoft.Extensions.DependencyInjection;
using System;
using TechTalk.SpecFlow;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.SplitEstoque
{
    [Binding]
    public class ESTOQUE_Produto_Saida_V2_RotinaSteps
    {
        private readonly ContextoBdProvider contextoBdProvider;
        public ESTOQUE_Produto_Saida_V2_RotinaSteps()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            this.contextoBdProvider = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
        }
        [Given(@"Zerar todo o estoque")]
        public void GivenZerarTodoOEstoque()
        { 
            var db = contextoBdProvider.GetContextoGravacaoParaUsing();
            GerenciamentoBancoSteps.LimparTabelaDbSet(db.TestoqueItems);
            GerenciamentoBancoSteps.LimparTabelaDbSet(db.Testoques);
            db.SaveChanges();
        }


        private string produtoUmfabricante;
        private string produtoUmproduto;

        [Given(@"Usar produto ""um"" como fabricante = ""(.*)"", produto = ""(.*)""")]
        public void GivenUsarProdutoComoFabricanteProduto(string fabricante , string produto)
        {
            produtoUmfabricante = fabricante;
            produtoUmproduto = produto;
        }

    }
}
