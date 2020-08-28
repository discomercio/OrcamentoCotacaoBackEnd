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
        public decimal Preco_Venda { get; set; }// = VlUnitario

        /// <summary>
        /// Preco_NF preço que será impresso na nota fiscal, inclui o rateio do frete
        /// <hr />
        /// </summary>
        [Required]
        public decimal Preco_NF { get; set; } // se permite RA = Preco_Lista / senão VlUnitario

        /*
         * Os campos Preco_Fabricante, CustoFinancFornecCoeficiente, CustoFinancFornecPrecoListaBase e Preco_Fabricante vamos ler das tabelas
         * Os campos Preco_Lista e Desc_Dado serão preenchidos por nós e devemos calcular de forma que fiquem consistentes.
        */

        public static Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados ProdutosDePedidoProdutoMagentoDto(
            PedidoProdutoMagentoDto produtoDto, Produto.Dados.ProdutoDados produtoDados, float coeficiente)
        {
            var ret = new Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados()
            {
                /*
                 * afazer revisar 
                 * 
                Fabricante = produtoDto.Fabricante,
                NumProduto = produtoDto.Produto,
                Qtde = produtoDto.Qtde,
                Permite_Ra_Status = 1,//sempre true
                BlnTemRa = true,
                Preco_fabricante = produtoDados.Preco_lista,
                Preco_RA = produtoDto.Preco_NF,
                Preco_Lista = Math.Round((decimal)(produtoDados.Preco_lista * (decimal)coeficiente), 2),
                Desconto = 0, //produtoDto.Desc_Dado,
                Preco_venda = Math.Round((decimal)(produtoDados.Preco_lista * (decimal)coeficiente), 2),
                TotalItem = Math.Round((decimal)(produtoDto.Preco_Venda * produtoDto.Qtde), 2),
                TotalItemRA = Math.Round((decimal)(produtoDados.Preco_lista * produtoDto.Qtde), 2),
                CustoFinancFornecCoeficiente = coeficiente, 
                Preco_NF = produtoDto.Preco_NF //sempre vai receber Preco_NF, pois do magento sempre permite ra
                */
            };

            return ret;
        }
    }
}
