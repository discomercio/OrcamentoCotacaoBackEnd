using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;


namespace Pedido.Criacao.Passo60.Gravacao
{
    class Gravacao : PassoBase
    {
        public Gravacao(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
            : base(pedido, retorno, pedidoCriacao)
        {
        }

        //dados de gravacao
        public class ExecucaoDados
        {
            public string Id_pedido_base = "";
        }
        public ExecucaoDados Execucao = new ExecucaoDados();

        public async Task Executar()
        {
            using var contextoBdGravacao = Criacao.ContextoProvider.GetContextoGravacaoParaUsing();

            //veja Especificacao\Pedido\Passo60\Gravacao\FluxoGravacaoPedido.feature

            //Passo01: Gerar o NSU do pedido (para bloquear transações concorrentes)
            Execucao.Id_pedido_base = await new Grava01.Grava01(contextoBdGravacao, Pedido, Retorno, Criacao).Gerar_id_pedido_base();
            if (Retorno.AlgumErro()) return;

            //Passo10: Fazer todas as validações (documentado em FluxoCriacaoPedido.feature e nos passos dele).
            //portanto, não fazemos o Passo10 aqui

            //Passo15: Verificar pedidos repetidos
            await new Grava15.Grava15(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();
            if (Retorno.AlgumErro()) return;

            //Passo20: LER AS REGRAS DE CONSUMO DO ESTOQUE
            await new Grava20.Grava20(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo25:  VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK - linha 1010
            await new Grava25.Grava25(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo30: OBTÉM DISPONIBILIDADE DO PRODUTO NO ESTOQUE - linha 1083
            await new Grava30.Grava30(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo40: Verifica se a disponibilidade do estoque foi alterada - Linha 1159
            await new Grava40.Grava40(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo50: ANALISA A QUANTIDADE DE PEDIDOS QUE SERÃO CADASTRADOS (AUTO-SPLIT) - linha 1184
            await new Grava50.Grava50(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo55: Contagem de pedidos a serem gravados - Linha 1286
            await new Grava55.Grava55(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo60: criar pedidos
            await new Grava60.Grava60(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo70: ajustes adicionais no pedido pai
            await new Grava70.Grava70(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo80: VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE (POSSÍVEL FRAUDE)
            await new Grava80.Grava80(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //Passo90: log (Passo90/Log.feature)
            await new Grava90.Grava90(contextoBdGravacao, Pedido, Retorno, Criacao).Executar();

            //ainda não!
            //contextoBdGravacao.transacao.Commit();
        }
    }
}
