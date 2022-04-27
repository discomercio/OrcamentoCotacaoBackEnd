using AutoMapper;
using InfraBanco.Constantes;
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

        public ProdutoOrcamentoCotacaoBll(Produto.ProdutoGeralBll produtoGeralBll, CoeficienteBll coeficienteBll, IMapper mapper)
        {
            this.produtoGeralBll = produtoGeralBll;
            this.coeficienteBll = coeficienteBll;
            _mapper = mapper;
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
            return  produtoGeralBll.GravarPropriedadesProdutos(produtoCatalogoPropriedade);
        }

        public bool AtualizarPropriedadesProdutos(Produto.Dados.ProdutoCatalogoPropriedadeDados produtoCatalogoPropriedade)
        {
            return produtoGeralBll.AtualizarPropriedadesProdutos(produtoCatalogoPropriedade);
        }

    }
}
