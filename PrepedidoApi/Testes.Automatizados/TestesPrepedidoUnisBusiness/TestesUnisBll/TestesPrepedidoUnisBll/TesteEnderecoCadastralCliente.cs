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
public    class TesteEnderecoCadastralCliente : TestesPrepedidoUnisBll
    {
        public TesteEnderecoCadastralCliente(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll) :
            base(inicializarBanco, Output, prepedidoUnisBll, clienteUnisBll)
        {

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

    }
}
