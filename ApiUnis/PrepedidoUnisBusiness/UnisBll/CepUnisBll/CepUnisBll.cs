using PrepedidoBusiness.Bll;
using PrepedidoBusiness.Dto.Cep;
using PrepedidoBusiness.UtilsNfe;
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
        private readonly IBancoNFeMunicipio bancoNFeMunicipio;
        private readonly CepBll cepArclueBll;

        public CepUnisBll(CepBll cepArclueBll, InfraBanco.ContextoBdProvider contextoProvider, IBancoNFeMunicipio bancoNFeMunicipio)
        {
            this.cepArclueBll = cepArclueBll;
            this.contextoProvider = contextoProvider;
            this.bancoNFeMunicipio = bancoNFeMunicipio;
        }

        public async Task<CepUnisDto> BuscarCep(string cep)
        {
            List<CepDados> lstCepArclubeDto = (await cepArclueBll.BuscarCep(cep, "", "", "")).ToList();

            List<CepUnisDto> lst_cepUnisDto = new List<CepUnisDto>();

            if (lstCepArclubeDto.Count > 0)
            {
                CepUnisDto cepUnis = CepUnisDto.CepUnisDtoDeCepDados(lstCepArclubeDto[0]);
                return cepUnis;
            }

            return null;
        }

        public async Task<IEnumerable<UFeMunicipiosUnisDto>> BuscarUfs(string uf, string municipioParcial)
        {            
            //vamos buscar todos os estados
            List<UFeMunicipiosDados> lstUF_Municipio = (await bancoNFeMunicipio.BuscarSiglaTodosUf(contextoProvider, uf, municipioParcial)).ToList();

            List<UFeMunicipiosUnisDto> lstUnisUF_Municipio =
                UFeMunicipiosUnisDto.UFeMunicipiosUnisDtoDeUFeMunicipiosDados(lstUF_Municipio);

            return lstUnisUF_Municipio;
        }
    }
}
