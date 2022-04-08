using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using OrcamentoCotacaoBusiness.Dto.Produto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coeficiente;
using InfraBanco.Modelos.Filtros;
using OrcamentoCotacaoBusiness.Models.Response;
using OrcamentoCotacaoBusiness.Models.Request;
using Microsoft.EntityFrameworkCore;

namespace OrcamentoCotacaoBusiness.Bll
{
    public class CoeficienteBll
    {
        private readonly Produto.CoeficienteBll coeficienteBll;

        private readonly Coeficiente.CoeficienteBll _coeficienteBll;

        public CoeficienteBll(Produto.CoeficienteBll coeficienteBll, Coeficiente.CoeficienteBll _coeficienteBll)
        {
            this.coeficienteBll = coeficienteBll;
            this._coeficienteBll = _coeficienteBll;
        }

        public async Task<List<CoeficienteResponseViewModel>> BuscarListaCoeficientesFabricantesHistoricoDistinct(CoeficienteRequestViewModel request)
        {
            var response = _coeficienteBll.PorFiltro(new TpercentualCustoFinanceiroFornecedorHistoricoFiltro() 
            {
                LstFabricantes = request.LstFabricantes,
                DataRefCoeficiente = request.DataRefCoeficiente,
                QtdeParcelas = request.QtdeParcelas,
                TipoParcela = request.TipoParcela,
            });

            return await Task.FromResult(CoeficienteResponseViewModel.ListaCoeficienteResponseViewModel_De_ListaTpercentualCustoFinanceiroFornecedorHistorico(response));
        }




        public async Task<IEnumerable<CoeficienteDto>> BuscarListaCoeficientes(List<PrepedidoProdutoDtoPrepedido> produtos)
        {
            List<string> fabricantesDistinct = (from c in produtos
                                                select c.Fabricante).Distinct().ToList();
            IEnumerable<Produto.Dados.CoeficienteDados> ret = await coeficienteBll.BuscarListaCoeficientesFabricantesDistinct(fabricantesDistinct);

            return CoeficienteDto.CoeficienteDtoLista_De_CoeficienteDados(ret);
        }

        public async Task<IEnumerable<IEnumerable<CoeficienteDto>>> BuscarListaCoeficientesFornecedores(List<string> lstFornecedores)
        {
            IEnumerable<IEnumerable<Produto.Dados.CoeficienteDados>> ret = await coeficienteBll.BuscarListaCoeficientesFornecedores(lstFornecedores);
            return CoeficienteDto.CoeficienteDtoListaLista_De_CoeficienteDados(ret);
        }

    }
}
