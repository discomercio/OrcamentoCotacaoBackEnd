using System;
using Xunit;

/*
 * s� para registrar o status
 * 
 * 
 * Pedido: 
 *      feito loja/resumo.asp
 *      feito loja/ClienteEdita.asp
 *      feito loja/PedidoNovoProdCompostoMask.asp
 *      feito loja/PedidoNovo.asp 
 *      feito loja/PedidoNovoConsiste.asp 
 * 
    AFAZER: TODO: onde salva: loja/PedidoNovoConfirma.asp linha 1747

Valida��es ao salvar:
1:
if Not ESTOQUE_produto_saida_v2(usuario, id_pedido_temp, vEmpresaAutoSplit(iv), .fabricante, .produto, vProdRegra(iRegra).regra.regraUF.regraPessoa.vCD(iCD).estoque.qtde_solicitada, qtde_spe, qtde_estoque_vendido_aux, qtde_estoque_sem_presenca_aux, msg_erro) then
2:
								alerta = "Senha de autoriza��o para desconto superior n�o encontrado."

* * 
 * */

namespace Especificacao
{
    public class Status
    {
        public void Test1()
        {

        }
    }
}



/*
 * todo: melhorar as dependencias
todo: implementar cadastro do prepedidoapi

 * todo: fazer valida��o dos dados cadastrais na API (no ERP, estas mensagens devem estar separadas pq vamos falar para o usu�rio editar o cliente antes de continuar)
* 
* 
 * 
*/
