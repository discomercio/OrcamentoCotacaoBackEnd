﻿using Produto.Dados;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrepedidoBusiness.Dto.Produto
{
    public class ProdutoComboDto
    {
        public List<ProdutoDto> ProdutoDto { get; set; }
        public List<ProdutoCompostoDto> ProdutoCompostoDto { get; set; }

        internal static ProdutoComboDto ProdutoComboDto_De_ProdutoComboDados(ProdutoComboDados aux)
        {
            if (aux == null) return null;
            return new ProdutoComboDto()
            {
                ProdutoDto = PrepedidoBusiness.Dto.Produto.ProdutoDto.ProdutoDtoLista_De_ProdutoDados(aux.ProdutoDados),
                ProdutoCompostoDto = PrepedidoBusiness.Dto.Produto.ProdutoCompostoDto.ProdutoCompostoDtoLista_De_ProdutoCompostoDados(aux.ProdutoCompostoDados)
            };
        }
    }
}
