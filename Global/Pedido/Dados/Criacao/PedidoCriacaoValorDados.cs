using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoValorDados
    {
        public PedidoCriacaoValorDados(float perc_RT, decimal vl_total, decimal vl_total_NF, bool permiteRAStatus)
        {
            Perc_RT = perc_RT;
            Vl_total = vl_total;
            Vl_total_NF = vl_total_NF;
            PermiteRAStatus = permiteRAStatus;
        }

        //Armazena o percentual de comissão para o indicador selecionado (reserva técnica).
        //precisa ser alterado no caso da API do magento ou no semi-automático porque no magento esse valor vem de uma variável
        public float Perc_RT { get; set; }

        /*
        No banco de dados, o campo OpcaoPossuiRa é gravado da seguinte forma::
        Quando o indicador está habilitado a fazer uso do RA, indica se o pedido foi cadastrado com a opção de usar ou não o RA:
            'S' = pedido cadastrado informando que o RA será usado.
            'N' = pedido cadastrado informando que o RA não será usado.
            '-' = não se aplica (indicador não está habilitado p/ o uso de RA ou não há indicador cadastrado no pedido)
            Quer dizer, este flag indica o estado do pedido; o PermiteRAStatus é se o indicador tem essa permissão
        */
        public bool PedidoPossuiRa() => Vl_total != Vl_total_NF;

        public decimal Vl_total_RA => Vl_total_NF - Vl_total;

        //Armazena o valor total do pedido
        public decimal Vl_total { get; }

        //Armazena o valor total de pedido com RA
        //Caso o indicador selecionado permita RA esse campo deve receber o valor total do Pedido com RA
        public decimal Vl_total_NF { get; }

        /*
Flag que informa se o indicador pode ou não fazer uso de RA:
	0 = Não permite RA
	1 = Permite RA
*/
        //todo: remover esta variavel
        public bool PermiteRAStatus { get; }
    }
}
