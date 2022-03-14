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
            var dicCoeficiente = await coeficienteBll.BuscarListaCoeficientesFabricantesHistoricoDistinct(lstFabricate, tipoParcela, qtdeParcelas, dataRefCoeficiente.GetValueOrDefault(new DateTime()));

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

                    produtoCompostoResponse.Filhos.Add(ProdutoCompostoFilhosResponseViewModel.ConverterProdutoFilhoDados(filho, filhos.Qtde, GetCoeficienteOuNull(dicCoeficiente, composto.PaiFabricante)));
                }
                var coeficiente = GetCoeficienteOuNull(dicCoeficiente, composto.PaiFabricante).Coeficiente;
                produtoCompostoResponse.PaiPrecoTotalBase = composto.PaiPrecoTotal;
                produtoCompostoResponse.PaiPrecoTotal = produtoCompostoResponse.PaiPrecoTotal * Convert.ToDecimal(coeficiente);

                produtoResponseViewModel.ProdutosCompostos.Add(produtoCompostoResponse);

            }

            foreach (var produto in produtoComboDados.ProdutoDados)
            {
                produtoResponseViewModel.ProdutosSimples.Add(ProdutoSimplesResponseViewModel.ConverterProdutoDados(produto, null, GetCoeficienteOuNull(dicCoeficiente, produto.Fabricante)));
                
            }

            return produtoResponseViewModel;
        }

        private Produto.Dados.CoeficienteDados GetCoeficienteOuNull(IDictionary<string, Produto.Dados.CoeficienteDados> dicCoeficiente, string fabricante)
        {
            if (dicCoeficiente.ContainsKey(fabricante))
            {
                return dicCoeficiente[fabricante];
            }

            return null;
        }
    }
}
