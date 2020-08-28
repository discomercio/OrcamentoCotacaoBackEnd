using InfraBanco.Constantes;
using MagentoBusiness.MagentoDto.ClienteMagentoDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MagentoBusiness.MagentoDto.PedidoMagentoDto
{
    public class PedidoMagentoDto
    {
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

        public EnderecoEntregaClienteMagentoDto EnderecoEntrega { get; set; }

        [Required]
        public List<PedidoProdutoMagentoDto> ListaProdutos { get; set; }

        //PermiteRAStatus = true, sempre

        [Required]
        public decimal VlTotalDestePedido { get; set; }

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
        public string Obs_1 { get; set; }

        public string PontoReferencia { get; set; }
        public decimal? Frete { get; set; }

        public static Pedido.Dados.Criacao.PedidoCriacaoDados PedidoDadosCriacaoDePedidoMagentoDto(Cliente.Dados.DadosClienteCadastroDados dadosClienteMagento,
            Cliente.Dados.EnderecoCadastralClientePrepedidoDados enderecoCadastralClienteMagento, Cliente.Dados.EnderecoEntregaClienteCadastroDados enderecoEntregaMagento,
            List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados> lstProdutosMagento, Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacaoMagento)
        {
            Pedido.Dados.Criacao.PedidoCriacaoDados pedidoCriacao = new Pedido.Dados.Criacao.PedidoCriacaoDados();

            //afazer: realizar a conversão de dados
            pedidoCriacao.LojaUsuario = "ler do appsettings";
            //Armazena nome do usuário logado
            pedidoCriacao.Usuario = "ler do appsettings";
            //Armazena o nome do vendedor externo
            //obs: analisar melhor quando esse campos será preenchido
            pedidoCriacao.VendedorExterno = "Ler do token ou ler do appsettings";

            //Armazena os dados cadastrados do cliente
            pedidoCriacao.DadosCliente = dadosClienteMagento;

            //Armazena os dados do cliente para o Pedido
            pedidoCriacao.EnderecoCadastralCliente = enderecoCadastralClienteMagento;

            //Armazena os dados de endereço de entrega
            pedidoCriacao.EnderecoEntrega = enderecoEntregaMagento;

            //Armazena os dados dos produtos selecionados
            //afazer: converter os produtos
            pedidoCriacao.ListaProdutos = lstProdutosMagento;

            //Armazena os dados da forma de pagto selecionado
            pedidoCriacao.FormaPagtoCriacao = formaPagtoCriacaoMagento;

            //Armazena os dados de entrega imediata, obs, instalador instala, bem de uso comum
            pedidoCriacao.DetalhesPedido = new Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados();
            pedidoCriacao.DetalhesPedido.BemDeUso_Consumo = Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM.ToString();
            pedidoCriacao.DetalhesPedido.InstaladorInstala = Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO.ToString();
            pedidoCriacao.DetalhesPedido.EntregaImediata = dadosClienteMagento.Tipo == Constantes.ID_PF ?
                            Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM.ToString() :
                            Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO.ToString();
            pedidoCriacao.DetalhesPedido.EntregaImediataData = null;
            pedidoCriacao.DetalhesPedido.Observacoes = "";

            //Flag para saber se tem indicador selecionado 
            pedidoCriacao.ComIndicador = false;

            //Armazena o nome do indicador selecionado
            pedidoCriacao.NomeIndicador = null;

            //Armazena o percentual de comissão para o indicador selecionado
            pedidoCriacao.PercRT = 0;

            //Armazena "S" ou "N" para caso de o indicador selecionado permita RA
            pedidoCriacao.OpcaoPossuiRa = true;

            //Armazena o id do centro de distribuição selecionado manualmente
            pedidoCriacao.IdNfeSelecionadoManual = 0; //será sempre automático

            //Flag para saber se o cliente aceitou finalizar o pedido mesmo com produto sem estoque
            pedidoCriacao.OpcaoVendaSemEstoque = false;

            //Armazena o valor total do pedido
            pedidoCriacao.VlTotalDestePedido = 0;

            //Armazena o valor total de pedido com RA
            //Caso o indicador selecionado permita RA esse campo deve receber o valor total do Pedido com RA
            pedidoCriacao.VlTotalDestePedidoComRa = 0;

            return pedidoCriacao;
        }
    }
}
