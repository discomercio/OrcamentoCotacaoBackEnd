using Cep.Dados;
using InfraBanco;
using InfraBanco.Modelos;
using Prepedido.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Especificacao.Testes.Utils.BancoTestes
{
    class TestesBancoNFeMunicipio : Cep.IBancoNFeMunicipio
    {
        public static string Cidade_somente_no_IBGE = "Cidade somente no IBGE";

        public Task<IEnumerable<UFeMunicipiosDados>> BuscarSiglaTodosUf(ContextoBdProvider contextoProvider, string uf, string municipioParcial)
        {
            //nao fazemos nada...
#pragma warning disable IDE0028 // Simplify collection initialization
            var ret = new List<UFeMunicipiosDados>();
#pragma warning restore IDE0028 // Simplify collection initialization

            //para o prepedido
            ret.Add(new UFeMunicipiosDados()
            {
                SiglaUF = "SP",
                ListaMunicipio = new List<MunicipioDados>() { new MunicipioDados() {
                    Descricao = "São Paulo",
                    DescricaoSemAcento="São Paulo"
                } }
            });
            ret.Add(new UFeMunicipiosDados()
            {
                SiglaUF = "SP",
                ListaMunicipio = new List<MunicipioDados>() { new MunicipioDados() {
                    Descricao = "Santo André",
                    DescricaoSemAcento="Santo André"
                } }
            });
            ret.Add(new UFeMunicipiosDados()
            {
                SiglaUF = "BA",
                ListaMunicipio = new List<MunicipioDados>() { new MunicipioDados() {
                    Descricao = "Salvador",
                    DescricaoSemAcento="Salvador"
                } }
            });

            ret.Add(new UFeMunicipiosDados()
            {
                SiglaUF = "SP",
                ListaMunicipio = new List<MunicipioDados>() { new MunicipioDados() {
                    Descricao = "Uma cidade em SP",
                    DescricaoSemAcento="Uma cidade em SP"
                } }
            });
            ret.Add(new UFeMunicipiosDados()
            {
                SiglaUF = "SP",
                ListaMunicipio = new List<MunicipioDados>() { new MunicipioDados() {
                    Descricao = "Outra cidade em SP",
                    DescricaoSemAcento="Outra cidade em SP"
                } }
            });
            return Task.FromResult(ret.AsEnumerable());
        }

        Task<IEnumerable<NfeMunicipio>> Cep.IBancoNFeMunicipio.BuscarSiglaUf(string uf, string municipio, bool buscaParcial, ContextoBdProvider contextoProvider)
        {
            //nao fazemos nada...
            var ret = new List<NfeMunicipio>();
            if (municipio == Cidade_somente_no_IBGE)
                ret.Add(new NfeMunicipio() { Descricao = Cidade_somente_no_IBGE });

            //cidades que aceitamos sempre
            var listaMunicipios = new List<string>()
            {
                "São Paulo",
                "Salvador",
                "Santo André",
                "Uma cidade em SP",
                "Outra cidade em SP",
            };
            foreach (var este in listaMunicipios)
            {
                if (municipio == este)
                    ret.Add(new NfeMunicipio() { Descricao = este });
            }

            return Task.FromResult(ret.AsEnumerable());
        }

        Task<string> Cep.IBancoNFeMunicipio.MontarProviderStringParaNFeMunicipio(ContextoBdProvider contextoProvider)
        {
            var ret = "vazia";
            return Task.FromResult(ret);
        }
    }
}
