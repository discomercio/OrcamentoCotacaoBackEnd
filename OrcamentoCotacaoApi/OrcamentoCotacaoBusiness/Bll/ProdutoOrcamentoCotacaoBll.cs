using AutoMapper;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using InfraBanco.Modelos.Filtros;
using InfraIdentity;
using Loja;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Response.FormaPagamento;
using Produto;
using Produto.Dto;
using ProdutoCatalogo;
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
        private readonly LojaOrcamentoCotacaoBll _lojaOrcamentoCotacaoBll;
        private readonly LojaBll _lojaBll;
        private readonly ProdutoCatalogoBll _produtoCatalogoBll;

        public ProdutoOrcamentoCotacaoBll(Produto.ProdutoGeralBll produtoGeralBll, CoeficienteBll coeficienteBll, IMapper mapper,
            OrcamentoCotacaoOpcaoItemUnificado.OrcamentoCotacaoOpcaoItemUnificadoBll orcamentoCotacaoOpcaoItemUnificadoBll,
            OrcamentoCotacaoOpcaoItemAtomico.OrcamentoCotacaoOpcaoItemAtomicoBll orcamentoCotacaoOpcaoItemAtomicoBll,
            OrcamentoCotacaoOpcaoItemAtomicoCustoFin.OrcamentoCotacaoOpcaoItemAtomicoCustoFinBll orcamentoCotacaoOpcaoItemAtomicoCustoFinBll,
            LojaOrcamentoCotacaoBll _lojaOrcamentoCotacaoBll,
            LojaBll _lojaBll,
            ProdutoCatalogoBll _produtoCatalogoBll)
        {
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

        public async Task<List<ProdutoCatalogoItemProdutosAtivosResponseViewModel>> ListarProdutoCatalogoParaVisualizacao(bool propriedadeOculta, bool propriedadeOcultaItem)
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

        public List<TorcamentoCotacaoItemUnificado> AtualizarOrcamentoCotacaoOpcaoProdutosUnificadosComTransacao(List<ProdutoOrcamentoOpcaoResponseViewModel> lstProdutos, int idOpcao,
            ContextoBdGravacao contextoBdGravacao)
        {
            List<TorcamentoCotacaoItemUnificado> lstIdsUnificados = new List<TorcamentoCotacaoItemUnificado>();

            var produtosUnificados = orcamentoCotacaoOpcaoItemUnificadoBll.PorFiltroComTransacao(new TorcamentoCotacaoOpcaoItemUnificadoFiltro() { IdOpcao = idOpcao }, contextoBdGravacao);
            if (produtosUnificados == null) throw new ArgumentException("Falha ao buscar itens para atualização!");

            foreach (var item in lstProdutos)
            {

                var produto = produtosUnificados.Where(x => x.Fabricante == item.Fabricante && x.Produto == item.Produto).FirstOrDefault();
                if (produto == null) throw new ArgumentException("Falha ao buscar item para atualização!");

                produto.Qtde = item.Qtde;
                orcamentoCotacaoOpcaoItemUnificadoBll.AtualizarComTransacao(produto, contextoBdGravacao);

                lstIdsUnificados.Add(produto);
            }
            return lstIdsUnificados;
        }

        public List<TorcamentoCotacaoItemUnificado> AtualizarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(List<ProdutoOrcamentoOpcaoResponseViewModel> lstProdutos, int idOpcao,
            List<TorcamentoCotacaoItemUnificado> produtosUnificados, ContextoBdGravacao contextoBdGravacao)
        {
            List<TorcamentoCotacaoItemUnificado> lstUnificados = new List<TorcamentoCotacaoItemUnificado>();


            foreach (var item in lstProdutos)
            {

                var unificado = produtosUnificados.Where(x => x.Fabricante == item.Fabricante && x.Produto == item.Produto).FirstOrDefault();
                if (unificado == null) throw new ArgumentException("Falha ao buscar item para atualização!");

                var TorcamentoCotacaoOpcaoItemAtomicos = orcamentoCotacaoOpcaoItemAtomicoBll.PorFiltroComTransacao(new TorcamentoCotacaoOpcaoItemAtomicoFiltro() { IdItemUnificado = item.IdItemUnificado }, contextoBdGravacao);
                if (TorcamentoCotacaoOpcaoItemAtomicos == null) throw new ArgumentException("Falha ao buscar itens para atualização!");

                foreach (var atomico in TorcamentoCotacaoOpcaoItemAtomicos)
                {
                    atomico.Qtde = (short)item.Qtde;
                    var retorno = orcamentoCotacaoOpcaoItemAtomicoBll.AtualizarComTransacao(atomico, contextoBdGravacao);
                }
                lstUnificados.Add(unificado);
            }
            return lstUnificados;
        }

        public void AtualizarProdutoAtomicoCustoFinComTransacao(OrcamentoOpcaoResponseViewModel opcao, List<TorcamentoCotacaoItemUnificado> produtosUnificados,
            List<TorcamentoCotacaoOpcaoPagto> opcaoPagtos, ContextoBdGravacao contextoBdGravacao,
            UsuarioLogin usuarioLogado, OrcamentoResponseViewModel orcamento)
        {
            foreach (var item in opcao.ListaProdutos)
            {
                //precisa cadastrar o custo fin no caso de ter cadastrado um pagamento 
                var unificado = produtosUnificados.Where(x => x.Fabricante == item.Fabricante && x.Produto == item.Produto).FirstOrDefault();
                if (unificado.TorcamentoCotacaoOpcaoItemAtomicos.Count <= 0) throw new ArgumentException("Falha ao buscar item atômico para atualização!");

                var produtosEspecificos = unificado.TorcamentoCotacaoOpcaoItemAtomicos.Select(x => x.Produto).ToList();
                var pDados = produtoGeralBll.BuscarProdutosEspecificos(opcao.Loja, produtosEspecificos).Result;

                foreach (var atomico in unificado.TorcamentoCotacaoOpcaoItemAtomicos)
                {
                    var torcamentoCotacaoOpcaoItemAtomicosFin = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll
                        .PorFiltroComTransacao(new TorcamentoCotacaoOpcaoItemAtomicoCustoFinFiltro() { IdItemAtomico = atomico.Id }, contextoBdGravacao);

                    foreach (var pagto in opcao.FormaPagto)
                    {
                        var pg = torcamentoCotacaoOpcaoItemAtomicosFin.Where(x => x.TorcamentoCotacaoOpcaoPagto.Tipo_parcelamento == pagto.Tipo_parcelamento).FirstOrDefault();
                        if (ValidarDescontoComissaoAlcada(item, usuarioLogado, orcamento, opcao))
                        {
                            var p = pDados.Where(x => x.Produto == atomico.Produto).FirstOrDefault();

                            bool usoAlcada = VerificarUsoDeAlcada(usuarioLogado, orcamento, item, opcao.PercRT);
                            List<int> usuarioAlcadas;
                            int idAlcada = 0;
                            if (usoAlcada)
                            {
                                usuarioAlcadas = new List<int>();
                                if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_1))
                                    usuarioAlcadas.Add(int.Parse(Constantes.COMISSAO_DESCONTO_ALCADA_1));
                                if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_2))
                                    usuarioAlcadas.Add(int.Parse(Constantes.COMISSAO_DESCONTO_ALCADA_2));
                                if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_3))
                                    usuarioAlcadas.Add(int.Parse(Constantes.COMISSAO_DESCONTO_ALCADA_3));

                                idAlcada = usuarioAlcadas.Max(x => x);
                            }


                            if (pg == null)
                            {
                                var opcaoPagtoAvista = opcaoPagtos.Where(x => x.Tipo_parcelamento == pagto.Tipo_parcelamento).FirstOrDefault();
                                pg = new TorcamentoCotacaoOpcaoItemAtomicoCustoFin();
                                pg.IdItemAtomico = atomico.Id;
                                pg.IdOpcaoPagto = opcaoPagtoAvista.Id;
                                pg.DescDado = item.DescDado;
                                pg.PrecoLista = (decimal)p.Preco_lista;
                                pg.PrecoVenda = (decimal)p.Preco_lista * (decimal)(1 - item.DescDado / 100);
                                pg.PrecoNF = (decimal)p.Preco_lista * (decimal)(1 - item.DescDado / 100);
                                pg.CustoFinancFornecCoeficiente = 0;
                                pg.CustoFinancFornecPrecoListaBase = (decimal)p.Preco_lista;
                                pg.StatusDescontoSuperior = usoAlcada ? true : false;
                                pg.DataHoraDescontoSuperior = usoAlcada ? (DateTime?)DateTime.Now : null;
                                pg.IdUsuarioDescontoSuperior = usoAlcada ? (int?)usuarioLogado.Id : null;
                                pg.IdOperacaoAlcadaDescontoSuperior = usoAlcada ? (int?)idAlcada : null;

                                var orc = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.InserirComTransacao(pg, contextoBdGravacao);
                            }
                            else
                            {
                                pg.Id = pg.Id;
                                pg.IdItemAtomico = atomico.Id;
                                pg.IdOpcaoPagto = pagto.Id;
                                pg.DescDado = item.DescDado;
                                pg.PrecoLista = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? (decimal)p.Preco_lista : (decimal)p.Preco_lista * (decimal)item.CustoFinancFornecCoeficiente;
                                pg.PrecoVenda = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? (decimal)p.Preco_lista * (decimal)(1 - item.DescDado / 100) : (decimal)p.Preco_lista * (decimal)item.CustoFinancFornecCoeficiente * (decimal)(1 - item.DescDado / 100);
                                pg.PrecoNF = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? (decimal)p.Preco_lista * (decimal)(1 - item.DescDado / 100) : (decimal)p.Preco_lista * (decimal)item.CustoFinancFornecCoeficiente * (decimal)(1 - item.DescDado / 100);
                                pg.CustoFinancFornecCoeficiente = pagto.Tipo_parcelamento == (int)Constantes.TipoParcela.A_VISTA ? 0 : item.CustoFinancFornecCoeficiente;
                                pg.CustoFinancFornecPrecoListaBase = (decimal)p.Preco_lista;
                                pg.StatusDescontoSuperior = usoAlcada ? true : false;
                                pg.DataHoraDescontoSuperior = usoAlcada ? (DateTime?)DateTime.Now : null;
                                pg.IdUsuarioDescontoSuperior = usoAlcada ? (int?)usuarioLogado.Id : null;
                                pg.IdOperacaoAlcadaDescontoSuperior = usoAlcada ? (int?)idAlcada : null;

                                var orc = orcamentoCotacaoOpcaoItemAtomicoCustoFinBll.AtualizarComTransacao(pg, contextoBdGravacao);
                            }
                        };

                    }
                }
            }

        }

        public bool VerificarUsoDeAlcada(UsuarioLogin usuarioLogado, OrcamentoResponseViewModel orcamento,
            ProdutoOrcamentoOpcaoResponseViewModel produto, float percRT)
        {
            PercMaxDescEComissaoResponseViewModel percMaxPorAlcada = new PercMaxDescEComissaoResponseViewModel();
            if (usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_1) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_2) ||
                usuarioLogado.Permissoes.Contains((string)Constantes.COMISSAO_DESCONTO_ALCADA_3))
            {
                percMaxPorAlcada = _lojaOrcamentoCotacaoBll.BuscarPercMaxPorLojaAlcada(orcamento.Loja, orcamento.ClienteOrcamentoCotacaoDto.Tipo, usuarioLogado.Permissoes);
            }

            var percPadrao = _lojaOrcamentoCotacaoBll.BuscarPercMaxPorLoja(orcamento.Loja);
            if (percPadrao == null) throw new ArgumentException("Ops! Não falha ao validar opcão!");

            var percPadraoPorTipo = orcamento.ClienteOrcamentoCotacaoDto.Tipo == Constantes.ID_PF ?
                percPadrao.PercMaxComissaoEDesconto : percPadrao.PercMaxComissaoEDescontoPJ;

            if (produto.DescDado > percPadraoPorTipo && percMaxPorAlcada.PercMaxComissaoEDesconto == 0)
                throw new ArgumentException("Ops! Não pode execeder o limite máximo de desconto.");

            if (produto.DescDado > 0 || percRT > 0)
            {
                if (produto.DescDado > percPadraoPorTipo) return true;
            }

            return false;
        }

        private bool ValidarDescontoComissaoAlcada(ProdutoOrcamentoOpcaoResponseViewModel produto,
            UsuarioLogin usuarioLogado, OrcamentoResponseViewModel orcamento, OrcamentoOpcaoResponseViewModel opcao)
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
            ProdutoOrcamentoOpcaoResponseViewModel produto, string tipoCliente, bool temAlcada)
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
            OrcamentoResponseViewModel orcamento, OrcamentoOpcaoResponseViewModel opcao)
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

                var produtoResponse = new ProdutoOrcamentoOpcaoResponseViewModel()
                {
                    //ITEM ATOMICO
                    //Id = item.Id,
                    IdItemUnificado = item.Id,
                    Fabricante = item.Fabricante,
                    FabricanteNome = (await produtoGeralBll.ObterListaFabricante()).Where(x => x.Fabricante == item.Fabricante).FirstOrDefault().Nome,
                    Produto = item.Produto,
                    UrlImagem = _produtoCatalogoBll.ObterDadosImagemPorProduto(item.Produto).FirstOrDefault().Caminho,
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
                    TotalItem = itemAtomico.Sum(x => x.Qtde * itemAtomicoCusto.Where(y => y.IdItemAtomico == x.Id && y.CustoFinancFornecCoeficiente > 0).FirstOrDefault().PrecoNF),
                    IdOperacaoAlcadaDescontoSuperior = itemAtomicoCusto.FirstOrDefault().IdOperacaoAlcadaDescontoSuperior
                };

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
