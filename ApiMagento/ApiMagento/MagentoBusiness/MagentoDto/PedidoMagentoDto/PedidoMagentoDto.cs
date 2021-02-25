using InfraBanco.Constantes;
using MagentoBusiness.MagentoDto.ClienteMagentoDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PedidoMagentoDto
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        [Required]
        public string TokenAcesso { get; set; }

        //Orcamentista = "FRETE" (vamos ler do appsettings)
        //Loja = "201" (vamos ler do appsettings)
        //Vendedor = usuário que fez o login (ler do token)

        [MaxLength(14)]
        [Required]
        public string Cnpj_Cpf { get; set; }

        [Required]
        public InfCriacaoPedidoMagentoDto InfCriacaoPedido { get; set; }

        [Required]
        public EnderecoCadastralClienteMagentoDto EnderecoCadastralCliente { get; set; }

        /// <summary>
        /// Indica se existe um endereço de entrega
        /// <hr />
        /// </summary>
        [Required]
        public bool OutroEndereco { get; set; }

        public EnderecoEntregaClienteMagentoDto? EnderecoEntrega { get; set; }

        [Required]
        public List<PedidoProdutoMagentoDto> ListaProdutos { get; set; }

        //PermiteRAStatus = true, sempre

        //[Required]
        //public decimal VlTotalDestePedido { get; set; }

        //nao existe o DetalhesPedidoMagentoDto. Os valores a usar são:
        //St_Entrega_Imediata: se for PF, sim. Se for PJ, não
        // PrevisaoEntregaData = null
        // BemDeUso_Consumo = COD_ST_BEM_USO_CONSUMO_SIM
        //InstaladorInstala = COD_INSTALADOR_INSTALA_NAO

        [Required]
        public FormaPagtoCriacaoMagentoDto FormaPagtoCriacao { get; set; }

        //CDManual = false
        //PercRT = calculado automaticamente{ get; set; }
        //OpcaoPossuiRA = sim

        [MaxLength(500)]
        public string? Obs_1 { get; set; }

        public decimal? Frete { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public static Pedido.Dados.Criacao.PedidoCriacaoDados? PedidoDadosCriacaoDePedidoMagentoDto(Cliente.Dados.DadosClienteCadastroDados dadosClienteMagento,
            Cliente.Dados.EnderecoCadastralClientePrepedidoDados enderecoCadastralClienteMagento, Cliente.Dados.EnderecoEntregaClienteCadastroDados enderecoEntregaMagento,
            List<Pedido.Dados.Criacao.PedidoCriacaoProdutoDados> lstProdutosMagento, Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacaoMagento,
            PedidoMagentoDto pedidoMagento,
            InfraBanco.Constantes.Constantes.CodSistemaResponsavel sistemaResponsavelCadastro,
            List<string> lstErros, UtilsMagento.ConfiguracaoApiMagento configuracaoApiMagento,
            string? dadosClienteMidia,
            string? dadosClienteIndicador,
            string? nfe_texto_constar)
        {
            if (!Constantes.TipoPessoa.TipoValido(dadosClienteMagento.Tipo))
            {
                lstErros.Add("Tipo de cliente não é PF nem PJ.");
                return null;
            }

            //Armazena os dados de entrega imediata, obs, instalador instala, bem de uso comum
#pragma warning disable IDE0017 // Simplify object initialization
            var pedidoCriacaoDetalhesPedido = new Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados();
#pragma warning restore IDE0017 // Simplify object initialization
            pedidoCriacaoDetalhesPedido.BemDeUso_Consumo = (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM;
            pedidoCriacaoDetalhesPedido.InstaladorInstala = (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO;
            pedidoCriacaoDetalhesPedido.EntregaImediata = dadosClienteMagento.Tipo == Constantes.ID_PF ?
                            Convert.ToString((byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM) :
                            Convert.ToString((byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO);
            pedidoCriacaoDetalhesPedido.EntregaImediataData = null;
            pedidoCriacaoDetalhesPedido.Observacoes = pedidoMagento.Obs_1;
            pedidoCriacaoDetalhesPedido.GarantiaIndicador = Constantes.COD_GARANTIA_INDICADOR_STATUS__NAO;

            var pedidoCriacaoValor = new Pedido.Dados.Criacao.PedidoCriacaoValorDados(
                perc_RT: 0,
                permiteRAStatus: true,

                //Armazena o valor total do pedido
                vl_total: lstProdutosMagento.Select(x => x.TotalItem()).Sum(),

                //Armazena o valor total de pedido com RA
                vl_total_NF: lstProdutosMagento.Select(x => x.TotalItemRA() ?? 0).Sum()
                );

            Pedido.Dados.Criacao.PedidoCriacaoDados pedidoCriacao = new Pedido.Dados.Criacao.PedidoCriacaoDados(
                detalhesPedido: pedidoCriacaoDetalhesPedido,
                ambiente: new Pedido.Dados.Criacao.PedidoCriacaoAmbienteDados(
                    loja: dadosClienteMagento.Loja,
                    //Armazena nome do usuário logado
                    usuario: dadosClienteMagento.Vendedor,
                    vendedor: dadosClienteMagento.Vendedor,

                    venda_Externa: true,

                    //Flag para saber se tem indicador selecionado 
                    //campo "frete"->se for <> 0, vamos usar o indicador.se for 0, sem indicador
                    comIndicador: ((pedidoMagento.Frete ?? 0) != 0) ? true : false,

                    //Armazena o nome do indicador selecionado
                    //campo "frete"->se for <> 0, vamos usar o indicador.se for 0, sem indicador
                    indicador: ((pedidoMagento.Frete ?? 0) != 0) ? configuracaoApiMagento.DadosIndicador.Indicador : "",
                    orcamentista: "",

                    //Armazena o id do centro de distribuição selecionado manualmente
                    id_nfe_emitente_selecao_manual: 0, //será sempre automático

                    //Flag para saber se o cliente aceitou finalizar o pedido mesmo com produto sem estoque
                    opcaoVendaSemEstoque: true,

                    loja_indicou: "",

                    operacao_origem: Constantes.Op_origem__pedido_novo.OP_ORIGEM__PEDIDO_NOVO_EC_SEMI_AUTO,
                    id_param_site: InfraBanco.Constantes.Constantes.Cod_site.COD_SITE_ARTVEN_BONSHOP
                    ),

                extra: new Pedido.Dados.Criacao.PedidoCriacaoExtraDados(
                    pedido_bs_x_at: null,
                    nfe_Texto_Constar: nfe_texto_constar,
                    nfe_XPed: null),

                //Armazena os dados cadastrados do cliente
                cliente: Pedido.Dados.Criacao.PedidoCriacaoClienteDados.PedidoCriacaoClienteDados_de_DadosClienteCadastroDados(
                    dadosClienteMagento, midia: dadosClienteMidia, indicador: dadosClienteIndicador),

                //Armazena os dados do cliente para o Pedido
                enderecoCadastralCliente: enderecoCadastralClienteMagento,

                //Armazena os dados de endereço de entrega
                enderecoEntrega: enderecoEntregaMagento,

                //Armazena os dados dos produtos selecionados
                listaProdutos: lstProdutosMagento,

                //Armazena os dados da forma de pagto selecionado
                formaPagtoCriacao: formaPagtoCriacaoMagento,

                valor: pedidoCriacaoValor,

                configuracao: new Pedido.Dados.Criacao.PedidoCriacaoConfiguracaoDados(
                    limiteArredondamento: configuracaoApiMagento.LimiteArredondamentoPrecoVendaOrcamentoItem,
                    maxErroArredondamento: 0.1M,
                    sistemaResponsavelCadastro: sistemaResponsavelCadastro,
                    limitePedidosExatamenteIguais_Numero: configuracaoApiMagento.LimitePedidos.LimitePedidosExatamenteIguais_Numero,
                    limitePedidosExatamenteIguais_TempoSegundos: configuracaoApiMagento.LimitePedidos.LimitePedidosExatamenteIguais_TempoSegundos,
                    limitePedidosMesmoCpfCnpj_Numero: configuracaoApiMagento.LimitePedidos.LimitePedidosMesmoCpfCnpj_Numero,
                    limitePedidosMesmoCpfCnpj_TempoSegundos: configuracaoApiMagento.LimitePedidos.LimitePedidosMesmoCpfCnpj_TempoSegundos
                    ),
                marketplace: new Pedido.Dados.Criacao.PedidoCriacaoMarketplaceDados(
                        pedido_bs_x_ac: pedidoMagento.InfCriacaoPedido.Pedido_bs_x_ac,
                        marketplace_codigo_origem: pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem,
                        pedido_bs_x_marketplace: pedidoMagento.InfCriacaoPedido.Pedido_bs_x_marketplace)
                );

            return pedidoCriacao;
        }

    }
}
