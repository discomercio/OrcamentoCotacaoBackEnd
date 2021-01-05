using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pedido.Dados;
using MagentoBusiness.MagentoDto.ClienteMagentoDto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InfraBanco.Constantes;
using Prepedido;
using MagentoBusiness.UtilsMagento;
using MagentoBusiness.MagentoDto;
using MagentoBusiness.MagentoDto.MarketplaceDto;

#nullable enable

namespace MagentoBusiness.MagentoBll.MagentoBll
{
    public class PedidoMagentoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly Produto.ProdutoGeralBll produtoGeralBll;
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;
        private readonly PrepedidoBll prepedidoBll;
        private readonly ConfiguracaoApiMagento configuracaoApiMagento;
        private readonly Pedido.PedidoCriacao pedidoCriacao;
        private readonly PedidoMagentoClienteBll pedidoMagentoClienteBll;

        public PedidoMagentoBll(InfraBanco.ContextoBdProvider contextoProvider,
            Produto.ProdutoGeralBll produtoGeralBll,
            Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll, PrepedidoBll prepedidoBll,
            ConfiguracaoApiMagento configuracaoApiMagento, Pedido.PedidoCriacao pedidoCriacao,
            PedidoMagentoClienteBll pedidoMagentoClienteBll)
        {
            this.contextoProvider = contextoProvider;
            this.produtoGeralBll = produtoGeralBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
            this.prepedidoBll = prepedidoBll;
            this.configuracaoApiMagento = configuracaoApiMagento;
            this.pedidoCriacao = pedidoCriacao;
            this.pedidoMagentoClienteBll = pedidoMagentoClienteBll;
        }

        public async Task<PedidoResultadoMagentoDto> CadastrarPedidoMagento(PedidoMagentoDto pedidoMagento, string usuario)
        {
            PedidoResultadoMagentoDto resultado = new PedidoResultadoMagentoDto();

            var orcamentistaindicador_vendedor_loja = await Calcular_orcamentistaindicador_vendedor_loja(pedidoMagento, usuario, resultado.ListaErros);
            pedidoMagentoClienteBll.LimitarPedidosMagentoPJ(pedidoMagento, resultado.ListaErros);
            await ValidarPedidoMagentoEMarketplace(pedidoMagento, resultado.ListaErros);
            if (resultado.ListaErros.Count > 0)
                return resultado;

            //Cadastrar cliente
            pedidoMagentoClienteBll.MoverEnderecoEntregaParaEnderecoCadastral(pedidoMagento, resultado.ListaErros);
            //se der erro ao mover não podemos tentar cadastrar
            if (resultado.ListaErros.Count > 0)
                return resultado;

            await pedidoMagentoClienteBll.CadastrarClienteSeNaoExistir(pedidoMagento, resultado.ListaErros, orcamentistaindicador_vendedor_loja, usuario);
            if (resultado.ListaErros.Count > 0)
                return resultado;

            //estamos criando o pedido com os dados do cliente que vem e não com os dados do cliente que esta na base
            //ex: se o cliente já cadastrado, utilizamos o que vem em PedidoMagentoDto.EnderecoCadastralClienteMagentoDto
            Pedido.Dados.Criacao.PedidoCriacaoDados pedidoDados = await CriarPedidoCriacaoDados(pedidoMagento,
                orcamentistaindicador_vendedor_loja, resultado.ListaErros,
                Convert.ToDecimal(configuracaoApiMagento.LimiteArredondamentoPrecoVendaOrcamentoItem), 0.1M,
                pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac, pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem,
                pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace,
                Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ERP_WEBAPI);
            if (resultado.ListaErros.Count != 0)
                return resultado;

            Pedido.Dados.Criacao.PedidoCriacaoRetornoDados ret = await pedidoCriacao.CadastrarPedido(pedidoDados);

            resultado.IdPedidoCadastrado = ret.Id;
            resultado.IdsPedidosFilhotes = ret.ListaIdPedidosFilhotes;
            resultado.ListaErros = ret.ListaErros;
            resultado.ListaErros.AddRange(ret.ListaErrosValidacao);

            return resultado;
        }

        internal class Orcamentistaindicador_vendedor_loja
        {
            public readonly string? orcamentista_indicador;
            public readonly string vendedor;
            public readonly string loja;

            public Orcamentistaindicador_vendedor_loja(string? orcamentista_indicador, string vendedor, string loja)
            {
                this.orcamentista_indicador = orcamentista_indicador;
                this.vendedor = vendedor;
                this.loja = loja;
            }
        }

        private async Task<Orcamentistaindicador_vendedor_loja> Calcular_orcamentistaindicador_vendedor_loja(PedidoMagentoDto pedidoMagento, string usuario, List<string> listaErros)
        {
            //campo "frete"->se for <> 0, vamos usar o indicador.se for 0, sem indicador
            string? orcamentista_indicador = null;
            if (pedidoMagento.Frete.HasValue && pedidoMagento.Frete != 0)
            {
                orcamentista_indicador = configuracaoApiMagento.DadosOrcamentista.Orcamentista;
                if (!await prepedidoBll.TorcamentistaExiste(orcamentista_indicador))
                    listaErros.Add("O Orçamentista não existe!");
            }
            string vendedor = usuario;
            string loja = configuracaoApiMagento.DadosOrcamentista.Loja;
            return new Orcamentistaindicador_vendedor_loja(orcamentista_indicador, vendedor, loja);
        }

        private async Task<IEnumerable<Pedido.Dados.Criacao.PedidoProdutoPedidoDados>> ConverterProdutosMagento(PedidoMagentoDto pedidoMagento,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao, string loja, List<string> lstErros)
        {
            List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados> listaProdutos = new List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados>();
            List<string> lstFornec = new List<string>();
            lstFornec = pedidoMagento.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            //preciso da lista de coeficientes de cada fabricante da lista de produtos
            //preciso obter a qtde de parcelas e a sigla de pagto
            var qtdeParcelas = PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(formaPagtoCriacao);
            var siglaParc = prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao);
            List<Produto.Dados.CoeficienteDados> lstCoeficiente = (await validacoesPrepedidoBll.MontarListaCoeficiente(lstFornec, qtdeParcelas,
                prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao))).ToList();

            List<string> lstProdutosDistintos = pedidoMagento.ListaProdutos.Select(x => x.Produto).Distinct().ToList();
            List<Produto.Dados.ProdutoDados> lstProdutosUsados = (await produtoGeralBll.BuscarProdutosEspecificos(loja, lstProdutosDistintos)).ToList();

            foreach (var y in pedidoMagento.ListaProdutos)
            {
                Produto.Dados.ProdutoDados produto = (from c in lstProdutosUsados
                                                      where c.Fabricante == y.Fabricante && c.Produto == y.Produto
                                                      select c).FirstOrDefault();

                Produto.Dados.CoeficienteDados coeficiente = (from c in lstCoeficiente
                                                              where c.Fabricante == y.Fabricante &&
                                                                    c.TipoParcela == siglaParc
                                                              select c).FirstOrDefault();

                if (produto != null && coeficiente != null)
                {
                    listaProdutos.Add(PedidoProdutoMagentoDto.ProdutosDePedidoProdutoMagentoDto(y, produto, coeficiente.Coeficiente));
                }
                else
                {
                    if (produto == null)
                        lstErros.Add($"Produto não cadastrado para a loja. Produto: {y.Produto}, loja: {loja}");
                    if (coeficiente == null)
                        lstErros.Add($"Coeficiente não cadastrado para o fabricante. Fabricante: {y.Fabricante}, TipoParcela: {siglaParc}");
                }
            }

            return await Task.FromResult(listaProdutos);
        }

        private async Task<Pedido.Dados.Criacao.PedidoCriacaoDados> CriarPedidoCriacaoDados(PedidoMagentoDto pedidoMagento,
            Orcamentistaindicador_vendedor_loja orcamentistaindicador_vendedor_loja , List<string> lstErros,
            decimal limiteArredondamento,
            decimal maxErroArredondamento, string? pedido_bs_x_ac, string? marketplace_codigo_origem, string? pedido_bs_x_marketplace,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavelCadastro)
        {
            //o cliente existe então vamos converter os dados do cliente para DadosCliente e EnderecoCadastral
            Cliente.Dados.DadosClienteCadastroDados dadosCliente =
                EnderecoCadastralClienteMagentoDto.DadosClienteDeEnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente,
                orcamentistaindicador_vendedor_loja.loja,
                orcamentistaindicador_vendedor_loja.vendedor, configuracaoApiMagento.DadosOrcamentista.Orcamentista);

            Cliente.Dados.EnderecoCadastralClientePrepedidoDados enderecoCadastral =
                EnderecoCadastralClienteMagentoDto.EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente);

            Cliente.Dados.EnderecoEntregaClienteCadastroDados enderecoEntrega =
                EnderecoEntregaClienteMagentoDto.EnderecoEntregaDeEnderecoEntregaClienteMagentoDto(pedidoMagento.EnderecoEntrega, pedidoMagento.OutroEndereco,
                configuracaoApiMagento.EndEtg_cod_justificativa);

            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao =
                FormaPagtoCriacaoMagentoDto.FormaPagtoCriacaoDados_De_FormaPagtoCriacaoMagentoDto(pedidoMagento.FormaPagtoCriacao,
                configuracaoApiMagento, pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem);

            List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados> listaProdutos =
                (await ConverterProdutosMagento(pedidoMagento, formaPagtoCriacao, configuracaoApiMagento.DadosOrcamentista.Loja, lstErros)).ToList();

            //Precisamos buscar os produtos para poder incluir os valores para incluir na classe de produto
            Pedido.Dados.Criacao.PedidoCriacaoDados pedidoDadosCriacao =
                PedidoMagentoDto.PedidoDadosCriacaoDePedidoMagentoDto(dadosCliente, enderecoCadastral, enderecoEntrega,
                listaProdutos, formaPagtoCriacao, pedidoMagento.VlTotalDestePedido, pedidoMagento,
                limiteArredondamento,
                maxErroArredondamento, pedido_bs_x_ac, marketplace_codigo_origem, pedido_bs_x_marketplace,
                sistemaResponsavelCadastro);

            return await Task.FromResult(pedidoDadosCriacao);
        }

        private async Task ValidarPedidoMagentoEMarketplace(PedidoMagentoDto pedidoMagento, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();
            //vamos validar o número do pedido magento
            if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac))
                lstErros.Add("Favor informar o número do pedido Magento(Pedido_bs_x_ac)!");

            if (pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac.Length != Constantes.MAX_TAMANHO_ID_PEDIDO_MAGENTO)
                lstErros.Add("Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!");


            /*
             * Pedido_bs_x_marketplace e Marketplace_codigo_origem
             * ou os dois existem ou nenhum dos dois existe
             * */
            if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace) && !string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem))
                lstErros.Add("Informe o Pedido_bs_x_marketplace.");
            if (!string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace) && string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem))
                lstErros.Add("Informe o Marketplace_codigo_origem.");

            //validar o Marketplace_codigo_origem
            if (!string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem))
            {
                if (!await UtilsGlobais.Util.ListarCodigoMarketPlace(contextoProvider)
                    .Where(x => x.Codigo == pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem)
                    .AnyAsync())
                {
                    lstErros.Add("Código Marketplace não encontrado.");
                }
            }
        }


    }
}
