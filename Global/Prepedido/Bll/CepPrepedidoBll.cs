using Cep;
using Prepedido.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prepedido.Bll
{
    public class CepPrepedidoBll
    {
        private readonly CepBll cepBll;

        public CepPrepedidoBll(Cep.CepBll cepBll)
        {
            this.cepBll = cepBll;
        }
        public async Task<IEnumerable<CepDto>> BuscarCep(string cep, string endereco, string uf, string cidade)
        {
            IEnumerable<Cep.Dados.CepDados> lista = await cepBll.BuscarCep(cep, endereco, uf, cidade);
            var ret = CepDto.CepDtoListaDeCepDados(lista);
            return ret;
        }
        public async Task<IEnumerable<CepDto>> BuscarCepPorEndereco(string endereco, string cidade, string uf)
        {
            IEnumerable<Cep.Dados.CepDados> lista = await cepBll.BuscarCepPorEndereco(endereco, cidade, uf);
            var ret = CepDto.CepDtoListaDeCepDados(lista);
            return ret;
        }
    }
}
