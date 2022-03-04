using AutoMapper;
using InfraBanco.Constantes;
using InfraBanco.Modelos;
using OrcamentoCotacaoBusiness.Models.Request;
using OrcamentoCotacaoBusiness.Models.Response;
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
    public class ProdutoOrcamentoCotacaoBll
    {
        private readonly ProdutoGeralBll produtoGeralBll;
        private readonly IMapper _mapper;

        public ProdutoOrcamentoCotacaoBll(Produto.ProdutoGeralBll produtoGeralBll, IMapper mapper)
        {
            this.produtoGeralBll = produtoGeralBll;
            _mapper = mapper;
        }
     
        public async Task<ProdutoResponseViewModel> ListaProdutosComboApiArclube(ProdutosRequestViewModel produtos)
        {
            Constantes.ContribuinteICMS contribuinteICMSStatus = Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
            Constantes.ProdutorRural produtorRuralStatus = Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;

            var aux = await produtoGeralBll.ListaProdutosComboDados(produtos.Loja, produtos.UF, produtos.TipoCliente, 
                contribuinteICMSStatus, produtorRuralStatus, produtos.TipoParcela, produtos.QtdeParcelas, produtos.DataRefCoeficiente);
            return await GetProdutosCompostosESimples(aux);
        }

        private async Task<ProdutoResponseViewModel> GetProdutosCompostosESimples(Produto.Dados.ProdutoComboDados produtoComboDados)
        {
            if (produtoComboDados == null) return null;

            ProdutoResponseViewModel produtoResponseViewModel = new ProdutoResponseViewModel();
            produtoResponseViewModel.ProdutosCompostos = new List<ProdutoCompostoResponseViewModel>();
            produtoResponseViewModel.ProdutosSimples = new List<ProdutoSimplesResponseViewModel>();

            List<Produto.Dados.ProdutoDados> produtoDadossRemove = new List<Produto.Dados.ProdutoDados>();
            List<ProdutoCompostoResponseViewModel> produtoCompostosRemove = new List<ProdutoCompostoResponseViewModel>();

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
                        //o produto pai não deve estar na lista de compostos
                        produtoCompostosRemove.Add(produtoCompostoResponse);
                        break;
                    }

                    produtoCompostoResponse.Filhos.Add(ProdutoSimplesResponseViewModel.ConverterProdutoDados(filho, filhos.Qtde));
                    produtoDadossRemove.Add(filho);
                }
                produtoResponseViewModel.ProdutosCompostos.Add(produtoCompostoResponse);

                var paiRemove = produtoComboDados.ProdutoDados
                    .Where(x => x.Produto == composto.PaiProduto)
                    .Select(x => x).FirstOrDefault();
                if (paiRemove != null) produtoDadossRemove.Add(paiRemove);
            }

            produtoComboDados.ProdutoDados.RemoveAll(x => produtoDadossRemove.Any(c => x.Produto == c.Produto));
            produtoResponseViewModel.ProdutosCompostos.RemoveAll(x => produtoCompostosRemove.Any(c => x.PaiProduto == c.PaiProduto));

            foreach (var produto in produtoComboDados.ProdutoDados)
            {
                produtoResponseViewModel.ProdutosSimples.Add(ProdutoSimplesResponseViewModel.ConverterProdutoDados(produto, null));
            }

            return await Task.FromResult(produtoResponseViewModel);
        }
    }
}
