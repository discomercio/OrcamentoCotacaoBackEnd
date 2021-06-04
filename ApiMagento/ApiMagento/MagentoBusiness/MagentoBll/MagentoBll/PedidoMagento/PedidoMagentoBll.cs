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
using Cliente;
using static InfraBanco.Constantes.Constantes;
using InfraBanco.Modelos;

#nullable enable

namespace MagentoBusiness.MagentoBll.MagentoBll.PedidoMagento
{
    public class PedidoMagentoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly Produto.ProdutoGeralBll produtoGeralBll;
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;
        private readonly PrepedidoBll prepedidoBll;
        private readonly ConfiguracaoApiMagento configuracaoApiMagento;
        private readonly Pedido.Criacao.PedidoCriacao pedidoCriacao;
        private readonly PedidoMagento.PedidoMagentoClienteBll pedidoMagentoClienteBll;
        private readonly P40Produtos p40produtos;

        public PedidoMagentoBll(InfraBanco.ContextoBdProvider contextoProvider,
            Produto.ProdutoGeralBll produtoGeralBll,
            Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll, PrepedidoBll prepedidoBll,
            ConfiguracaoApiMagento configuracaoApiMagento, Pedido.Criacao.PedidoCriacao pedidoCriacao,
            PedidoMagento.PedidoMagentoClienteBll pedidoMagentoClienteBll)
        {
            this.contextoProvider = contextoProvider;
            this.produtoGeralBll = produtoGeralBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
            this.prepedidoBll = prepedidoBll;
            this.configuracaoApiMagento = configuracaoApiMagento;
            this.pedidoCriacao = pedidoCriacao;
            this.pedidoMagentoClienteBll = pedidoMagentoClienteBll;
            this.p40produtos = new P40Produtos(prepedidoBll: prepedidoBll, validacoesPrepedidoBll: validacoesPrepedidoBll,
                produtoGeralBll: produtoGeralBll, contextoProvider: contextoProvider);
        }

        public async Task<PedidoMagentoResultadoDto> CadastrarPedidoMagento(PedidoMagentoDto pedidoMagento, string usuario)
        {
            PedidoMagentoResultadoDto resultado = new PedidoMagentoResultadoDto();

            #region setup de variáveis
            //normalizacao de campos
            pedidoMagento.Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(pedidoMagento.Cnpj_Cpf);
            var indicador_vendedor_loja = await Calcular_indicador_vendedor_loja(pedidoMagento, usuario, resultado.ListaErros);
            #endregion

            #region nfe_Texto_Constar
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

            #endregion


            Tcliente? cliente = await P10_Cliente(pedidoMagento, usuario, resultado, indicador_vendedor_loja);
            if (cliente == null)
            {
                resultado.ListaErros.Add("Erro ao cadastrar cliente");
                return resultado;
            }


            Passos.P30_InfPedido_MagentoPedidoStatus(pedidoMagento, resultado.ListaErros);
            Passos.P35_Totais(pedidoMagento, resultado.ListaErros, configuracaoApiMagento.LimiteArredondamentoTotais);
            Passos.P39_Servicos(pedidoMagento.ListaServicos, resultado.ListaErros, configuracaoApiMagento.LimiteArredondamentoPorItem);
            if (resultado.ListaErros.Count > 0)
                return resultado;

            #region setup para calcular produtos
            //fazemos a verificação de loja aqui pois caso a loja não exista a busca pelos produtos não encontrará nada
            var db = contextoProvider.GetContextoLeitura();
            var lojaExiste = await db.Tlojas.Where(c => c.Loja == indicador_vendedor_loja.loja).AnyAsync();
            if (!lojaExiste)
                resultado.ListaErros.Add($"Loja não existe, loja = {indicador_vendedor_loja.loja}!");

            //converte a forma de pagamento
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao =
                FormaPagtoCriacaoMagentoDto.FormaPagtoCriacaoDados_De_FormaPagtoCriacaoMagentoDto(pedidoMagento.FormaPagtoCriacao,
                configuracaoApiMagento, pedidoMagento.InfCriacaoPedido);
            #endregion

            //gera a lista de produtos
            List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> listaProdutos = await p40produtos.ExecutarAsync(
                pedidoMagento, resultado.ListaErros, formaPagtoCriacao,
                configuracaoApiMagento.DadosIndicador.Loja,
                configuracaoApiMagento.LimiteArredondamentoPorItem,
                configuracaoApiMagento.LimiteArredondamentoTotais,
                configuracaoApiMagento.LimitePedidos.LimiteItens);


            //estamos criando o pedido com os dados do cliente que vem e não com os dados do cliente que esta na base
            Pedido.Dados.Criacao.PedidoCriacaoDados? pedidoDados = CriarPedidoCriacaoDados(pedidoMagento,
                indicador_vendedor_loja, resultado.ListaErros, id_cliente: cliente.Id, dadosClienteMidia: cliente.Midia,
                dadosClienteIndicador: cliente.Indicador, nfe_Texto_Constar, listaProdutos, formaPagtoCriacao);
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

        private async Task<Tcliente?> P10_Cliente(PedidoMagentoDto pedidoMagento, string usuario, PedidoMagentoResultadoDto resultado, Indicador_vendedor_loja indicador_vendedor_loja)
        {
            pedidoMagentoClienteBll.LimitarPedidosMagentoPJ(pedidoMagento, resultado.ListaErros);
            await ValidarPedidoMagentoEMarketplace(pedidoMagento, resultado.ListaErros);
            if (resultado.ListaErros.Count > 0)
                return null;

            //Cadastrar cliente
            //#    Endereço
            //#    Se o cliente for PF, sempre será usado somente o endereço de entrega como sendo o único endereço do cliente.
            //#    Se for PF, copio o endereço de entrega para o endereço de cobrança e apago o endereço de entrega.
            //#	    no caso de campos que só existam no endereço de cobrança (exemplo: telefone) mantemos o do endereço de cobrança e não exigimos o campo.
            //#    Depois disso, se o cliente não existir, cadastrar com o endereço de cobrança
            pedidoMagentoClienteBll.MoverEnderecoEntregaParaEnderecoCadastral(pedidoMagento, resultado.ListaErros);
            //se der erro ao mover não podemos tentar cadastrar
            if (resultado.ListaErros.Count > 0)
                return null;

            var cliente = await pedidoMagentoClienteBll.CadastrarClienteSeNaoExistir(pedidoMagento, resultado.ListaErros, indicador_vendedor_loja, usuario);
            return cliente;
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
            if (pedidoMagento.TotaisPedido.FreteBruto != 0)
            {
                indicador = configuracaoApiMagento.DadosIndicador.Indicador;
                if (!await prepedidoBll.TorcamentistaExiste(indicador))
                    listaErros.Add("O Indicador não existe!");
            }
            string vendedor = usuario;
            string loja = configuracaoApiMagento.DadosIndicador.Loja;
            return new Indicador_vendedor_loja(indicador, vendedor, loja);
        }

        private Pedido.Dados.Criacao.PedidoCriacaoDados? CriarPedidoCriacaoDados(PedidoMagentoDto pedidoMagento,
            Indicador_vendedor_loja indicador_Vendedor_Loja, List<string> lstErros,
            string id_cliente, string? dadosClienteMidia, string? dadosClienteIndicador,
            string? nfe_Texto_Constar, List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> listaProdutos,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao)
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

            //vamos validar o número do pedido magento
            if (string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Pedido_magento))
                lstErros.Add("Favor informar o número do pedido Magento(Pedido_bs_x_ac)!");

            if (pedidoMagento.InfCriacaoPedido.Pedido_magento.Length != Constantes.MAX_TAMANHO_ID_PEDIDO_MAGENTO)
                lstErros.Add("Nº pedido Magento(Pedido_bs_x_ac) com formato inválido!");

            if (UtilsGlobais.Util.ExisteLetras(pedidoMagento.InfCriacaoPedido.Pedido_magento))
                lstErros.Add("O número Magento deve conter apenas dígitos!");

        }

    }
}
