using PrepedidoBusiness.Dto.Cep;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PrepedidoUnisBusiness.UnisDto.CepUnisDto
{
    public class UFeMunicipiosUnisDto
    {
        [MaxLength(2)]
        public string Codigo { get; set; }

        [MaxLength(2)]
        public string SiglaUF { get; set; }

        [MaxLength(50)]
        public string Descricao { get; set; }

        public List<MunicipioUnisDto> ListaMunicipio { get; set; }

        public static List<UFeMunicipiosUnisDto> UFeMunicipiosUnisDtoDeUFeMunicipiosDados(List<UFeMunicipiosDados> lstUfeMunicipio)
        {
            List<UFeMunicipiosUnisDto> lstRet = new List<UFeMunicipiosUnisDto>();

            lstUfeMunicipio.ForEach(x =>
            {
                UFeMunicipiosUnisDto ufRet = new UFeMunicipiosUnisDto();
                ufRet.Codigo = x.Codigo;
                ufRet.SiglaUF = x.SiglaUF;
                ufRet.Descricao = x.Descricao;
                ufRet.ListaMunicipio = new List<MunicipioUnisDto>();
                x.ListaMunicipio.ForEach(y =>
                {
                    ufRet.ListaMunicipio.Add(new MunicipioUnisDto()
                    {
                        Codigo = y.Codigo,
                        Descricao = y.Descricao,
                        DescricaoSemAcento = y.DescricaoSemAcento
                    });
                });

                lstRet.Add(ufRet);
            });

            return lstRet;
        }
    }
}
