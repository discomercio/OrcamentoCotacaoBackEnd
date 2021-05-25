using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using Prepedido;
using Prepedido.Dados.DetalhesPrepedido;
using Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoBusiness.MagentoBll.MagentoBll.PedidoMagento
{
    class P40Produtos
    {
        private readonly ProdutoGeralBll produtoGeralBll;
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;
        private readonly PrepedidoBll prepedidoBll;

        public P40Produtos(Produto.ProdutoGeralBll produtoGeralBll,
            ValidacoesPrepedidoBll validacoesPrepedidoBll,
            PrepedidoBll prepedidoBll)
        {
            this.produtoGeralBll = produtoGeralBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
            this.prepedidoBll = prepedidoBll;
        }

        internal async Task<List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados>> ExecutarAsync(PedidoMagentoDto pedidoMagento, List<string> listaErros,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao, string loja, decimal limiteArredondamentoPorItem, int limite_de_itens)
        {
            //P05: para cada linha, consistir Quantidade > 0, RowTotal = Subtotal - DiscountAmount dentro do arredondamento
            Passos.P40_P05_LinhasProdutos(pedidoMagento.ListaProdutos, listaErros, limiteArredondamentoPorItem);

            //P10: Transformar produtos compostos em simples
            List<ProdutoConvertido> produtosConvertidos = await P10TransformarProdutosCompostosEmSimples(pedidoMagento.ListaProdutos, listaErros);

            //P20: Carregar valores dos produtos do banco
            await P20CarregarValoresDosProdutosDoBanco(produtosConvertidos, formaPagtoCriacao, loja, listaErros);

            //P30_CalcularDesconto: Inserir os descontos de forma a chegar nos valores do magento com o frete diluído
            P30_CalcularDesconto(produtosConvertidos, pedidoMagento.TotaisPedido, listaErros);

            //P80: Garatir que tem menos ou igual a 12 itens (conforme configuração)
            if (produtosConvertidos.Count > limite_de_itens)
                listaErros.Add($"São permitidos apenas {limite_de_itens} itens.");


            //antigo
            produtosConvertidos = await Afazer_remover_ProdutoConvertidoLista(pedidoMagento.ListaProdutos, formaPagtoCriacao, loja, listaErros);
            var ret = await ConverterProdutosMagento(produtosConvertidos, formaPagtoCriacao, loja, listaErros);
            return ret;
        }

        private void P30_CalcularDesconto(List<ProdutoConvertido> produtosConvertidos, PedidoTotaisMagentoDto totaisPedido, List<string> listaErros)
        {
            //todo: fazer

            /*
                    Ver planilha, resumo das fórmulas:
                        desconto_preco_nf: (valor total do pedido no verdinho - valor total no magento) / valor total do pedido no verdinho
                        desc_dado = (total do verdinho - total do magento sem o frete) / total do verdinho
                        preco_lista = valor base
                        preco_nf = valor base * (1 - desconto_preco_nf)
                        preco_venda = valor base * (1 - desc_dado)
                        Todos os valores são arredondados para 2 casas decimais (não o desconto)
                    Garantir o menor arredondamento possível
                        Desejado: GrandTotal = soma (qde * preco_nf) + total de serviços
                        ajuste_arredondamento: GrandTotal - (soma (qde * preco_nf) + total de serviços) com todos com 2 casas decimais
                        Escolher a linha com a menor resto de ( abs(ajuste_arredondamento) / qde) e, nesssas, com a menor qde 
                        alterar preco_nf = preco_nf - ajuste_arredondamento / qde (arredondado para 2 casas decimais)
                        Isso já faz o melhor ajuste possível para RA também
                        * testar com ajustes positivos e negativos
                    Consistências (todas com arredondamento): 
                        RA = soma (qde * preco_nf) - soma (qde * preco_venda)
                        RA = FreteBruto - DescontoFrete
                        GrandTotal = soma (qde * preco_nf) + total de serviços
            */
        }

        private async Task P20CarregarValoresDosProdutosDoBanco(List<ProdutoConvertido> produtosConvertidos,
                                                                FormaPagtoCriacaoDados formaPagtoCriacao,
                                                                string loja,
                                                                List<string> listaErros)
        {
            //todo: fazer
        }

        private async Task<List<ProdutoConvertido>> P10TransformarProdutosCompostosEmSimples(List<PedidoProdutoMagentoDto> listaProdutos, List<string> listaErros)
        {
            /*
                    Buscamos na t_EC_PRODUTO_COMPOSTO e expandimos os produtos conforme t_EC_PRODUTO_COMPOSTO_ITEM. Se não for composto, mantemos.
                        t_EC_PRODUTO_COMPOSTO.produto_composto == Sku
                            se achar registro, busca em t_EC_PRODUTO_COMPOSTO_ITEM com t_EC_PRODUTO_COMPOSTO_ITEM.produto_composto == Sku && t_EC_PRODUTO_COMPOSTO_ITEM.fabricante_composto == t_EC_PRODUTO_COMPOSTO.fabricante_composto
                            se não achar busca em t_PRODUTO com t_PRODUTO.produto == Sku 
                    Agrupamos produtos iguais, mantendo a ordem original.
            */
            List<ProdutoConvertido> produtosConvertidos = new List<ProdutoConvertido>();
            //todo: fazer
            return produtosConvertidos;
        }

        //todo: apagar esta rotina
        private async Task<List<ProdutoConvertido>> Afazer_remover_ProdutoConvertidoLista(List<PedidoProdutoMagentoDto> listaProdutos,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao, string loja, List<string> lstErros)
        {

            //todo: fazer todo o processo p40. por enquanto, somente convertendo os produtos. está com muito codigo duplicado da rotina ConverterProdutosMagento

            List<string> lstFornec = listaProdutos.Select(x => x.Sku.Substring(0, 3)).Distinct().ToList();

            //preciso da lista de coeficientes de cada fabricante da lista de produtos
            //preciso obter a qtde de parcelas e a sigla de pagto
            var qtdeParcelas = PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(formaPagtoCriacao);
            var siglaParc = prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao);
            List<Produto.Dados.CoeficienteDados> lstCoeficiente = (await validacoesPrepedidoBll.MontarListaCoeficiente(
                lstFornec, qtdeParcelas, siglaParc)).ToList();

            List<string> lstProdutosDistintos = listaProdutos.Select(x => x.Sku).Distinct().ToList();
            List<Produto.Dados.ProdutoDados> lstProdutosUsados = (await produtoGeralBll.BuscarProdutosEspecificos(loja, lstProdutosDistintos)).ToList();


            List<ProdutoConvertido> produtosConvertidos = new List<ProdutoConvertido>();
            foreach (var y in listaProdutos)
            {
                Produto.Dados.ProdutoDados produto = (from c in lstProdutosUsados
                                                      where c.Fabricante == y.Sku.Substring(0, 3) && c.Produto == y.Sku
                                                      select c).FirstOrDefault();

                Produto.Dados.CoeficienteDados coeficiente = (from c in lstCoeficiente
                                                              where c.Fabricante == y.Sku.Substring(0, 3) &&
                                                                    c.TipoParcela == siglaParc
                                                              select c).FirstOrDefault();


                if (produto == null)
                    lstErros.Add($"Produto não cadastrado para a loja. Produto: {y.Sku}, loja: {loja}");
                if (coeficiente == null)
                    lstErros.Add($"Coeficiente não cadastrado para o fabricante. Fabricante: {y.Sku.Substring(0, 3)}, TipoParcela: {siglaParc}");
                if (produto != null && coeficiente != null)
                    produtosConvertidos.Add(ProdutoConvertido.ProdutoConvertido_De_PedidoProdutoMagentoDto(y, produto, coeficiente.Coeficiente));
            }

            return produtosConvertidos;
        }

        private async Task<List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados>> ConverterProdutosMagento(List<ProdutoConvertido> produtosConvertidos,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao, string loja, List<string> lstErros)
        {
            List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> listaProdutos = new List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados>();
            List<string> lstFornec = produtosConvertidos.Select(x => x.Fabricante).Distinct().ToList();

            //preciso da lista de coeficientes de cada fabricante da lista de produtos
            //preciso obter a qtde de parcelas e a sigla de pagto
            var qtdeParcelas = PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(formaPagtoCriacao);
            var siglaParc = prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao);
            List<Produto.Dados.CoeficienteDados> lstCoeficiente = (await validacoesPrepedidoBll.MontarListaCoeficiente(
                lstFornec, qtdeParcelas, siglaParc)).ToList();

            List<string> lstProdutosDistintos = produtosConvertidos.Select(x => x.Produto).Distinct().ToList();
            List<Produto.Dados.ProdutoDados> lstProdutosUsados = (await produtoGeralBll.BuscarProdutosEspecificos(loja, lstProdutosDistintos)).ToList();

            foreach (var y in produtosConvertidos)
            {
                Produto.Dados.ProdutoDados produto = (from c in lstProdutosUsados
                                                      where c.Fabricante == y.Fabricante && c.Produto == y.Produto
                                                      select c).FirstOrDefault();

                Produto.Dados.CoeficienteDados coeficiente = (from c in lstCoeficiente
                                                              where c.Fabricante == y.Fabricante &&
                                                                    c.TipoParcela == siglaParc
                                                              select c).FirstOrDefault();

                if (produto == null)
                    lstErros.Add($"Produto não cadastrado para a loja. Produto: {y.Produto}, loja: {loja}");
                if (coeficiente == null)
                    lstErros.Add($"Coeficiente não cadastrado para o fabricante. Fabricante: {y.Fabricante}, TipoParcela: {siglaParc}");
                if (produto != null && coeficiente != null)
                    listaProdutos.Add(MagentoBusiness.MagentoBll.MagentoBll.PedidoMagento.ProdutoConvertido.PedidoCriacaoProdutoDados_De_ProdutoConvertido(y, produto, coeficiente.Coeficiente));
            }

            return listaProdutos;
        }

    }
}
