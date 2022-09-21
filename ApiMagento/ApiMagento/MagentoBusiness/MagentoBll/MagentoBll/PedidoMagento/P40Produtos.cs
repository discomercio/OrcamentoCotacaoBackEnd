using FormaPagamento.Dados;
using InfraBanco;
using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using Microsoft.EntityFrameworkCore;
using Prepedido;
using Prepedido.Bll;
using Produto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagentoBusiness.MagentoBll.MagentoBll.PedidoMagento
{
    class P40Produtos
    {
        private readonly ProdutoGeralBll produtoGeralBll;
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;
        private readonly PrepedidoBll prepedidoBll;
        private readonly ContextoBdProvider contextoProvider;

        public P40Produtos(Produto.ProdutoGeralBll produtoGeralBll,
            ValidacoesPrepedidoBll validacoesPrepedidoBll,
            PrepedidoBll prepedidoBll,
            InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.produtoGeralBll = produtoGeralBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
            this.prepedidoBll = prepedidoBll;
            this.contextoProvider = contextoProvider;
        }

        internal async Task<List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados>> ExecutarAsync(PedidoMagentoDto pedidoMagento, List<string> listaErros,
            FormaPagtoCriacaoDados formaPagtoCriacao, string loja, decimal limiteArredondamentoPorItem,
            decimal limiteArredondamentoTotais, int limite_de_itens)
        {
            //P05: para cada linha, consistir Quantidade > 0, RowTotal = Subtotal - DiscountAmount dentro do arredondamento
            Passos.P40_P05_LinhasProdutos(pedidoMagento.ListaProdutos, pedidoMagento.ListaServicos, listaErros, limiteArredondamentoPorItem);

            //P10: Transformar produtos compostos em simples
            List<ProdutoConvertido> produtosConvertidos = await P10TransformarProdutosCompostosEmSimples(pedidoMagento.ListaProdutos, listaErros);

            //P20: Carregar valores dos produtos do banco
            await P20CarregarValoresDosProdutosDoBanco(produtosConvertidos, formaPagtoCriacao, loja, listaErros);

            //P30_CalcularDesconto: Inserir os descontos de forma a chegar nos valores do magento com o frete diluído
            P30_CalcularDesconto(produtosConvertidos, pedidoMagento.TotaisPedido, pedidoMagento.ListaProdutos, listaErros,
                limiteArredondamentoTotais: limiteArredondamentoTotais, listaServicos: pedidoMagento.ListaServicos);

            //P80: Garatir que tem menos ou igual a 12 itens (conforme configuração)
            if (produtosConvertidos.Count > limite_de_itens)
                listaErros.Add($"São permitidos no máximo {limite_de_itens} itens por pedido.");

            var ret = ConverterProdutosMagento(produtosConvertidos);
            return ret;
        }

        private void P30_CalcularDesconto(List<ProdutoConvertido> produtosConvertidos, PedidoTotaisMagentoDto totaisPedido,
            List<PedidoProdutoMagentoDto> listaProdutosApiMagento, List<string> listaErros,
            decimal limiteArredondamentoTotais, List<PedidoServicoMagentoDto> listaServicos)
        {
            /*
                Ver planilha, resumo das fórmulas:
                    desconto_preco_nf: (valor total do pedido no verdinho - valor total no magento) / valor total do pedido no verdinho
                    desc_dado = (total do verdinho - total do magento sem o frete) / total do verdinho
                    preco_lista = valor base
                    preco_nf = valor base * (1 - desconto_preco_nf)
                    preco_venda = valor base * (1 - desc_dado)
                    Todos os valores são arredondados para 2 casas decimais (não o desconto)
            */

            //desconto_preco_nf: (valor total do pedido no verdinho - valor total no magento) / valor total do pedido no verdinho
            var valor_total_do_pedido_no_verdinho = produtosConvertidos.Select(r => r.Preco_Lista * r.Qtde).Sum();
            valor_total_do_pedido_no_verdinho = UtilsGlobais.Valores.Arredondar2casas(valor_total_do_pedido_no_verdinho);

            var valor_total_no_magento = listaProdutosApiMagento.Select(r => r.Subtotal).Sum()
                    + totaisPedido.FreteBruto
                    - listaProdutosApiMagento.Select(r => r.DiscountAmount).Sum();
            valor_total_no_magento = UtilsGlobais.Valores.Arredondar2casas(valor_total_no_magento);

            if (valor_total_do_pedido_no_verdinho < 0.01M)
                valor_total_do_pedido_no_verdinho = 0.01M;
            var desconto_preco_nf = (valor_total_do_pedido_no_verdinho - valor_total_no_magento) / valor_total_do_pedido_no_verdinho;

            //desc_dado = (total do verdinho - total do magento sem o frete) / total do verdinho
            var total_do_magento_sem_o_frete = valor_total_no_magento - totaisPedido.FreteBruto + totaisPedido.DescontoFrete;
            total_do_magento_sem_o_frete = UtilsGlobais.Valores.Arredondar2casas(total_do_magento_sem_o_frete);

            var desc_dado = (valor_total_do_pedido_no_verdinho - total_do_magento_sem_o_frete) / valor_total_do_pedido_no_verdinho;

            foreach (var produto in produtosConvertidos)
            {
                var valor_base = produto.Preco_Lista;
                //preco_nf = valor base * (1 - desconto_preco_nf)
                //preco_venda = valor base * (1 - desc_dado)
                produto.Preco_NF = UtilsGlobais.Valores.Arredondar2casas(valor_base * (1 - desconto_preco_nf));
                produto.Preco_Venda = UtilsGlobais.Valores.Arredondar2casas(valor_base * (1 - desc_dado));
            }


            /*
                Garantir o menor arredondamento possível
                    Desejado: GrandTotal = soma (qde * preco_nf) + total de serviços
                    ajuste_arredondamento: GrandTotal - (soma (qde * preco_nf) + total de serviços) com todos com 2 casas decimais
                    Escolher a linha com a menor resto de ( abs(ajuste_arredondamento) / qde) e, nesssas, com a menor qde 
                    alterar preco_nf = preco_nf - ajuste_arredondamento / qde (arredondado para 2 casas decimais)
                    Isso já faz o melhor ajuste possível para RA também
                    * testar com ajustes positivos e negativos
            */
            //todo: fazer


            /*
               Consistências (todas com arredondamento): 
                   RA = soma (qde * preco_nf) - soma (qde * preco_venda)
                   RA = FreteBruto - DescontoFrete
                   GrandTotal = soma (qde * preco_nf) + total de serviços
           */
            var ra1 = produtosConvertidos.Select(r => r.Preco_NF * r.Qtde).Sum() - produtosConvertidos.Select(r => r.Preco_Venda * r.Qtde).Sum();
            ra1 = UtilsGlobais.Valores.Arredondar2casas(ra1);
            var ra2 = totaisPedido.FreteBruto - totaisPedido.DescontoFrete;
            ra2 = UtilsGlobais.Valores.Arredondar2casas(ra2);
            if (!Passos.IgualComArredondamento(ra1, ra2, limiteArredondamentoTotais))
                listaErros.Add($"Valores de RA inconsistentes na conversão dos valores: soma (qde * preco_nf) - soma (qde * preco_venda) = {ra1}, FreteBruto - DescontoFrete = {ra2}");

            var grandTotal = produtosConvertidos.Select(r => r.Preco_NF * r.Qtde).Sum() + listaServicos.Select(r => r.RowTotal).Sum();
            grandTotal = UtilsGlobais.Valores.Arredondar2casas(grandTotal);
            if (!Passos.IgualComArredondamento(grandTotal, totaisPedido.GrandTotal, limiteArredondamentoTotais))
                listaErros.Add($"Valores totais inconsistentes na conversão dos valores: soma (qde * preco_nf) + total de serviços = {grandTotal}, GrandTotal = {totaisPedido.GrandTotal}");
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

            #region consultas ao banco somente com os dados que vamos precisar
            //fazemos uam query inicial para ficar mais rápido
            var skusDistintos = (from p in listaProdutos select p.Sku).Distinct().ToList();
            var task_t_ec_produto_composto_carregada = (from pc in contextoProvider.GetContextoLeitura().TecProdutoComposto
                                                        where skusDistintos.Contains(pc.Produto_Composto)
                                                        select new { pc.Produto_Composto, pc.Fabricante_Composto }).ToListAsync();
            var task_t_ec_produto_composto_item_carregada = (from pc in contextoProvider.GetContextoLeitura().TecProdutoCompostoItem
                                                             where skusDistintos.Contains(pc.Produto_composto)
                                                             select new
                                                             {
                                                                 pc.Produto_composto,
                                                                 pc.Fabricante_composto,
                                                                 pc.Produto_item,
                                                                 pc.Fabricante_item,
                                                                 pc.Sequencia,
                                                                 pc.Qtde
                                                             }).ToListAsync();

            //os produtos podem ser somente os SKUs
            var task_t_produto_carregada = (from pc in contextoProvider.GetContextoLeitura().Tproduto
                                            where skusDistintos.Contains(pc.Produto)
                                            select new { pc.Produto, pc.Fabricante }).ToListAsync();
            var t_ec_produto_composto_item_carregada = await task_t_ec_produto_composto_item_carregada;
            var t_ec_produto_composto_carregada = await task_t_ec_produto_composto_carregada;
            var t_produto_carregada = await task_t_produto_carregada;
            #endregion

            foreach (var linha in listaProdutos)
            {
                var produto_composto_lista = (from pc in t_ec_produto_composto_carregada where pc.Produto_Composto == linha.Sku select pc).ToList();
                if (produto_composto_lista.Count() > 0)
                {
                    if (produto_composto_lista.Count() > 0)
                        listaErros.Add($"Erro na configuração dos produtos: SKU {linha.Sku} possui mais de uma entrada na tabela T_EC_PRODUTO_COMPOSTO");
                    var produto_composto = produto_composto_lista.First();
                    //se achar registro, busca em t_EC_PRODUTO_COMPOSTO_ITEM com t_EC_PRODUTO_COMPOSTO_ITEM.produto_composto == Sku && t_EC_PRODUTO_COMPOSTO_ITEM.fabricante_composto == t_EC_PRODUTO_COMPOSTO.fabricante_composto
                    var expandidos = (from pc in t_ec_produto_composto_item_carregada
                                      where pc.Fabricante_composto == produto_composto.Fabricante_Composto && pc.Produto_composto == produto_composto.Produto_Composto
                                      orderby pc.Sequencia
                                      select pc).ToList();
                    foreach (var produtoItem in expandidos)
                    {
                        AdicionarProdutoConvertido(produtosConvertidos, new ProdutoConvertido(fabricante: produtoItem.Fabricante_item,
                            produto: produtoItem.Produto_item, qtde: (short)(produtoItem.Qtde * (short)linha.Quantidade)));
                    }

                }
                else
                {
                    //se não achar busca em t_PRODUTO com t_PRODUTO.produto == Sku 
                    var produto_lista = (from pc in t_produto_carregada where pc.Produto == linha.Sku select pc).ToList();
                    if (produto_lista.Count() > 1)
                        listaErros.Add($"Erro na confiugração dos produtos: SKU {linha.Sku} não está na tabela T_EC_PRODUTO_COMPOSTO e possui mais de um registro em T_PRODUTO");
                    if (produto_lista.Count() == 0)
                        listaErros.Add($"Erro na confiugração dos produtos: SKU {linha.Sku} não está na tabela T_EC_PRODUTO_COMPOSTO e nem na tabela T_PRODUTO");

                    if (produto_lista.Count() == 1)
                    {
                        var produto = produto_lista.First();
                        AdicionarProdutoConvertido(produtosConvertidos, new ProdutoConvertido(fabricante: produto.Fabricante,
                            produto: produto.Produto, qtde: linha.Quantidade));
                    }
                }
            }

            return produtosConvertidos;
        }

        private static void AdicionarProdutoConvertido(List<ProdutoConvertido> produtosConvertidos, ProdutoConvertido novo)
        {
            //Agrupamos produtos iguais, mantendo a ordem original.
            var existente = (from pc in produtosConvertidos where pc.Fabricante == novo.Fabricante && pc.Produto == novo.Produto select pc).FirstOrDefault();
            if (existente == null)
            {
                produtosConvertidos.Add(novo);
            }
            else
            {
                //somente soma a quantidade
                existente.Qtde += novo.Qtde;
            }
        }

        private async Task P20CarregarValoresDosProdutosDoBanco(List<ProdutoConvertido> produtosConvertidos,
                                                                FormaPagtoCriacaoDados formaPagtoCriacao,
                                                                string loja,
                                                                List<string> lstErros)
        {
            List<string> lstFornec = produtosConvertidos.Select(x => x.Fabricante).Distinct().ToList();

            //preciso da lista de coeficientes de cada fabricante da lista de produtos
            //preciso obter a qtde de parcelas e a sigla de pagto
            var qtdeParcelas = PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(formaPagtoCriacao);
            var siglaParc = prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao);
            List<Produto.Dados.CoeficienteDados> lstCoeficiente = (await validacoesPrepedidoBll.MontarListaCoeficiente(
                lstFornec, qtdeParcelas, siglaParc)).ToList();

            List<string> lstProdutosDistintos = produtosConvertidos.Select(x => x.Produto).Distinct().ToList();
            List<Produto.Dados.ProdutoDados> lstProdutosUsados = (await produtoGeralBll.BuscarProdutosEspecificos(loja, lstProdutosDistintos)).ToList();

            foreach (var produtoConvertido in produtosConvertidos)
            {
                Produto.Dados.ProdutoDados produtoDados = (from c in lstProdutosUsados
                                                           where c.Fabricante == produtoConvertido.Fabricante && c.Produto == produtoConvertido.Produto
                                                           select c).FirstOrDefault();

                Produto.Dados.CoeficienteDados coeficiente = (from c in lstCoeficiente
                                                              where c.Fabricante == produtoConvertido.Fabricante &&
                                                                    c.TipoParcela == siglaParc
                                                              select c).FirstOrDefault();

                if (produtoDados == null)
                    lstErros.Add($"Produto não cadastrado para a loja. Produto: {produtoConvertido.Produto}, loja: {loja}");
                if (coeficiente == null)
                    lstErros.Add($"Coeficiente não cadastrado para o fabricante. Fabricante: {produtoConvertido.Fabricante}, TipoParcela: {siglaParc}");
                if (produtoDados != null && coeficiente != null)
                    produtoConvertido.InicializarValores(produtoDados, coeficiente.Coeficiente);
            }
        }

        private List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> ConverterProdutosMagento(List<ProdutoConvertido> produtosConvertidos)
        {
            List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> listaProdutos = new List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados>();
            foreach (var y in produtosConvertidos)
                listaProdutos.Add(MagentoBusiness.MagentoBll.MagentoBll.PedidoMagento.ProdutoConvertido.PedidoCriacaoProdutoDados_De_ProdutoConvertido(y));

            return listaProdutos;
        }

    }
}
