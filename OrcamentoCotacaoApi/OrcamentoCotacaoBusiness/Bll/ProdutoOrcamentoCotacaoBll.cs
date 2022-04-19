﻿using AutoMapper;
using InfraBanco;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
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

        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos()
        {
            var lstProdutoPropriedades = await produtoGeralBll.ObterListaPropriedadesProdutos();

            return lstProdutoPropriedades;
        }
        public async Task<List<Produto.Dados.ProdutoCatalogoPropriedadeDados>> ObterListaPropriedadesProdutos(int id)
        {
            var lstProdutoPropriedades = await produtoGeralBll.ObterListaPropriedadesProdutos(id);

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
            int idOrcamentoCotacaoOpcao, ContextoBdGravacao contextoBdGravacao)
        {
            List<TorcamentoCotacaoItemUnificado> itensUnificados = new List<TorcamentoCotacaoItemUnificado>();
            int sequencia = 0;

            foreach (var produto in opcao.ListaProdutos)
            {
                var tProduto = produtoGeralBll.BuscarProdutoPorFabricanteECodigoComTransacao(produto.Fabricante,
                    produto.Produto, contextoBdGravacao).Result;

                if (tProduto == null) throw new ArgumentException("Ops! Não achamos o produto!");

                sequencia++;
                TorcamentoCotacaoItemUnificado torcamentoCotacaoItemUnificado = new TorcamentoCotacaoItemUnificado(idOrcamentoCotacaoOpcao,
                    tProduto.Fabricante, tProduto.Produto, produto.Qtde, tProduto.Descricao, tProduto.Descricao_Html, sequencia);

                var produtoUnificado = orcamentoCotacaoOpcaoItemUnificadoBll.InserirComTransacao(torcamentoCotacaoItemUnificado, contextoBdGravacao);

                if (produtoUnificado.Id == 0) throw new ArgumentException("Ops! Não gerou o Id de produto unificado!");

                itensUnificados.Add(produtoUnificado);

                var itensAtomicos = CadastrarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(produtoUnificado.Id,
                    tProduto, produto.Qtde, contextoBdGravacao);

                if (itensAtomicos.Count <= 0) throw new ArgumentException("Ops! Não gerou o Id de produto atomicos!");
            }

            return itensUnificados;
        }

        private List<TorcamentoCotacaoOpcaoItemAtomico> CadastrarTorcamentoCotacaoOpcaoItemAtomicoComTransacao(int idItemUnificado, 
            Tproduto tProduto, int qtde, ContextoBdGravacao contextoBdGravacao)
        {
            List<TorcamentoCotacaoOpcaoItemAtomico> torcamentoCotacaoOpcaoItemAtomicos = new List<TorcamentoCotacaoOpcaoItemAtomico>();
            if (tProduto.TecProdutoComposto != null)
            {
                foreach (var item in tProduto.TecProdutoComposto.TecProdutoCompostoItems)
                {
                    TorcamentoCotacaoOpcaoItemAtomico itemAtomico = new TorcamentoCotacaoOpcaoItemAtomico(idItemUnificado, tProduto.Fabricante, tProduto.Produto,
                    (short)(qtde * tProduto.TecProdutoComposto.TecProdutoCompostoItems.First().Qtde),
                    tProduto.Descricao, tProduto.Descricao_Html);

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
            List<TorcamentoCotacaoItemUnificado> tOrcamentoCotacaoItemUnificados, ContextoBdGravacao contextoBdGravacao)
        {
            foreach(var itemUnificado in tOrcamentoCotacaoItemUnificados)
            {
                //buscar os itens atomicos pelo id do item unificado
                //var itensAtomicos = 
            }

            return new List<TorcamentoCotacaoOpcaoItemAtomicoCustoFin>();
        }
    }
}
