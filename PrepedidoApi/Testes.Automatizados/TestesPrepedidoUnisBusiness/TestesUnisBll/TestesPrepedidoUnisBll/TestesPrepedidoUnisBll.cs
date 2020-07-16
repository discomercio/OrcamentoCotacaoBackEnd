using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Testes.Automatizados.InicializarBanco;
using Xunit;
using Xunit.Abstractions;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll
{
    public class TestesPrepedidoUnisBll : IDisposable
    {
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly ITestOutputHelper output;
        private readonly PrePedidoUnisBll prepedidoUnisBll;
        private readonly ClienteUnisBll clienteUnisBll;

        public TestesPrepedidoUnisBll(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll)
        {
            this.inicializarBanco = inicializarBanco;
            output = Output;
            this.prepedidoUnisBll = prepedidoUnisBll;
            this.clienteUnisBll = clienteUnisBll;

            //cadastar o nosso cliente
            var cliente = InicializarClienteDados.ClienteNaoCadastradoPF();
            cliente.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(cliente).Wait();

        }

        internal delegate void DeixarDtoErrado(PrePedidoUnisDto clienteDto);
        internal void Teste(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez();
            deixarDtoErrado(prePedido);

            var res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;


            if (incluirEsteErro)
            {
                if (!res.ListaErros.Contains(mensagemErro))
                    if (output != null)
                        output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Contains(mensagemErro, res.ListaErros);
            }
            else
            {
                if (res.ListaErros.Contains(mensagemErro))
                    if (output != null)
                        output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.DoesNotContain(mensagemErro, res.ListaErros);
            }

        }

        //testa se consegue cadastrar
        internal void TestarSucesso(DeixarDtoErrado deixarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez();
            deixarDtoErrado(prePedido);

            var res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;

            if (res.ListaErros.Count > 0)
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
            Assert.Empty(res.ListaErros);
        }

        public void Dispose()
        {
            inicializarBanco.TclientesApagar();
        }
    }
}
