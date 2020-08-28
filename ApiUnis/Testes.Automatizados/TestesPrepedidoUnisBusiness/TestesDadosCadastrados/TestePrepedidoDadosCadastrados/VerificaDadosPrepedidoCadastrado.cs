using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.PrePedidoUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Testes.Automatizados.InicializarBanco;
using Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesPrepedidoUnisBll;
using Xunit;
using System.Linq;
using Xunit.Abstractions;
using Newtonsoft.Json;
using PrepedidoBusiness.Bll.ClienteBll;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesDadosCadastrados.TestePrepedidoDadosCadastrados
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class VerificaDadosPrepedidoCadastrado
    {
        private readonly PrePedidoUnisBll prepedidoUnisBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly ITestOutputHelper output;
        private readonly ClienteBll clienteBll;
        private readonly ClienteUnisBll clienteUnisBll;
        public VerificaDadosPrepedidoCadastrado(PrePedidoUnisBll prepedidoUnisBll, InfraBanco.ContextoBdProvider contextoProvider,
            InicializarBancoGeral inicializarBanco, ITestOutputHelper output, ClienteBll clienteBll, ClienteUnisBll clienteUnisBll)
        {
            this.prepedidoUnisBll = prepedidoUnisBll;
            this.contextoProvider = contextoProvider;
            this.inicializarBanco = inicializarBanco;
            this.clienteUnisBll = clienteUnisBll;

            var cliente = InicializarClienteDados.ClienteNaoCadastradoPF();
            cliente.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(cliente).Wait();

            var clientePJ = InicializarClienteDados.ClienteNaoCadastradoPJ();
            clientePJ.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParcelaUnica().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(clientePJ).Wait();
        }

        [Fact]
        public void TotalPrepedidoParcelaAvista()
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoAvista();



            var res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;
            if (res.ListaErros.Count == 0)
            {
                var db = contextoProvider.GetContextoLeitura();

                var ret = (from c in db.Torcamentos
                           where c.Orcamento == res.IdPrePedidoCadastrado
                           select c).FirstOrDefault();

                Assert.Equal((2 * 652.71m) + (2 * 979.06m), ret.Vl_Total);
                Assert.Equal((2 * 694.05m) + (2 * 1041.07m), ret.Vl_Total_NF);
                Assert.Equal(((2 * 694.05m) + (2 * 1041.07m)) - ((2 * 652.71m) + (2 * 979.06m)), ret.Vl_Total_RA);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }

            prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoAvista();
            prePedido.Indicador_Orcamentista = "Apelido_sem_ra";
            prePedido.PermiteRAStatus = false;
            prePedido.ListaProdutos[0].Preco_NF = 652.71m;
            prePedido.ListaProdutos[0].Preco_Lista = 652.71m;
            prePedido.ListaProdutos[1].Preco_NF = 979.06m;
            prePedido.ListaProdutos[1].Preco_Lista = 979.06m;

            res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;
            if (res.ListaErros.Count == 0)
            {
                var db = contextoProvider.GetContextoLeitura();

                var ret = (from c in db.Torcamentos
                           where c.Orcamento == res.IdPrePedidoCadastrado
                           select c).FirstOrDefault();

                Assert.Equal((2 * 652.71m) + (2 * 979.06m), ret.Vl_Total);
                Assert.Equal((2 * 652.71m) + (2 * 979.06m), ret.Vl_Total_NF);
                Assert.Equal(((2 * 652.71m) + (2 * 979.06m)) - ((2 * 652.71m) + (2 * 979.06m)), ret.Vl_Total_RA);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }
        }

        [Fact]
        public void TotalPrepedidoParcelaCartao()
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaCartao();
            prePedido.FormaPagtoCriacao.C_pc_valor = 867.56m;
            prePedido.ListaProdutos[0].Preco_Venda = prePedido.ListaProdutos[0].Preco_Lista *
                (decimal)(1 - prePedido.ListaProdutos[0].Desc_Dado / 100);
            prePedido.ListaProdutos[1].Preco_Venda = prePedido.ListaProdutos[1].Preco_Lista *
                (decimal)(1 - prePedido.ListaProdutos[1].Desc_Dado / 100);

            TestarCadastroFormaPagto(prePedido);

            prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaCartao();
            prePedido.FormaPagtoCriacao.C_pc_valor = 858.89m;//3.435,54
            prePedido.ListaProdutos[0].Preco_Venda = 687.11m;
            prePedido.ListaProdutos[1].Preco_Venda = 1030.66m;
            prePedido.ListaProdutos[0].Preco_NF = 687.11m;
            prePedido.ListaProdutos[1].Preco_NF = 1030.66m;
            prePedido.VlTotalDestePedido = (2 * 687.11m) + (2 * 1030.66m);
            prePedido.Indicador_Orcamentista = "Apelido_sem_ra";
            prePedido.PermiteRAStatus = false;

            TestarCadastroFormaPagtoSemRA(prePedido);
        }

        [Fact]
        public void TotalPrepedidoParcelaCartaoMaquineta()
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaCartaoMaquineta();
            prePedido.FormaPagtoCriacao.C_pc_maquineta_valor = 867.56m;
            prePedido.ListaProdutos[0].Preco_Venda = prePedido.ListaProdutos[0].Preco_Lista *
                (decimal)(1 - prePedido.ListaProdutos[0].Desc_Dado / 100);
            prePedido.ListaProdutos[1].Preco_Venda = prePedido.ListaProdutos[1].Preco_Lista *
                (decimal)(1 - prePedido.ListaProdutos[1].Desc_Dado / 100);

            TestarCadastroFormaPagto(prePedido);

            prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaCartaoMaquineta();
            prePedido.FormaPagtoCriacao.C_pc_maquineta_valor = 858.89m;
            prePedido.ListaProdutos[0].Preco_Venda = 687.11m;
            prePedido.ListaProdutos[1].Preco_Venda = 1030.66m;
            prePedido.ListaProdutos[0].Preco_NF = 687.11m;
            prePedido.ListaProdutos[1].Preco_NF = 1030.66m;
            prePedido.VlTotalDestePedido = (2 * 687.11m) + (2 * 1030.66m);
            prePedido.Indicador_Orcamentista = "Apelido_sem_ra";
            prePedido.PermiteRAStatus = false;

            TestarCadastroFormaPagtoSemRA(prePedido);
        }

        [Fact]
        public void TotalPrepedidoParcelaUnica()
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaUnica();
            prePedido.FormaPagtoCriacao.C_pu_valor = 3470.24m;
            prePedido.ListaProdutos[0].Preco_Venda = prePedido.ListaProdutos[0].Preco_Lista *
                (decimal)(1 - prePedido.ListaProdutos[0].Desc_Dado / 100);
            prePedido.ListaProdutos[1].Preco_Venda = prePedido.ListaProdutos[1].Preco_Lista *
                (decimal)(1 - prePedido.ListaProdutos[1].Desc_Dado / 100);

            TestarCadastroFormaPagto(prePedido);

            prePedido = DadosPrepedidoUnisBll.PrepedidoParcelaUnica();
            prePedido.FormaPagtoCriacao.C_pu_valor = 3435.54m;
            prePedido.ListaProdutos[0].Preco_Venda = 687.11m;
            prePedido.ListaProdutos[1].Preco_Venda = 1030.66m;
            prePedido.ListaProdutos[0].Preco_NF = 687.11m;
            prePedido.ListaProdutos[1].Preco_NF = 1030.66m;
            prePedido.VlTotalDestePedido = (2 * 687.11m) + (2 * 1030.66m);
            prePedido.Indicador_Orcamentista = "Apelido_sem_ra";
            prePedido.PermiteRAStatus = false;

            TestarCadastroFormaPagtoSemRA(prePedido);
        }

        [Fact]
        public void TotalPrepedidoParcelaComEntrada()
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoPagtoComEntrada();
            prePedido.FormaPagtoCriacao.C_pce_entrada_valor = 100m;
            prePedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = "2";
            prePedido.FormaPagtoCriacao.C_pce_prestacao_valor = 1123.41m;
            prePedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = "2";
            prePedido.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 4;
            prePedido.ListaProdutos[0].Preco_Venda = prePedido.ListaProdutos[0].Preco_Lista *
                (decimal)(1 - prePedido.ListaProdutos[0].Desc_Dado / 100);
            prePedido.ListaProdutos[1].Preco_Venda = prePedido.ListaProdutos[1].Preco_Lista *
                (decimal)(1 - prePedido.ListaProdutos[1].Desc_Dado / 100);

            TestarCadastroFormaPagto(prePedido);

            prePedido = DadosPrepedidoUnisBll.PrepedidoPagtoComEntrada();
            prePedido.FormaPagtoCriacao.C_pce_entrada_valor = 100m;
            prePedido.FormaPagtoCriacao.Op_pce_entrada_forma_pagto = "2";
            prePedido.FormaPagtoCriacao.C_pce_prestacao_valor = 1111.85m;
            prePedido.FormaPagtoCriacao.Op_pce_prestacao_forma_pagto = "2";
            prePedido.FormaPagtoCriacao.CustoFinancFornecQtdeParcelas = 4;
            prePedido.VlTotalDestePedido = (2 * 687.11m) + (2 * 1030.66m);
            prePedido.ListaProdutos[0].Preco_Venda = 687.11m;
            prePedido.ListaProdutos[1].Preco_Venda = 1030.66m;
            prePedido.ListaProdutos[0].Preco_NF = 687.11m;
            prePedido.ListaProdutos[1].Preco_NF = 1030.66m;
            prePedido.VlTotalDestePedido = (2 * 687.11m) + (2 * 1030.66m);
            prePedido.Indicador_Orcamentista = "Apelido_sem_ra";
            prePedido.PermiteRAStatus = false;

            TestarCadastroFormaPagtoSemRA(prePedido);
        }

        private void TestarCadastroFormaPagto(PrePedidoUnisDto prePedido)
        {

            var res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;
            if (res.ListaErros.Count == 0)
            {
                var db = contextoProvider.GetContextoLeitura();

                var ret = (from c in db.Torcamentos
                           where c.Orcamento == res.IdPrePedidoCadastrado
                           select c).FirstOrDefault();

                Assert.Equal((2 * 687.11m) + (2 * 1030.66m), ret.Vl_Total);
                Assert.Equal((2 * 694.05m) + (2 * 1041.07m), ret.Vl_Total_NF);
                Assert.Equal(((2 * 694.05m) + (2 * 1041.07m)) - ((2 * 687.11m) + (2 * 1030.66m)), ret.Vl_Total_RA);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }
        }

        private void TestarCadastroFormaPagtoSemRA(PrePedidoUnisDto prePedido)
        {
            var res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;
            if (res.ListaErros.Count == 0)
            {
                var db = contextoProvider.GetContextoLeitura();

                var ret = (from c in db.Torcamentos
                           where c.Orcamento == res.IdPrePedidoCadastrado
                           select c).FirstOrDefault();

                Assert.Equal((2 * 687.11m) + (2 * 1030.66m), ret.Vl_Total);
                Assert.Equal((2 * 694.05m) + (2 * 1041.07m), ret.Vl_Total_NF);
                Assert.Equal(0m, ret.Vl_Total_RA);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }
        }
    }
}
