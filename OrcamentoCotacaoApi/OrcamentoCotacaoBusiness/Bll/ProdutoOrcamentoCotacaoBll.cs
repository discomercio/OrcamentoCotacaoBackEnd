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
     
        public async Task<IEnumerable<ProdutoSimplesResponseViewModel>> ListaProdutosComboApiArclube(ProdutosRequestViewModel produtos)
        {
            Constantes.ContribuinteICMS contribuinteICMSStatus = Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
            Constantes.ProdutorRural produtorRuralStatus = Constantes.ProdutorRural.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;

            var aux = await produtoGeralBll.ListaProdutosComboDados(produtos.Loja, produtos.UF, produtos.TipoCliente, 
                contribuinteICMSStatus, produtorRuralStatus);
            return await CalcularCoeficiente(aux, produtos.TipoParcela, produtos.QtdeParcelas, produtos.DataRefCoeficiente);
        }

        private async Task<IEnumerable<ProdutoSimplesResponseViewModel>> CalcularCoeficiente(Produto.Dados.ProdutoComboDados produtoComboDados, 
            string tipoParcela, short qtdeParcelas, DateTime? dataRefCoeficiente)
        {
            if (produtoComboDados == null) return null;

            var produtosSimples = new List<ProdutoSimplesResponseViewModel>();

    


            var lstFabricate = produtoComboDados.ProdutoDados.Select(x => x.Fabricante).Distinct().ToList();
            var dicCoeficiente = await coeficienteBll.BuscarListaCoeficientesFabricantesHistoricoDistinct(lstFabricate, tipoParcela, qtdeParcelas, dataRefCoeficiente.GetValueOrDefault(new DateTime()));
            
            foreach (var produto in produtoComboDados.ProdutoDados)
            {
                if(dicCoeficiente.ContainsKey(produto.Fabricante))
                {
                    produtosSimples.Add(ProdutoSimplesResponseViewModel.ConverterProdutoDados(produto, null, dicCoeficiente[produto.Fabricante]));
                }
                else
                {
                    produtosSimples.Add(ProdutoSimplesResponseViewModel.ConverterProdutoDados(produto, null, null));

                }
            }

            return await Task.FromResult(produtosSimples);
        }
    }
}
