using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagentoBusiness.MagentoBll.MagentoBll.PedidoMagento
{
    class ProdutoConvertido
    {
        public ProdutoConvertido(string fabricante, string produto, short qtde = 0)
        {
            Fabricante = fabricante;
            Produto = produto;
            Qtde = qtde;
        }

        public string Fabricante { get; }
        public string Produto { get; } //  = NumProduto
        public short Qtde { get; set; }
        public decimal CustoFinancFornecPrecoListaBase_Conferencia { get; private set; } = 0;
        public decimal Preco_Lista { get; private set; } = 0;
        public decimal Preco_Venda { get; set; } = 0;
        public decimal Preco_NF { get; set; } = 0;
        public float CustoFinancFornecCoeficiente_Conferencia { get; private set; } = 0;

        public void InicializarValores(Produto.Dados.ProdutoDados produtoDados, float coeficiente)
        {
            var preco_Lista = Math.Round((decimal)((produtoDados.Preco_lista ?? 0) * (decimal)coeficiente), 2);
            this.CustoFinancFornecPrecoListaBase_Conferencia = produtoDados.Preco_lista ?? 0;
            this.Preco_Lista = preco_Lista;
            this.CustoFinancFornecCoeficiente_Conferencia = coeficiente;
        }

        /*
         * Os campos Preco_Fabricante, CustoFinancFornecCoeficiente, CustoFinancFornecPrecoListaBase e Preco_Fabricante vamos ler das tabelas
         * Os campos Preco_Lista e Desc_Dado serão preenchidos por nós e devemos calcular de forma que fiquem consistentes.
        */

        public static Pedido.Dados.Criacao.PedidoCriacaoProdutoDados PedidoCriacaoProdutoDados_De_ProdutoConvertido(ProdutoConvertido produtoDto)
        {
            //y.Preco_Venda = Math.Round(y.Preco_Lista * (decimal)(1 - y.Desc_Dado / 100), 2);
            var produtoDto_Preco_Lista = produtoDto.Preco_Lista;
            if (produtoDto_Preco_Lista == 0)
                produtoDto_Preco_Lista = 0.01M;

            var desc_Dado = 100M * (1M - (produtoDto.Preco_Venda / produtoDto_Preco_Lista));
            var ret = new Pedido.Dados.Criacao.PedidoCriacaoProdutoDados(
                fabricante: produtoDto.Fabricante,
                produto: produtoDto.Produto,
                qtde: produtoDto.Qtde,
                custoFinancFornecPrecoListaBase_Conferencia: produtoDto.CustoFinancFornecPrecoListaBase_Conferencia,
                preco_Lista: produtoDto.Preco_Lista,
                desc_Dado: (float)Convert.ToDouble(desc_Dado),
                preco_Venda: produtoDto.Preco_Venda,
                preco_NF: produtoDto.Preco_NF,
                custoFinancFornecCoeficiente_Conferencia: produtoDto.CustoFinancFornecCoeficiente_Conferencia,
                //no magento não validamos se o estoque mudou
                qtde_spe_usuario_aceitou: null
            );

            return ret;
        }


    }
}

