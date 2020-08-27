using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Dto.Produto;
using PrepedidoUnisBusiness.UnisDto.CoeficienteUnisDto;
using Produto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.CoeficienteUnisBll
{
    public class CoeficienteUnisBll
    {
        private readonly CoeficienteBll coeficienteBll;

        public CoeficienteUnisBll(CoeficienteBll coeficienteBll)
        {
            this.coeficienteBll = coeficienteBll;
        }

        public async Task<IEnumerable<IEnumerable<CoeficienteUnisDto>>> BuscarListaCoeficientesFornecedores(List<string> lstFornecedores)
        {
            List<List<CoeficienteUnisDto>> lstRetorno = new List<List<CoeficienteUnisDto>>();

            List<List<CoeficienteDto>> lstCoeficienteDtoArclube = new List<List<CoeficienteDto>>();
            List<CoeficienteDto> coefDtoArclube = new List<CoeficienteDto>();

            var lstCoeficienteDtoArclubeTask = coeficienteBll.BuscarListaCoeficientesFornecedores(lstFornecedores);
            
            if (await lstCoeficienteDtoArclubeTask != null)
            {
                foreach(var i in await lstCoeficienteDtoArclubeTask)
                {
                    coefDtoArclube = new List<CoeficienteDto>();
                    foreach (var y in i)
                    {
                        coefDtoArclube.Add(new CoeficienteDto()
                        {
                            Fabricante = y.Fabricante,
                            TipoParcela = y.TipoParcela,
                            QtdeParcelas = y.QtdeParcelas,
                            Coeficiente = y.Coeficiente
                        });                             
                    }
                    lstCoeficienteDtoArclube.Add(coefDtoArclube);
                }

                lstRetorno = CoeficienteUnisDto.CoeficienteUnisDtoDeCoeficienteDto(lstCoeficienteDtoArclube);
            }

            return lstRetorno;
        }
    }
}
