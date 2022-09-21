using Produto.Dados;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prepedido.Dto
{
    public class ProdutoFilhoDto
    {
        public string Fabricante { get; set; }
        public string Fabricante_Nome { get; set; }
        public string Produto { get; set; }
        public int Qtde { get; set; }

        internal static List<ProdutoFilhoDto> ProdutoFilhoDtoLista_De_ProdutoFilhoDados(List<ProdutoFilhoDados> filhos)
        {
            if (filhos == null) return null;
            var ret = new List<ProdutoFilhoDto>();
            if (filhos != null)
                foreach (var p in filhos)
                    ret.Add(ProdutoFilhoDto_De_ProdutoFilhoDados(p));
            return ret;
        }

        internal static ProdutoFilhoDto ProdutoFilhoDto_De_ProdutoFilhoDados(ProdutoFilhoDados p)
        {
            if (p == null) return null;
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
