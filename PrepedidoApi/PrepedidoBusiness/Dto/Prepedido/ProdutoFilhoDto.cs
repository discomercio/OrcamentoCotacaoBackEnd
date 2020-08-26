using Produto.ProdutoDados;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Prepedido
{
    public class ProdutoFilhoDto
    {
        public string Fabricante { get; set; }
        public string Fabricante_Nome { get; set; }
        public string Produto { get; set; }
        public int Qtde { get; set; }

        internal static List<ProdutoFilhoDto> ProdutoFilhoDtoListaDeProdutoFilhoDados(List<ProdutoFilhoDados> filhos)
        {
            var ret = new List<ProdutoFilhoDto>();
            if (filhos != null)
                foreach (var p in filhos)
                    ret.Add(ProdutoFilhoDtoDeProdutoFilhoDados(p));
            return ret;
        }

        internal static ProdutoFilhoDto ProdutoFilhoDtoDeProdutoFilhoDados(ProdutoFilhoDados p)
        {
            return new ProdutoFilhoDto()
            {
                Fabricante = p.Fabricante,
                Fabricante_Nome = p.Fabricante_Nome,
                Produto = p.Produto,
                Qtde = p.Qtde
            };
        }
    }
}
