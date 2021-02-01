using Pedido.Dados.Criacao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pedido.Criacao.Passo60.Validacao
{
    class ValidacaoCampos
    {
        private readonly PedidoCriacaoDados Pedido;
        private readonly PedidoCriacaoRetornoDados Retorno;
        private readonly Pedido.Criacao.PedidoCriacao Criacao;
        public ValidacaoCampos(PedidoCriacaoDados pedido, PedidoCriacaoRetornoDados retorno, PedidoCriacao pedidoCriacao)
        {
            this.Pedido = pedido ?? throw new ArgumentNullException(nameof(pedido));
            this.Retorno = retorno ?? throw new ArgumentNullException(nameof(retorno));
            this.Criacao = pedidoCriacao ?? throw new ArgumentNullException(nameof(pedidoCriacao));
        }

        public async Task Executar()
        {
            //todo: validar estes campos e colocar nos .feature estas validações

            /*
             * 
             campo NFe_xPed:
function filtra_nome_identificador() {
var letra;
	letra=String.fromCharCode(window.event.keyCode);
	if ((letra=="|")||(window.event.keyCode==34)||(window.event.keyCode==39)) window.event.keyCode=0;
}


*/
        }

    }
}
