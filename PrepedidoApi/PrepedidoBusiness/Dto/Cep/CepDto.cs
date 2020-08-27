using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrepedidoBusiness.Dto.Cep
{
    public class CepDto
    {
        public string Cep { get; set; }
        public string Uf { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string LogradouroComplemento { get; set; }
        public List<string> ListaCidadeIBGE { get; set; }

        public static CepDto CepDtoDeCepDados(global::Cep.Dados.CepDados cepDados)
        {
            if (cepDados == null) return null;
            return new CepDto()
            {
                Cep = cepDados.Cep,
                Uf = cepDados.Uf,
                Cidade = cepDados.Cidade,
                Bairro = cepDados.Bairro,
                Endereco = cepDados.Endereco,
                Numero = cepDados.Numero,
                Complemento = cepDados.Complemento,
                LogradouroComplemento = cepDados.LogradouroComplemento,
                ListaCidadeIBGE = cepDados.ListaCidadeIBGE.ToList()
            };
        }

        public static List<CepDto> CepDtoListaDeCepDados(IEnumerable<global::Cep.Dados.CepDados> cepDados)
        {
            if (cepDados == null) return null;
            var ret = new List<CepDto>();
            if (cepDados != null)
                foreach (var p in cepDados)
                    ret.Add(CepDtoDeCepDados(p));
            return ret;
        }


    }
}
