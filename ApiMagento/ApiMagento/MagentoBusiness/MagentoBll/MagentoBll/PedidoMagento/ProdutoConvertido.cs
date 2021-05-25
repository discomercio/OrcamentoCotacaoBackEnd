using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.MagentoBll.MagentoBll.PedidoMagento
{
    class ProdutoConvertido
    {
        public string Fabricante { get; set; }
        public string Produto { get; set; } //  = NumProduto
        public short Qtde { get; set; }
        public decimal Preco_Venda { get; set; }
        public decimal Preco_NF { get; set; }

        /*
         * Os campos Preco_Fabricante, CustoFinancFornecCoeficiente, CustoFinancFornecPrecoListaBase e Preco_Fabricante vamos ler das tabelas
         * Os campos Preco_Lista e Desc_Dado serão preenchidos por nós e devemos calcular de forma que fiquem consistentes.
        */

        public static Pedido.Dados.Criacao.PedidoCriacaoProdutoDados PedidoCriacaoProdutoDados_De_ProdutoConvertido(
            ProdutoConvertido produtoDto, Produto.Dados.ProdutoDados produtoDados, float coeficiente)
        {
            //y.Preco_Venda = Math.Round(y.Preco_Lista * (decimal)(1 - y.Desc_Dado / 100), 2);
            var preco_Lista = Math.Round((decimal)((produtoDados.Preco_lista ?? 0) * (decimal)coeficiente), 2);
            if (preco_Lista == 0)
                preco_Lista = 0.01M;
            var desc_Dado = 100M * (1M - (produtoDto.Preco_Venda / preco_Lista));
            var ret = new Pedido.Dados.Criacao.PedidoCriacaoProdutoDados(
                fabricante: produtoDto.Fabricante,
                produto: produtoDto.Produto,
                qtde: produtoDto.Qtde,
                custoFinancFornecPrecoListaBase_Conferencia: produtoDados.Preco_lista ?? 0,
                preco_Lista: preco_Lista,
                desc_Dado: (float)Convert.ToDouble(desc_Dado),
                preco_Venda: produtoDto.Preco_Venda,
                preco_NF: produtoDto.Preco_NF,
                custoFinancFornecCoeficiente_Conferencia: coeficiente,
                //no magento não validamos se o estoque mudou
                qtde_spe_usuario_aceitou: null
            );

            return ret;
        }

        /* DETALHES DE COMO FAZER OS CÁLCULOS
         * ==================================
         * preco_nf = RowTotal / Quantidade
         * preco_lista = Subtotal / Quantidade
         * desc_dado = 100 * (preco_lista - preco_venda) / preco_lista
         */
        public static ProdutoConvertido ProdutoConvertido_De_PedidoProdutoMagentoDto(
            PedidoProdutoMagentoDto produtoDto, Produto.Dados.ProdutoDados produtoDados, float coeficiente)
        {
            /* o valor de produtoDados.Preco_lista não esta calculado com o coeficiente e para fazer o cálculo 
             * de desconto corretamente deve estar calculado com o coeficiente 
             * por isso, criei essa váriavel precoListaBase
             */
            decimal? precoListaBase = Math.Round((produtoDados.Preco_lista ?? 0) * (decimal)coeficiente, 2);
            decimal preco_venda = produtoDto.Subtotal / (produtoDto.Quantidade == 0 ? 1 : produtoDto.Quantidade);
            decimal preco_nf = produtoDto.RowTotal / (produtoDto.Quantidade == 0 ? 1 : produtoDto.Quantidade);

            var ret = new ProdutoConvertido()
            {
                Fabricante = produtoDados.Fabricante,
                Produto = produtoDados.Produto,
                Qtde = produtoDto.Quantidade,
                Preco_Venda = preco_venda,
                Preco_NF = preco_nf
            };

            return ret;
        }

    }
}

