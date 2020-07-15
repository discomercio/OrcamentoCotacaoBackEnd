using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static Testes.Automatizados.InicializarBanco.InicializarBancoGeral;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class CamposSoltos : TestesPrepedidoUnisBll
    {
        public CamposSoltos(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll) :
            base(inicializarBanco, Output, prepedidoUnisBll, clienteUnisBll)
        {

        }

        /*
         * vamos testar os campos soltos:
Cnpj_Cpf*	string 
Indicador_Orcamentista*	string 
PermiteRAStatus	boolean
ValorTotalDestePedidoComRA	number($double)
VlTotalDestePedido	number($double)
*/
        [Fact]
        public void Cnpj_Cpf()
        {
            Teste(c => c.Cnpj_Cpf = "", "Cliente não localizado");
            Teste(c => c.Cnpj_Cpf = "123", "Cliente não localizado");
            Teste(c => c.Cnpj_Cpf = "12349535078", "Cliente não localizado");
            Teste(c => c.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().Cnpj_Cpf, "Cliente não localizado", false);
        }


        [Fact]
        public void ValorTotalDestePedidoComRA()
        {
            Teste(c => c.ValorTotalDestePedidoComRA = 1, "ainda fazendo");
            Teste(c => c.VlTotalDestePedido = 1, "ainda fazendo");
        }
        [Fact]
        public void Indicador_Orcamentista()
        {
            Teste(c => c.Indicador_Orcamentista = "um que nao existe", "O Orçamentista não existe!");
            Teste(c => c.Indicador_Orcamentista = "", "O Orçamentista não existe!");

            Teste(c => c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_com_ra, "Permite RA status divergente do cadastro do indicador/orçamentista!", false);
            Teste(c => c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_sem_ra, "Permite RA status divergente do cadastro do indicador/orçamentista!");

            Teste(c =>
            {
                c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_com_ra;
                c.PermiteRAStatus = false;
            }, "Permite RA status divergente do cadastro do indicador/orçamentista!");
            Teste(c =>
            {
                c.Indicador_Orcamentista = Dados.Orcamentista.Apelido_sem_ra;
                c.PermiteRAStatus = true;
            }
            , "Permite RA status divergente do cadastro do indicador/orçamentista!");
        }

        /*
         * todo:
         * 
        EnderecoCadastralCliente	EnderecoCadastralClientePrepedidoUnisDto{...}
        OutroEndereco*	boolean
        EnderecoEntrega	EnderecoEntregaClienteCadastroUnisDto{...}
        ListaProdutos	[...]
        PermiteRAStatus	boolean
        ValorTotalDestePedidoComRA	number($double)
        VlTotalDestePedido	number($double)
        DetalhesPrepedido	DetalhesPrePedidoUnisDto{...}
        FormaPagtoCriacao	FormaPagtoCriacaoUnisDto{...}
        */


    }
}
