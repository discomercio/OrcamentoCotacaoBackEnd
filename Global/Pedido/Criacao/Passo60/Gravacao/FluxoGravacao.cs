using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;


namespace Pedido.Criacao.Passo60.Gravacao
{
    class FluxoGravacao : PassoBase
    {
        public FluxoGravacao(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
            : base(pedido, retorno, pedidoCriacao)
        {
        }

        public async Task Executar()
        {
            var Execucao = Criacao.Execucao;
            var Gravacao = Criacao.Gravacao;
            using var contextoBdGravacao = Criacao.ContextoProvider.GetContextoGravacaoParaUsing(InfraBanco.ContextoBdGravacao.BloqueioTControle.XLOCK_SYNC_PEDIDO);

            //veja Especificacao\Pedido\Passo60\Gravacao\FluxoGravacaoPedido.feature

            //Passo01: Gerar o NSU do pedido (para bloquear transações concorrentes)
            Gravacao.Id_pedido_base = await new Grava01.Grava01(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).Gerar_id_pedido_base();
            if (Retorno.AlgumErro()) return;

            //Passo10: Fazer todas as validações (documentado em FluxoCriacaoPedido.feature e nos passos dele).
            //portanto, não fazemos o Passo10 aqui

            //Passo15: Verificar pedidos repetidos
            await new Grava15.Grava15(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).ExecutarAsync();
            if (Retorno.AlgumErro()) return;

            //Passo17: Marcar t_DESCONTO que forem usados
            await new Grava17.Grava17(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).ExecutarAsync();
            if (Retorno.AlgumErro()) return;

            //Passo20: LER AS REGRAS DE CONSUMO DO ESTOQUE
            Gravacao.ListaRegrasControleEstoque = await new Grava20.Grava20(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).ExecutarAsync();

            //estou alterando o local dessa validação, pois nos testes caso tenha erro ao obter o ctrl de 
            //estoque, estamos tendo erro de null na lista "regraCrtlEstoque"
            //se tiver erro, nao continua
            if (Retorno.AlgumErro()) return;

            //Passo25:  VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK - linha 1010
            await new Grava25.Grava25(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).ExecutarAsync();

            //Passo30: OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE - linha 1083
            await new Grava30.Grava30(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).ExecutarAsync();

            //Passo40: Verifica se a disponibilidade do estoque foi alterada - Linha 1159
            new Grava40.Grava40(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).Executar();

            //Passo50: ANALISA A QUANTIDADE DE PEDIDOS QUE SERÃO CADASTRADOS (AUTO-SPLIT) - linha 1184
            new Grava50.Grava50(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).Executar();

            //Passo55: Contagem de pedidos a serem gravados - Linha 1286
            new Grava55.Grava55(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).Executar();

            //Passo60: criar pedidos
            //se tiver erro, nao continua
            if (Retorno.AlgumErro()) return;
            await new Grava60.Grava60(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).ExecutarAsync();

            //Passo70: ajustes adicionais no pedido pai
            //o passo70 é feito dentro do passo60

            //Passo80: VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)
            await new Grava80.Grava80(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).ExecutarAsync();

            //Passo90: log (Passo90/Log.feature)
            await new Grava90.Grava90(contextoBdGravacao, Pedido, Retorno, Criacao, Execucao, Gravacao).ExecutarAsync();


            //se tiver erro, nao continua
            if (Retorno.AlgumErro())
            {
                contextoBdGravacao.transacao.Rollback();
                return;
            }

            await contextoBdGravacao.SaveChangesAsync();
            contextoBdGravacao.transacao.Commit();
        }
    }
}
