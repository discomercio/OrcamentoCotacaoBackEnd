using MagentoBusiness.MagentoDto.PedidoMagentoDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pedido.Dados;
using MagentoBusiness.MagentoDto.ClienteMagentoDto;
using System.Linq;
using InfraBanco.Constantes;
using Prepedido;

namespace MagentoBusiness.MagentoBll.PedidoMagentoBll
{
    public class PedidoMagentoBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly Cliente.ClienteBll clienteBll;
        private readonly Produto.ProdutoGeralBll produtoGeralBll;
        private readonly Produto.CoeficienteBll coeficienteBll;
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;

        public PedidoMagentoBll(InfraBanco.ContextoBdProvider contextoProvider,
            Cliente.ClienteBll clienteBll, Produto.ProdutoGeralBll produtoGeralBll,
            Produto.CoeficienteBll coeficienteBll,
            Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll)
        {
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
            this.produtoGeralBll = produtoGeralBll;
            this.coeficienteBll = coeficienteBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
        }

        public async Task CadastrarPedidoMagento(PedidoMagentoDto pedidoMagento)
        {
            /* Começar a implantar o 
             * ArClube/ApiMagento/ApiMagento/ApiMagento/Controllers/PedidoMagentoController.cs CadastrarPrepedido
             * Converter a estrutura de dados da Api Magento para PedidoDados
             * começar as validações do PedidoCriacao/CadastrarPrepedido.
             * Conforne precisar, mover código da PrepedidoAPi para GLobal/Utils ou GLobal/Prepedido.
             * Isso a gente vai se falando.
             * ========================================================================================
             * Colocar na API: 
             * - campo "ponto de referência" 
             * - campo "frete" -> se for <> 0, vamos usar o indicador. se for 0, sem indicador 
             * ter uma função para listar o código do marketplace 
             * (tabela t_CODIGO_DESCRICAO grupo = PedidoECommerce_Origem 
             * vamos retornar o código (tipo 003) e o campo parametro_campo_texto ou parametro_2_campo_texto)
             * */

            //Afazer: verificar se será feito validação de indicador

            //Fluxo para cadastrar Pedido Magento
            // Validar orçamentista??????
            // Verificar se o cliente existe no cadastro:
            //      Se não existe => 
            //          passar EnderecoCadastralClienteMagentoDto para 
            //      Se existe => 
            //          buscar o orçamentista/indicador Magento
            //          converter enderecoCadastral 
            //          converter enderecoEntrega com o mesmo os mesmos dados de endereco cadastral
            //          converter lista de produtos
            //          converter detalhes do pedido
            //          converter para PedidoDadosCriacao 
            // Cadastrar Pedido

            var db = contextoProvider.GetContextoLeitura();

            string orcamentista = "buscar do appsettings";
            string loja = "buscar do appsettings";

            var t = UtilsGlobais.Util.LojaHabilitadaProdutosECommerce(loja, contextoProvider);

            var clienteMagento = clienteBll.BuscarCliente(pedidoMagento?.Cnpj_Cpf, orcamentista);

            if (await clienteMagento == null)
            {
                //vamos seguir o fluxo para cadastrar o cliente e depois fazer o cadastro do pedido
            }

            //o cliente existe então vamos converter os dados do cliente para DadosCliente e EnderecoCadastral
            Cliente.Dados.DadosClienteCadastroDados dadosCliente = DadosClienteDeEnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente, "vem do appsettings");
            Cliente.Dados.EnderecoCadastralClientePrepedidoDados enderecoCadastral = EnderecoCadastralClienteMagentoDto.EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente);
            Cliente.Dados.EnderecoEntregaClienteCadastroDados enderecoEntrega = EnderecoEntregaClienteMagentoDto.EnderecoEntregaDeEnderecoEntregaClienteMagentoDto(pedidoMagento.EnderecoEntrega, pedidoMagento.OutroEndereco);
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao = FormaPagtoCriacaoMagentoDto.FormaPagtoCriacaoDados_De_FormaPagtoCriacaoMagentoDto(pedidoMagento.FormaPagtoCriacao);
            List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados> listaProdutos = await ConverterProdutosMagento(pedidoMagento, formaPagtoCriacao, loja);

            //Precisamos buscar os produtos para poder incluir os valores para incluir na classe de produto
            Pedido.Dados.Criacao.PedidoCriacaoDados pedidoDadosCriacao =
                PedidoMagentoDto.PedidoDadosCriacaoDePedidoMagentoDto(
                    dadosCliente, enderecoCadastral, enderecoEntrega, listaProdutos, formaPagtoCriacao);

        }

        private async Task<List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados>> ConverterProdutosMagento(PedidoMagentoDto pedidoMagento,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao, string loja)
        {
            List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados> listaProdutos = new List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados>();
            List<string> lstFornec = new List<string>();
            lstFornec = pedidoMagento.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            //preciso da lista de coeficientes de cada fabricante da lista de produtos
            List<Produto.Dados.CoeficienteDados> lstCoeficiente = new List<Produto.Dados.CoeficienteDados>();
            //preciso obter a qtde de parcelas e a sigla de pagto
            lstCoeficiente = (await validacoesPrepedidoBll.MontarListaCoeficiente(lstFornec,
                Duplicado_em_prepedido_remover_daqui_ObterQtdeParcelasFormaPagto(formaPagtoCriacao), 
                Duplicado_em_prepedido_remover_daqui_ObterSiglaFormaPagto(formaPagtoCriacao))).ToList();

            List<Produto.Dados.ProdutoDados> lstTodosProdutos = (await produtoGeralBll.BuscarTodosProdutos(loja)).ToList();

            if (lstTodosProdutos?.Count > 0)
            {
                pedidoMagento.ListaProdutos.ForEach(y =>
                {
                    lstTodosProdutos.ForEach(x =>
                    {
                        if (x.Fabricante == y.Fabricante && x.Produto == y.Produto)
                        {
                            lstCoeficiente.ForEach(z =>
                            {
                                if (y.Fabricante == z.Fabricante)
                                {
                                    //criar a variável de produtos na entrada do método de conversão
                                    listaProdutos.Add(PedidoProdutoMagentoDto.ProdutosDePedidoProdutoMagentoDto(y, x, z.Coeficiente));
                                }
                            });
                        }
                    });

                });
            }

            return listaProdutos;
        }

        /* Criamos essa classe apenas para converter os dados de Endereço cadastral para 
         * Pedido.PedidoDadosCriacao.DadosCliente para montar os dados para inserir um novo Pedido
         */

        public static Cliente.Dados.DadosClienteCadastroDados DadosClienteDeEnderecoCadastralClienteMagentoDto(
            EnderecoCadastralClienteMagentoDto dadosClienteMagento, string loja)
        {
            var ret = new Cliente.Dados.DadosClienteCadastroDados()
            {
                Indicador_Orcamentista = "vem do appsettings",
                Loja = loja,
                Nome = dadosClienteMagento.Endereco_nome,
                Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(dadosClienteMagento.Endereco_cnpj_cpf.Trim()),
                Tipo = dadosClienteMagento.Endereco_tipo_pessoa,
                Sexo = dadosClienteMagento.Endereco_tipo_pessoa == Constantes.ID_PJ ? "" : "XX",
                Rg = "",
                Nascimento = new DateTime(),//verificar o que vai ser inserido aqui
                DddCelular = dadosClienteMagento.Endereco_ddd_cel,
                Celular = dadosClienteMagento.Endereco_tel_cel,
                DddResidencial = dadosClienteMagento.Endereco_ddd_res == null ? "" : dadosClienteMagento.Endereco_ddd_res,
                TelefoneResidencial = dadosClienteMagento.Endereco_tel_res == null ? "" : dadosClienteMagento.Endereco_tel_res,
                DddComercial = dadosClienteMagento.Endereco_ddd_com,
                TelComercial = dadosClienteMagento.Endereco_tel_com,
                Ramal = dadosClienteMagento.Endereco_ramal_com,
                DddComercial2 = dadosClienteMagento.Endereco_ddd_com_2,
                TelComercial2 = dadosClienteMagento.Endereco_tel_com_2,
                Ramal2 = dadosClienteMagento.Endereco_ramal_com_2,
                Ie = "",
                ProdutorRural = dadosClienteMagento.Endereco_tipo_pessoa == Constantes.ID_PJ ?
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL :
                    (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO,
                Contribuinte_Icms_Status = dadosClienteMagento.Endereco_tipo_pessoa == Constantes.ID_PJ ?
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL :
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO,
                Email = dadosClienteMagento.Endereco_email,
                EmailXml = dadosClienteMagento.Endereco_email_xml,
                Vendedor = "vem do appsettings",
                Cep = dadosClienteMagento.Endereco_cep,
                Endereco = dadosClienteMagento.Endereco_logradouro,
                Numero = dadosClienteMagento.Endereco_numero,
                Bairro = dadosClienteMagento.Endereco_bairro,
                Cidade = dadosClienteMagento.Endereco_cidade,
                Uf = dadosClienteMagento.Endereco_uf,
                Complemento = dadosClienteMagento.Endereco_complemento,
                Contato = dadosClienteMagento.Endereco_contato
                //Observacao_Filiacao = dadosClienteMagento.Observacao_Filiacao **Verificar se mandamos esse campo
            };

            return ret;
        }


        private int Duplicado_em_prepedido_remover_daqui_ObterQtdeParcelasFormaPagto(Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagto)
        {
            int qtdeParcelas = 0;

            if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
                qtdeParcelas = 0;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                qtdeParcelas = 1;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                qtdeParcelas = (int)formaPagto.C_pc_qtde;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                qtdeParcelas = (int)formaPagto.C_pc_maquineta_qtde;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                qtdeParcelas = (int)formaPagto.C_pce_prestacao_qtde;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                qtdeParcelas = (int)formaPagto.C_pse_demais_prest_qtde++;

            return qtdeParcelas;
        }

        private string Duplicado_em_prepedido_remover_daqui_ObterSiglaFormaPagto(Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagto)
        {
            string retorno = "";

            if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_A_VISTA)
                retorno = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__A_VISTA;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELA_UNICA)
                retorno = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO)
                retorno = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_CARTAO_MAQUINETA)
                retorno = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_COM_ENTRADA)
                retorno = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__COM_ENTRADA;
            else if (formaPagto.Rb_forma_pagto == InfraBanco.Constantes.Constantes.COD_FORMA_PAGTO_PARCELADO_SEM_ENTRADA)
                retorno = InfraBanco.Constantes.Constantes.COD_CUSTO_FINANC_FORNEC_TIPO_PARCELAMENTO__SEM_ENTRADA;

            return retorno;
        }
    }
}
