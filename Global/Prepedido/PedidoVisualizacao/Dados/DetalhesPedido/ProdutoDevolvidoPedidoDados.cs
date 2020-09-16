using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.PedidoVisualizacao.Dados.DetalhesPedido
{
    public class ProdutoDevolvidoPedidoDados
    {
        /*
         * data
         * hora
         * qtde = unidade
         * produto = código do produto
         * descrição do produto
         * motivo se tiver motivo
         * numero da nota fiscal
         */

        public DateTime? Data { get; set; }
        public string Hora { get; set; }
        public short? Qtde { get; set; }
        public string CodProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string Motivo { get; set; }
        public int NumeroNF { get; set; }
    }
}
