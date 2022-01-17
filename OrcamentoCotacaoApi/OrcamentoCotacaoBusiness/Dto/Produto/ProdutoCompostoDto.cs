using PrepedidoBusiness.Dto.Prepedido;
using Produto.Dados;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class ProdutoCompostoDto
    {
        public string PaiFabricante { get; set; }
        public string PaiFabricanteNome { get; set; }
        public string PaiProduto { get; set; }
        public decimal Preco_total_Itens { get; set; }
        public List<ProdutoFilhoDto> Filhos { get; set; }

        internal static List<ProdutoCompostoDto> ProdutoCompostoDtoLista_De_ProdutoCompostoDados(List<ProdutoCompostoDados> produtoCompostoDados)
        {
            if (produtoCompostoDados == null) return null;
            var ret = new List<ProdutoCompostoDto>();
            if (produtoCompostoDados != null)
                foreach (var p in produtoCompostoDados)
                    ret.Add(ProdutoCompostoDto_De_ProdutoCompostoDados(p));
            return ret;
        }

        private static ProdutoCompostoDto ProdutoCompostoDto_De_ProdutoCompostoDados(ProdutoCompostoDados p)
        {
            if (p == null) return null;
            return new ProdutoCompostoDto()
            {
                PaiFabricante = p.PaiFabricante,
                PaiFabricanteNome = p.PaiFabricanteNome,
                PaiProduto = p.PaiProduto,
                Preco_total_Itens = p.Preco_total_Itens,
                Filhos = ProdutoFilhoDto.ProdutoFilhoDtoLista_De_ProdutoFilhoDados(p.Filhos)
            };
        }
    }
}
