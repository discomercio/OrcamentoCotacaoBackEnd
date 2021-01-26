using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava80
{
    class Grava80 : PassoBaseGravacao
    {
        public Grava80(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao)
            : base(contextoBdGravacao, pedido, retorno, criacao)
        {
        }

        public async Task Executar()
        {


            //Passo80: VERIFICA SE O ENDEREÇO JÁ FOI USADO ANTERIORMENTE POR OUTRO CLIENTE(POSSÍVEL FRAUDE)
            //	Passo80 / FluxoVerificacaoEndereco.feature


        }
    }
}
