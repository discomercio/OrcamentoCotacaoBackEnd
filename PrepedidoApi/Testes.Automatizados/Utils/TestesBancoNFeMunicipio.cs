﻿using InfraBanco;
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
        public static string Cidade_somente_no_IBGE = "Cidade somente no IBGE";

        public Task<IEnumerable<UFeMunicipiosDto>> BuscarSiglaTodosUf(ContextoBdProvider contextoProvider, string uf, string municipioParcial)
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
            ret.Add(new UFeMunicipiosDto()
            {
                SiglaUF = InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Uf,
                ListaMunicipio = new List<MunicipioDto>() { new MunicipioDto() {
                    Descricao = Cidade_somente_no_IBGE,
                    DescricaoSemAcento=Cidade_somente_no_IBGE
                } }
            });
            //para o prepedido
            ret.Add(new UFeMunicipiosDto()
            {
                SiglaUF = "SP",
                ListaMunicipio = new List<MunicipioDto>() { new MunicipioDto() {
                    Descricao = "São Paulo",
                    DescricaoSemAcento="São Paulo"
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
            if (municipio == Cidade_somente_no_IBGE)
                ret.Add(new NfeMunicipio() { Descricao = Cidade_somente_no_IBGE });

            //para o prepedido
            if (municipio == "São Paulo")
                ret.Add(new NfeMunicipio() { Descricao = "São Paulo" });

            return Task.FromResult(ret.AsEnumerable());
        }

        Task<string> IBancoNFeMunicipio.MontarProviderStringParaNFeMunicipio(ContextoBdProvider contextoProvider)
        {
            var ret = "vazia";
            return Task.FromResult(ret);
        }
    }
}