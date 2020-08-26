using PrepedidoBusiness.Dto.Prepedido;
using Produto.ProdutoDados;
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

        internal static List<ProdutoCompostoDto> ProdutoCompostoDtoListaDeProdutoCompostoDados(List<ProdutoCompostoDados> produtoCompostoDados)
        {
            var ret = new List<ProdutoCompostoDto>();
            if (produtoCompostoDados != null)
                foreach (var p in produtoCompostoDados)
                    ret.Add(ProdutoCompostoDtoDeProdutoCompostoDados(p));
            return ret;
        }

        private static ProdutoCompostoDto ProdutoCompostoDtoDeProdutoCompostoDados(ProdutoCompostoDados p)
        {
            return new ProdutoCompostoDto()
            {
                PaiFabricante = p.PaiFabricante,
                PaiFabricanteNome = p.PaiFabricanteNome,
                PaiProduto = p.PaiProduto,
                Preco_total_Itens = p.Preco_total_Itens,
                Filhos = ProdutoFilhoDto.ProdutoFilhoDtoListaDeProdutoFilhoDados(p.Filhos)
            };
        }
    }
}
