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

namespace MagentoBusiness.MagentoBll.PedidoMagentoBll
{
    public class PedidoMagentoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly Cliente.ClienteBll clienteBll;
        private readonly Produto.ProdutoGeralBll produtoGeralBll;
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;
        private readonly PrepedidoBll prepedidoBll;
        private readonly ConfiguracaoApiMagento configuracaoApiMagento;
        private readonly Pedido.PedidoCriacao pedidoCriacao;

        public PedidoMagentoBll(InfraBanco.ContextoBdProvider contextoProvider,
            Cliente.ClienteBll clienteBll, Produto.ProdutoGeralBll produtoGeralBll,
            Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll, PrepedidoBll prepedidoBll,
            ConfiguracaoApiMagento configuracaoApiMagento, Pedido.PedidoCriacao pedidoCriacao)
        {
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
            this.produtoGeralBll = produtoGeralBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
            this.prepedidoBll = prepedidoBll;
            this.configuracaoApiMagento = configuracaoApiMagento;
            this.pedidoCriacao = pedidoCriacao;
        }

        public async Task<PedidoResultadoMagentoDto> CadastrarPedidoMagento(PedidoMagentoDto pedidoMagento, string usuario)
        {
            PedidoResultadoMagentoDto resultado = new PedidoResultadoMagentoDto
            {
                IdsPedidosFilhotes = new List<string>(),
                ListaErros = new List<string>()
            };

            string orcamentista = configuracaoApiMagento.DadosOrcamentista.Orcamentista;
            string vendedor = usuario;
            string loja = configuracaoApiMagento.DadosOrcamentista.Loja;

            InfraBanco.Modelos.TorcamentistaEindicador torcamentista = await prepedidoBll.BuscarTorcamentista(orcamentista);
            if (torcamentista == null)
            {
                resultado.ListaErros.Add("O Orçamentista não existe!");
                return resultado;
            }

            Cliente.Dados.ClienteCadastroDados clienteMagento = await clienteBll.BuscarCliente(pedidoMagento.Cnpj_Cpf, orcamentista);

            //Cadastrar cliente
            if (clienteMagento == null)
            {
                //vamos seguir o fluxo para cadastrar o cliente e depois fazer o cadastro do pedido
                Cliente.Dados.ClienteCadastroDados clienteCadastro = new Cliente.Dados.ClienteCadastroDados
                {
                    DadosCliente =
                    EnderecoCadastralClienteMagentoDto.DadosClienteDeEnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente, loja, pedidoMagento.Frete, vendedor, orcamentista),
                    RefBancaria = new List<Cliente.Dados.Referencias.RefBancariaClienteDados>(),
                    RefComercial = new List<Cliente.Dados.Referencias.RefComercialClienteDados>()
                };

                //criei o código para sistema_responsavel_cadastro 
                List<string> lstRet = (await clienteBll.CadastrarCliente(clienteCadastro, orcamentista,
                    (byte)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__APIMAGENTO)).ToList();

                //é erro
                if (lstRet.Count > 1)
                {
                    resultado.ListaErros = lstRet;
                    return resultado;
                }
                else if (lstRet.Count == 1)
                {
                    //é o número do id do cliente
                    if (lstRet[0].Length != 12)
                    {
                        resultado.ListaErros = lstRet;
                        return resultado;
                    }
                }
            }

            if (await ValidarPedidoMagentoEMarketplace(pedidoMagento, resultado.ListaErros))
            {
                //estamos criando o pedido com os dados do cliente que vem e não com os dados do cliente que esta na base
                //ex: se o cliente já cadastrado, utilizamos o que vem em PedidoMagentoDto.EnderecoCadastralClienteMagentoDto
                Pedido.Dados.Criacao.PedidoCriacaoDados pedidoDados = await CriarPedidoCriacaoDados(pedidoMagento, orcamentista, loja, vendedor);

                Pedido.Dados.Criacao.PedidoCriacaoRetornoDados ret = await pedidoCriacao.CadastrarPedido(pedidoDados,
                    Convert.ToDecimal(configuracaoApiMagento.LimiteArredondamentoPrecoVendaOrcamentoItem), 0.1M,
                    pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac, pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem,
                    pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace,
                    (int)Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__APIMAGENTO);

                resultado.IdPedidoCadastrado = ret.Id;
                resultado.IdsPedidosFilhotes = ret.ListaIdPedidosFilhotes;
                resultado.ListaErros = ret.ListaErros;
            }

            return resultado;
        }

        public async Task<IEnumerable<Pedido.Dados.Criacao.PedidoProdutoPedidoDados>> ConverterProdutosMagento(PedidoMagentoDto pedidoMagento,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao, string loja)
        {
            List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados> listaProdutos = new List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados>();
            List<string> lstFornec = new List<string>();
            lstFornec = pedidoMagento.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            //preciso da lista de coeficientes de cada fabricante da lista de produtos
            List<Produto.Dados.CoeficienteDados> lstCoeficiente = new List<Produto.Dados.CoeficienteDados>();
            //preciso obter a qtde de parcelas e a sigla de pagto
            var qtdeParcelas = prepedidoBll.ObterQtdeParcelasFormaPagto(formaPagtoCriacao);
            var siglaParc = prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao);
            lstCoeficiente = (await validacoesPrepedidoBll.MontarListaCoeficiente(lstFornec, qtdeParcelas,
                prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao))).ToList();

            List<Produto.Dados.ProdutoDados> lstTodosProdutos = (await produtoGeralBll.BuscarTodosProdutos(loja)).ToList();

            if (lstTodosProdutos?.Count > 0)
            {
                foreach (var y in pedidoMagento.ListaProdutos)
                {
                    Produto.Dados.ProdutoDados produto = lstTodosProdutos.Select(x => x)
                    .Where(x => x.Fabricante == y.Fabricante && x.Produto == y.Produto)
                    .FirstOrDefault();

                    Produto.Dados.CoeficienteDados coeficiente = (from c in lstCoeficiente
                                                                  where c.Fabricante == produto.Fabricante &&
                                                                        c.TipoParcela == siglaParc
                                                                  select c).FirstOrDefault();

                    if (y.Fabricante == produto.Fabricante &&
                        y.Fabricante == coeficiente.Fabricante &&
                        y.Produto == produto.Produto && y.Fabricante == coeficiente.Fabricante)
                    {
                        listaProdutos.Add(PedidoProdutoMagentoDto.ProdutosDePedidoProdutoMagentoDto(y, produto, coeficiente.Coeficiente));
                    }

                };
            }

            return await Task.FromResult(listaProdutos);
        }

        private async Task<Pedido.Dados.Criacao.PedidoCriacaoDados> CriarPedidoCriacaoDados(PedidoMagentoDto pedidoMagento,
            string orcamentista, string loja, string vendedor)
        {
            //o cliente existe então vamos converter os dados do cliente para DadosCliente e EnderecoCadastral
            Cliente.Dados.DadosClienteCadastroDados dadosCliente =
                EnderecoCadastralClienteMagentoDto.DadosClienteDeEnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente, loja,
                pedidoMagento.Frete, vendedor, orcamentista);

            Cliente.Dados.EnderecoCadastralClientePrepedidoDados enderecoCadastral =
                EnderecoCadastralClienteMagentoDto.EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente);

            Cliente.Dados.EnderecoEntregaClienteCadastroDados enderecoEntrega =
                EnderecoEntregaClienteMagentoDto.EnderecoEntregaDeEnderecoEntregaClienteMagentoDto(pedidoMagento.EnderecoEntrega, pedidoMagento.OutroEndereco);

            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao =
                FormaPagtoCriacaoMagentoDto.FormaPagtoCriacaoDados_De_FormaPagtoCriacaoMagentoDto(pedidoMagento.FormaPagtoCriacao,
                configuracaoApiMagento, pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem);

            List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados> listaProdutos =
                (await ConverterProdutosMagento(pedidoMagento, formaPagtoCriacao, configuracaoApiMagento.DadosOrcamentista.Loja)).ToList();

            //Precisamos buscar os produtos para poder incluir os valores para incluir na classe de produto
            Pedido.Dados.Criacao.PedidoCriacaoDados pedidoDadosCriacao =
                PedidoMagentoDto.PedidoDadosCriacaoDePedidoMagentoDto(dadosCliente, enderecoCadastral, enderecoEntrega,
                listaProdutos, formaPagtoCriacao, pedidoMagento.VlTotalDestePedido, pedidoMagento);

            return await Task.FromResult(pedidoDadosCriacao);
        }

        public async Task<MarketplaceResultadoDto> ObterCodigoMarketplace()
        {
            MarketplaceResultadoDto resultado = new MarketplaceResultadoDto
            {
                ListaMarketplace = new List<MarketplaceMagentoDto>(),
                ListaErros = new List<string>()
            };


            List<InfraBanco.Modelos.TcodigoDescricao> listarCodigo = (await UtilsGlobais.Util.ListarCodigoMarketPlace(contextoProvider)).ToList();

            if (listarCodigo == null)
            {
                resultado.ListaErros.Add("Falha ao buscar lista Marketplace.");
                return resultado;
            }


            foreach (var x in listarCodigo)
            {
                resultado.ListaMarketplace.Add(new MarketplaceMagentoDto()
                {
                    Grupo = x.Grupo,
                    Descricao = x.Descricao,
                    Descricao_parametro = x.Descricao_parametro,
                    Parametro_1_campo_flag = x.Parametro_1_campo_flag,
                    Parametro_2_campo_flag = x.Parametro_2_campo_flag,
                    Parametro_2_campo_texto = x.Parametro_2_campo_texto,
                    Parametro_3_campo_flag = x.Parametro_3_campo_flag,
                    Parametro_3_campo_texto = x.Parametro_3_campo_texto,
                    Parametro_4_campo_flag = x.Parametro_4_campo_flag,
                    Parametro_5_campo_flag = x.Parametro_5_campo_flag,
                    Parametro_campo_texto = x.Parametro_campo_texto
                });
            };

            return resultado;
        }

        private async Task<bool> ValidarPedidoMagentoEMarketplace(PedidoMagentoDto pedidoMagento, List<string> lstErros)
        {
            var db = contextoProvider.GetContextoLeitura();
            //vamos validar o número do pedido magento
            if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac))
            {
                lstErros.Add("Favor informar o número do pedido Magento(Pedido_bs_x_ac)!");
                return false;
            }

            if (pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac.Length != Constantes.MAX_TAMANHO_ID_PEDIDO_MAGENTO)
            {
                lstErros.Add("Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!");
                return false;
            }


            /*
             * Pedido_bs_x_marketplace e Marketplace_codigo_origem
             * ou os dois existem ou nenhum dos dois existe
             * */
            if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace) && string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem))
                return true;

            if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace) && !string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem))
            {
                lstErros.Add("Informe o Pedido_bs_x_marketplace.");
                return false;
            }
            if (!string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace) && string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem))
            {
                lstErros.Add("Informe o Marketplace_codigo_origem.");
                return false;
            }

            //validar o Marketplace_codigo_origem
            List<InfraBanco.Modelos.TcodigoDescricao> listarCodigo = (await UtilsGlobais.Util.ListarCodigoMarketPlace(contextoProvider)).ToList();
            //todo: afazer: veriicar como é validado
            InfraBanco.Modelos.TcodigoDescricao tcodigo = listarCodigo.Select(x => x)
                .Where(x => x.Codigo == pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem)
                .FirstOrDefault();

            if (tcodigo == null)
            {
                lstErros.Add("Código Marketplace não encontrado.");
                return false;
            }

            return true;
        }

    }
}
