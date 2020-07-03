using InfraBanco;
using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.Cep;
using PrepedidoBusiness.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Testes.Automatizados.InicializarBanco;

namespace Testes.Automatizados.Utils
{
    class TestesBancoNFeMunicipio : PrepedidoBusiness.Utils.IBancoNFeMunicipio
    {
        Task<IEnumerable<UFeMunicipiosDto>> IBancoNFeMunicipio.BuscarSiglaTodosUf(ContextoBdProvider contextoProvider)
        {
            //nao fazemos nada...
            var ret = new List<UFeMunicipiosDto>();

            ret.Add(new UFeMunicipiosDto()
            {
                SiglaUF = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Uf,
                ListaMunicipio = new List<MunicipioDto>() { new MunicipioDto() {
                    Descricao = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cidade,
                    DescricaoSemAcento=InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cidade
                } }
            });
            return Task.FromResult(ret.AsEnumerable());
        }

        Task<IEnumerable<NfeMunicipio>> IBancoNFeMunicipio.BuscarSiglaUf(string uf, string municipio, bool buscaParcial, ContextoBdProvider contextoProvider)
        {
            //nao fazemos nada...
            var ret = new List<NfeMunicipio>();
            if (municipio == InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cidade)
                ret.Add(new NfeMunicipio() { Descricao = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Cidade });
            return Task.FromResult(ret.AsEnumerable());
        }

        Task<string> IBancoNFeMunicipio.MontarProviderStringParaNFeMunicipio(ContextoBdProvider contextoProvider)
        {
            var ret = "vazia";
            return Task.FromResult(ret);
        }
    }
}
