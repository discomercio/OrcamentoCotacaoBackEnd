﻿using Cep.Dados;
using InfraBanco;
using InfraBanco.Modelos;
using PrepedidoBusiness.Dto.Cep;
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
            var ret = new List<UFeMunicipiosDados>();

            //para o prepedido
            ret.Add(new UFeMunicipiosDados()
            {
                SiglaUF = "SP",
                ListaMunicipio = new List<MunicipioDados>() { new MunicipioDados() {
                    Descricao = "São Paulo",
                    DescricaoSemAcento="São Paulo"
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

            //para o prepedido
            if (municipio == "São Paulo")
                ret.Add(new NfeMunicipio() { Descricao = "São Paulo" });

            return Task.FromResult(ret.AsEnumerable());
        }

        Task<string> Cep.IBancoNFeMunicipio.MontarProviderStringParaNFeMunicipio(ContextoBdProvider contextoProvider)
        {
            var ret = "vazia";
            return Task.FromResult(ret);
        }
    }
}