using PrepedidoBusiness.Dto.Prepedido.DetalhesPrepedido;
using PrepedidoBusiness.Dto.Produto;
using Produto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class CoeficientePrepedidoBll
    {
        private readonly CoeficienteBll coeficienteBll;

        public CoeficientePrepedidoBll(CoeficienteBll coeficienteBll)
        {
            this.coeficienteBll = coeficienteBll;
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
