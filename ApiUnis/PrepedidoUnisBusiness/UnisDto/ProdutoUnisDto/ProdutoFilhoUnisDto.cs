using Produto.Dados;
using System.Collections.Generic;

namespace PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto
{
    public class ProdutoFilhoUnisDto : Prepedido.Dto.ProdutoFilhoDto
    {
        internal static List<ProdutoFilhoUnisDto> ProdutoFilhoUnisDtoListaDeProdutoFilhoDados(List<ProdutoFilhoDados> filhos)
        {
            var ret = new List<ProdutoFilhoUnisDto>();
            if (filhos != null)
                foreach (var p in filhos)
                    ret.Add(ProdutoFilhoDtoDeProdutoFilhoDados(p));
            return ret;
        }

        internal static ProdutoFilhoUnisDto ProdutoFilhoDtoDeProdutoFilhoDados(ProdutoFilhoDados p)
        {
            return new ProdutoFilhoUnisDto()
            {
                Fabricante = p.Fabricante,
                Fabricante_Nome = p.Fabricante_Nome,
                Produto = p.Produto,
                Qtde = p.Qtde
            };
        }

    }
}
