using Produto.ProdutoDados;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class ProdutoComboDto
    {
        public List<ProdutoDto> ProdutoDto { get; set; }
        public List<ProdutoCompostoDto> ProdutoCompostoDto { get; set; }

        internal static ProdutoComboDto ProdutoComboDtoDeProdutoComboDados(ProdutoComboDados aux)
        {
            return new ProdutoComboDto()
            {
                ProdutoDto = PrepedidoBusiness.Dto.Produto.ProdutoDto.ProdutoDtoListaDeProdutoDados(aux.ProdutoDados),
                ProdutoCompostoDto = PrepedidoBusiness.Dto.Produto.ProdutoCompostoDto.ProdutoCompostoDtoListaDeProdutoCompostoDados(aux.ProdutoCompostoDados)
            };
        }
    }
}
