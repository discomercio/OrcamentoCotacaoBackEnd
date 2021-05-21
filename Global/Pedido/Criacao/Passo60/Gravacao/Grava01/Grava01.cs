using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava01
{
    class Grava01 : PassoBaseGravacao
    {
        public Grava01(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao, Execucao.Gravacao gravacao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao, gravacao)
        {
        }

        public async Task<string> Gerar_id_pedido_base()
        {
            //Passo01: Gerar o NSU do pedido(para bloquear transações concorrentes)
            var idPedidoBase = await global::Pedido.Criacao.Passo60.Gravacao.Grava60.Gera_num_pedido.Gera_num_pedido_pai(Retorno.ListaErros, ContextoBdGravacao);
            return idPedidoBase;
        }
    }
}
