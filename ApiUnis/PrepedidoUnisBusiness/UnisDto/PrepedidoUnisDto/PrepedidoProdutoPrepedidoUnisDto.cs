using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using System;
using System.ComponentModel.DataAnnotations;

namespace PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto
{
    #region Comentários
    /// <summary>
    /// preco_venda é o valor de venda efetivamente, esse é o valor que fica p/ a empresa. O RA é um valor adicionado sobre o preco_venda e resulta no preco_nf
    /// <br/> 
    /// <br/> 
    /// preco_fabricante é o valor nominal de custo do produto, não reflete o valor real negociado na compra do fabricante, 
    /// é como se fosse o valor de tabela do fabricante para os distribuidores
    /// <br/> 
    /// <br/> 
    /// preco_lista é o preço de tabela que vendemos para os nossos clientes. Os descontos são aplicados sobre o valor do preco_lista 
    /// e resultam no preco_venda. O detalhe é que o preco_lista varia de acordo com a forma de pagamento. 
    /// O valor cadastrado do preco_lista é somente para venda à vista. Quando se escolhe algum parcelamento, 
    /// é preciso calcular se é um parcelamento "com entrada" ou "sem entrada". Em seguida, a quantidade de parcelas. 
    /// Com isso, para cada produto, obtém-se o coeficiente do custo financeiro que deve ser multiplicado por preco_lista para obter 
    /// o novo preco_lista p/ o parcelamento em questão.
    /// <br/> 
    /// A diferença entre Preco_Lista e Preco_Venda representa quanto foi dado de desconto para o cliente.
    /// <br/> 
    /// <br/> 
    /// Preco_NF: é o preço que constará na nota fiscal e é o valor que o cliente irá pagar efetivamente.A diferença entre 
    /// Preco_NF e Preco_Venda representa o RA, que é um valor repassado para o parceiro.
    /// <br/> 
    /// Se PermiteRAStatus = false, Preco_Venda e Preco_NF devem ser iguais.
    /// <br/> 
    /// Se PermiteRAStatus = true, Preco_Venda e Preco_NF podem ou não ser iguais (o RA é permitido, mas o parceiro pode ou não fazer uso; 
    /// caso o pré-pedido não tenha RA, Preco_Venda e Preco_NF serão iguais em todos os itens).
    /// <br/> 
    /// <br/> 
    /// Preco_Lista é considerado basicamente quando se valida o limite máximo de desconto.
    /// <br/>
    /// <br/>
    /// </summary>
    #endregion
    public class PrePedidoProdutoPrePedidoUnisDto
    {
        [Required]
        [MaxLength(4)]
        public string Fabricante { get; set; }

        [Required]
        [MaxLength(8)]
        public string Produto { get; set; } //  = NumProduto

        [Required]
        public short Qtde { get; set; }

        [Required]
        public float Desc_Dado { get; set; }

        /// <summary>
        /// Preco_Venda = (CustoFinancFornecPrecoListaBase * CustoFinancFornecCoeficiente) * (1 - Desc_Dado / 100)
        /// </summary>
        [Required]
        public decimal Preco_Venda { get; set; }

        /// <summary>
        /// Preco_Lista = CustoFinancFornecPrecoListaBase * CustoFinancFornecCoeficiente
        /// </summary>
        [Required]
        public decimal Preco_Lista { get; set; } //recebe Preco_Lista

        /// <summary>
        /// Preco_NF = Se PrePedidoUnisDto.PermiteRAStatus == false, Preco_NF deve ser igual a Preco_Venda. Se PrePedidoUnisDto.PermiteRAStatus == true, valor com RA.
        /// </summary>
        [Required]
        public decimal Preco_NF { get; set; } // Caso RA = False,   "Preco_NF"  deve ser  = "Preco_Venda"

        /// <summary>
        /// Caso seja pagamento a vista, deve ser 1. Caso contrário, o coeficiente do fabricante para a quantidade de parcelas e forma de pagamento.
        /// </summary>
        [Required]
        public float CustoFinancFornecCoeficiente { get; set; }

        /// <summary>
        /// CustoFinancFornecPrecoListaBase = o campo Preco_lista da lista de produtos
        /// </summary>
        [Required]
        public decimal CustoFinancFornecPrecoListaBase { get; set; }

        public static PrepedidoProdutoDtoPrepedido PrepedidoProdutoDtoPrepedidoDePrePedidoProdutoPrePedidoUnisDto(PrePedidoProdutoPrePedidoUnisDto produtoDto,
            short permiteRaStatus)
        {
            var ret = new PrepedidoProdutoDtoPrepedido()
            {
                Fabricante = produtoDto.Fabricante,
                Produto = produtoDto.Produto,
                Qtde = produtoDto.Qtde,
                Permite_Ra_Status = permiteRaStatus,
                BlnTemRa = produtoDto.Preco_NF != produtoDto.Preco_Venda ? true : false,
                CustoFinancFornecPrecoListaBase = produtoDto.CustoFinancFornecPrecoListaBase,
                Preco_Lista = produtoDto.Preco_Lista,
                Desc_Dado = produtoDto.Desc_Dado,
                Preco_Venda = produtoDto.Preco_Venda,
                TotalItem = Math.Round((decimal)(produtoDto.Preco_Venda * produtoDto.Qtde), 2),
                TotalItemRA = Math.Round((decimal)(produtoDto.Preco_NF * produtoDto.Qtde), 2),
                CustoFinancFornecCoeficiente = produtoDto.CustoFinancFornecCoeficiente,
                Preco_NF = produtoDto.Preco_NF
            };

            return ret;
        }

        public static Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados PrepedidoProdutoPrepedidoDadosDePrePedidoProdutoPrePedidoUnisDto(PrePedidoProdutoPrePedidoUnisDto produtoDto,
            short permiteRaStatus)
        {
            var ret = new Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados()
            {
                Fabricante = produtoDto.Fabricante,
                Produto = produtoDto.Produto,
                Qtde = produtoDto.Qtde,
                Permite_Ra_Status = permiteRaStatus,
                BlnTemRa = produtoDto.Preco_NF != produtoDto.Preco_Venda ? true : false,
                CustoFinancFornecPrecoListaBase = produtoDto.CustoFinancFornecPrecoListaBase,
                Preco_Lista = produtoDto.Preco_Lista,
                Desc_Dado = produtoDto.Desc_Dado,
                Preco_Venda = produtoDto.Preco_Venda,
                TotalItem = Math.Round((decimal)(produtoDto.Preco_Venda * produtoDto.Qtde), 2),
                TotalItemRA = Math.Round((decimal)(produtoDto.Preco_NF * produtoDto.Qtde), 2),
                CustoFinancFornecCoeficiente = produtoDto.CustoFinancFornecCoeficiente,
                Preco_NF = produtoDto.Preco_NF
            };

            return ret;
        }
    }
}
