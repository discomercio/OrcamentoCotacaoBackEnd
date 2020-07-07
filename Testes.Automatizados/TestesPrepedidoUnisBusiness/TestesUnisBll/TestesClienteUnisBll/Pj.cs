using InfraBanco;
using InfraBanco.Constantes;
using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoBusiness.Bll.ClienteBll;
using System;
using System.Collections.Generic;
using System.Linq;
using Testes.Automatizados.InicializarBanco;
using Xunit;
using Xunit.Abstractions;
using static Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll.TestesClienteUnisBll;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll
{

    [Collection("Testes não multithread porque o banco é unico")]
    public class Pj
    {
        private readonly TestesClienteUnisBll testesClienteUnisBll;
        private readonly ITestOutputHelper output;
        private readonly PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll clienteUnisBll;
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly ContextoBdProvider contextoProvider;

        public Pj(TestesClienteUnisBll testesClienteUnisBll, ITestOutputHelper output,
            PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll clienteUnisBll,
            InicializarBanco.InicializarBancoGeral inicializarBanco,
            InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.testesClienteUnisBll = testesClienteUnisBll;
            this.output = output;
            this.clienteUnisBll = clienteUnisBll;
            this.inicializarBanco = inicializarBanco;
            this.contextoProvider = contextoProvider;
            this.testesClienteUnisBll.Output = output;
        }

        [Fact]
        public void Telefones_nao_aceitos()
        {
            inicializarBanco.TclientesApagar();

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddResidencial = "11";
                c.DadosCliente.TelefoneResidencial = "12345678";
            },
            "Se cliente é tipo PJ, não pode ter os campos de Telefone e DDD residencial preenchidos! ",
                    TipoPessoa.PJ);


            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddCelular = "11";
                c.DadosCliente.Celular = "12345678";
            },
            "Se cliente é tipo PJ, não pode ter os campos de Telefone e DDD celular preenchidos! ",
                    TipoPessoa.PJ);
        }

        [Fact]
        public void RefBancaria_banco_invalido()
        {
            inicializarBanco.TclientesApagar();

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.RefBancaria[0].Banco = "987";
            },
            "Ref Bancária: código do banco inválido",
                    TipoPessoa.PJ);

        }


    }
}
