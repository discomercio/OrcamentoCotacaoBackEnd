using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loja.Bll.Dto.CepDto;
using Loja.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Cep;

namespace Loja.Bll.CepBll
{
    public class CepBll
    {
        private readonly InfraBanco.ContextoCepProvider contextoCepProvider;
        private readonly Cep.CepBll cepBll;

        public CepBll(InfraBanco.ContextoCepProvider contextoCepProvider, Cep.CepBll cepBll)
        {
            this.contextoCepProvider = contextoCepProvider;
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
