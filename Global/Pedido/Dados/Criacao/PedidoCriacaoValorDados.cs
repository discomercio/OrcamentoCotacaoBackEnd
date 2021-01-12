using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace Pedido.Dados.Criacao
{
    public class PedidoCriacaoValorDados
    {
        public PedidoCriacaoValorDados(float percRT, bool opcaoPossuiRa, decimal vl_total, decimal vl_total_NF, short permiteRAStatus)
        {
            PercRT = percRT;
            OpcaoPossuiRa = opcaoPossuiRa;
            Vl_total = vl_total;
            Vl_total_NF = vl_total_NF;
            PermiteRAStatus = permiteRAStatus;
        }

        //Armazena o percentual de comissão para o indicador selecionado
        public float PercRT { get; }

        //Armazena "S" ou "N" para caso de o indicador selecionado permita RA
        public bool OpcaoPossuiRa { get; }

        //Armazena o valor total do pedido
        public decimal Vl_total { get; }

        //Armazena o valor total de pedido com RA
        //Caso o indicador selecionado permita RA esse campo deve receber o valor total do Pedido com RA
        public decimal Vl_total_NF { get; }

        public short PermiteRAStatus { get; }
    }
}
