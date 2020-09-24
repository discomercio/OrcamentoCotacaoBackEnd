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
            List<Pedido.Dados.Criacao.PedidoProdutoPedidoDados> lstProdutosMagento, Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacaoMagento, 
            decimal vlTotalDestePedido, PedidoMagentoDto pedidoMagento)
        {
            Pedido.Dados.Criacao.PedidoCriacaoDados pedidoCriacao = new Pedido.Dados.Criacao.PedidoCriacaoDados();

            pedidoCriacao.LojaUsuario = dadosClienteMagento.Loja;
            //Armazena nome do usuário logado
            pedidoCriacao.Usuario = dadosClienteMagento.Indicador_Orcamentista;
            //Armazena o nome do vendedor externo
            //obs: analisar melhor quando esse campos será preenchido
            pedidoCriacao.VendedorExterno = dadosClienteMagento.Vendedor;

            //Armazena os dados cadastrados do cliente
            pedidoCriacao.DadosCliente = dadosClienteMagento;

            //Armazena os dados do cliente para o Pedido
            pedidoCriacao.EnderecoCadastralCliente = enderecoCadastralClienteMagento;

            //Armazena os dados de endereço de entrega
            pedidoCriacao.EnderecoEntrega = enderecoEntregaMagento;

            //Armazena os dados dos produtos selecionados
            pedidoCriacao.ListaProdutos = lstProdutosMagento;

            //Armazena os dados da forma de pagto selecionado
            pedidoCriacao.FormaPagtoCriacao = formaPagtoCriacaoMagento;

            //Armazena os dados de entrega imediata, obs, instalador instala, bem de uso comum
            pedidoCriacao.DetalhesPedido = new Prepedido.Dados.DetalhesPrepedido.DetalhesPrepedidoDados();
            pedidoCriacao.DetalhesPedido.BemDeUso_Consumo = (short)Constantes.Bem_DeUsoComum.COD_ST_BEM_USO_CONSUMO_SIM;
            pedidoCriacao.DetalhesPedido.InstaladorInstala = (short)Constantes.Instalador_Instala.COD_INSTALADOR_INSTALA_NAO;
            pedidoCriacao.DetalhesPedido.EntregaImediata = dadosClienteMagento.Tipo == Constantes.ID_PF ?
                            Convert.ToString((byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_SIM) :
                            Convert.ToString((byte)Constantes.EntregaImediata.COD_ETG_IMEDIATA_NAO);
            pedidoCriacao.DetalhesPedido.EntregaImediataData = null;
            pedidoCriacao.DetalhesPedido.Observacoes = pedidoMagento.Obs_1;
            //Ponto de referência é armazenado onde???
            //Frete é armazenado onde???

            //Flag para saber se tem indicador selecionado 
            //campo "frete"->se for <> 0, vamos usar o indicador.se for 0, sem indicador
            pedidoCriacao.ComIndicador = !string.IsNullOrEmpty(dadosClienteMagento.Vendedor) ? true : false;

            //Armazena o nome do indicador selecionado
            pedidoCriacao.NomeIndicador = !string.IsNullOrEmpty(dadosClienteMagento.Vendedor) ? dadosClienteMagento.Vendedor : null;

            //Armazena o percentual de comissão para o indicador selecionado
            //afazer: verificar se esta calculando corretamente
            float percRT = 0f;
            lstProdutosMagento.ForEach(x =>
            {
                percRT = percRT + (float)((x.Preco_Lista - (x.Preco_Venda + 1)) / x.Preco_Venda * 100);
            });
            pedidoCriacao.PercRT = percRT;

            //Armazena "S" ou "N" para caso de o indicador selecionado permita RA
            pedidoCriacao.OpcaoPossuiRa = true;
            pedidoCriacao.PermiteRAStatus = 1;

            //Armazena o id do centro de distribuição selecionado manualmente
            pedidoCriacao.IdNfeSelecionadoManual = 0; //será sempre automático

            //Flag para saber se o cliente aceitou finalizar o pedido mesmo com produto sem estoque
            //afazer: verificar se passa true ou false
            pedidoCriacao.OpcaoVendaSemEstoque = true;

            //Armazena o valor total do pedido
            decimal vl_total = lstProdutosMagento.Select(x => x.TotalItem).Sum();
            pedidoCriacao.Vl_total = vl_total;

            //Armazena o valor total de pedido com RA
            //Caso o indicador selecionado permita RA esse campo deve receber o valor total do Pedido com RA
            pedidoCriacao.Vl_total_NF = vlTotalDestePedido;

            return pedidoCriacao;
        }

    }
}
