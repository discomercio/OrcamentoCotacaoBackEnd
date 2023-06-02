using AutoMapper;
using Cfg.CfgParametro;
using Cfg.CfgUnidadeNegocio;
using Cfg.CfgUnidadeNegocioParametro;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Loja;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Request.Orcamento;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.Cfg.CfgParametro;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using OrcamentoCotacaoBusiness.Models.Response.GrupoSubgrupoProduto;
using OrcamentoCotacaoBusiness.Models.Response.Orcamento;
using OrcamentoCotacaoBusiness.Models.Response.ProdutoCatalogo;
using Prepedido.Bll;
using Produto;
using Produto.Dto;
using ProdutoCatalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class ProdutoOrcamentoCotacaoBll
    {
        private readonly ILogger<ProdutoOrcamentoCotacaoBll> _logger;
        private readonly ProdutoGeralBll produtoGeralBll;
        private readonly CoeficienteBll coeficienteBll;
        private readonly IMapper _mapper;
        private readonly OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoBll orcamentoCotacaoOpcaoItemUnificadoBll;
        private readonly OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoBll orcamentoCotacaoOpcaoItemAtomicoBll;
        private readonly OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll orcamentoCotacaoOpcaoItemAtomicoCustoFinBll;
        private readonly LojaOrcamentoCotacaoBll _lojaOrcamentoCotacaoBll;
        private readonly LojaBll _lojaBll;
        private readonly ProdutoCatalogoBll _produtoCatalogoBll;
        private readonly CfgParametroBll cfgParametroBll;
        private readonly CfgUnidadeNegocioParametroBll cfgUnidadeNegocioParametroBll;
        private readonly CfgUnidadeNegocioBll cfgUnidadeNegocioBll;

        public ProdutoOrcamentoCotacaoBll(
            ILogger<ProdutoOrcamentoCotacaoBll> logger,
            Produto.ProdutoGeralBll produtoGeralBll, CoeficienteBll coeficienteBll, IMapper mapper,
            OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoBll orcamentoCotacaoOpcaoItemUnificadoBll,
            OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoBll orcamentoCotacaoOpcaoItemAtomicoBll,
            OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll orcamentoCotacaoOpcaoItemAtomicoCustoFinBll,
            LojaOrcamentoCotacaoBll _lojaOrcamentoCotacaoBll,
            LojaBll _lojaBll,
            ProdutoCatalogoBll _produtoCatalogoBll,
            CfgParametroBll cfgParametroBll,
            CfgUnidadeNegocioParametroBll cfgUnidadeNegocioParametroBll,
            CfgUnidadeNegocioBll cfgUnidadeNegocioBll)
        {
            _logger = logger;
            this.produtoGeralBll = produtoGeralBll;
            this.coeficienteBll = coeficienteBll;
            _mapper = mapper;
            this.orcamentoCotacaoOpcaoItemUnificadoBll = orcamentoCotacaoOpcaoItemUnificadoBll;
            this.orcamentoCotacaoOpcaoItemAtomicoBll = orcamentoCotacaoOpcaoItemAtomicoBll;
            this.orcamentoCotacaoOpcaoItemAtomicoCustoFinBll = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll;
            this._lojaOrcamentoCotacaoBll = _lojaOrcamentoCotacaoBll;
            this._lojaBll = _lojaBll;
            this._produtoCatalogoBll = _produtoCatalogoBll;
            this.cfgParametroBll = cfgParametroBll;
            this.cfgUnidadeNegocioParametroBll = cfgUnidadeNegocioParametroBll;
            this.cfgUnidadeNegocioBll = cfgUnidadeNegocioBll;
        }

        public async Task<ProdutoResponseViewModel> ListaProdutosCombo(ProdutosRequestViewModel produtos)
        {
            Constantes.ContribuinteICMS contribuinteICMSStatus = Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
            Constantes.ProdutorRural produtorRuralStatus = Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;

            var aux = await produtoGeralBll.ListaProdutosComboDados(produtos.Loja, produtos.UF, produtos.TipoCliente,
                contribuinteICMSStatus, produtorRuralStatus);
            return await CalcularCoeficiente(aux, produtos.TipoParcela, produtos.QtdeParcelas, produtos.DataRefCoeficiente);
        }

        private async Task<ProdutoResponseViewModel> CalcularCoeficiente(Produto.Dados.ProdutoComboDados produtoComboDados,
            string tipoParcela, short qtdeParcelas, DateTime? dataRefCoeficiente)
        {
            try
            {
                if (produtoComboDados == null) return null;

                ProdutoResponseViewModel produtoResponseViewModel = new ProdutoResponseViewModel();
                produtoResponseViewModel.ProdutosCompostos = new List<ProdutoCompostoResponseViewModel>();
                produtoResponseViewModel.ProdutosSimples = new List<ProdutoSimplesResponseViewModel>();


                var lstFabricate = produtoComboDados.ProdutoDados.Select(x => x.Fabricante).Distinct().ToList();
                var dicCoeficiente = await coeficienteBll.BuscarListaCoeficientesFabricantesHistoricoDistinct(new CoeficienteRequestViewModel()
                {
                    LstFabricantes = lstFabricate,
                    TipoParcela = tipoParcela,
                    QtdeParcelas = qtdeParcelas,
                    DataRefCoeficiente = dataRefCoeficiente.GetValueOrDefault(new DateTime())
                });

                foreach (var produto in produtoComboDados.ProdutoDados)
                {

                    var responseItem = ProdutoSimplesResponseViewModel
                        .ConverterProdutoDados(produto, null, GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), produto.Fabricante));

                    // se o produto for um filho não insere aqui
                    var pai = produtoComboDados.ProdutoCompostoDados.Where(x => x.Filhos.Where(f => f.Produto == produto.Produto).Any()).FirstOrDefault();
                    if (pai != null)
                    {
                        responseItem.UnitarioVendavel = false;
                    }
                    produtoResponseViewModel.ProdutosSimples.Add(responseItem);

                }

                foreach (Produto.Dados.ProdutoCompostoDados composto in produtoComboDados.ProdutoCompostoDados)
                {
                    var produtoCompostoResponse = new ProdutoCompostoResponseViewModel();
                    produtoCompostoResponse.Filhos = new List<ProdutoCompostoFilhosResponseViewModel>();
                    decimal? somaFilhotes = 0;
                    decimal? somaFilhotesBase = 0;

                    var produtoCompostoResponseApoio = new ProdutoCompostoResponseViewModel();
                    produtoCompostoResponseApoio.Filhos = new List<ProdutoCompostoFilhosResponseViewModel>();

                    var filhotes = (from c in produtoComboDados.ProdutoDados
                                    join d in composto.Filhos on new { a = c.Fabricante, b = c.Produto } equals new { a = d.Fabricante, b = d.Produto }
                                    select c).ToList();

                    if (composto.Filhos.Count != filhotes.Count)
                    {
                        var prodARemover = produtoResponseViewModel.ProdutosSimples.Where(x => x.Fabricante == composto.PaiFabricante && x.Produto == composto.PaiProduto).FirstOrDefault();
                        if (prodARemover != null) produtoResponseViewModel.ProdutosSimples.Remove(prodARemover);
                        continue;
                    }

                    var coeficiente2 = GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), composto.PaiFabricante).Coeficiente;
                    foreach (var filho in filhotes)
                    {
                        var compostoFilho = composto.Filhos.Where(x => x.Produto == filho.Produto).FirstOrDefault();
                        produtoCompostoResponseApoio.Filhos.Add(ProdutoCompostoFilhosResponseViewModel.ConverterProdutoFilhoDados(filho, compostoFilho.Qtde, GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), composto.PaiFabricante)));

                        //verificar se o verdinho calcula coeficiente e depois a qtde do filho
                        somaFilhotesBase += Math.Round((decimal)filho.Preco_lista * compostoFilho.Qtde, 2);
                        var somafilhotescoeficiente = Math.Round((decimal)filho.Preco_lista * (decimal)coeficiente2, 2);
                        var somafilhotescoefQtdefilhote = Math.Round(somafilhotescoeficiente * compostoFilho.Qtde, 2);
                        somaFilhotes += somafilhotescoefQtdefilhote;
                    }

                    var pai = produtoComboDados.ProdutoDados.Where(x => x.Fabricante == composto.PaiFabricante && x.Produto == composto.PaiProduto).FirstOrDefault();

                    var novoItem = new ProdutoSimplesResponseViewModel();
                    novoItem.Fabricante = composto.PaiFabricante;
                    novoItem.FabricanteNome = composto.PaiFabricanteNome;
                    novoItem.Produto = composto.PaiProduto;
                    novoItem.Qtde = filhotes.Count;
                    novoItem.DescricaoHtml = pai != null ? pai.Descricao_html : composto.PaiDescricao;
                    novoItem.PrecoLista = (decimal)somaFilhotes;
                    novoItem.PrecoListaBase = (decimal)somaFilhotesBase;
                    novoItem.QtdeMaxVenda = filhotes.Min(x => x.Qtde_Max_Venda);
                    novoItem.DescMax = filhotes.Min(x => x.Desc_Max);
                    novoItem.Estoque = filhotes.Min(x => x.Estoque);
                    novoItem.Alertas = filhotes.Min(x => x.Alertas);

                    if (pai != null) produtoResponseViewModel.ProdutosSimples.RemoveAll(x => x.Produto == pai.Produto);

                    produtoResponseViewModel.ProdutosSimples.Add(novoItem);

                    produtoCompostoResponse = ProdutoCompostoResponseViewModel.ConverterProdutoCompostoDados(composto);

                    var coeficiente = GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), composto.PaiFabricante).Coeficiente;
                    produtoCompostoResponse.PaiPrecoTotalBase = somaFilhotesBase;
                    produtoCompostoResponse.PaiPrecoTotal = somaFilhotes;
                    produtoCompostoResponse.Filhos = produtoCompostoResponseApoio.Filhos;
                    produtoResponseViewModel.ProdutosCompostos.Add(produtoCompostoResponse);
                }

                produtoResponseViewModel.ProdutosSimples = produtoResponseViewModel.ProdutosSimples.OrderBy(x => x.Fabricante).ToList();

                return produtoResponseViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private CoeficienteResponseViewModel GetCoeficienteOuNull(IDictionary<string, CoeficienteResponseViewModel> dicCoeficiente, string fabricante)
        {
            if (dicCoeficiente.ContainsKey(fabricante))
            {
                return dicCoeficiente[fabricante];
            }

            return null;
        }

        public async Task<List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>> BuscarProdutoCatalogoParaVisualizacao(int idProduto,
            bool? propriedadeOculta, bool? propriedadeOcultaItem)
        {
            var produtosPropTexto = await produtoGeralBll.ObterProdutoPropriedadesAtivosTexto(idProduto, propriedadeOculta, propriedadeOcultaItem);
            var produtosPropListas = await produtoGeralBll.ObterProdutoPropriedadesAtivosLista(idProduto, propriedadeOculta, propriedadeOcultaItem);

            _logger.LogInformation("Quantidade de propriedades texto: {0}", produtosPropTexto.Count);
            _logger.LogInformation("Quantidade de propriedades lista: {0}", produtosPropListas.Count);

            var retorno = new List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>();

            if (produtosPropListas != null && produtosPropTexto != null)
            {
                produtosPropListas.AddRange(produtosPropTexto);
                retorno = _mapper.Map<List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>>(produtosPropListas);
            }
            return retorno.OrderBy(x => int.Parse(x.Ordem)).ToList(); ;
        }

        public async Task<List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>> ListarProdutoCatalogoParaVisualizacao(bool? propriedadeOculta, bool? propriedadeOcultaItem)
        {
            var produtosPropTexto = await produtoGeralBll.ListarProdutoPropriedadesAtivosTexto(propriedadeOculta, propriedadeOcultaItem);
            var produtosPropListas = await produtoGeralBll.ListarProdutoPropriedadesAtivosLista(propriedadeOculta, propriedadeOcultaItem);

            List<ProdutoCatalogoItemProdutosAtivosResponseViewModel> retorno = new List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>();
            if (produtosPropListas != null && produtosPropTexto != null)
            {
                produtosPropListas.AddRange(produtosPropTexto);
                retorno = _mapper.Map<List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>>(produtosPropListas);
            }
            return retorno;
        }

        public async Task<List<ProdutoAtivoDto>> ObterProdutosAtivos()
        {
            return await produtoGeralBll.ObterProdutosAtivos();
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos()
        {
            var lstProdutoPropriedades = await produtoGeralBll.ObterListaPropriedadesProdutos();

            return lstProdutoPropriedades.OrderBy(x => x.ordem).ToList();
        }

        public async Task<List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>> ObterListaProdutosPropriedadesProdutosAtivos()
        {
            var catalagoItens = await produtoGeralBll.ObterListaProdutosPropriedadesProdutosAtivos();

            if (catalagoItens == null)
            {
                throw new ArgumentException("Falha ao buscar produtos!");
            }
            var response = _mapper.Map<List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>>(catalagoItens);
            return response;
        }

        public async Task<List<Produto.Dados.CatalogoPropriedadesOpcaoDados>> ObterPropriedadesEOpcoesProdutosAtivos()
        {
            var catalogoPropriedadesOpcoes = await produtoGeralBll.ObterPropriedadesEOpcoesProdutosAtivos();

            if (catalogoPropriedadesOpcoes == null) throw new ArgumentException("Falha ao buscar propriedades!");

            return catalogoPropriedadesOpcoes;
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos(int id)
        {
            var lstProdutoPropriedades = await produtoGeralBll.ObterListaPropriedadesProdutos(id);

            return lstProdutoPropriedades;
        }

        public async Task<List<Produto.Dados.FabricanteDados>> ObterListaFabricante()
        {
            var lstFabricantes = await produtoGeralBll.ObterListaFabricante();
            lstFabricantes = lstFabricantes.OrderBy(x => x.Nome).ToList();

            return lstFabricantes;
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeOpcoesDados>> ObterListaPropriedadesOpcoes()
        {
            var lstProdutoPropriedades = await produtoGeralBll.ObterListaPropriedadesOpcoes();

            return lstProdutoPropriedades.OrderBy(x => x.ordem).ToList();
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemDados>> ObterListaPropriedadesProdutosById(int idProdutoCatalogo)
        {
            var lstProdutoPropriedades = await produtoGeralBll.ObterListaPropriedadesProdutosById(idProdutoCatalogo);

            return lstProdutoPropriedades;
        }

        public async Task<List<Produto.Dados.ProdutoCatalogoItemDados>> ObterListaPropriedadesOpcoesProdutosById(int IdProdutoCatalogoPropriedade)
        {
            var lstProdutoPropriedades = await produtoGeralBll.ObterListaPropriedadesOpcoesProdutosById(IdProdutoCatalogoPropriedade);

            return lstProdutoPropriedades;
        }

        public CadastroOrcamentoOpcaoProdutoUnificadoResponse CadastrarOrcamentoCotacaoOpcaoProdutosUnificadosEAtomicosComTransacao(CadastroOrcamentoOpcaoRequest opcao,
            int idOrcamentoCotacaoOpcao, string loja, ContextoBdGravacao contextoBdGravacao, Guid correlationId)
        {
            CadastroOrcamentoOpcaoProdutoUnificadoResponse response = new CadastroOrcamentoOpcaoProdutoUnificadoResponse();
            response.TorcamentoCotacaoItemUnificados = new List<TorcamentoCotacaoItemUnificado>();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();
            int sequencia = 0;

            foreach (var produto in opcao.ListaProdutos)
            {
                TorcamentoCotacaoItemUnificado torcamentoCotacaoItemUnificado = new TorcamentoCotacaoItemUnificado();
                Tproduto tProduto = new Tproduto();
                //buscar produto composto para cadastrar os atômicos
                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Buscando produto composto. Fabricante => [{produto.Fabricante}], Produto => [{produto.Produto}], Loja => [{loja}].");
                var tEcProdutoComposto = produtoGeralBll.BuscarProdutoCompostoPorFabricanteECodigoComTransacao(produto.Fabricante,
                    produto.Produto, loja, contextoBdGravacao).Result;

                sequencia++;
                //se não é composto
                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Iniciando montagem de produto unificado para cadastro.");
                if (tEcProdutoComposto == null)
                {
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Buscando produto simples. Fabricante => [{produto.Fabricante}], Produto => [{produto.Produto}], Loja => [{loja}].");
                    tProduto = produtoGeralBll.BuscarProdutoSimplesPorFabricanteECodigoComTransacao(produto.Fabricante,
                    produto.Produto, loja, contextoBdGravacao).Result;

                    if (tProduto == null)
                    {
                        response.Mensagem = "Ops! Não achamos o produto!";
                        return response;
                    }
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Retorno da busca de produto simples. Response => [{JsonSerializer.Serialize(tProduto)}].");

                    torcamentoCotacaoItemUnificado = new TorcamentoCotacaoItemUnificado(idOrcamentoCotacaoOpcao,
                    tProduto.Fabricante, tProduto.Produto, produto.Qtde, tProduto.Descricao, tProduto.Descricao_Html, sequencia);
                }
                else
                {
                    torcamentoCotacaoItemUnificado = new TorcamentoCotacaoItemUnificado(idOrcamentoCotacaoOpcao,
                    tEcProdutoComposto.Fabricante_Composto, tEcProdutoComposto.Produto_Composto, produto.Qtde, tEcProdutoComposto.Descricao, tEcProdutoComposto.Descricao, sequencia);
                }



                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando produto unificado. Request => [{JsonSerializer.Serialize(torcamentoCotacaoItemUnificado)}].");
                var produtoUnificado = orcamentoCotacaoOpcaoItemUnificadoBll.InserirComTransacao(torcamentoCotacaoItemUnificado, contextoBdGravacao);

                if (produtoUnificado.Id == 0)
                {
                    response.Mensagem = "Ops! Não gerou o Id de produto unificado!";
                    return response;
                }

                response.TorcamentoCotacaoItemUnificados.Add(produtoUnificado);


                var atomicosResponse = CadastrarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(produtoUnificado.Id,
                    tProduto, tEcProdutoComposto, produto.Qtde, loja, contextoBdGravacao, correlationId);

                if (atomicosResponse.Sucesso && atomicosResponse.TorcamentoCotacaoOpcaoItemAtomicos.Count <= 0)
                {
                    response.Mensagem = "Ops! Não gerou o Id de produto atomicos!";
                    return response;
                }
            }

            response.Sucesso = true;
            return response;
        }

        private CadastroOrcamentoOpcaoProdutoAtomicoResponse CadastrarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(int idItemUnificado,
            Tproduto tProduto, TecProdutoComposto tEcProdutoComposto, int qtde, string loja, ContextoBdGravacao contextoBdGravacao,
            Guid correlationId)
        {
            CadastroOrcamentoOpcaoProdutoAtomicoResponse response = new CadastroOrcamentoOpcaoProdutoAtomicoResponse();
            response.TorcamentoCotacaoOpcaoItemAtomicos = new List<TorcamentoCotacaoOpcaoItemAtomico>();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Iniciando cadastro de produtos atômicos.");
            if (tEcProdutoComposto != null)
            {
                foreach (var item in tEcProdutoComposto.TecProdutoCompostoItems)
                {
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Buscando produto simples parte de composto. Fabricante => [{item.Fabricante_item}], Produto => [{item.Produto_item}], Loja => [{loja}].");
                    var tProdutoAtomico = produtoGeralBll.BuscarProdutoSimplesPorFabricanteECodigoComTransacao(item.Fabricante_item,
                        item.Produto_item, loja, contextoBdGravacao).Result;

                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Retorno da busca simples parte de composto. Response => [{JsonSerializer.Serialize(tProdutoAtomico)}].");

                    TorcamentoCotacaoOpcaoItemAtomico itemAtomico = new TorcamentoCotacaoOpcaoItemAtomico(idItemUnificado,
                        tProdutoAtomico.Fabricante, tProdutoAtomico.Produto, item.Qtde,
                        tProdutoAtomico.Descricao, tProdutoAtomico.Descricao_Html);

                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando produto atômico. Request => [{JsonSerializer.Serialize(itemAtomico)}].");
                    response.TorcamentoCotacaoOpcaoItemAtomicos.Add(orcamentoCotacaoOpcaoItemAtomicoBll.InserirComTransacao(itemAtomico, contextoBdGravacao));
                }

                response.Sucesso = true;
                return response;
            }

            //se produto é simples, salvamos sempre como 1 => quem guarda a quantidade selecionada é o item unificado
            var torcamentoCotacaoOpcaoItemAtomico = new TorcamentoCotacaoOpcaoItemAtomico(idItemUnificado, tProduto.Fabricante,
                tProduto.Produto, (short)1, tProduto.Descricao, tProduto.Descricao_Html);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando produto atômico. Request => [{JsonSerializer.Serialize(torcamentoCotacaoOpcaoItemAtomico)}].");
            response.TorcamentoCotacaoOpcaoItemAtomicos.Add(orcamentoCotacaoOpcaoItemAtomicoBll.InserirComTransacao(torcamentoCotacaoOpcaoItemAtomico,
               contextoBdGravacao));

            response.Sucesso = true;
            return response;
        }

        public CadastroOrcamentoOpcaoProdutoAtomicoCustoFinResponse CadastrarProdutoAtomicoCustoFinComTransacao(List<TorcamentoCotacaoOpcaoPagto> tOrcamentoCotacaoOpcaoPagtos,
            List<TorcamentoCotacaoItemUnificado> tOrcamentoCotacaoItemUnificados, List<CadastroOrcamentoOpcaoProdutoRequest> produtosOpcao, string loja,
            ContextoBdGravacao contextoBdGravacao, Guid correlationId)
        {
            CadastroOrcamentoOpcaoProdutoAtomicoCustoFinResponse response = new CadastroOrcamentoOpcaoProdutoAtomicoCustoFinResponse();
            response.TorcamentoCotacaoOpcaoItemAtomicoCustoFins = new List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin>();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();

            int index = 0;
            foreach (var pagto in tOrcamentoCotacaoOpcaoPagtos)
            {
                foreach (var unificado in tOrcamentoCotacaoItemUnificados)
                {

                    var produtosEspecificos = unificado.TorcamentoCotacaoOpcaoItemAtomicos.Select(x => x.Produto).ToList();
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Buscando lista de produtos específicos. Loja => [{loja}]");
                    var pDados = produtoGeralBll.BuscarProdutosEspecificos(loja, produtosEspecificos).Result;
                    if (pDados == null)
                    {
                        response.Mensagem = $"Ops! Falha ao buscar lista de produtos específicos!";
                        return response;
                    }
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Retorno da lista de produtos específicos. Response => [{JsonSerializer.Serialize(pDados)}]");

                    var itensAtomicos = unificado.TorcamentoCotacaoOpcaoItemAtomicos;
                    var itemOpcao = produtosOpcao.Where(x => x.Produto == unificado.Produto).FirstOrDefault();
                    foreach (var atomico in itensAtomicos)
                    {
                        var item = pDados.Where(x => x.Produto == atomico.Produto).FirstOrDefault();
                        TorcamentoCotacaoOpcaoItemAtomicoCustoFin atomicoCustoFin = new TorcamentoCotacaoOpcaoItemAtomicoCustoFin()
                        {
                            IdItemAtomico = atomico.Id,
                            IdOpcaoPagto = pagto.Id,
                            DescDado = itemOpcao.DescDado,
                            PrecoLista = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? Math.Round((decimal)item.Preco_lista, 2) : Math.Round((decimal)item.Preco_lista * (decimal)itemOpcao.CustoFinancFornecCoeficiente, 2),
                            PrecoVenda = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? Math.Round((decimal)item.Preco_lista * (decimal)(1 - itemOpcao.DescDado / 100), 2) : Math.Round(Math.Round((decimal)item.Preco_lista * (decimal)itemOpcao.CustoFinancFornecCoeficiente, 2) * (decimal)(1 - itemOpcao.DescDado / 100), 2),
                            PrecoNF = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? Math.Round((decimal)item.Preco_lista * (decimal)(1 - itemOpcao.DescDado / 100), 2) : Math.Round(Math.Round((decimal)item.Preco_lista * (decimal)itemOpcao.CustoFinancFornecCoeficiente, 2) * (decimal)(1 - itemOpcao.DescDado / 100), 2),
                            CustoFinancFornecCoeficiente = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? 0 : itemOpcao.CustoFinancFornecCoeficiente,
                            CustoFinancFornecPrecoListaBase = Math.Round((decimal)item.Preco_lista, 2)
                        };

                        _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando produto atômico custo fin. Request => [{JsonSerializer.Serialize(atomicoCustoFin)}]");
                        var retorno = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.InserirComTransacao(atomicoCustoFin, contextoBdGravacao);
                        if (retorno.Id == 0)
                        {
                            response.Mensagem = "Ops! Falha ao gravar o produto!";
                            return response;
                        }
                        response.TorcamentoCotacaoOpcaoItemAtomicoCustoFins.Add(retorno);
                    }
                }

            }

            response.Sucesso = true;
            return response;
        }

        public AtualizarOrcamentoOpcaoProdutoUnificadoResponse AtualizarOrcamentoCotacaoOpcaoProdutosUnificadosComTransacao(List<AtualizarOrcamentoOpcaoProdutoRequest> lstProdutos, int idOpcao,
            ContextoBdGravacao contextoBdGravacao, Guid correlationId, List<TorcamentoCotacaoItemUnificado> TorcamentoCotacaoItemUnificadosAntigos, int opcaoSequencia)
        {
            var response = new AtualizarOrcamentoOpcaoProdutoUnificadoResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();
            response.TorcamentoCotacaoItemUnificados = new List<TorcamentoCotacaoItemUnificado>();

            _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Buscando lista de produtos unificados.");
            var produtosUnificados = orcamentoCotacaoOpcaoItemUnificadoBll.PorFiltroComTransacao(new TorcamentoCotacaoOpcaoItemUnificadoFiltro() { IdOpcao = idOpcao }, contextoBdGravacao);
            if (produtosUnificados == null)
            {
                response.Mensagem = "Falha ao buscar itens para atualização!";
                return response;
            }

            string logRetorno = "";
            foreach (var item in lstProdutos)
            {
                var produto = produtosUnificados.Where(x => x.Id == item.IdItemUnificado).FirstOrDefault();

                if (produto == null)
                {
                    response.Mensagem = "Falha ao buscar item para atualização!";
                    return response;
                }

                produto.Qtde = item.Qtde;
                JsonSerializerOptions options = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = false };
                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Atualizando produto unificado Id[{produto.Id}]. Resquest => {JsonSerializer.Serialize(produto, options)}");
                var retorno = orcamentoCotacaoOpcaoItemUnificadoBll.AtualizarComTransacao(produto, contextoBdGravacao);

                response.TorcamentoCotacaoItemUnificados.Add(retorno);

                var produtoUnificadoAntigo = TorcamentoCotacaoItemUnificadosAntigos.Where(x => x.Id == produto.Id).FirstOrDefault();


                string logApoio = UtilsGlobais.Util.MontalogComparacao(produto, produtoUnificadoAntigo, "", "");
                if (!string.IsNullOrEmpty(logApoio))
                {
                    if (!string.IsNullOrEmpty(logRetorno)) logRetorno += "\r      ";
                    logRetorno += $"Id={produto.Id}; IdOrcamentoCotacaoOpcao={produto.IdOrcamentoCotacaoOpcao}; {logApoio}";
                }
            }

            if (!string.IsNullOrEmpty(logRetorno)) response.LogOperacao = $"\r   Lista de produtos unificados opção {opcaoSequencia}: {logRetorno}";

            response.Sucesso = true;
            return response;
        }

        public AtualizarOrcamentoOpcaoProdutoAtomicoResponse AtualizarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(List<AtualizarOrcamentoOpcaoProdutoRequest> lstProdutos, int idOpcao,
            List<TorcamentoCotacaoItemUnificado> produtosUnificados, ContextoBdGravacao contextoBdGravacao, Guid correlationId)
        {
            var response = new AtualizarOrcamentoOpcaoProdutoAtomicoResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();
            response.TorcamentoCotacaoOpcaoItemAtomicos = new List<TorcamentoCotacaoOpcaoItemAtomico>();

            foreach (var item in lstProdutos)
            {
                var unificado = produtosUnificados.Where(x => x.Fabricante == item.Fabricante && x.Produto == item.Produto).FirstOrDefault();
                if (unificado == null)
                {
                    response.Mensagem = "Falha ao buscar item para atualização!";
                    return response;
                }

                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Buscando lista de produtos atômicos.");
                var TorcamentoCotacaoOpcaoItemAtomicos = orcamentoCotacaoOpcaoItemAtomicoBll.PorFiltroComTransacao(new TorcamentoCotacaoOpcaoItemAtomicoFiltro() { IdItemUnificado = item.IdItemUnificado }, contextoBdGravacao);
                if (TorcamentoCotacaoOpcaoItemAtomicos == null)
                {
                    response.Mensagem = "Falha ao buscar itens para atualização!";
                    return response;
                }

                JsonSerializerOptions options = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };
                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Retorno da busca de lista de produtos atômicos. Response => [{JsonSerializer.Serialize(TorcamentoCotacaoOpcaoItemAtomicos, options)}]");

                foreach (var atomico in TorcamentoCotacaoOpcaoItemAtomicos)
                {
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Atualizando produto atômico. Resquest => {JsonSerializer.Serialize(atomico, options)}");
                    var retorno = orcamentoCotacaoOpcaoItemAtomicoBll.AtualizarComTransacao(atomico, contextoBdGravacao);
                    response.TorcamentoCotacaoOpcaoItemAtomicos.Add(retorno);
                }
            }
            response.Sucesso = true;
            return response;
        }

        public AtualizarOrcamentoOpcaoProdutoAtomicoCustoFinResponse AtualizarProdutoAtomicoCustoFinComTransacao(AtualizarOrcamentoOpcaoRequest opcao, List<TorcamentoCotacaoItemUnificado> produtosUnificados,
            List<TorcamentoCotacaoOpcaoPagto> opcaoPagtos, ContextoBdGravacao contextoBdGravacao,
            UsuarioLogin usuarioLogado, OrcamentoResponse orcamento, Guid correlationId,
            List<TorcamentoCotacaoOpcaoPagto> formaPagtoAntiga)
        {
            var response = new AtualizarOrcamentoOpcaoProdutoAtomicoCustoFinResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();
            response.TorcamentoCotacaoOpcaoItemAtomicoCustoFins = new List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin>();

            string logEdicaoAprazo = "";
            string logCriacaoAPrazo = "";
            string logEdicaoAVista = "";
            string logCriacaoAVista = "";

            foreach (var item in opcao.ListaProdutos)
            {
                var unificado = produtosUnificados.Where(x => x.Fabricante == item.Fabricante && x.Produto == item.Produto).FirstOrDefault();
                if (unificado.TorcamentoCotacaoOpcaoItemAtomicos.Count <= 0)
                {
                    response.Mensagem = "Falha ao buscar item atômico para atualização!";
                    return response;
                }

                var produtosEspecificos = unificado.TorcamentoCotacaoOpcaoItemAtomicos.Select(x => x.Produto).ToList();

                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Buscando lista de produtos específicos. Loja => [{opcao.Loja}]");
                var pDados = produtoGeralBll.BuscarProdutosEspecificos(opcao.Loja, produtosEspecificos).Result;
                if (pDados == null)
                {
                    response.Mensagem = "Ops! Falha ao buscar lista de produtos específicos!";
                    return response;
                }
                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Retorno da lista de produtos específicos. Response => [{JsonSerializer.Serialize(pDados)}]");

                foreach (var atomico in unificado.TorcamentoCotacaoOpcaoItemAtomicos)
                {
                    _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Buscando lista de produtos atômicos custo fin.");
                    var torcamentoCotacaoOpcaoItemAtomicosFin = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll
                        .PorFiltroComTransacao(new TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro() { IdItemAtomico = atomico.Id }, contextoBdGravacao);

                    foreach (var pagto in opcao.FormaPagto)
                    {
                        var pagtoAntigo = formaPagtoAntiga.Where(x => x.Id == pagto.Id).FirstOrDefault();
                        var atomicoCustoFin = torcamentoCotacaoOpcaoItemAtomicosFin.Where(x => x.TorcamentoCotacaoOpcaoPagto.Tipo_parcelamento == pagto.Tipo_parcelamento).FirstOrDefault();

                        _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Validando desconto e comissão por alçada.");
                        var validacaoResponse = ValidarDescontoComissaoAlcada(item, usuarioLogado, orcamento, opcao);
                        if (!string.IsNullOrEmpty(validacaoResponse))
                        {
                            response.Mensagem = validacaoResponse;
                            return response;
                        }

                        var p = pDados.Where(x => x.Produto == atomico.Produto).FirstOrDefault();

                        _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Verificando uso por alçada.");
                        var responseUsoAlcada = VerificarUsoDeAlcada(usuarioLogado, orcamento, item, opcao.PercRT);
                        List<int> usuarioAlcadas;
                        int idAlcada = 0;
                        if (!string.IsNullOrEmpty(responseUsoAlcada) && responseUsoAlcada == "Sim")
                        {
                            usuarioAlcadas = new List<int>();
                            if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_1))
                                usuarioAlcadas.Add(int.Parse(Constantes.COMISSAO_DESCONTO_ALCADA_1));
                            if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_2))
                                usuarioAlcadas.Add(int.Parse(Constantes.COMISSAO_DESCONTO_ALCADA_2));
                            if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_3))
                                usuarioAlcadas.Add(int.Parse(Constantes.COMISSAO_DESCONTO_ALCADA_3));

                            idAlcada = usuarioAlcadas.Max(x => x);
                            _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Usuário com permissão de alçada. Id => [{idAlcada}]");
                        }

                        if (atomicoCustoFin == null)
                        {
                            var opcaoPagtoAvista = opcaoPagtos.Where(x => x.Tipo_parcelamento == pagto.Tipo_parcelamento).FirstOrDefault();
                            atomicoCustoFin = new TorcamentoCotacaoOpcaoItemAtomicoCustoFin();
                            atomicoCustoFin.IdItemAtomico = atomico.Id;
                            atomicoCustoFin.IdOpcaoPagto = opcaoPagtoAvista.Id;
                            atomicoCustoFin.DescDado = item.DescDado;
                            atomicoCustoFin.PrecoLista = Math.Round((decimal)p.Preco_lista, 2);
                            atomicoCustoFin.PrecoVenda = Math.Round((decimal)p.Preco_lista * (decimal)(1 - item.DescDado / 100), 2);
                            atomicoCustoFin.PrecoNF = Math.Round((decimal)p.Preco_lista * (decimal)(1 - item.DescDado / 100), 2);
                            atomicoCustoFin.CustoFinancFornecCoeficiente = 0;
                            atomicoCustoFin.CustoFinancFornecPrecoListaBase = Math.Round((decimal)p.Preco_lista, 2);
                            atomicoCustoFin.StatusDescontoSuperior = idAlcada > 0 ? true : false;
                            atomicoCustoFin.DataHoraDescontoSuperior = idAlcada > 0 ? (DateTime?)DateTime.Now : null;
                            atomicoCustoFin.IdUsuarioDescontoSuperior = idAlcada > 0 ? (int?)usuarioLogado.Id : null;
                            atomicoCustoFin.IdOperacaoAlcadaDescontoSuperior = idAlcada > 0 ? (int?)idAlcada : null;

                            _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando produto atômico custo fin. Request => [{JsonSerializer.Serialize(atomicoCustoFin)}]");

                            var responseAtomicoCustoFin = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.InserirComTransacao(atomicoCustoFin, contextoBdGravacao);
                            response.TorcamentoCotacaoOpcaoItemAtomicoCustoFins.Add(responseAtomicoCustoFin);

                            string camposAOmitir = "|Id|IdOpcaoPagto|DescDado|CustoFinancFornecCoeficiente|CustoFinancFornecPrecoListaBase|StatusDescontoSuperior|IdUsuarioDescontoSuperior|DataHoraDescontoSuperior|IdOperacaoAlcadaDescontoSuperior|";
                            string logApoio = UtilsGlobais.Util.MontaLog(responseAtomicoCustoFin, "", camposAOmitir);

                            if (pagto.Tipo_parcelamento == int.Parse(Constantes.COD_FORMA_PAGTO_A_VISTA))
                            {
                                if (!string.IsNullOrEmpty(logCriacaoAVista)) logCriacaoAVista += "\r      ";
                                if (!string.IsNullOrEmpty(logApoio))
                                    logCriacaoAVista += $"{logApoio}";
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(logCriacaoAPrazo)) logCriacaoAPrazo += "\r      ";
                                if (!string.IsNullOrEmpty(logApoio))
                                    logCriacaoAPrazo += $"{logApoio}";
                            }
                        }
                        else
                        {
                            atomicoCustoFin.Id = atomicoCustoFin.Id;
                            atomicoCustoFin.IdItemAtomico = atomico.Id;
                            atomicoCustoFin.IdOpcaoPagto = pagto.Id;
                            atomicoCustoFin.DescDado = item.DescDado;
                            atomicoCustoFin.PrecoLista = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? Math.Round((decimal)p.Preco_lista, 2) : Math.Round((decimal)p.Preco_lista * (decimal)item.CustoFinancFornecCoeficiente, 2);
                            atomicoCustoFin.PrecoVenda = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? Math.Round((decimal)p.Preco_lista * (decimal)(1 - item.DescDado / 100), 2) : Math.Round((decimal)p.Preco_lista * (decimal)item.CustoFinancFornecCoeficiente * (decimal)(1 - item.DescDado / 100), 2);
                            atomicoCustoFin.PrecoNF = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? Math.Round((decimal)p.Preco_lista * (decimal)(1 - item.DescDado / 100), 2) : Math.Round((decimal)p.Preco_lista * (decimal)item.CustoFinancFornecCoeficiente * (decimal)(1 - item.DescDado / 100), 2);
                            atomicoCustoFin.CustoFinancFornecCoeficiente = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? 0 : item.CustoFinancFornecCoeficiente;
                            atomicoCustoFin.CustoFinancFornecPrecoListaBase = Math.Round((decimal)p.Preco_lista, 2);
                            atomicoCustoFin.StatusDescontoSuperior = idAlcada > 0 ? true : false;
                            atomicoCustoFin.DataHoraDescontoSuperior = idAlcada > 0 ? (DateTime?)DateTime.Now : null;
                            atomicoCustoFin.IdUsuarioDescontoSuperior = idAlcada > 0 ? (int?)usuarioLogado.Id : null;
                            atomicoCustoFin.IdOperacaoAlcadaDescontoSuperior = idAlcada > 0 ? (int?)idAlcada : null;

                            JsonSerializerOptions options = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.Preserve, WriteIndented = true };
                            _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Atualizando produto atômico custo fin. Id => [{atomicoCustoFin.Id}]");
                            var responseAtomicoCustoFin = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.AtualizarComTransacao(atomicoCustoFin, contextoBdGravacao);

                            response.TorcamentoCotacaoOpcaoItemAtomicoCustoFins.Add(responseAtomicoCustoFin);

                            var atomicoCustoAntigo = pagtoAntigo.TorcamentoCotacaoOpcaoItemAtomicoCustoFin.Where(x => x.Id == atomicoCustoFin.Id).FirstOrDefault();
                            string logApoio = UtilsGlobais.Util.MontalogComparacao(responseAtomicoCustoFin, atomicoCustoAntigo, "", "");
                            if (!string.IsNullOrEmpty(logApoio))
                                logApoio = $"Id={responseAtomicoCustoFin.Id}; IdItemAtomico={responseAtomicoCustoFin.IdItemAtomico}; IdOpcaoPagto={responseAtomicoCustoFin.IdOpcaoPagto}; {logApoio}";

                            if (pagto.Tipo_parcelamento == int.Parse(Constantes.COD_FORMA_PAGTO_A_VISTA))
                            {
                                if (!string.IsNullOrEmpty(logEdicaoAVista)) logEdicaoAVista += "\r      ";
                                if (!string.IsNullOrEmpty(logApoio))
                                    logEdicaoAVista += $"{logApoio}";
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(logEdicaoAprazo) && !string.IsNullOrEmpty(logApoio))
                                    logEdicaoAprazo += "\r      ";
                                if (!string.IsNullOrEmpty(logApoio))
                                    logEdicaoAprazo += $"{logApoio}";
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(logEdicaoAprazo)) response.LogOperacao += $"\r   Valor dos produtos atômicos a prazo: {logEdicaoAprazo}";
            if (!string.IsNullOrEmpty(logCriacaoAPrazo)) response.LogOperacao += $"\r   Valor dos produtos inseridos atômicos a prazo: {logCriacaoAPrazo}";
            if (!string.IsNullOrEmpty(logEdicaoAVista)) response.LogOperacao += $"\r   Valor dos produtos atômicos a vista: {logEdicaoAVista}";
            if (!string.IsNullOrEmpty(logCriacaoAVista)) response.LogOperacao += $"\r   Valor dos produtos inseridos atômicos a vista: {logCriacaoAVista}";

            response.Sucesso = true;
            return response;
        }

        public string VerificarUsoDeAlcada(UsuarioLogin usuarioLogado, OrcamentoResponse orcamento,
            AtualizarOrcamentoOpcaoProdutoRequest produto, float percRT)
        {
            PercMaxDescEComissaoResponseViewModel percMaxPorAlcada = new PercMaxDescEComissaoResponseViewModel();
            if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_1) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_2) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_3))
            {
                percMaxPorAlcada = _lojaOrcamentoCotacaoBll.BuscarPercMaxPorLojaAlcada(orcamento.Loja, orcamento.ClienteOrcamentoCotacaoDto.Tipo, usuarioLogado.Permissoes);
            }

            var percPadrao = _lojaOrcamentoCotacaoBll.BuscarPercMaxPorLoja(orcamento.Loja);
            if (percPadrao == null) return "Ops! Não falha ao validar opcão!";

            var percPadraoPorTipo = orcamento.ClienteOrcamentoCotacaoDto.Tipo == Constantes.ID_PF ?
                percPadrao.PercMaxComissaoEDesconto : percPadrao.PercMaxComissaoEDescontoPJ;

            if (produto.DescDado > percPadraoPorTipo && percMaxPorAlcada.PercMaxComissaoEDesconto == 0)
                return "Ops! Não pode execeder o limite máximo de desconto.";

            if (produto.DescDado > 0 || percRT > 0)
            {
                if (produto.DescDado > percPadraoPorTipo) return "Sim";
            }

            return null;
        }

        private string ValidarDescontoComissaoAlcada(AtualizarOrcamentoOpcaoProdutoRequest produto,
            UsuarioLogin usuarioLogado, OrcamentoResponse orcamento, AtualizarOrcamentoOpcaoRequest opcao)
        {
            var opcaoAntiga = orcamento.ListaOrcamentoCotacaoDto.Where(x => x.Id == opcao.Id).FirstOrDefault();
            if (opcaoAntiga == null) return "Falha ao buscar a opção!";

            bool temAlcada = false;
            PercMaxDescEComissaoResponseViewModel percMaxPorAlcada = new PercMaxDescEComissaoResponseViewModel();
            if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_1) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_2) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_3))
            {
                percMaxPorAlcada = _lojaOrcamentoCotacaoBll.BuscarPercMaxPorLojaAlcada(orcamento.Loja, orcamento.ClienteOrcamentoCotacaoDto.Tipo, usuarioLogado.Permissoes);
                if (produto.DescDado > percMaxPorAlcada.PercMaxComissaoEDesconto)
                    return $"O desconto no '{produto.Produto}' está excedendo o máximo permitido!";
                temAlcada = true;
            }

            var percPadrao = _lojaOrcamentoCotacaoBll.BuscarPercMaxPorLoja(orcamento.Loja);

            var percAUtilizar = percMaxPorAlcada.PercMaxComissao > 0 ? percMaxPorAlcada : percPadrao;

            var responseComissao = ValidarComissao(percAUtilizar, orcamento, opcao);
            if (!string.IsNullOrEmpty(responseComissao)) return responseComissao;

            var responseDesconto = ValidarDescontos(percAUtilizar, produto, orcamento.ClienteOrcamentoCotacaoDto.Tipo, temAlcada);
            if (!string.IsNullOrEmpty(responseDesconto)) return responseDesconto;

            return null;
        }

        public string ValidarDescontos(PercMaxDescEComissaoResponseViewModel percMaxDescEComissaoResponse,
            AtualizarOrcamentoOpcaoProdutoRequest produto, string tipoCliente, bool temAlcada)
        {
            var percMaxDescComissao = temAlcada ? percMaxDescEComissaoResponse.PercMaxComissaoEDesconto :
                tipoCliente == Constantes.ID_PF ? percMaxDescEComissaoResponse.PercMaxComissaoEDesconto :
                percMaxDescEComissaoResponse.PercMaxComissaoEDescontoPJ;

            if (produto.DescDado > percMaxDescComissao) return $"O valor de desconto do produto " +
                $"{produto.Fabricante}/{produto.Produto} excede o máximo permitido";

            return null;
        }

        public string ValidarComissao(PercMaxDescEComissaoResponseViewModel percMaxDescEComissaoResponse,
            OrcamentoResponse orcamento, AtualizarOrcamentoOpcaoRequest opcao)
        {
            if (opcao.PercRT < 0) throw new ArgumentException("Ops! A comissão não pode ser negativa!");

            if (orcamento.Parceiro == null && opcao.PercRT > 0)
                return "Ops! Não pode conter percentual de comissão!";

            if (opcao.PercRT > 0)
                if (opcao.PercRT > percMaxDescEComissaoResponse.PercMaxComissao)
                    return "Ops! O valor de comissão excede o máximo permitido";

            return null;
        }

        public async Task<List<ProdutoOrcamentoOpcaoResponseViewModel>> BuscarOpcaoProdutos(int idOpcao)
        {
            var opcaoProdutosUnificados = orcamentoCotacaoOpcaoItemUnificadoBll.PorFiltro(new TorcamentoCotacaoOpcaoItemUnificadoFiltro() { IdOpcao = idOpcao });

            if (opcaoProdutosUnificados == null) return null;

            List<ProdutoOrcamentoOpcaoResponseViewModel> produtosResponse = new List<ProdutoOrcamentoOpcaoResponseViewModel>();
            foreach (var item in opcaoProdutosUnificados)
            {
                var itemAtomico = orcamentoCotacaoOpcaoItemAtomicoBll.PorFiltro(new TorcamentoCotacaoOpcaoItemAtomicoFiltro() { IdItemUnificado = item.Id });
                var itemAtomicoCusto = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.PorFiltro(new TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro() { LstIdItemAtomico = itemAtomico.Select(x => x.Id).ToList() });
                var urlImagem = _produtoCatalogoBll.ObterDadosImagemPorProduto(item.Produto).FirstOrDefault().Caminho;
                
                if (itemAtomico == null || itemAtomicoCusto == null) break;

                var produtoResponse = new ProdutoOrcamentoOpcaoResponseViewModel();

                produtoResponse.IdItemUnificado = item.Id;
                produtoResponse.Fabricante = item.Fabricante;
                produtoResponse.FabricanteNome = (await produtoGeralBll.ObterListaFabricante()).Where(x => x.Fabricante == item.Fabricante).FirstOrDefault().Nome;
                produtoResponse.Produto = item.Produto;
                produtoResponse.UrlImagem = urlImagem == "sem-imagem.png" ? null : urlImagem;
                produtoResponse.Descricao = item.DescricaoHtml;
                produtoResponse.Qtde = item.Qtde;
                produtoResponse.IdOpcaoPagto = item.Id;
                produtoResponse.DescDado = itemAtomicoCusto.FirstOrDefault().DescDado;

                decimal precoLista = 0;
                decimal precoVenda = 0;
                decimal precoNf = 0;

                foreach (var atomico in itemAtomico)
                {
                    var itemCusto = itemAtomicoCusto.Where(c => c.IdItemAtomico == atomico.Id && c.CustoFinancFornecCoeficiente > 0).FirstOrDefault();
                    precoLista += Math.Round(itemCusto.PrecoLista * (decimal)atomico.Qtde, 2);
                    precoVenda += Math.Round(itemCusto.PrecoVenda * (decimal)atomico.Qtde, 2);
                    precoNf += Math.Round(itemCusto.PrecoVenda * (decimal)atomico.Qtde, 2);
                }
                produtoResponse.PrecoLista = precoLista;
                produtoResponse.PrecoVenda = precoVenda;
                produtoResponse.PrecoNf = precoNf;
                produtoResponse.CustoFinancFornecPrecoListaBase = itemAtomico.Sum(x => x.Qtde * itemAtomicoCusto.Where(c => c.IdItemAtomico == x.Id && c.CustoFinancFornecCoeficiente > 0).FirstOrDefault().CustoFinancFornecPrecoListaBase);

                produtoResponse.CustoFinancFornecCoeficiente = itemAtomicoCusto.Where(x => x.CustoFinancFornecCoeficiente > 0).FirstOrDefault().CustoFinancFornecCoeficiente;
                produtoResponse.TotalItem = Math.Round(precoNf * item.Qtde, 2);
                produtoResponse.IdOperacaoAlcadaDescontoSuperior = itemAtomicoCusto.FirstOrDefault().IdOperacaoAlcadaDescontoSuperior;

                produtosResponse.Add(produtoResponse);
            }

            return produtosResponse;
        }

        public async Task<List<TorcamentoCotacaoOpcaoItemAtomico>> BuscarTorcamentoCotacaoOpcaoItemAtomicos(int idAtomico)
        {
            var retorno = orcamentoCotacaoOpcaoItemAtomicoBll.PorFiltro(new TorcamentoCotacaoOpcaoItemAtomicoFiltro() { IdItemUnificado = idAtomico });
            return await Task.FromResult(retorno);
        }

        public async Task<List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin>> BuscarTorcamentoCotacaoOpcaoItemAtomicosCustoFin(TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro filtro)
        {
            var retorno = await Task.FromResult(orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.PorFiltro(filtro));
            return await Task.FromResult(retorno);
        }

        public ProdutosGruposResponse BuscarGruposProdutos()
        {
            try
            {
                var response = new ProdutosGruposResponse();
                response.Sucesso = false;

                var retorno = _produtoCatalogoBll.BuscarProdutoGrupos(new TprodutoGrupoFiltro() { IncluirTProduto = true });
                if (retorno == null)
                {
                    response.Mensagem = "Ops! Falha ao buscar grupos de produtos!";
                    return response;
                }

                retorno = retorno.Distinct().ToList();

                response.ProdutosGrupos = new List<ProdutoGrupoResponse>();
                foreach (var grupo in retorno)
                {
                    var produtoGrupo = new ProdutoGrupoResponse();
                    produtoGrupo.Codigo = grupo.Codigo;
                    produtoGrupo.Descricao = grupo.Descricao;
                    response.ProdutosGrupos.Add(produtoGrupo);
                }

                response.ProdutosGrupos = response.ProdutosGrupos.OrderBy(x => x.Descricao).ToList();
                response.Sucesso = true;
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CfgParametroResponse BuscarParametro(int id)
        {
            var parametro = cfgParametroBll.PorFiltro(new InfraBanco.Modelos.Filtros.TcfgParametroFiltro() { Id = id }).FirstOrDefault();
            if (parametro == null) return null;

            CfgParametroResponse response = new CfgParametroResponse();
            response.Id = parametro.Id;
            response.Descricao = parametro.Descricao;
            response.Sigla = parametro.Sigla;
            response.IdCfgDataType = parametro.IdCfgDataType;

            return response;
        }

        public ListaGruposSubgruposProdutosResponse BuscarGrupoSubgrupoProduto(int id, string loja)
        {
            var response = new ListaGruposSubgruposProdutosResponse();

            response.Sucesso = false;
            var parametro = BuscarParametro(38);
            if (parametro == null)
            {
                response.Mensagem = "Falha ao buscar parâmetro para categorias!";
                return response;
            }

            var objLoja = _lojaBll.PorFiltro(new TlojaFiltro() { Loja = loja }).FirstOrDefault();
            if (objLoja == null)
            {
                response.Mensagem = "Falha ao buscar loja para categorias!";
                return response;
            }

            var unidadeNegocio = cfgUnidadeNegocioBll.PorFiltro(new TcfgUnidadeNegocioFiltro() { Sigla = objLoja.Unidade_Negocio }).FirstOrDefault();
            if (unidadeNegocio == null)
            {
                response.Mensagem = "Falha ao buscar unidade de negócio para categorias!";
                return response;
            }

            var unidadeNegocioParametro = cfgUnidadeNegocioParametroBll.PorFiltro(
                new TcfgUnidadeNegocioParametroFiltro()
                {
                    IdCfgUnidadeNegocio = unidadeNegocio.Id,
                    IdCfgParametro = parametro.Id
                }).FirstOrDefault();
            if (unidadeNegocioParametro == null)
            {
                response.Mensagem = "Falha ao buscar parâmetro de unidade de negócio para categorias!";
                return response;
            }

            var grupo = _produtoCatalogoBll.BuscarProdutoGrupos(new TprodutoGrupoFiltro());
            if (grupo == null)
            {
                response.Mensagem = "Falha ao buscar grupo para categorias!";
                return response;
            }

            var subGrupo = _produtoCatalogoBll.BuscarProdutosSubgrupos(new TprodutoSubgrupoFiltro());
            if (subGrupo == null)
            {
                response.Mensagem = "Falha ao buscar subgrupos para categorias!";
                return response;
            }

            response.ListaGruposSubgruposProdutos = new List<GrupoSubgrupoProdutoResponse>();

            //CRT§CRT|FAN§DUT|FAN§HW|FAN§K74
            var listaSplit = unidadeNegocioParametro.Valor.Split("|");
            foreach (var split in listaSplit)
            {
                var listaSplit2 = split.Split("§");
                var grp = grupo.Where(x => x.Codigo == listaSplit2[0]).FirstOrDefault();
                var sbgrp = subGrupo.Where(x => x.Codigo == listaSplit2[1]).FirstOrDefault();

                var grupoSubgrupoResponse = new GrupoSubgrupoProdutoResponse();

                if (grp.Codigo == sbgrp.Codigo)
                {
                    grupoSubgrupoResponse.Descricao = grp.Descricao;
                }
                else
                {
                    grupoSubgrupoResponse.Descricao = $"{grp.Descricao} - {sbgrp.Descricao}";
                }

                grupoSubgrupoResponse.Codigo = split;
                response.ListaGruposSubgruposProdutos.Add(grupoSubgrupoResponse);
            }

            response.Sucesso = true;

            return response;
        }
    }
}
