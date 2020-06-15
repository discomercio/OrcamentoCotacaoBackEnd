using PrepedidoUnisBusiness.UnisDto.CoeficienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.CoeficienteUnisBll
{
    public class CoeficienteUnisBll
    {

        public async Task<IEnumerable<IEnumerable<CoeficienteUnisDto>>> BuscarListaCoeficientesFornecedores(List<string> lstFornecedores)
        {
            List<List<CoeficienteUnisDto>> lstRetorno = new List<List<CoeficienteUnisDto>>();

            return lstRetorno;
        }
    }
}
