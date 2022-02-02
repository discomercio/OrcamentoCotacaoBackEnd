using InfraBanco.Constantes;
using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using Produto;
using Produto.RegrasCrtlEstoque;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class ProdutoPrepedidoBll
    {
        private readonly ProdutoGeralBll produtoGeralBll;

        public ProdutoPrepedidoBll(Produto.ProdutoGeralBll produtoGeralBll)
        {
            this.produtoGeralBll = produtoGeralBll;
        }
        public async Task<Dto.Produto.ProdutoComboDto> ListaProdutosComboApiArclube(string loja, string uf, string tipo)
        {
            Constantes.ContribuinteICMS contribuinteICMSStatus = Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
            Constantes.ProdutorRural produtorRuralStatus = Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;
            
            var aux = await produtoGeralBll.ListaProdutosComboDados(loja, uf, tipo, contribuinteICMSStatus, produtorRuralStatus);
            //tratar os produtos para remover os produtos da lista de produtos simples no caso de existir no composto
            aux = await GetProdutosCompostosESimples(loja, aux);

            return Dto.Produto.ProdutoComboDto.ProdutoComboDto_De_ProdutoComboDados(aux);
        }

        private async Task<Produto.Dados.ProdutoComboDados> GetProdutosCompostosESimples(string loja, 
            Produto.Dados.ProdutoComboDados produtoComboDados)
        {
            if (produtoComboDados != null)
            {
                var produtosGroup = produtoComboDados.ProdutoCompostoDados.GroupBy(x => x.PaiProduto).Select(x => x);

                foreach (var agrupados in produtosGroup)
                {
                    //var filhos = agrupados.Select(x => x).ToList();

                    //produtoCompostoResponse = _mapper.Map<ProdutoCompostoResponseViewModel>(filhos[0]);
                    //produtoCompostoResponse.PaiPrecoTotal = 0;
                    //produtoCompostoResponse.Filhos = new List<ProdutoSimplesResponseViewModel>();
                    //var pai = produtosSimples.FirstOrDefault(x => x.Produto == agrupados.Key);
                    //if (pai != null)
                    //    produtosSimplesCopy.Remove(pai);

                    //foreach (var filho in filhos)
                    //{
                    //    var f = produtosSimples.Where(x => x.Produto == filho.ProdutoFilho).Select(x => x).FirstOrDefault();
                    //    if (f == null)
                    //    {
                    //        produtoCompostoResponse = null;
                    //        break;
                    //    }

                    //    var filhoResponse = _mapper.Map<ProdutoSimplesResponseViewModel>(f);
                    //    var precoListaXCoeficiente = filhoResponse.PrecoLista * (decimal)filhoResponse.CoeficienteDeCalculo;
                    //    produtoCompostoResponse.PaiPrecoTotal += Math.Round(precoListaXCoeficiente, 2);
                    //    produtoCompostoResponse.PaiPrecoTotalAVista += filhoResponse.PrecoLista;//analisar o produto simples
                    //    produtoCompostoResponse.Filhos.Add(filhoResponse);
                    //    produtosSimplesCopy.Remove(f);
                    //}

                    //if (produtoCompostoResponse != null)
                    //    produtosCompostosResponse.Add(produtoCompostoResponse);
                }
            }
            //ProdutoResponseViewModel produtoResponseViewModel = new ProdutoResponseViewModel();
            //produtoResponseViewModel.ProdutosCompostos = produtosCompostosResponse;
            //produtoResponseViewModel.ProdutosSimples = _mapper.Map<List<ProdutoSimplesResponseViewModel>>(produtosSimplesCopy);
            return produtoComboDados;
        }
    }
}
