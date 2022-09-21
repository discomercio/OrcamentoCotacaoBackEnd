using Prepedido.Dto;
using System.Collections.Generic;

namespace PrepedidoUnisBusiness.UnisDto.CoeficienteUnisDto
{
    public class CoeficienteUnisDto : CoeficienteDto
    {
        public static List<List<CoeficienteUnisDto>> CoeficienteUnisDtoDeCoeficienteDto(List<List<CoeficienteDto>> lstCoeficienteDto)
        {
            List<List<CoeficienteUnisDto>> lstRetorno = new List<List<CoeficienteUnisDto>>();
            List<CoeficienteUnisDto> lstCoeficiente = new List<CoeficienteUnisDto>();

            lstCoeficienteDto.ForEach(x =>
            {
                lstCoeficiente = new List<CoeficienteUnisDto>();
                x.ForEach(y =>
                {
                    lstCoeficiente.Add(new CoeficienteUnisDto()
                    {
                        Fabricante = y.Fabricante,
                        TipoParcela = y.TipoParcela,
                        QtdeParcelas = y.QtdeParcelas,
                        Coeficiente = y.Coeficiente
                    });
                });
                lstRetorno.Add(lstCoeficiente);
            });

            return lstRetorno;
        }
    }
}
