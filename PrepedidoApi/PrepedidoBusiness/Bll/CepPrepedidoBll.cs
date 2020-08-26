using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrepedidoBusiness.Bll
{
    public class CepPrepedidoBll
    {
        private readonly CepBll cepBll;

        public CepPrepedidoBll(PrepedidoBusiness.Bll.CepBll cepBll)
        {
            this.cepBll = cepBll;
        }
        public async Task<IEnumerable<Dto.Cep.CepDto>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            IEnumerable<Cep.Dados.CepDados> lista = await cepBll.BuscarCep(cep, endereco, uf, cidade);
            var ret = Dto.Cep.CepDto.CepDtoListaDeCepDados(lista);
            return ret;
        }
        public async Task<IEnumerable<Dto.Cep.CepDto>> BuscarCepPorEndereco(string endereco, string cidade, string uf)
        {
            IEnumerable<Cep.Dados.CepDados> lista = await cepBll.BuscarCepPorEndereco(endereco, cidade, uf);
            var ret = Dto.Cep.CepDto.CepDtoListaDeCepDados(lista);
            return ret;
        }
    }
}
