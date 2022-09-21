using Prepedido;
using Prepedido.Bll;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class TesteEnderecoCadastralCliente : TestesPrepedidoUnisBll
    {
        public TesteEnderecoCadastralCliente(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll, PrepedidoBll prepedidoBll, Cliente.ClienteBll clienteBll) :
            base(inicializarBanco, Output, prepedidoUnisBll, clienteUnisBll, prepedidoBll, clienteBll)
        {

        }

        //testar o         EnderecoCadastralCliente	EnderecoCadastralClientePrepedidoUnisDto{...}
        [Fact]
        public void Endereco_logradouro()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_logradouro = "nao existe", "Endereço não confere!", false);
            string endereco = "Pneumoultramicroscopicossilicovulcanoconióticoparticularidade";
            Teste(c => c.EnderecoCadastralCliente.Endereco_logradouro = endereco,
                "ENDEREÇO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    endereco.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    "60 CARACTERES", true);
        }
        [Fact]
        public void Endereco_bairro()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_bairro = "nao existe", "Bairro não confere!", false);
            string bairro = "Pneumoultramicroscopicossilicovulcanoconióticoparticularidade";
            Teste(c => c.EnderecoCadastralCliente.Endereco_bairro = bairro,
                "BAIRRO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    bairro.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    "60 CARACTERES", true);
        }
        [Fact]
        public void Endereco_cidade()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_cidade = "nao existe", "Cidade não confere");
            string cidade = "Pneumoultramicroscopicossilicovulcanoconióticoparticularidade";
            Teste(c => c.EnderecoCadastralCliente.Endereco_cidade = cidade,
                "CIDADE EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    cidade.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    "60 CARACTERES", true);
        }
        [Fact]
        public void Endereco_uf()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_uf = "XX", "UF INVÁLIDA.");
        }
        [Fact]
        public void Endereco_cep()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_cep = "nao existe", "INFORME O CEP.");
            Teste(c => c.EnderecoCadastralCliente.Endereco_cep = "12nao existe", "Cep não existe!");
        }

        [Fact]
        public void Endereco_numero()
        {
            string numero = "0123456789012345678901234567890123456789012345678901234567890";
            Teste(c => c.EnderecoCadastralCliente.Endereco_numero = numero,
                "NÚMERO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    numero.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    "60 CARACTERES", true);
        }

        [Fact]
        public void Endereco_complemento()
        {
            string complemento = "Pneumoultramicroscopicossilicovulcanoconióticoparticularidade";
            Teste(c => c.EnderecoCadastralCliente.Endereco_complemento = complemento,
                "COMPLEMENTO EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " +
                    complemento.Length + " CARACTERES<br>TAMANHO MÁXIMO: " +
                    "60 CARACTERES", true);
        }
        // Foi solicitado pelo Hamilton que removesse a confrontação de nome do cliente para ApiUnis.
        // Impossibilitava que para cliente tipo PF não poderia ter o nome diferente do cadastro.
        // Para flexibilizar estamos alterando a validação e iremos salvar para o prepedido
        // o nome que vier na solicitação de cadastro de prepedido.Caso ocorra alteração no cadastro do cliente
        // isso impediria de realizar o cadastro de prepedido e acarretaria que, alguém deveria ajustar o cadastro do
        // cliente pelo ERP para que a ApiUnis pudesse cadastrar um prepedido com o cadastro do cliente alterado
        //[Fact]
        //public void TesteConfrontarNomeClientePF()
        //{
        //    Teste(c => c.EnderecoCadastralCliente.Endereco_nome = "Gabriel Teste",
        //        "Nome do cliente diferente do nome cadastrado!");
        //}


    }
}
