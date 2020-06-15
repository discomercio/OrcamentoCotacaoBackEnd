using PrepedidoBusiness.Dto.Cep;
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
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;

        public CepUnisBll(InfraBanco.ContextoCepProvider contextoCepProvider)
        {
            this.contextoCepProvider = contextoCepProvider;
        }

        public async Task<IEnumerable<CepUnisDto>> BuscarCep(string cep)
        {
            PrepedidoBusiness.Bll.CepBll cepArcluebBll = new PrepedidoBusiness.Bll.CepBll(contextoCepProvider);

            List<CepDto> lstCepArclubeDto = (await cepArcluebBll.BuscarCep(cep, "", "", "")).ToList();

            List<CepUnisDto> lst_cepUnisDto = new List<CepUnisDto>();

            if (lstCepArclubeDto.Count > 0)
            {
                lstCepArclubeDto.ForEach(x =>
                {
                    CepUnisDto cepUnis = new CepUnisDto();
                    cepUnis.Cep = x.Cep;
                    cepUnis.Uf = x.Uf;
                    cepUnis.Cidade = x.Cidade;
                    cepUnis.Bairro = x.Bairro;
                    cepUnis.Endereco = x.Endereco;
                    cepUnis.LogradouroComplemento = x.LogradouroComplemento;

                    lst_cepUnisDto.Add(cepUnis);
                });
            }

            return lst_cepUnisDto;
        }
    }
}
