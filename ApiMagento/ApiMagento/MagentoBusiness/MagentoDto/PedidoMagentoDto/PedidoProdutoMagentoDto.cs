using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PedidoProdutoMagentoDto
    {
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Required]
        [MaxLength(8)]
        public string Produto { get; set; } //  = NumProduto

        [Required]
        public short Qtde { get; set; }

        /// <summary>
        /// Preço de venda do item sem o rateio do frete
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_Venda { get; set; }// = NormalizacaoCampos_Preco_Venda

        /// <summary>
        /// Preco_NF preço que será impresso na nota fiscal, inclui o rateio do frete
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_NF { get; set; } // se permite RA = Preco_Lista / senão NormalizacaoCampos_Preco_Venda

        /*
         * Os campos Preco_Fabricante, CustoFinancFornecCoeficiente, CustoFinancFornecPrecoListaBase e Preco_Fabricante vamos ler das tabelas
         * Os campos Preco_Lista e Desc_Dado serão preenchidos por nós e devemos calcular de forma que fiquem consistentes.
        */

        public static Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados ProdutosDePedidoProdutoMagentoDto(
            PedidoProdutoMagentoDto produtoDto, Produto.Dados.ProdutoDados produtoDados, float coeficiente)
        {
            var ret = new Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados()
            {
                Fabricante = produtoDto.Fabricante,
                NormalizacaoCampos_Produto = produtoDto.Produto,
                Descricao = produtoDados.Descricao,
                Qtde = produtoDto.Qtde,
                Permite_Ra_Status = 1,//sempre true
                BlnTemRa = true,
                NormalizacaoCampos_CustoFinancFornecPrecoListaBase = 0m,
                NormalizacaoCampos_Preco_Lista = Math.Round((decimal)(produtoDados.Preco_lista * (decimal)coeficiente), 2),
                NormalizacaoCampos_Desc_Dado = 0, //produtoDto.Desc_Dado,
                NormalizacaoCampos_Preco_Venda = Math.Round((decimal)(produtoDados.Preco_lista * (decimal)coeficiente), 2),
                Preco_NF = produtoDto.Preco_NF,
                TotalItem = Math.Round((produtoDto.Preco_Venda * produtoDto.Qtde), 2),
                TotalItemRA = Math.Round((produtoDto.Preco_NF * produtoDto.Qtde), 2),
                CustoFinancFornecCoeficiente = coeficiente                
            };

            return ret;
        }
    }
}
