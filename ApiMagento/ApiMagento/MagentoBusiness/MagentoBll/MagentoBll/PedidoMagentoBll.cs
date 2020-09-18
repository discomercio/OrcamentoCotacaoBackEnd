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
        private readonly Produto.CoeficienteBll coeficienteBll;
        private readonly ValidacoesPrepedidoBll validacoesPrepedidoBll;
        private readonly PrepedidoBll prepedidoBll;
        private readonly ConfiguracaoApiMagento configuracaoApiMagento;
        private readonly Pedido.PedidoCriacao pedidoCriacao;

        public PedidoMagentoBll(InfraBanco.ContextoBdProvider contextoProvider,
            Cliente.ClienteBll clienteBll, Produto.ProdutoGeralBll produtoGeralBll,
            Produto.CoeficienteBll coeficienteBll,
            Prepedido.ValidacoesPrepedidoBll validacoesPrepedidoBll, PrepedidoBll prepedidoBll,
            ConfiguracaoApiMagento configuracaoApiMagento, Pedido.PedidoCriacao pedidoCriacao)
        {
            this.contextoProvider = contextoProvider;
            this.clienteBll = clienteBll;
            this.produtoGeralBll = produtoGeralBll;
            this.coeficienteBll = coeficienteBll;
            this.validacoesPrepedidoBll = validacoesPrepedidoBll;
            this.prepedidoBll = prepedidoBll;
            this.configuracaoApiMagento = configuracaoApiMagento;
            this.pedidoCriacao = pedidoCriacao;
        }

        public async Task<PedidoResultadoMagentoDto> CadastrarPedidoMagento(PedidoMagentoDto pedidoMagento, string usuario)
        {
            /* Começar a implantar o 
             * ArClube/ApiMagento/ApiMagento/ApiMagento/Controllers/PedidoMagentoController.cs CadastrarPrepedido
             * Converter a estrutura de dados da Api Magento para PedidoDados
             * começar as validações do PedidoCriacao/CadastrarPrepedido.
             * Conforne precisar, mover código da PrepedidoAPi para GLobal/Utils ou GLobal/Prepedido.
             * Isso a gente vai se falando.
             * ========================================================================================
             */
            PedidoResultadoMagentoDto resultado = new PedidoResultadoMagentoDto();
            resultado.IdsPedidosFilhotes = new List<string>();
            resultado.ListaErros = new List<string>();

            var db = contextoProvider.GetContextoLeitura();

            string orcamentista = configuracaoApiMagento.DadosOrcamentista.Orcamentista;
            string vendedor = usuario;
            string loja = configuracaoApiMagento.DadosOrcamentista.Loja;

            InfraBanco.Modelos.TorcamentistaEindicador torcamentista = await prepedidoBll.BuscarTorcamentista(orcamentista);
            if (torcamentista == null)
            {
                resultado.ListaErros.Add("O Orçamentista não existe!");
                return resultado;
            }

            Cliente.Dados.ClienteCadastroDados clienteMagento = await clienteBll.BuscarCliente(pedidoMagento?.Cnpj_Cpf, orcamentista);

            Cliente.Dados.DadosClienteCadastroDados dadosCliente = new Cliente.Dados.DadosClienteCadastroDados();
            if (clienteMagento == null)
            {
                //vamos seguir o fluxo para cadastrar o cliente e depois fazer o cadastro do pedido
                Cliente.Dados.ClienteCadastroDados clienteCadastro = new Cliente.Dados.ClienteCadastroDados();
                clienteCadastro.DadosCliente =
                    DadosClienteDeEnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente, loja, pedidoMagento.Frete, vendedor, orcamentista);
                clienteCadastro.RefBancaria = new List<Cliente.Dados.Referencias.RefBancariaClienteDados>();
                clienteCadastro.RefComercial = new List<Cliente.Dados.Referencias.RefComercialClienteDados>();

                //criei o código para sistema_responsavel_cadastro 
                List<string> lstRet = (await clienteBll.CadastrarCliente(clienteCadastro, orcamentista,
                    (byte)InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__APIMAGENTO)).ToList();

                if(lstRet.Count == 1)
                {
                    //é o número do pedido
                    if(lstRet[0].Length == 12)
                    {

                    }
                    else
                    {
                        //é erro
                        resultado.ListaErros = lstRet;
                    }
                }
                //é erro
                if(lstRet.Count > 1)
                {
                    resultado.ListaErros = lstRet;
                }
            }

            if(resultado.ListaErros.Count == 0)
            {
                /*
             * olhar Marketplace_codigo_origem para saber se é marketplace ou magento
             * se não tiver dados nele veio do magento
             */
                if (!string.IsNullOrEmpty(pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem))
                {
                    List<InfraBanco.Modelos.TcodigoDescricao> listarCodigo = (await UtilsGlobais.Util.ListarCodigoMarketPlace(contextoProvider)).ToList();

                    InfraBanco.Modelos.TcodigoDescricao tcodigo = listarCodigo.Select(x => x)
                        .Where(x => x.Codigo == pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem)
                        .FirstOrDefault();

                    if (tcodigo == null)
                    {
                        resultado.ListaErros.Add("Código Marketplace não encontrado.");
                        return resultado;
                    }
                }

                Pedido.Dados.Criacao.PedidoCriacaoRetornoDados ret =
                    await pedidoCriacao.CadastrarPedido(await CriarPedidoCriacaoDados(pedidoMagento, dadosCliente, orcamentista, loja, vendedor));

                resultado.IdPedidoCadastrado = ret.Id;
                resultado.IdsPedidosFilhotes = ret.ListaIdPedidosFilhotes;
                resultado.ListaErros = ret.ListaErrosValidacao;
            }            

            return resultado;
        }

        private async Task<IEnumerable<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados>> ConverterProdutosMagento(PedidoMagentoDto pedidoMagento,
            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao, string loja)
        {
            List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados> listaProdutos = new List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados>();
            List<string> lstFornec = new List<string>();
            lstFornec = pedidoMagento.ListaProdutos.Select(x => x.Fabricante).Distinct().ToList();

            //preciso da lista de coeficientes de cada fabricante da lista de produtos
            List<Produto.Dados.CoeficienteDados> lstCoeficiente = new List<Produto.Dados.CoeficienteDados>();
            //preciso obter a qtde de parcelas e a sigla de pagto
            var qtdeParcelas = prepedidoBll.ObterQtdeParcelasFormaPagto(formaPagtoCriacao);
            lstCoeficiente = (await validacoesPrepedidoBll.MontarListaCoeficiente(lstFornec, qtdeParcelas,
                prepedidoBll.ObterSiglaFormaPagto(formaPagtoCriacao))).ToList();

            List<Produto.Dados.ProdutoDados> lstTodosProdutos = (await produtoGeralBll.BuscarTodosProdutos(loja)).ToList();

            if (lstTodosProdutos?.Count > 0)
            {
                pedidoMagento.ListaProdutos.ForEach(y =>
                {
                    Produto.Dados.ProdutoDados produto = lstTodosProdutos.Select(x => x)
                    .Where(x => x.Fabricante == y.Fabricante && x.Produto == y.Produto)
                    .FirstOrDefault();

                    Produto.Dados.CoeficienteDados coeficiente = lstCoeficiente.Select(x => x)
                    .Where(x => x.Fabricante == produto.Fabricante && x.QtdeParcelas == qtdeParcelas)
                    .FirstOrDefault();

                    if (y.Fabricante == produto.Fabricante &&
                        y.Fabricante == coeficiente.Fabricante &&
                        y.Produto == produto.Produto && y.Fabricante == coeficiente.Fabricante)
                    {
                        listaProdutos.Add(PedidoProdutoMagentoDto.ProdutosDePedidoProdutoMagentoDto(y, produto, coeficiente.Coeficiente));
                    }

                    //lstTodosProdutos.ForEach(x =>
                    //{
                    //    if (x.Fabricante == y.Fabricante && x.Produto == y.Produto)
                    //    {
                    //        lstCoeficiente.ForEach(z =>
                    //        {
                    //            if (y.Fabricante == z.Fabricante)
                    //            {
                    //                //criar a variável de produtos na entrada do método de conversão
                    //                listaProdutos.Add(PedidoProdutoMagentoDto.ProdutosDePedidoProdutoMagentoDto(y, x, z.Coeficiente));
                    //            }
                    //        });
                    //    }
                    //});

                });
            }

            return await Task.FromResult(listaProdutos);
        }

        /* Criamos essa classe apenas para converter os dados de Endereço cadastral para 
         * Pedido.PedidoDadosCriacao.DadosCliente para montar os dados para inserir um novo Pedido
         */
        public static Cliente.Dados.DadosClienteCadastroDados DadosClienteDeEnderecoCadastralClienteMagentoDto(
            EnderecoCadastralClienteMagentoDto dadosClienteMagento, string loja, decimal? frete,
            string vendedor, string orcamentista)
        {
            var ret = new Cliente.Dados.DadosClienteCadastroDados()
            {
                Indicador_Orcamentista = orcamentista,
                Loja = loja,
                Vendedor = frete > 0 ? vendedor : "",//campo "frete"->se for <> 0, vamos usar o indicador.se for 0, sem indicador
                Nome = dadosClienteMagento.Endereco_nome,
                Cnpj_Cpf = UtilsGlobais.Util.SoDigitosCpf_Cnpj(dadosClienteMagento.Endereco_cnpj_cpf.Trim()),
                Tipo = dadosClienteMagento.Endereco_tipo_pessoa,
                Sexo = "",
                Rg = "",
                Nascimento = null,
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
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO :
                    (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL,
                Email = dadosClienteMagento.Endereco_email,
                EmailXml = dadosClienteMagento.Endereco_email_xml,
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

        private async Task<Pedido.Dados.Criacao.PedidoCriacaoDados> CriarPedidoCriacaoDados(PedidoMagentoDto pedidoMagento,
            Cliente.Dados.DadosClienteCadastroDados dadosCliente, string orcamentista, string loja, string vendedor)
        {
            //o cliente existe então vamos converter os dados do cliente para DadosCliente e EnderecoCadastral
            dadosCliente =
                DadosClienteDeEnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente, loja,
                pedidoMagento.Frete, vendedor, orcamentista);

            Cliente.Dados.EnderecoCadastralClientePrepedidoDados enderecoCadastral =
                EnderecoCadastralClienteMagentoDto.EnderecoCadastralClientePrepedidoDados_De_EnderecoCadastralClienteMagentoDto(pedidoMagento.EnderecoCadastralCliente);

            Cliente.Dados.EnderecoEntregaClienteCadastroDados enderecoEntrega =
                EnderecoEntregaClienteMagentoDto.EnderecoEntregaDeEnderecoEntregaClienteMagentoDto(pedidoMagento.EnderecoEntrega, pedidoMagento.OutroEndereco);

            Prepedido.Dados.DetalhesPrepedido.FormaPagtoCriacaoDados formaPagtoCriacao =
                FormaPagtoCriacaoMagentoDto.FormaPagtoCriacaoDados_De_FormaPagtoCriacaoMagentoDto(pedidoMagento.FormaPagtoCriacao,
                configuracaoApiMagento, pedidoMagento.InfCriacaoPedido.Marketplace_codigo_origem);

            List<Prepedido.Dados.DetalhesPrepedido.PrepedidoProdutoPrepedidoDados> listaProdutos =
                (await ConverterProdutosMagento(pedidoMagento, formaPagtoCriacao, configuracaoApiMagento.DadosOrcamentista.Loja)).ToList();

            //Precisamos buscar os produtos para poder incluir os valores para incluir na classe de produto
            Pedido.Dados.Criacao.PedidoCriacaoDados pedidoDadosCriacao =
                PedidoMagentoDto.PedidoDadosCriacaoDePedidoMagentoDto(
                    dadosCliente, enderecoCadastral, enderecoEntrega, listaProdutos, formaPagtoCriacao);

            return await Task.FromResult(pedidoDadosCriacao);
        }

        public async Task<MarketplaceResultadoDto> ObterCodigoMarketplace()
        {
            MarketplaceResultadoDto resultado = new MarketplaceResultadoDto();
            resultado.ListaMarketplace = new List<MarketplaceMagentoDto>();
            resultado.ListaErros = new List<string>();
            

            List<InfraBanco.Modelos.TcodigoDescricao> listarCodigo = (await UtilsGlobais.Util.ListarCodigoMarketPlace(contextoProvider)).ToList();

            if (listarCodigo == null)
            {
                resultado.ListaErros.Add("Falha ao buscar lista Marketplace.");
                return resultado;
            }

            listarCodigo.ForEach(x =>
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
            });            

            return resultado;
        }

    }
}
