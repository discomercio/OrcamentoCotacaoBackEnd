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
    public class VerificarDadosEnderecoCadastral
    {
        private readonly PrePedidoUnisBll prepedidoUnisBll;
        private readonly PrePedidoUnisDto prePedidoUnisDto;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly ITestOutputHelper output;
        private readonly ClienteBll clienteBll;
        private readonly ClienteUnisBll clienteUnisBll;

        public VerificarDadosEnderecoCadastral(PrePedidoUnisBll prepedidoUnisBll, InfraBanco.ContextoBdProvider contextoProvider,
            InicializarBancoGeral inicializarBanco, ClienteBll clienteBll, ClienteUnisBll clienteUnisBll,
            ITestOutputHelper output)
        {
            this.prepedidoUnisBll = prepedidoUnisBll;
            this.contextoProvider = contextoProvider;
            this.inicializarBanco = inicializarBanco;
            this.clienteBll = clienteBll;
            this.clienteUnisBll = clienteUnisBll;
            this.output = output;

            var cliente = InicializarClienteDados.ClienteNaoCadastradoPF();
            cliente.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(cliente).Wait();

            var clientePJ = InicializarClienteDados.ClienteNaoCadastradoPJ();
            clientePJ.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParcelaUnica().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(clientePJ).Wait();
        }

        internal delegate void ArrumarDtoErrado(PrePedidoUnisDto prePedido);

        internal void TesteEnderecoCadastralPF(ArrumarDtoErrado arrumarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez();
            VerificarCamposEnderecoCadastralPF(prePedido, arrumarDtoErrado);
        }
        internal void TesteEnderecoCadastralPJ(ArrumarDtoErrado arrumarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.EnderecoCadastralPJ();
            VerificarCamposEnderecoCadastralPJ(prePedido, arrumarDtoErrado);
        }

        private void VerificarCamposEnderecoCadastralPF(PrePedidoUnisDto prePedido, ArrumarDtoErrado arrumarDtoErrado)
        {
            arrumarDtoErrado(prePedido);

            var res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;

            if (res.ListaErros.Count == 0)
            {
                var db = contextoProvider.GetContextoLeitura();

                var ret = (from c in db.Torcamentos
                           where c.Orcamento == res.IdPrePedidoCadastrado
                           select c).FirstOrDefault();

                //vamos verificar
                Assert.Equal("02045080", ret.Endereco_cep);
                Assert.Equal("Rua Professor Fábio Fanucchi", ret.Endereco_logradouro);
                Assert.Equal("420", ret.Endereco_numero);
                Assert.Equal("", ret.Endereco_complemento);
                Assert.Equal("Jardim São Paulo(Zona Norte)", ret.Endereco_bairro);
                Assert.Equal("São Paulo", ret.Endereco_cidade);
                Assert.Equal("SP", ret.Endereco_uf);
                Assert.Equal("PF", ret.Endereco_tipo_pessoa);
                Assert.Equal("35270445824", ret.Endereco_cnpj_cpf);
                Assert.Equal("Teste Cadastro Empresa Unis 4", ret.Endereco_nome);
                Assert.Equal("", ret.Endereco_rg);
                Assert.Equal(2, ret.Endereco_produtor_rural_status);
                Assert.Equal(2, ret.Endereco_contribuinte_icms_status);
                Assert.Equal("244.355.757.113", ret.Endereco_ie);
                Assert.Equal("", ret.Endereco_contato);
                Assert.Equal("gabrie@gmail.com", ret.Endereco_email);
                Assert.Equal("", ret.Endereco_email_xml);
                Assert.Equal("11", ret.Endereco_ddd_res);
                Assert.Equal("12213333", ret.Endereco_tel_res);
                Assert.Equal("11", ret.Endereco_ddd_cel);
                Assert.Equal("981603313", ret.Endereco_tel_cel);
                Assert.Equal("", ret.Endereco_ddd_com);
                Assert.Equal("", ret.Endereco_tel_com);
                Assert.Equal("", ret.Endereco_ramal_com);
                Assert.Equal("", ret.Endereco_ddd_com_2);
                Assert.Equal("", ret.Endereco_tel_com_2);
                Assert.Equal("", ret.Endereco_ramal_com_2);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }
        }

        private void VerificarCamposEnderecoCadastralPJ(PrePedidoUnisDto prePedido, ArrumarDtoErrado arrumarDtoErrado)
        {
            arrumarDtoErrado(prePedido);

            var res = prepedidoUnisBll.CadastrarPrepedidoUnis(prePedido).Result;

            if (res.ListaErros.Count == 0)
            {
                var db = contextoProvider.GetContextoLeitura();

                var ret = (from c in db.Torcamentos
                           where c.Orcamento == res.IdPrePedidoCadastrado
                           select c).FirstOrDefault();

                //vamos verificar
                Assert.Equal("02045080", ret.Endereco_cep);
                Assert.Equal("Rua Professor Fábio Fanucchi", ret.Endereco_logradouro);
                Assert.Equal("420", ret.Endereco_numero);
                Assert.Equal("", ret.Endereco_complemento);
                Assert.Equal("Jardim São Paulo(Zona Norte)", ret.Endereco_bairro);
                Assert.Equal("São Paulo", ret.Endereco_cidade);
                Assert.Equal("SP", ret.Endereco_uf);
                Assert.Equal("PJ", ret.Endereco_tipo_pessoa);
                Assert.Equal("00371048000106", ret.Endereco_cnpj_cpf);
                Assert.Equal("Teste Cadastro Empresa Unis 4", ret.Endereco_nome);
                Assert.Equal("", ret.Endereco_rg);
                Assert.Equal(0, ret.Endereco_produtor_rural_status);
                Assert.Equal(2, ret.Endereco_contribuinte_icms_status);
                Assert.Equal("244.355.757.113", ret.Endereco_ie);
                Assert.Equal("EDSON", ret.Endereco_contato);
                Assert.Equal("edsondasso@teste.com.br", ret.Endereco_email);
                Assert.Equal("contato@teste.com.br", ret.Endereco_email_xml);
                Assert.Equal("", ret.Endereco_ddd_res);
                Assert.Equal("", ret.Endereco_tel_res);
                Assert.Equal("", ret.Endereco_ddd_cel);
                Assert.Equal("", ret.Endereco_tel_cel);
                Assert.Equal("19", ret.Endereco_ddd_com);
                Assert.Equal("983462361", ret.Endereco_tel_com);
                Assert.Equal("12", ret.Endereco_ramal_com);
                Assert.Equal("19", ret.Endereco_ddd_com_2);
                Assert.Equal("123456578", ret.Endereco_tel_com_2);
                Assert.Equal("12", ret.Endereco_ramal_com_2);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }
        }

        [Fact]
        public void TestarEnderecoCadastralPF()
        {
            TesteEnderecoCadastralPF(c =>
            {
                c.EnderecoCadastralCliente.Endereco_produtor_rural_status = 2;
                c.EnderecoCadastralCliente.Endereco_contribuinte_icms_status = 2;
                c.EnderecoCadastralCliente.Endereco_contato = "";
                c.EnderecoCadastralCliente.Endereco_ie = "244.355.757.113";
            });
        }

        [Fact]
        public void TestarEnderecoCadastralPJ()
        {
            TesteEnderecoCadastralPJ(c => {
                c.FormaPagtoCriacao.C_pc_qtde = 1;
                c.FormaPagtoCriacao.C_pc_valor = 3470.24m;
            });
        }
    }
}
