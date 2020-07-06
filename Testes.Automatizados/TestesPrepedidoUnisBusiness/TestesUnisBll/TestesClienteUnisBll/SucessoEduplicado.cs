﻿using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Testes.Automatizados.InicializarBanco;
using Xunit;
using Xunit.Abstractions;
using static Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll.TestesClienteUnisBll;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class SucessoEduplicado
    {
        private readonly ClienteUnisBll clienteUnisBll;
        private readonly ITestOutputHelper output;
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly TestesClienteUnisBll testesClienteUnisBll;

        public SucessoEduplicado(ClienteUnisBll clienteUnisBll, ITestOutputHelper output, InicializarBanco.InicializarBancoGeral inicializarBanco,
            TestesClienteUnisBll testesClienteUnisBll)
        {
            this.clienteUnisBll = clienteUnisBll;
            this.output = output;
            this.inicializarBanco = inicializarBanco;
            this.testesClienteUnisBll = testesClienteUnisBll;
            testesClienteUnisBll.Output = output;
        }


        [Fact]
        public void CadastrarClienteUnis_Sucesso_PJ()
        {
            //este é o que deve dar certo
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            ClienteCadastroResultadoUnisDto res;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //se cadastrar de novo, tem que dar erro
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Cnpj_Cpf = c.DadosCliente.Cnpj_Cpf,
                PrepedidoBusiness.Bll.ClienteBll.ClienteBll.MensagensErro.REGISTRO_COM_ID_JA_EXISTE(""),
                TipoPessoa.PJ);

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }

        [Fact]
        public void CadastrarClienteUnis_Sucesso_PF()
        {
            //este é o que deve dar certo
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPF();
            ClienteCadastroResultadoUnisDto res;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //se cadastrar de novo, tem que dar erro
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Cnpj_Cpf = c.DadosCliente.Cnpj_Cpf,
                PrepedidoBusiness.Bll.ClienteBll.ClienteBll.MensagensErro.REGISTRO_COM_ID_JA_EXISTE(""),
                TipoPessoa.PF);

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }
    }
}
