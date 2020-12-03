using Prepedido.PedidoVisualizacao.Dados.DetalhesPedido;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.PedidoUnisDto
{
    public class ProdutoDevolvidoUnisDto
    {
        public DateTime? Data { get; set; }
        public string Hora { get; set; }
        public short? Qtde { get; set; }
        public string CodProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string Motivo { get; set; }
        public int NumeroNF { get; set; }


        public static ProdutoDevolvidoUnisDto ProdutoDevolvidoUnisDto_De_ProdutoDevolvidoPedidoDados(ProdutoDevolvidoPedidoDados origem)
        {
            if (origem == null) return null;
            return new ProdutoDevolvidoUnisDto()
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
        public static List<ProdutoDevolvidoUnisDto> ListaProdutoDevolvidoUnisDto_De_ProdutoDevolvidoPedidoDados(IEnumerable<ProdutoDevolvidoPedidoDados> listaBancoDados)
        {
            if (listaBancoDados == null) return null;
            var ret = new List<ProdutoDevolvidoUnisDto>();
            if (listaBancoDados != null)
                foreach (var p in listaBancoDados)
                    ret.Add(ProdutoDevolvidoUnisDto_De_ProdutoDevolvidoPedidoDados(p));
            return ret;
        }

    }
}
