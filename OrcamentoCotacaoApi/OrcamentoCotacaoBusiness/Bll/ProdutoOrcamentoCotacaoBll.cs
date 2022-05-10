using AutoMapper;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class ProdutoOrcamentoCotacaoBll
    {
        private readonly ProdutoGeralBll produtoGeralBll;
        private readonly CoeficienteBll coeficienteBll;
        private readonly IMapper _mapper;
        private readonly OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoBll orcamentoCotacaoOpcaoItemUnificadoBll;
        private readonly OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoBll orcamentoCotacaoOpcaoItemAtomicoBll;
        private readonly OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll orcamentoCotacaoOpcaoItemAtomicoCustoFinBll;

        public ProdutoOrcamentoCotacaoBll(Produto.ProdutoGeralBll produtoGeralBll, CoeficienteBll coeficienteBll, IMapper mapper,
            OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoBll orcamentoCotacaoOpcaoItemUnificadoBll,
            OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoBll orcamentoCotacaoOpcaoItemAtomicoBll,
            OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll orcamentoCotacaoOpcaoItemAtomicoCustoFinBll)
        {
            this.produtoGeralBll = produtoGeralBll;
            this.coeficienteBll = coeficienteBll;
            _mapper = mapper;
            this.orcamentoCotacaoOpcaoItemUnificadoBll = orcamentoCotacaoOpcaoItemUnificadoBll;
            this.orcamentoCotacaoOpcaoItemAtomicoBll = orcamentoCotacaoOpcaoItemAtomicoBll;
            this.orcamentoCotacaoOpcaoItemAtomicoCustoFinBll = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll;
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
                var produtoCompostoResponse = ProdutoCompostoResponseViewModel.ConverterProdutoCompostoDados(composto);

                foreach (var filhos in composto.Filhos)
                {


                    var filho = produtoComboDados.ProdutoDados
                    .Where(x => x.Produto == filhos.Produto && x.Fabricante == filhos.Fabricante)
                    .Select(x => x)
                    .FirstOrDefault();

                    if (filho == null)
                    {
                        break;
                    }

                    produtoCompostoResponse.Filhos.Add(ProdutoCompostoFilhosResponseViewModel.ConverterProdutoFilhoDados(filho, filhos.Qtde, GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), composto.PaiFabricante)));
                }
                var coeficiente = GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), composto.PaiFabricante).Coeficiente;
                produtoCompostoResponse.PaiPrecoTotalBase = composto.PaiPrecoTotal;
                produtoCompostoResponse.PaiPrecoTotal = produtoCompostoResponse.PaiPrecoTotal * Convert.ToDecimal(coeficiente);

                produtoResponseViewModel.ProdutosCompostos.Add(produtoCompostoResponse);

            }

            foreach (var produto in produtoComboDados.ProdutoDados)
            {
                produtoResponseViewModel.ProdutosSimples.Add(ProdutoSimplesResponseViewModel.ConverterProdutoDados(produto, null, GetCoeficienteOuNull(dicCoeficiente.ToDictionary(x => x.Fabricante, x => x), produto.Fabricante)));

            }

            return produtoResponseViewModel;
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
            bool propriedadeOculta, bool propriedadeOcultaItem)
        {
            var produtosPropTexto = await produtoGeralBll.ObterProdutoPropriedadesAtivosTexto(idProduto, propriedadeOculta, propriedadeOcultaItem);
            var produtosPropListas = await produtoGeralBll.ObterProdutoPropriedadesAtivosLista(idProduto, propriedadeOculta, propriedadeOcultaItem);

            List<ProdutoCatalogoItemProdutosAtivosResponseViewModel> retorno = new List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>();
            if (produtosPropListas != null && produtosPropTexto != null)
            {
                produtosPropListas.AddRange(produtosPropTexto);
                retorno = _mapper.Map<List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>>(produtosPropListas);
            }
            return retorno;
        }


        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos()
        {
            var lstProdutoPropriedades = await produtoGeralBll.ObterListaPropriedadesProdutos();

            return lstProdutoPropriedades;
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

            return lstProdutoPropriedades;
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

        public bool GravarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            return produtoGeralBll.GravarPropriedadesProdutos(produtoCatalogoPropriedade);
        }
        public bool AtualizarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            return produtoGeralBll.AtualizarPropriedadesProdutos(produtoCatalogoPropriedade);
        }

        public List<TorcamentoCotacaoItemUnificado> CadastrarOrcamentoCotacaoOpcaoProdutosUnificadosComTransacao(OrcamentoOpcaoRequestViewModel opcao,
            int idOrcamentoCotacaoOpcao, string loja, ContextoBdGravacao contextoBdGravacao)
        {
            List<TorcamentoCotacaoItemUnificado> itensUnificados = new List<TorcamentoCotacaoItemUnificado>();
            int sequencia = 0;

            foreach (var produto in opcao.ListaProdutos)
            {
                var tProduto = produtoGeralBll.BuscarProdutoPorFabricanteECodigoComTransacao(produto.Fabricante,
                    produto.Produto, loja, contextoBdGravacao).Result;

                if (tProduto == null) throw new ArgumentException("Ops! Não achamos o produto!");

                sequencia++;
                TorcamentoCotacaoItemUnificado torcamentoCotacaoItemUnificado = new TorcamentoCotacaoItemUnificado(idOrcamentoCotacaoOpcao,
                    tProduto.Fabricante, tProduto.Produto, produto.Qtde, tProduto.Descricao, tProduto.Descricao_Html, sequencia);

                var produtoUnificado = orcamentoCotacaoOpcaoItemUnificadoBll.InserirComTransacao(torcamentoCotacaoItemUnificado, contextoBdGravacao);

                if (produtoUnificado.Id == 0) throw new ArgumentException("Ops! Não gerou o Id de produto unificado!");

                itensUnificados.Add(produtoUnificado);

                var itensAtomicos = CadastrarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(produtoUnificado.Id,
                    tProduto, produto.Qtde, loja, contextoBdGravacao);

                if (itensAtomicos.Count <= 0) throw new ArgumentException("Ops! Não gerou o Id de produto atomicos!");
            }

            return itensUnificados;
        }

        private List<TorcamentoCotacaoOpcaoItemAtomico> CadastrarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(int idItemUnificado,
            Tproduto tProduto, int qtde, string loja, ContextoBdGravacao contextoBdGravacao)
        {
            List<TorcamentoCotacaoOpcaoItemAtomico> torcamentoCotacaoOpcaoItemAtomicos = new List<TorcamentoCotacaoOpcaoItemAtomico>();
            if (tProduto.TecProdutoComposto != null)
            {
                foreach (var item in tProduto.TecProdutoComposto.TecProdutoCompostoItems)
                {

                    var tProdutoAtomico = produtoGeralBll.BuscarProdutoPorFabricanteECodigoComTransacao(item.Fabricante_item,
                        item.Produto_item, loja, contextoBdGravacao).Result;

                    TorcamentoCotacaoOpcaoItemAtomico itemAtomico = new TorcamentoCotacaoOpcaoItemAtomico(idItemUnificado,
                        tProdutoAtomico.Fabricante, tProdutoAtomico.Produto, (short)(qtde * tProduto.TecProdutoComposto.TecProdutoCompostoItems.First().Qtde),
                        tProdutoAtomico.Descricao, tProdutoAtomico.Descricao_Html);

                    torcamentoCotacaoOpcaoItemAtomicos.Add(orcamentoCotacaoOpcaoItemAtomicoBll.InserirComTransacao(itemAtomico, contextoBdGravacao));
                }

                return torcamentoCotacaoOpcaoItemAtomicos;
            }

            var torcamentoCotacaoOpcaoItemAtomico = new TorcamentoCotacaoOpcaoItemAtomico(idItemUnificado, tProduto.Fabricante,
                tProduto.Produto, (short)qtde, tProduto.Descricao, tProduto.Descricao_Html);

            var rettorcamentoCotacaoOpcaoItemAtomico = orcamentoCotacaoOpcaoItemAtomicoBll.InserirComTransacao(torcamentoCotacaoOpcaoItemAtomico,
               contextoBdGravacao);

            torcamentoCotacaoOpcaoItemAtomicos.Add(rettorcamentoCotacaoOpcaoItemAtomico);

            return torcamentoCotacaoOpcaoItemAtomicos;
        }

        public List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin> CadastrarProdutoAtomicoCustoFinComTransacao(List<TorcamentoCotacaoOpcaoPagto> tOrcamentoCotacaoOpcaoPagtos,
            List<TorcamentoCotacaoItemUnificado> tOrcamentoCotacaoItemUnificados, List<ProdutoRequestViewModel> produtosOpcao, string loja, ContextoBdGravacao contextoBdGravacao)
        {
            int index = 0;
            foreach (var pagto in tOrcamentoCotacaoOpcaoPagtos)
            {
                foreach (var unificado in tOrcamentoCotacaoItemUnificados)
                {
                    var produtosEspecificos = unificado.TorcamentoCotacaoOpcaoItemAtomicos.Select(x => x.Produto).ToList();
                    var pDados = produtoGeralBll.BuscarProdutosEspecificos(loja, produtosEspecificos).Result;
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
                            PrecoLista = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? (decimal)item.Preco_lista : (decimal)item.Preco_lista * (decimal)itemOpcao.CustoFinancFornecCoeficiente,
                            PrecoVenda = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? (decimal)item.Preco_lista * (decimal)(1 - itemOpcao.DescDado / 100) : (decimal)item.Preco_lista * (decimal)itemOpcao.CustoFinancFornecCoeficiente * (decimal)(1 - itemOpcao.DescDado / 100),
                            PrecoNF = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? (decimal)item.Preco_lista * (decimal)(1 - itemOpcao.DescDado / 100) : (decimal)item.Preco_lista * (decimal)itemOpcao.CustoFinancFornecCoeficiente * (decimal)(1 - itemOpcao.DescDado / 100),
                            CustoFinancFornecCoeficiente = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? 0 : itemOpcao.CustoFinancFornecCoeficiente,
                            CustoFinancFornecPrecoListaBase = (decimal)item.Preco_lista
                        };

                        var orc = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.InserirComTransacao(atomicoCustoFin, contextoBdGravacao);
                    }
                }

            }

            return new List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin>();
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

                var produtoResponse = new ProdutoOrcamentoOpcaoResponseViewModel()
                {
                    //ITEM ATOMICO
                    //Id = item.Id,
                    IdItemUnificado = item.Id,
                    Fabricante = item.Fabricante,
                    FabricanteNome = (await produtoGeralBll.ObterListaFabricante()).Where(x => x.Fabricante == item.Fabricante).FirstOrDefault().Nome,
                    Produto = item.Produto,
                    Descricao = item.DescricaoHtml,
                    Qtde = item.Qtde,
                    //ITEM ATOMICO CUSTO
                    IdOpcaoPagto = item.Id,
                    DescDado = itemAtomicoCusto.FirstOrDefault().DescDado,
                    PrecoLista = itemAtomicoCusto.Where(x => x.CustoFinancFornecCoeficiente > 0).Sum(x => x.PrecoLista),
                    PrecoVenda = itemAtomicoCusto.Where(x => x.CustoFinancFornecCoeficiente > 0).Sum(x => x.PrecoVenda),
                    PrecoNf = itemAtomicoCusto.Where(x => x.CustoFinancFornecCoeficiente > 0).Sum(x => x.PrecoNF),
                    CustoFinancFornecPrecoListaBase = itemAtomicoCusto.Where(x => x.CustoFinancFornecCoeficiente > 0).Sum(x => x.CustoFinancFornecPrecoListaBase),
                    CustoFinancFornecCoeficiente = itemAtomicoCusto.FirstOrDefault().CustoFinancFornecCoeficiente,
                    TotalItem = itemAtomico.Sum(x => x.Qtde * itemAtomicoCusto.Where(y => y.IdItemAtomico == x.Id && y.CustoFinancFornecCoeficiente > 0).FirstOrDefault().PrecoNF)
                };

                produtosResponse.Add(produtoResponse);
            }

            return produtosResponse;
        }
    }
}
