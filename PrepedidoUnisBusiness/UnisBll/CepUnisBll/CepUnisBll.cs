using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Dto.Cep;
using PrepedidoBusiness.Utils;
using PrepedidoUnisBusiness.UnisDto.CepUnisDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoUnisBusiness.UnisBll.CepUnisBll
{
    public class CepUnisBll
    {
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly CepBll cepArclueBll;

        public CepUnisBll(CepBll cepArclueBll, InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.cepArclueBll = cepArclueBll;
            this.contextoProvider = contextoProvider;
        }

        public async Task<CepUnisDto> BuscarCep(string cep)
        {
            List<CepDto> lstCepArclubeDto = (await cepArclueBll.BuscarCep(cep, "", "", "")).ToList();

            List<CepUnisDto> lst_cepUnisDto = new List<CepUnisDto>();

            if (lstCepArclubeDto.Count > 0)
            {
                var x = lstCepArclubeDto[0];
                CepUnisDto cepUnis = CepUnisDto.CepUnisDtoDeCepDto(lstCepArclubeDto[0]);

                return cepUnis;
            }

            return null;
        }

        public async Task<IEnumerable<UFeMunicipiosUnisDto>> BuscarUfs()
        {            
            //vamos buscar todos os estados
            List<UFeMunicipiosDto> lstUF_Municipio = (await Util.BuscarSiglaTodosUf(contextoProvider)).ToList();

            List<UFeMunicipiosUnisDto> lstUnisUF_Municipio =
                UFeMunicipiosUnisDto.UFeMunicipiosUnisDtoDeUFeMunicipiosDto(lstUF_Municipio);

            return lstUnisUF_Municipio;
        }
    }
}
