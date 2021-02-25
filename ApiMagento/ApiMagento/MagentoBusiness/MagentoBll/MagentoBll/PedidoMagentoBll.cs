﻿using MagentoBusiness.MagentoDto.PedidoMagentoDto;
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
using Cliente;

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
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        private readonly PedidoMagentoClienteBll pedidoMagentoClienteBll;

        public PedidoMagentoBll(InfraBanco.ContextoBdProvider contextoProvider,
            Produto.ProdutoGeralBll produtoGeralBll,
            Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll, PrepedidoBll prepedidoBll,
            ConfiguracaoApiMagento configuracaoApiMagento, Pedido.Criacao.PedidoCriacao pedidoCriacao,
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

            //normalizacao de campos
            pedidoMagento.Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedidoMagento.Cnpj_Cpf);


            var indicador_vendedor_loja = await Calcular_indicador_vendedor_loja(pedidoMagento, usuario, resultado.ListaErros);
            pedidoMagentoClienteBll.LimitarPedidosMagentoPJ(pedidoMagento, resultado.ListaErros);
            await ValidarPedidoMagentoEMarketplace(pedidoMagento, resultado.ListaErros);
            if (resultado.ListaErros.Count > 0)
                return resultado;

            //truncar EndEtg_endereco_complemento
            string? nfe_Texto_Constar = null;
            if (pedidoMagento.EnderecoEntrega?.EndEtg_endereco_complemento?.Length > Constantes.MAX_TAMANHO_CAMPO_ENDERECO_COMPLEMENTO)
                nfe_Texto_Constar = "Complemento do endereço: " + pedidoMagento.EnderecoEntrega?.EndEtg_endereco_complemento;

            if (pedidoMagento.EnderecoEntrega?.PontoReferencia?.ToUpper().Trim() !=
                pedidoMagento.EnderecoEntrega?.EndEtg_endereco_complemento?.ToUpper().Trim() &&
                !string.IsNullOrEmpty(pedidoMagento.EnderecoEntrega?.PontoReferencia))
            {
                if (!string.IsNullOrEmpty(nfe_Texto_Constar))
                    nfe_Texto_Constar += "\n";
                nfe_Texto_Constar = nfe_Texto_Constar + "Ponto de referência: " + pedidoMagento.EnderecoEntrega?.PontoReferencia;
            }

            //Cadastrar cliente
            //#    Endereço
            //#    Se o cliente for PF, sempre será usado somente o endereço de entrega como sendo o único endereço do cliente.
            //#    Se for PF, copio o endereço de entrega para o endereço de cobrança e apago o endereço de entrega.
            //#	    no caso de campos que só existam no endereço de cobrança (exemplo: telefone) mantemos o do endereço de cobrança e não exigimos o campo.
            //#    Depois disso, se o cliente não existir, cadastrar com o endereço de cobrança
            pedidoMagentoClienteBll.MoverEnderecoEntregaParaEnderecoCadastral(pedidoMagento, resultado.ListaErros);
            //se der erro ao mover não podemos tentar cadastrar
            if (resultado.ListaErros.Count > 0)
                return resultado;

            var cliente = await pedidoMagentoClienteBll.CadastrarClienteSeNaoExistir(pedidoMagento, resultado.ListaErros, indicador_vendedor_loja, usuario);
            if (cliente == null)
            {
                resultado.ListaErros.Add("Erro ao cadastrar cliente");
                return resultado;
            }
            if (resultado.ListaErros.Count > 0)
                return resultado;

            /* ====================================
             * Inclui a verificação de loja aqui pois caso a loja não exista a rotina "CriarPedidoCriacaoDados" abaixo irá             
             * gerar erro ao complementar os produtos "ConverterProdutosMagento"
            */
            var db = contextoProvider.GetContextoLeitura();
            var lojaExiste = await db.Tlojas.Where(c => c.Loja == indicador_vendedor_loja.loja).AnyAsync();
            if (!lojaExiste)
                resultado.ListaErros.Add("Loja não existe!");

            //estamos criando o pedido com os dados do cliente que vem e não com os dados do cliente que esta na base
            //ex: se o cliente já cadastrado, utilizamos o que vem em PedidoMagentoDto.EnderecoCadastralClienteMagentoDto
            Pedido.Dados.Criacao.PedidoCriacaoDados? pedidoDados = await CriarPedidoCriacaoDados(pedidoMagento,
                indicador_vendedor_loja, resultado.ListaErros, id_cliente: cliente.Id, dadosClienteMidia: cliente.Midia,
                dadosClienteIndicador: cliente.Indicador, nfe_Texto_Constar);
            if (resultado.ListaErros.Count != 0)
                return resultado;
            if (pedidoDados == null)
            {
                resultado.ListaErros.Add("Erro na rotina CriarPedidoCriacaoDados");
                return resultado;
            }

            Pedido.Dados.Criacao.PedidoCriacaoRetornoDados ret = await pedidoCriacao.CadastrarPedido(pedidoDados);

            resultado.IdPedidoCadastrado = ret.Id;
            resultado.IdsPedidosFilhotes = ret.ListaIdPedidosFilhotes;
            resultado.ListaErros = ret.ListaErros;
            resultado.ListaErros.AddRange(ret.ListaErrosValidacao);

            return resultado;
        }

        internal class Indicador_vendedor_loja
        {
            public readonly string? indicador;
            public readonly string vendedor;
            public readonly string loja;

            public Indicador_vendedor_loja(string? indicador, string vendedor, string loja)
            {
                this.indicador = indicador;
                this.vendedor = vendedor;
                this.loja = loja;
            }
        }

        private async Task<Indicador_vendedor_loja> Calcular_indicador_vendedor_loja(PedidoMagentoDto pedidoMagento, string usuario, List<string> listaErros)
        {
            //campo "frete"->se for <> 0, vamos usar o indicador.se for 0, sem indicador
            string? indicador = null;
            if (pedidoMagento.Frete.HasValue && pedidoMagento.Frete != 0)
            {
                indicador = configuracaoApiMagento.DadosIndicador.Indicador;
                if (!await prepedidoBll.TorcamentistaExiste(indicador))
                    listaErros.Add("O Indicador não existe!");
            }
            string vendedor = usuario;
            string loja = configuracaoApiMagento.DadosIndicador.Loja;
            return new Indicador_vendedor_loja(indicador, vendedor, loja);
        }

        private async Task<IEnumerable<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados>> ConverterProdutosMagento(PedidoMagentoDto pedidoMagento,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao, string loja, List<string> lstErros)
        {
            List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> listaProdutos = new List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados>();
            List<string> lstFornec = pedidoMagento.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            //preciso da lista de coeficientes de cada fabricante da lista de produtos
            //preciso obter a qtde de parcelas e a sigla de pagto
            var qtdeParcelas = PrepedidoBll.ObterCustoFinancFornecQtdeParcelasDeFormaPagto(formaPagtoCriacao);
            var siglaParc = prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao);
            List<Produto.Dados.CoeficienteDados> lstCoeficiente = (await validacoesPrepedidoBll.MontarListaCoeficiente(
                lstFornec, qtdeParcelas, siglaParc)).ToList();

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

                if (produto == null)
                    lstErros.Add($"Produto não cadastrado para a loja. Produto: {y.Produto}, loja: {loja}");
                if (coeficiente == null)
                    lstErros.Add($"Coeficiente não cadastrado para o fabricante. Fabricante: {y.Fabricante}, TipoParcela: {siglaParc}");
                if (produto != null && coeficiente != null)
                    listaProdutos.Add(PedidoProdutoMagentoDto.PedidoCriacaoProdutoDados_De_PedidoProdutoMagentoDto(y, produto, coeficiente.Coeficiente));
            }

            return await Task.FromResult(listaProdutos);
        }

        private async Task<Pedido.Dados.Criacao.PedidoCriacaoDados?> CriarPedidoCriacaoDados(PedidoMagentoDto pedidoMagento,
            Indicador_vendedor_loja indicador_Vendedor_Loja, List<string> lstErros,
            string id_cliente, string? dadosClienteMidia, string? dadosClienteIndicador,
            string? nfe_Texto_Constar)
        {
            var sistemaResponsavelCadastro = Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO;

            //o cliente existe então vamos converter os dados do cliente para DadosCliente e EnderecoCadastral
            Cliente.Dados.DadosClienteCadastroDados dadosCliente =
                EnderecoCadastralClienteMagentoDto.DadosClienteDeEnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente,
                indicador_Vendedor_Loja.loja,
                indicador_Vendedor_Loja.vendedor, configuracaoApiMagento.DadosIndicador.Indicador,
                id_cliente);

            Cliente.Dados.EnderecoCadastralClientePrepedidoDados enderecoCadastral =
                EnderecoCadastralClienteMagentoDto.EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente);

            Cliente.Dados.EnderecoEntregaClienteCadastroDados enderecoEntrega =
                EnderecoEntregaClienteMagentoDto.EnderecoEntregaDeEnderecoEntregaClienteMagentoDto(pedidoMagento.EnderecoEntrega, pedidoMagento.OutroEndereco,
                configuracaoApiMagento.EndEtg_cod_justificativa);

            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao =
                FormaPagtoCriacaoMagentoDto.FormaPagtoCriacaoDados_De_FormaPagtoCriacaoMagentoDto(pedidoMagento.FormaPagtoCriacao,
                configuracaoApiMagento, pedidoMagento.InfCriacaoPedido);

            List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> listaProdutos =
                (await ConverterProdutosMagento(pedidoMagento, formaPagtoCriacao, configuracaoApiMagento.DadosIndicador.Loja, lstErros)).ToList();

            //Precisamos buscar os produtos para poder incluir os valores para incluir na classe de produto
            var pedidoDadosCriacao =
                PedidoMagentoDto.PedidoDadosCriacaoDePedidoMagentoDto(dadosCliente, enderecoCadastral, enderecoEntrega,
                listaProdutos, formaPagtoCriacao, pedidoMagento,
                sistemaResponsavelCadastro,
                lstErros,
                configuracaoApiMagento,
                dadosClienteMidia: dadosClienteMidia,
                dadosClienteIndicador: dadosClienteIndicador,
                nfe_Texto_Constar);

            return pedidoDadosCriacao;
        }

        private async Task ValidarPedidoMagentoEMarketplace(PedidoMagentoDto pedidoMagento, List<string> lstErros)
        {
            /* ALTERAÇÕES NA VALIDAÇÃO conversa com Hamilton - 23/02/2021
             * O campo "Marketplace_codigo_origem" é obrigatório e deve ser sempre enviado
             * no caso de Marketplace_codigo_origem = 001 veio da Arclube
             */
            var db = contextoProvider.GetContextoLeitura();

            //validar o Marketplace_codigo_origem
            if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem))
            {
                lstErros.Add("Informe o Marketplace_codigo_origem.");
                return;
            }

            var codigoMarketplace = await UtilsGlobais.Util.ListarCodigoMarketPlace(contextoProvider)
                       .Where(x => x.Codigo == pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem).AnyAsync();

            if (!codigoMarketplace)
                lstErros.Add("Código Marketplace não encontrado.");

            if (pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem == Constantes.COD_MARKETPLACE_ARCLUBE)
            {
                //vamos validar o número do pedido magento
                if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac))
                    lstErros.Add("Favor informar o número do pedido Magento(Pedido_bs_x_ac)!");

                if (pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac.Length != Constantes.MAX_TAMANHO_ID_PEDIDO_MAGENTO)
                    lstErros.Add("Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!");

                if (UtilsGlobais.Util.ExisteLetras(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac))
                    lstErros.Add("O número Magento deve conter apenas dígitos!");
            }

            if (pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem != Constantes.COD_MARKETPLACE_ARCLUBE)
            {
                //vamos validar o número do pedido marketplace
                if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace))
                    lstErros.Add("Informe o Pedido_bs_x_marketplace.");
            }

        }

    }
}
