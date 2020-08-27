using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoBusiness.Bll.ClienteBll;
using PrepedidoBusiness.Bll.PrepedidoBll;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
public    class TesteEnderecoCadastralCliente : TestesPrepedidoUnisBll
    {
        public TesteEnderecoCadastralCliente(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll, PrepedidoBll prepedidoBll, ClienteBll clienteBll) :
            base(inicializarBanco, Output, prepedidoUnisBll, clienteUnisBll, prepedidoBll, clienteBll)
        {

        }

        //testar o         EnderecoCadastralCliente	EnderecoCadastralClientePrepedidoUnisDto{...}
        [Fact]
        public void Endereco_logradouro()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_logradouro = "nao existe", "Endereço não confere!", false);
        }
        [Fact]
        public void Endereco_bairro()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_bairro = "nao existe", "Bairro não confere!", false);
        }
        [Fact]
        public void Endereco_cidade()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_cidade = "nao existe", "Cidade não confere");
        }
        [Fact]
        public void Endereco_uf()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_uf = "XX", "UF INVÁLIDA.");
        }
        [Fact]
        public void Endereco_cep()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_cep = "nao existe", "Cep não existe!");
        }
        //todo: terminar o teste do EnderecoCadastralCliente, mas reaproveitar o código de teste

        
        [Fact]
        public void TesteConfrontarNomeClientePF()
        {
            Teste(c => c.EnderecoCadastralCliente.Endereco_nome = "Gabriel Teste",
                "Nome do cliente diferente do nome cadastrado!");
        }

        
    }
}
