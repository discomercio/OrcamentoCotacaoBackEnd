﻿using InfraBanco.Constantes;
using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Testes.Automatizados.InicializarBanco;
using Xunit;
using Xunit.Abstractions;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll
{
    public class TestesClienteUnisBll
    {
        private readonly ClienteUnisBll clienteUnisBll;
        private readonly ITestOutputHelper output;
        private readonly InicializarBanco.InicializarBancoGeral inicializarBanco;

        public TestesClienteUnisBll(ClienteUnisBll clienteUnisBll, ITestOutputHelper output, InicializarBanco.InicializarBancoGeral inicializarBanco)
        {
            this.clienteUnisBll = clienteUnisBll;
            this.output = output;
            this.inicializarBanco = inicializarBanco;
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

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }

        [Fact]
        public void CadastrarClienteUnis_IE_ICMS()
        {
            //if (qtdeDig < 2 && qtdeDig > 14)
            TestarCadastro(c => c.DadosCliente.Ie = "1",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual);
            TestarCadastro(c => c.DadosCliente.Ie = "1",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                tipoPessoa: TipoPessoa.PF);

            TestarCadastro(c => c.DadosCliente.Ie = "11223344",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual);
            TestarCadastro(c => c.DadosCliente.Ie = "11223344",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                tipoPessoa: TipoPessoa.PF);

            //agora validado, não pode ter o erro
            TestarCadastro(c => c.DadosCliente.Ie = c.DadosCliente.Ie,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                false);
        }


        [Fact]
        public void CadastrarClienteUnis_Cidade_Nfe()
        {
            TestarCadastro(c => c.DadosCliente.Cidade = "Abacate da Pedreira",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Municipio_nao_consta_na_relacao_IBGE("Abacate da Pedreira", InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Uf));
        }

        [Fact]
        public void CadastrarClienteUnis_Orcamentista()
        {
            TestarCadastro(c => c.DadosCliente.Indicador_Orcamentista = InicializarBancoGeral.Dados.Orcamentista.ApelidoNaoExiste,
                PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll.MensagensErro.Orcamentista_nao_existe);
        }

        [Fact]
        public void CadastrarClienteUnis_Cep()
        {
            TestarCadastro(c => c.DadosCliente.Cep = InicializarBancoCep.DadosCep.CepNaoExiste,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Cep_nao_existe);
        }
        [Fact]
        public void CadastrarClienteUnis_Estado()
        {
            TestarCadastro(c => c.DadosCliente.Uf = InicializarBancoCep.DadosCep.Ufe_sgNaoExiste,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Estado_nao_confere);
        }


        private delegate void DeixarDtoErrado(ClienteCadastroUnisDto clienteDto);
        private enum TipoPessoa { PF, PJ };
        private void TestarCadastro(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, TipoPessoa tipoPessoa = TipoPessoa.PJ)
        {
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            if (tipoPessoa == TipoPessoa.PF)
                clienteDto = InicializarClienteDados.ClienteNaoCadastradoPF();
            deixarDtoErrado(clienteDto);

            ClienteCadastroResultadoUnisDto res;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto).Result;

            //sempre apaga o regsitro
            inicializarBanco.TclientesApagar();

            if (incluirEsteErro)
            {
                if (!res.ListaErros.Contains(mensagemErro))
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Contains(mensagemErro, res.ListaErros);
            }
            else
            {
                if (res.ListaErros.Contains(mensagemErro))
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.DoesNotContain(mensagemErro, res.ListaErros);
            }

        }
    }
}
