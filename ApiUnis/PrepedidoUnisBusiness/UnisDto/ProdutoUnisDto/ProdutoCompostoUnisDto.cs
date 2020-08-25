using Produto.ProdutoDados;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto
{
    public class ProdutoCompostoUnisDto
    {
        public string PaiFabricante { get; set; }
        public string PaiFabricanteNome { get; set; }
        public string PaiProduto { get; set; }
        public decimal Preco_total_Itens { get; set; }
        public List<ProdutoFilhoUnisDto> Filhos { get; set; }

        internal static List<ProdutoCompostoUnisDto> ProdutoCompostoUnisDtoListaDeProdutoCompostoDados(List<ProdutoCompostoDados> produtoCompostoDados)
        {
            var ret = new List<ProdutoCompostoUnisDto>();
            foreach (var p in produtoCompostoDados)
                ret.Add(PrepedidoUnisBusiness.UnisDto.ProdutoUnisDto.ProdutoCompostoUnisDto.ProdutoCompostoUnisDtoDeProdutoCompostoDados(p));
            return ret;
        }

        internal static ProdutoCompostoUnisDto ProdutoCompostoUnisDtoDeProdutoCompostoDados(ProdutoCompostoDados p)
        {
            return new ProdutoCompostoUnisDto()
            {
                PaiFabricante = p.PaiFabricante,
                PaiFabricanteNome = p.PaiFabricanteNome,
                PaiProduto = p.PaiProduto,
                Preco_total_Itens = p.Preco_total_Itens,
                Filhos = ProdutoFilhoUnisDto.ProdutoFilhoUnisDtoListaDeProdutoFilhoDados(p.Filhos)
            };
        }
    }
}
