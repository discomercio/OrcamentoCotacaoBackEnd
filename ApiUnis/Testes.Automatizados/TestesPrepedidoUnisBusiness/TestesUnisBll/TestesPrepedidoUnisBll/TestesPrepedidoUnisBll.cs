using Cliente;
using Newtonsoft.Json;
using Prepedido;
using Prepedido.Bll;
using Prepedido.Dto;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using PrepedidoUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly PrepedidoBll prepedidoBll;
        private readonly ClienteBll clienteBll;

        public TestesPrepedidoUnisBll(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll, PrepedidoBll prepedidoBll, ClienteBll clienteBll)
        {
            this.inicializarBanco = inicializarBanco;
            output = Output;
            this.prepedidoUnisBll = prepedidoUnisBll;
            this.prepedidoBll = prepedidoBll;
            this.clienteBll = clienteBll;

            //cadastar o nosso cliente
            var cliente = InicializarClienteDados.ClienteNaoCadastradoPF();
            cliente.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(cliente, cliente.DadosCliente.Indicador_Orcamentista).Wait();

            var clientePJ = InicializarClienteDados.ClienteNaoCadastradoPJ();
            clientePJ.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParcelaUnica().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(clientePJ, clientePJ.DadosCliente.Indicador_Orcamentista).Wait();
        }

        internal delegate void DeixarDtoErrado(PrePedidoUnisDto clienteDto);
        internal void Teste(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, bool testarPrepedidoBll = true)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez();
            TesteInterno(prePedido, deixarDtoErrado, mensagemErro, incluirEsteErro, testarPrepedidoBll);
        }
        internal void TesteAvista(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, bool testarPrepedidoBll = true)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoAvista();
            TesteInterno(prePedido, deixarDtoErrado, mensagemErro, incluirEsteErro, testarPrepedidoBll);
        }
        internal void TesteEnderecoEntrega(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, bool testarPrepedidoBll = true)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto();
            TesteInterno(prePedido, deixarDtoErrado, mensagemErro, incluirEsteErro, testarPrepedidoBll);
        }

        internal void TesteParcUnica(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, bool testarPrepedidoBll = true)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaUnica();
            TesteInterno(prePedido, deixarDtoErrado, mensagemErro, incluirEsteErro, testarPrepedidoBll);
        }

        internal void TesteParcCartao(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, bool testarPrepedidoBll = true)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaCartao();
            TesteInterno(prePedido, deixarDtoErrado, mensagemErro, incluirEsteErro, testarPrepedidoBll);
        }

        internal void TesteParcCartaoMaquineta(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, bool testarPrepedidoBll = true)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaCartaoMaquineta();
            TesteInterno(prePedido, deixarDtoErrado, mensagemErro, incluirEsteErro, testarPrepedidoBll);
        }

        internal void TestePagamentoComEntrada(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, bool testarPrepedidoBll = true)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoPagtoComEntrada();
            TesteInterno(prePedido, deixarDtoErrado, mensagemErro, incluirEsteErro, testarPrepedidoBll);
        }

        private void TesteInterno(PrePedidoUnisDto prePedido, DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true, bool testarPrepedidoBll = true)
        {
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

            if (testarPrepedidoBll)
                TesteInternoPrepedidoBll(prePedido, mensagemErro, incluirEsteErro);
        }

        private void TesteInternoPrepedidoBll(PrePedidoUnisDto prePedidoUnis, string mensagemErro, bool incluirEsteErro)
        {
            //agora no prepedidoapi

            EnderecoCadastralClientePrepedidoDto endCadastralArclube =
                EnderecoCadastralClientePrepedidoUnisDto.EnderecoCadastralClientePrepedidoDtoDeEnderecoCadastralClientePrepedidoUnisDto(
                    prePedidoUnis.EnderecoCadastralCliente);

            List<PrepedidoProdutoDtoPrepedido> lstProdutosArclube = new List<PrepedidoProdutoDtoPrepedido>();
            prePedidoUnis.ListaProdutos.ForEach(x =>
            {
                var ret = PrePedidoProdutoPrePedidoUnisDto.
                PrepedidoProdutoDtoPrepedidoDePrePedidoProdutoPrePedidoUnisDto(x,
                Convert.ToInt16(prePedidoUnis.PermiteRAStatus));

                lstProdutosArclube.Add(ret);
            });

            ClienteCadastroDto clienteArclube = ClienteCadastroDto.ClienteCadastroDto_De_ClienteCadastroDados(clienteBll.BuscarCliente(prePedidoUnis.Cnpj_Cpf,
                prePedidoUnis.Indicador_Orcamentista).Result);


            var prePedidoDto = PrePedidoUnisDto.PrePedidoDtoDePrePedidoUnisDto(prePedidoUnis, endCadastralArclube, lstProdutosArclube, clienteArclube.DadosCliente);
            string apelido = "KONAR";
            //IEnumerable<string> resi = prepedidoBll.CadastrarPrepedido(PrePedidoDto.PrePedidoDados_De_PrePedidoDto(prePedidoDto), apelido.Trim(), 0.01M, false /* permitimos repetidos */,
            //    InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__ITS, 12).Result;
            //var res = resi.ToList();
            //if (incluirEsteErro)
            //{
            //    if (!res.Contains(mensagemErro))
            //        if (output != null)
            //            output.WriteLine(JsonConvert.SerializeObject(res));
            //    Assert.Contains(mensagemErro, res);
            //}
            //else
            //{
            //    if (res.Contains(mensagemErro))
            //        if (output != null)
            //            output.WriteLine(JsonConvert.SerializeObject(res));
            //    Assert.DoesNotContain(mensagemErro, res);
            //}
        }

        internal void TestarSucesso(DeixarDtoErrado deixarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez();
            TestarSucessoInterno(prePedido, deixarDtoErrado);
        }
        internal void TestarSucessoAvista(DeixarDtoErrado deixarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoAvista();
            TestarSucessoInterno(prePedido, deixarDtoErrado);
        }
        //testa se consegue cadastrar
        private void TestarSucessoInterno(PrePedidoUnisDto prePedido, DeixarDtoErrado deixarDtoErrado)
        {
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
