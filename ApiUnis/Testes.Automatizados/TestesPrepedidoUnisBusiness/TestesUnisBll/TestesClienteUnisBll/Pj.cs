using InfraBanco;
using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System;
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
        public void Telefones_sem_separadores()
        {
            inicializarBanco.TclientesApagar();

            //tem que rejeitar pq está pequeno
            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "12";
                c.DadosCliente.TelComercial = "12-------";
            },
                "TELEFONE COMERCIAL INVÁLIDO.",
                    TipoPessoa.PJ);


            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "12";
                c.DadosCliente.TelComercial = "12-.+=,<>";
            },
                "TELEFONE COMERCIAL INVÁLIDO.",
                    TipoPessoa.PJ);



            //agora tem que cadastrar tirando o que não for digito
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            ClienteCadastroResultadoUnisDto res;
            clienteDto.DadosCliente.DddComercial = "12";
            clienteDto.DadosCliente.TelComercial = "1234-56789";
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto, clienteDto.DadosCliente.Indicador_Orcamentista).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //verifica se salvou direito
            var db = contextoProvider.GetContextoLeitura();

            var ret = (from c in db.Tclientes
                       where c.Id == res.IdClienteCadastrado
                       select c).FirstOrDefault();

            Assert.Equal("123456789", ret.Tel_Com);

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }


        [Fact]
        public void TesteClientePJ()
        {
            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.Rg = "123456";
            }, "Se cliente é tipo PJ, o RG não deve ser preenchido.", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.Nascimento = new DateTime(1984, 06, 19);
            }, "Se cliente é tipo PJ, o Nascimento não deve ser preenchido.", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.Nome = "";
            }, "PREENCHA A RAZÃO SOCIAL DO CLIENTE.", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.Cnpj_Cpf = "";
            }, "CNPJ NÃO FORNECIDO.", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.Contato = "";
            }, "INFORME O NOME DA PESSOA PARA CONTATO!", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.Email = "";
            }, "É OBRIGATÓRIO INFORMAR UM ENDEREÇO DE E-MAIL!", TipoPessoa.PJ);
        }

        [Fact]
        public void Telefones_incompletos()
        {
            inicializarBanco.TclientesApagar();


            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "12";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "";
            },
                //"PREENCHA O TELEFONE COMERCIAL.",
                "PREENCHA AO MENOS UM TELEFONE (COMERCIAL OU COMERCIAL 2)!",
                    TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "12";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "";
                c.DadosCliente.DddComercial2 = "12";
                c.DadosCliente.TelComercial2 = "123456";
                c.DadosCliente.Ramal2 = "12";
            },
                "PREENCHA O TELEFONE COMERCIAL.",
                    TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "12";
            },
                "PREENCHA AO MENOS UM TELEFONE (COMERCIAL OU COMERCIAL 2)!",
                    TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "";
                c.DadosCliente.DddComercial2 = "";
                c.DadosCliente.TelComercial2 = "";
                c.DadosCliente.Ramal2 = "";
            },
                "PREENCHA AO MENOS UM TELEFONE (COMERCIAL OU COMERCIAL 2)!",
                    TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial2 = "";
                c.DadosCliente.TelComercial2 = "";
                c.DadosCliente.Ramal2 = "12";
            },
                "Ramal comercial 2 preenchido sem telefone!",
                    TipoPessoa.PJ);
            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "12";
            },
                //"Ramal comercial preenchido sem telefone!",
                "PREENCHA AO MENOS UM TELEFONE (COMERCIAL OU COMERCIAL 2)!",
                    TipoPessoa.PJ);
            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial2 = "12";
                c.DadosCliente.TelComercial2 = "12345678";
                c.DadosCliente.DddComercial = "";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "12";
            },
                "Ramal comercial preenchido sem telefone!",
                    TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.TelComercial2 = "12345";
            }, "TELEFONE COMERCIAL2 INVÁLIDO.", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial2 = "1";
                c.DadosCliente.TelComercial2 = "123456";
            }, "DDD DO TELEFONE COMERCIAL2 INVÁLIDO.", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial2 = "";
                c.DadosCliente.TelComercial2 = "123456";
            }, "PREENCHA O DDD DO TELEFONE COMERCIAL2.", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial2 = "11";
                c.DadosCliente.TelComercial2 = "";
            }, "PREENCHA O TELEFONE COMERCIAL2.", TipoPessoa.PJ);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial2 = "1";
            }, "DDD DO TELEFONE COMERCIAL2 INVÁLIDO.", TipoPessoa.PJ);
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
