using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava25
{
    class Grava25 : PassoBaseGravacao
    {
        public Grava25(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao)
            : base(contextoBdGravacao, pedido, retorno, criacao)
        {
        }

        public async Task Executar()
        {

            //Passo25:  
            //	VERIFICA SE AS REGRAS ASSOCIADAS AOS PRODUTOS ESTÃO OK - linha 1010
            //		Verifica se todos os produtos possuem regra ativa e não bloqueada e ao menos um CD ativo.
            //	'NO CASO DE SELEÇÃO MANUAL DO CD, VERIFICA SE O CD SELECIONADO ESTÁ HABILITADO EM TODAS AS REGRAS - linha 1047
            //		No caso de CD manual, verifica se o CD tem regra ativa

        }
    }
}
