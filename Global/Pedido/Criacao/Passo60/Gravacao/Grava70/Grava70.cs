using InfraBanco;
using Pedido.Dados.Criacao;
using System;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Gravacao.Grava70
{
    class Grava70 : PassoBaseGravacao
    {
        public Grava70(ContextoBdGravacao contextoBdGravacao, PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao criacao, Execucao.Execucao execucao)
            : base(contextoBdGravacao, pedido, retorno, criacao, execucao)
        {
        }

        public async Task Executar()
        {

            //Passo70: ajustes adicionais no pedido pai
            //	No pedido pai atualiza campos de RA(Passo70/ calcula_total_RA_liquido_BD.feture)

            //	Caso tenha usado algum desconto superior ao limite, liberado pela t_DESCONTO, marca como usado(Passo70/ Senhas_de_autorizacao_para_desconto_superior.feature)

            //	INDICADOR: SE ESTE PEDIDO É COM INDICADOR E O CLIENTE AINDA NÃO TEM UM INDICADOR NO CADASTRO, ENTÃO CADASTRA ESTE. (Passo70 / CadastroIndicador.feature)
        }
    }
}
