using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Bll.Dto.PedidoDto.DetalhesPedido
{
    public class ProdutoDevolvidoDtoPedido
    {
        public DateTime? Data { get; set; }
        public string Hora { get; set; }
        public short? Qtde { get; set; }
        public string CodProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string Motivo { get; set; }
        public int NumeroNF { get; set; }


        public static ProdutoDevolvidoDtoPedido ProdutoDevolvidoDtoPedido_De_ProdutoDevolvidoPedidoDados(ProdutoDevolvidoPedidoDados origem)
        {
            if (origem == null) return null;
            return new ProdutoDevolvidoDtoPedido()
            {
                Data = origem.Data,
                Hora = origem.Hora,
                Qtde = origem.Qtde,
                CodProduto = origem.CodProduto,
                DescricaoProduto = origem.DescricaoProduto,
                Motivo = origem.Motivo,
                NumeroNF = origem.NumeroNF
            };
        }
        public static List<ProdutoDevolvidoDtoPedido> ListaProdutoDevolvidoDtoPedido_De_ProdutoDevolvidoPedidoDados(IEnumerable<ProdutoDevolvidoPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ProdutoDevolvidoDtoPedido>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ProdutoDevolvidoDtoPedido_De_ProdutoDevolvidoPedidoDados(p));
            return ret;
        }
    }
}
