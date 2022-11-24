﻿using AutoMapper;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Loja;
using Microsoft.Extensions.Logging;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
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

        public ProdutoOrcamentoCotacaoBll(
            ILogger<ProdutoOrcamentoCotacaoBll> logger,
            Produto.ProdutoGeralBll produtoGeralBll, CoeficienteBll coeficienteBll, IMapper mapper,
            OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoBll orcamentoCotacaoOpcaoItemUnificadoBll,
            OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoBll orcamentoCotacaoOpcaoItemAtomicoBll,
            OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll orcamentoCotacaoOpcaoItemAtomicoCustoFinBll,
            LojaOrcamentoCotacaoBll _lojaOrcamentoCotacaoBll,
            LojaBll _lojaBll,
            ProdutoCatalogoBll _produtoCatalogoBll)
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

                foreach (Produto.Dados.ProdutoCompostoDados composto in produtoComboDados.ProdutoCompostoDados)
                {
                    var produtoCompostoResponse = new ProdutoCompostoResponseViewModel();
                    produtoCompostoResponse.Filhos = new List<ProdutoCompostoFilhosResponseViewModel>();
                    decimal? somaFilhotes = 0;

                    var produtoCompostoResponseApoio = new ProdutoCompostoResponseViewModel();
                    produtoCompostoResponseApoio.Filhos = new List<ProdutoCompostoFilhosResponseViewModel>();

                    var filhotes = (from c in produtoComboDados.ProdutoDados
                                    join d in composto.Filhos on new { a = c.Fabricante, b = c.Produto } equals new { a = d.Fabricante, b = d.Produto }
                                    select c).ToList();

                    if (composto.Filhos.Count != filhotes.Count)
                    {
                        var prodARemover = produtoComboDados.ProdutoDados.Where(x => x.Fabricante == composto.PaiFabricante && x.Produto == composto.PaiProduto).FirstOrDefault();
                        if (prodARemover != null) produtoComboDados.ProdutoDados.Remove(prodARemover);
                        continue;
                    }

                    foreach (var filho in filhotes)
                    {
                        var compostoFilho = composto.Filhos.Where(x => x.Produto == filho.Produto).FirstOrDefault();
                        produtoCompostoResponseApoio.Filhos.Add(ProdutoCompostoFilhosResponseViewModel.ConverterProdutoFilhoDados(filho, compostoFilho.Qtde, GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), composto.PaiFabricante)));

                        somaFilhotes += filho.Preco_lista * compostoFilho.Qtde;
                    }

                    var pai = produtoComboDados.ProdutoDados.Where(x => x.Fabricante == composto.PaiFabricante && x.Produto == composto.PaiProduto).FirstOrDefault();
                    if (composto.PaiProduto == "034908")
                    {

                    }
                    if (pai == null)
                    {
                        var produtoCompostoAInserir = new Produto.Dados.ProdutoDados()
                        {
                            Fabricante = composto.PaiFabricante,
                            Fabricante_Nome = composto.PaiFabricanteNome,
                            Produto = composto.PaiProduto,
                            Descricao_html = composto.PaiDescricao,
                            Descricao = composto.PaiDescricao,
                            Preco_lista = somaFilhotes,
                            Qtde_Max_Venda = filhotes.Min(x => x.Qtde_Max_Venda),
                            Desc_Max = filhotes.Min(x => x.Desc_Max)
                        };
                        produtoComboDados.ProdutoDados.Add(produtoCompostoAInserir);
                    }
                    else
                    {
                        pai.Preco_lista = somaFilhotes;
                    }

                    produtoCompostoResponse = ProdutoCompostoResponseViewModel.ConverterProdutoCompostoDados(composto);

                    var coeficiente = GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), composto.PaiFabricante).Coeficiente;
                    produtoCompostoResponse.PaiPrecoTotalBase = somaFilhotes;
                    produtoCompostoResponse.PaiPrecoTotal = somaFilhotes * Convert.ToDecimal(coeficiente);
                    produtoCompostoResponse.Filhos = produtoCompostoResponseApoio.Filhos;
                    produtoResponseViewModel.ProdutosCompostos.Add(produtoCompostoResponse);


                }

                foreach (var produto in produtoComboDados.ProdutoDados)
                {
                    produtoResponseViewModel.ProdutosSimples.Add(ProdutoSimplesResponseViewModel.ConverterProdutoDados(produto, null, GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), produto.Fabricante)));

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

        public CadastroOpcaoProdutosUnificadosResponse CadastrarOrcamentoCotacaoOpcaoProdutosUnificadosEAtomicosComTransacao(OrcamentoOpcaoRequest opcao,
            int idOrcamentoCotacaoOpcao, string loja, ContextoBdGravacao contextoBdGravacao, ref string erro, Guid correlationId)
        {
            CadastroOpcaoProdutosUnificadosResponse response = new CadastroOpcaoProdutosUnificadosResponse();
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

        private CadastroOpcaoProdutosAtomicosResponse CadastrarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(int idItemUnificado,
            Tproduto tProduto, TecProdutoComposto tEcProdutoComposto, int qtde, string loja, ContextoBdGravacao contextoBdGravacao,
            Guid correlationId)
        {
            CadastroOpcaoProdutosAtomicosResponse response = new CadastroOpcaoProdutosAtomicosResponse();
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

            var torcamentoCotacaoOpcaoItemAtomico = new TorcamentoCotacaoOpcaoItemAtomico(idItemUnificado, tProduto.Fabricante,
                tProduto.Produto, (short)qtde, tProduto.Descricao, tProduto.Descricao_Html);

            _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Cadastrando produto atômico. Request => [{JsonSerializer.Serialize(torcamentoCotacaoOpcaoItemAtomico)}].");
            response.TorcamentoCotacaoOpcaoItemAtomicos.Add(orcamentoCotacaoOpcaoItemAtomicoBll.InserirComTransacao(torcamentoCotacaoOpcaoItemAtomico,
               contextoBdGravacao));

            response.Sucesso = true;
            return response;
        }

        public CadastroOpcaoProdutoAtomicosCustoFinResponse CadastrarProdutoAtomicoCustoFinComTransacao(List<TorcamentoCotacaoOpcaoPagto> tOrcamentoCotacaoOpcaoPagtos,
            List<TorcamentoCotacaoItemUnificado> tOrcamentoCotacaoItemUnificados, List<ProdutoRequestViewModel> produtosOpcao, string loja,
            ContextoBdGravacao contextoBdGravacao, Guid correlationId)
        {
            CadastroOpcaoProdutoAtomicosCustoFinResponse response = new CadastroOpcaoProdutoAtomicosCustoFinResponse();
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
            ContextoBdGravacao contextoBdGravacao, Guid correlationId)
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

            foreach (var item in lstProdutos)
            {
                var produto = produtosUnificados.Where(x => x.Id == item.IdItemUnificado).FirstOrDefault();

                if (produto == null)
                {
                    response.Mensagem = "Falha ao buscar item para atualização!";
                    return response;
                }

                produto.Qtde = item.Qtde;
                JsonSerializerOptions options = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };
                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Atualizando produto unificado Id[{produto.Id}]. Resquest => {JsonSerializer.Serialize(produto, options)}");
                var retorno = orcamentoCotacaoOpcaoItemUnificadoBll.AtualizarComTransacao(produto, contextoBdGravacao);

                response.TorcamentoCotacaoItemUnificados.Add(retorno);
            }
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
            UsuarioLogin usuarioLogado, OrcamentoResponseViewModel orcamento, Guid correlationId)
        {
            var response = new AtualizarOrcamentoOpcaoProdutoAtomicoCustoFinResponse();
            response.Sucesso = false;
            var nomeMetodo = response.ObterNomeMetodoAtualAsync();
            response.TorcamentoCotacaoOpcaoItemAtomicoCustoFins = new List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin>();

            foreach (var item in opcao.ListaProdutos)
            {
                //precisa cadastrar o custo fin no caso de ter cadastrado um pagamento 
                var unificado = produtosUnificados.Where(x => x.Fabricante == item.Fabricante && x.Produto == item.Produto).FirstOrDefault();
                if (unificado.TorcamentoCotacaoOpcaoItemAtomicos.Count <= 0) throw new ArgumentException("Falha ao buscar item atômico para atualização!");

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
                        var atomicoCustoFin = torcamentoCotacaoOpcaoItemAtomicosFin.Where(x => x.TorcamentoCotacaoOpcaoPagto.Tipo_parcelamento == pagto.Tipo_parcelamento).FirstOrDefault();
                        _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Validando desconto e comissão por alçada.");
                        if (ValidarDescontoComissaoAlcada(item, usuarioLogado, orcamento, opcao))
                        {
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

                                JsonSerializerOptions options = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, WriteIndented = true };
                                _logger.LogInformation($"CorrelationId => [{correlationId}]. {nomeMetodo}. Atualizando produto atômico custo fin. Resquest => {JsonSerializer.Serialize(atomicoCustoFin, options)}");
                                var responseAtomicoCustoFin = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.AtualizarComTransacao(atomicoCustoFin, contextoBdGravacao);

                                response.TorcamentoCotacaoOpcaoItemAtomicoCustoFins.Add(responseAtomicoCustoFin);
                            }
                        };
                    }
                }
            }

            response.Sucesso = true;
            return response;
        }

        public string VerificarUsoDeAlcada(UsuarioLogin usuarioLogado, OrcamentoResponseViewModel orcamento,
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

        private bool ValidarDescontoComissaoAlcada(AtualizarOrcamentoOpcaoProdutoRequest produto,
            UsuarioLogin usuarioLogado, OrcamentoResponseViewModel orcamento, AtualizarOrcamentoOpcaoRequest opcao)
        {
            var opcaoAntiga = orcamento.ListaOrcamentoCotacaoDto.Where(x => x.Id == opcao.Id).FirstOrDefault();
            if (opcaoAntiga == null) throw new ArgumentException("Falha ao buscar a opção!");

            bool temAlcada = false;
            PercMaxDescEComissaoResponseViewModel percMaxPorAlcada = new PercMaxDescEComissaoResponseViewModel();
            if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_1) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_2) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_3))
            {
                percMaxPorAlcada = _lojaOrcamentoCotacaoBll.BuscarPercMaxPorLojaAlcada(orcamento.Loja, orcamento.ClienteOrcamentoCotacaoDto.Tipo, usuarioLogado.Permissoes);
                if (produto.DescDado > percMaxPorAlcada.PercMaxComissaoEDesconto)
                    throw new ArgumentException($"O desconto no '{produto.Produto}' está excedendo o máximo permitido!");
                temAlcada = true;
            }

            var percPadrao = _lojaOrcamentoCotacaoBll.BuscarPercMaxPorLoja(orcamento.Loja);

            var percAUtilizar = percMaxPorAlcada.PercMaxComissao > 0 ? percMaxPorAlcada : percPadrao;

            ValidarComissao(percAUtilizar, orcamento, opcao);

            ValidarDescontos(percAUtilizar, produto, orcamento.ClienteOrcamentoCotacaoDto.Tipo, temAlcada);

            return true;
        }

        public void ValidarDescontos(PercMaxDescEComissaoResponseViewModel percMaxDescEComissaoResponse,
            AtualizarOrcamentoOpcaoProdutoRequest produto, string tipoCliente, bool temAlcada)
        {
            if (produto.DescDado < 0) throw new ArgumentException($"O valor de desconto do produto {produto.Fabricante}/" +
                 $"{produto.Produto} não pode ser negativo!");

            var percMaxDescComissao = temAlcada ? percMaxDescEComissaoResponse.PercMaxComissaoEDesconto :
                tipoCliente == Constantes.ID_PF ? percMaxDescEComissaoResponse.PercMaxComissaoEDesconto :
                percMaxDescEComissaoResponse.PercMaxComissaoEDescontoPJ;

            if (produto.DescDado > percMaxDescComissao) throw new ArgumentException($"O valor de desconto do produto " +
                $"{produto.Fabricante}/{produto.Produto} excede o máximo permitido");

        }

        public void ValidarComissao(PercMaxDescEComissaoResponseViewModel percMaxDescEComissaoResponse,
            OrcamentoResponseViewModel orcamento, AtualizarOrcamentoOpcaoRequest opcao)
        {
            if (opcao.PercRT < 0) throw new ArgumentException("Ops! A comissão não pode ser negativa!");

            if (orcamento.Parceiro == null && opcao.PercRT > 0)
                throw new ArgumentException("Ops! Não pode conter percentual de comissão!");

            if (opcao.PercRT > 0)
                if (opcao.PercRT > percMaxDescEComissaoResponse.PercMaxComissao)
                    throw new ArgumentException("Ops! O valor de comissão excede o máximo permitido");
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

                if (itemAtomico == null || itemAtomicoCusto == null) break;

                var produtoResponse = new ProdutoOrcamentoOpcaoResponseViewModel();

                //ITEM ATOMICO
                //Id = item.Id,
                produtoResponse.IdItemUnificado = item.Id;
                produtoResponse.Fabricante = item.Fabricante;
                produtoResponse.FabricanteNome = (await produtoGeralBll.ObterListaFabricante()).Where(x => x.Fabricante == item.Fabricante).FirstOrDefault().Nome;
                produtoResponse.Produto = item.Produto;
                produtoResponse.UrlImagem = _produtoCatalogoBll.ObterDadosImagemPorProduto(item.Produto).FirstOrDefault().Caminho;
                produtoResponse.Descricao = item.DescricaoHtml;
                produtoResponse.Qtde = item.Qtde;
                //ITEM ATOMICO CUSTO
                produtoResponse.IdOpcaoPagto = item.Id;
                produtoResponse.DescDado = itemAtomicoCusto.FirstOrDefault().DescDado;

                //itemAtomico.ForEach(x =>
                //{
                //    var custo = itemAtomicoCusto.Where(c => c.IdItemAtomico == x.Id && c.CustoFinancFornecCoeficiente > 0).FirstOrDefault();
                //    //testar
                //    produtoResponse.PrecoLista += x.Qtde * custo.PrecoLista;
                //    //produtoResponse.PrecoVenda += x.Qtde * custo.PrecoVenda;
                //    //produtoResponse.PrecoNf += x.Qtde * custo.PrecoNF;
                //    produtoResponse.CustoFinancFornecPrecoListaBase += x.Qtde * custo.CustoFinancFornecPrecoListaBase;
                //});
                var precoLista = itemAtomico.Sum(x => x.Qtde * itemAtomicoCusto.Where(c => c.IdItemAtomico == x.Id && c.CustoFinancFornecCoeficiente > 0).FirstOrDefault().PrecoLista);
                produtoResponse.PrecoLista = precoLista;
                produtoResponse.PrecoVenda = Math.Round(precoLista * (decimal)(1 - produtoResponse.DescDado / 100), 2);
                produtoResponse.PrecoNf = Math.Round(precoLista * (decimal)(1 - produtoResponse.DescDado / 100), 2);
                produtoResponse.CustoFinancFornecPrecoListaBase = itemAtomico.Sum(x => x.Qtde * itemAtomicoCusto.Where(c => c.IdItemAtomico == x.Id && c.CustoFinancFornecCoeficiente > 0).FirstOrDefault().CustoFinancFornecPrecoListaBase);

                produtoResponse.CustoFinancFornecCoeficiente = itemAtomicoCusto.Where(x => x.CustoFinancFornecCoeficiente > 0).FirstOrDefault().CustoFinancFornecCoeficiente;
                //produtoResponse.TotalItem = produtoResponse.PrecoNf * produtoResponse.Qtde;
                produtoResponse.TotalItem = produtoResponse.PrecoNf * item.Qtde; // aqui esta errado
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
    }
}
