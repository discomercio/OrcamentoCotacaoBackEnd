using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Testes.Automatizados.InicializarBanco;
using Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Cliente;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesDadosCadastrados.TestePrepedidoDadosCadastrados
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class VerificarDadosProdutos
    {
        private readonly PrePedidoUnisBll prepedidoUnisBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly ITestOutputHelper output;
        private readonly ClienteBll clienteBll;
        private readonly ClienteUnisBll clienteUnisBll;

        public VerificarDadosProdutos(PrePedidoUnisBll prepedidoUnisBll, InfraBanco.ContextoBdProvider contextoProvider,
            InicializarBancoGeral inicializarBanco, ClienteBll clienteBll, ClienteUnisBll clienteUnisBll,
            ITestOutputHelper output)
        {
            this.prepedidoUnisBll = prepedidoUnisBll;
            this.contextoProvider = contextoProvider;
            this.inicializarBanco = inicializarBanco;
            this.clienteBll = clienteBll;
            this.clienteUnisBll = clienteUnisBll;
            this.output = output;

            var cliente = InicializarClienteDados.ClienteNaoCadastradoPJ();
            cliente.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParcelaUnica().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(cliente).Wait();
        }

        internal delegate void ArrumarDtoErrado(PrePedidoUnisDto prePedido);

        internal void TesteProdutos(ArrumarDtoErrado arrumarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaCartao();
            VerificarProduto(prePedido, arrumarDtoErrado);
        }        

        private void VerificarProduto(PrePedidoUnisDto prePedido, ArrumarDtoErrado arrumarDtoErrado)
        {
            //preciso conseguir cadastrar prepedido, ao rodar o teste todo da erro pois, já cadastrou 
            //Limite de pré-pedidos por CPF/CNPJ excedido, existem 10 pré-pedidos há menos de 3600 segundos.
            //Pré-pedidos para o mesmo CPF/CNPJ

            arrumarDtoErrado(prePedido);

            var res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;

            if (res.ListaErros.Count == 0)
            {
                var db = contextoProvider.GetContextoLeitura();

                var ret = (from c in db.TorcamentoItems
                           where c.Orcamento == res.IdPrePedidoCadastrado
                           select c).ToList();


                if (prePedido.PermiteRAStatus == false) VerificarProdutoSemRA(ret, res);
                if (prePedido.PermiteRAStatus == true) VerificarProdutoComRA(ret, res);

            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res.ListaErros));
                Assert.Equal(1, 2);
            }
        }

        private void VerificarProdutoSemRA(List<InfraBanco.Modelos.TorcamentoItem> lstItens, PrePedidoResultadoUnisDto res)
        {
            if (lstItens.Count > 0)
            {
                //produto1
                Assert.Equal("003", lstItens[0].Fabricante);
                Assert.Equal("003220", lstItens[0].Produto);
                Assert.Equal(2, (short)lstItens[0].Qtde);
                Assert.Equal(1f, (float)lstItens[0].Desc_Dado);
                Assert.Equal(687.11m, lstItens[0].Preco_Venda);
                Assert.Equal(694.05m, lstItens[0].Preco_Lista);
                Assert.Equal(687.11m, lstItens[0].Preco_NF);
                Assert.Equal(1.0527f, lstItens[0].CustoFinancFornecCoeficiente);
                Assert.Equal(659.30m, lstItens[0].CustoFinancFornecPrecoListaBase);
                //produto2
                Assert.Equal("003", lstItens[1].Fabricante);
                Assert.Equal("003221", lstItens[1].Produto);
                Assert.Equal(2, (short)lstItens[1].Qtde);
                Assert.Equal(1f, (float)lstItens[1].Desc_Dado);
                Assert.Equal(1030.66m, lstItens[1].Preco_Venda);
                Assert.Equal(1041.07m, lstItens[1].Preco_Lista);
                Assert.Equal(1030.66m, lstItens[1].Preco_NF);
                Assert.Equal(1.0527f, lstItens[1].CustoFinancFornecCoeficiente);
                Assert.Equal(988.95m, lstItens[1].CustoFinancFornecPrecoListaBase);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res.ListaErros));
                Assert.Equal(1, 2);
            }
        }

        private void VerificarProdutoComRA(List<InfraBanco.Modelos.TorcamentoItem> lstItens, PrePedidoResultadoUnisDto res)
        {
            if (lstItens.Count > 0)
            {
                //produto1
                Assert.Equal("003", lstItens[0].Fabricante);
                Assert.Equal("003220", lstItens[0].Produto);
                Assert.Equal(2, (short)lstItens[0].Qtde);
                Assert.Equal(1f, (float)lstItens[0].Desc_Dado);
                Assert.Equal(687.11m, lstItens[0].Preco_Venda);
                Assert.Equal(694.05m, lstItens[0].Preco_Lista);
                Assert.Equal(694.05m, lstItens[0].Preco_NF);
                Assert.Equal(1.0527f, lstItens[0].CustoFinancFornecCoeficiente);
                Assert.Equal(659.30m, lstItens[0].CustoFinancFornecPrecoListaBase);
                //produto2
                Assert.Equal("003", lstItens[1].Fabricante);
                Assert.Equal("003221", lstItens[1].Produto);
                Assert.Equal(2, (short)lstItens[1].Qtde);
                Assert.Equal(1f, (float)lstItens[1].Desc_Dado);
                Assert.Equal(1030.66m, lstItens[1].Preco_Venda);
                Assert.Equal(1041.07m, lstItens[1].Preco_Lista);
                Assert.Equal(1041.07m, lstItens[1].Preco_NF);
                Assert.Equal(1.0527f, lstItens[1].CustoFinancFornecCoeficiente);
                Assert.Equal(988.95m, lstItens[1].CustoFinancFornecPrecoListaBase);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res.ListaErros));
                Assert.Equal(1, 2);
            }
        }

        [Fact]
        public void TestarProdutosSemRA()
        {
            TesteProdutos(c =>
            {
                c.FormaPagtoCriacao.C_pc_valor = 858.89m;
                c.ListaProdutos[0].Preco_Venda = 687.11m;
                c.ListaProdutos[1].Preco_Venda = 1030.66m;
                c.ListaProdutos[0].Preco_NF = 687.11m;
                c.ListaProdutos[1].Preco_NF = 1030.66m;
                c.Vl_total = (2 * 687.11m) + (2 * 1030.66m);
                c.Indicador_Orcamentista = "Apelido_sem_ra";
                c.PermiteRAStatus = false;
            });
        }

        [Fact]
        public void TestarProdutosComRA()
        {
            TesteProdutos(c =>
            {
                c.FormaPagtoCriacao.C_pc_valor = 867.56m;
            });
        }
    }
}
