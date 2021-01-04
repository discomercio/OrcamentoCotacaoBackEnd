using Newtonsoft.Json;
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
using Cliente;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesDadosCadastrados.TestePrepedidoDadosCadastrados
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class VerificarDadosEnderecoEntrega
    {
        private readonly PrePedidoUnisBll prepedidoUnisBll;
        private readonly InfraBanco.ContextoBdProvider contextoProvider;
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly ITestOutputHelper output;

        public VerificarDadosEnderecoEntrega(PrePedidoUnisBll prepedidoUnisBll, InfraBanco.ContextoBdProvider contextoProvider,
            InicializarBancoGeral inicializarBanco, ClienteUnisBll clienteUnisBll,
            ITestOutputHelper output)
        {
            this.prepedidoUnisBll = prepedidoUnisBll;
            this.contextoProvider = contextoProvider;
            this.inicializarBanco = inicializarBanco;
            this.output = output;

            var cliente = InicializarClienteDados.ClienteNaoCadastradoPF();
            cliente.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParceladoCartao1vez().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(cliente, cliente.DadosCliente.Indicador_Orcamentista).Wait();

            var clientePJ = InicializarClienteDados.ClienteNaoCadastradoPJ();
            clientePJ.DadosCliente.Cnpj_Cpf = DadosPrepedidoUnisBll.PrepedidoParcelaUnica().Cnpj_Cpf;
            clienteUnisBll.CadastrarClienteUnis(clientePJ, clientePJ.DadosCliente.Indicador_Orcamentista).Wait();
        }

        internal delegate void ArrumarDtoErrado(PrePedidoUnisDto prePedido);

        internal void TesteEnderecoEntregaPF(ArrumarDtoErrado arrumarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto();
            VerificarCamposEnderecoEntregaPF(prePedido, arrumarDtoErrado);
        }
        internal void TesteClientePJ_EnderecoEntregaPF(ArrumarDtoErrado arrumarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto();
            VerificarClientePJ_CamposEnderecoEntregaPF(prePedido, arrumarDtoErrado);
        }
        internal void TesteClientePJ_EnderecoEntregaPJ(ArrumarDtoErrado arrumarDtoErrado)
        {
            PrePedidoUnisDto prePedido = DadosPrepedidoUnisBll.PrepedidoEnderecoEntregaCompleto();
            VerificarClientePJ_CamposEnderecoEntregaPJ(prePedido, arrumarDtoErrado);
        }

        private void VerificarCamposEnderecoEntregaPF(PrePedidoUnisDto prePedido, ArrumarDtoErrado arrumarDtoErrado)
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
                Assert.Equal("003", ret.EndEtg_Cod_Justificativa);
                Assert.Equal("02045080", ret.EndEtg_CEP);
                Assert.Equal("Rua Professor Fábio Fanucchi", ret.EndEtg_Endereco);
                Assert.Equal("97", ret.EndEtg_Endereco_Numero);
                Assert.Equal("", ret.EndEtg_Endereco_Complemento);
                Assert.Equal("Jardim São Paulo(Zona Norte)", ret.EndEtg_Bairro);
                Assert.Equal("São Paulo", ret.EndEtg_Cidade);
                Assert.Equal("SP", ret.EndEtg_UF);
                Assert.Equal("PF", ret.EndEtg_tipo_pessoa);
                Assert.Equal("29756194804", ret.EndEtg_cnpj_cpf);
                Assert.Equal("Vivian", ret.EndEtg_nome);
                Assert.Equal("11997996-2", ret.EndEtg_rg);
                Assert.Equal(2, ret.EndEtg_produtor_rural_status);
                Assert.Equal(2, ret.EndEtg_contribuinte_icms_status);
                Assert.Equal("244.355.757.113", ret.EndEtg_ie);
                Assert.Equal("testeCad@Gabriel.com", ret.EndEtg_email);
                Assert.Equal("", ret.EndEtg_email_xml);
                Assert.Equal("11", ret.EndEtg_ddd_res);
                Assert.Equal("11111111", ret.EndEtg_tel_res);
                Assert.Equal("11", ret.EndEtg_ddd_cel);
                Assert.Equal("981603313", ret.EndEtg_tel_cel);
                Assert.Equal("", ret.EndEtg_ddd_com);
                Assert.Equal("", ret.EndEtg_tel_com);
                Assert.Equal("", ret.EndEtg_ramal_com);
                Assert.Equal("", ret.EndEtg_ddd_com_2);
                Assert.Equal("", ret.EndEtg_tel_com_2);
                Assert.Equal("", ret.EndEtg_ramal_com_2);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }
        }

        private void VerificarClientePJ_CamposEnderecoEntregaPF(PrePedidoUnisDto prePedido, ArrumarDtoErrado arrumarDtoErrado)
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
                Assert.Equal("003", ret.EndEtg_Cod_Justificativa);
                Assert.Equal("02045080", ret.EndEtg_CEP);
                Assert.Equal("Rua Professor Fábio Fanucchi", ret.EndEtg_Endereco);
                Assert.Equal("97", ret.EndEtg_Endereco_Numero);
                Assert.Equal("", ret.EndEtg_Endereco_Complemento);
                Assert.Equal("Jardim São Paulo(Zona Norte)", ret.EndEtg_Bairro);
                Assert.Equal("São Paulo", ret.EndEtg_Cidade);
                Assert.Equal("SP", ret.EndEtg_UF);
                Assert.Equal("PF", ret.EndEtg_tipo_pessoa);
                Assert.Equal("29756194804", ret.EndEtg_cnpj_cpf);
                Assert.Equal("Vivian", ret.EndEtg_nome);
                Assert.Equal("", ret.EndEtg_rg);
                Assert.Equal(2, ret.EndEtg_produtor_rural_status);
                Assert.Equal(2, ret.EndEtg_contribuinte_icms_status);
                Assert.Equal("244.355.757.113", ret.EndEtg_ie);
                Assert.Equal("testeCad@Gabriel.com", ret.EndEtg_email);
                Assert.Equal("", ret.EndEtg_email_xml);
                Assert.Equal("11", ret.EndEtg_ddd_res);
                Assert.Equal("11111111", ret.EndEtg_tel_res);
                Assert.Equal("11", ret.EndEtg_ddd_cel);
                Assert.Equal("981603313", ret.EndEtg_tel_cel);
                Assert.Equal("", ret.EndEtg_ddd_com);
                Assert.Equal("", ret.EndEtg_tel_com);
                Assert.Equal("", ret.EndEtg_ramal_com);
                Assert.Equal("", ret.EndEtg_ddd_com_2);
                Assert.Equal("", ret.EndEtg_tel_com_2);
                Assert.Equal("", ret.EndEtg_ramal_com_2);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }
        }

        private void VerificarClientePJ_CamposEnderecoEntregaPJ(PrePedidoUnisDto prePedido, ArrumarDtoErrado arrumarDtoErrado)
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
                Assert.Equal("003", ret.EndEtg_Cod_Justificativa);
                Assert.Equal("02045080", ret.EndEtg_CEP);
                Assert.Equal("Rua Professor Fábio Fanucchi", ret.EndEtg_Endereco);
                Assert.Equal("97", ret.EndEtg_Endereco_Numero);
                Assert.Equal("", ret.EndEtg_Endereco_Complemento);
                Assert.Equal("Jardim São Paulo(Zona Norte)", ret.EndEtg_Bairro);
                Assert.Equal("São Paulo", ret.EndEtg_Cidade);
                Assert.Equal("SP", ret.EndEtg_UF);
                Assert.Equal("PJ", ret.EndEtg_tipo_pessoa);
                Assert.Equal("00371048000106", ret.EndEtg_cnpj_cpf);
                Assert.Equal("Vivian", ret.EndEtg_nome);
                Assert.Equal("", ret.EndEtg_rg);
                Assert.Equal(0, ret.EndEtg_produtor_rural_status);
                Assert.Equal(2, ret.EndEtg_contribuinte_icms_status);
                Assert.Equal("244.355.757.113", ret.EndEtg_ie);
                Assert.Equal("testeCad@Gabriel.com", ret.EndEtg_email);
                Assert.Equal("", ret.EndEtg_email_xml);
                Assert.Equal("", ret.EndEtg_ddd_res);
                Assert.Equal("", ret.EndEtg_tel_res);
                Assert.Equal("", ret.EndEtg_ddd_cel);
                Assert.Equal("", ret.EndEtg_tel_cel);
                Assert.Equal("11", ret.EndEtg_ddd_com);
                Assert.Equal("25321634", ret.EndEtg_tel_com);
                Assert.Equal("32", ret.EndEtg_ramal_com);
                Assert.Equal("11", ret.EndEtg_ddd_com_2);
                Assert.Equal("85868586", ret.EndEtg_tel_com_2);
                Assert.Equal("11", ret.EndEtg_ramal_com_2);
            }
            else
            {
                if (output != null)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Equal(1, 2);
            }
        }

        [Fact]
        public void TestarEnderecoEntregaPF()
        {
            TesteEnderecoEntregaPF(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
            });
        }

        [Fact]
        public void TestarClientePJ_EnderecoEntregaPF()
        {
            TesteClientePJ_EnderecoEntregaPF(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_rg = "";
            });
        }

        [Fact]
        public void TestarClientePJ_EnderecoEntregaPJ()
        {
            TesteClientePJ_EnderecoEntregaPJ(c =>
            {
                c.OutroEndereco = true;
                c.Cnpj_Cpf = "00371048000106";
                c.EnderecoCadastralCliente.Endereco_email = "edsondasso@teste.com.br";
                c.EnderecoCadastralCliente.Endereco_email_xml = "contato@teste.com.br";
                c.EnderecoCadastralCliente.Endereco_ddd_res = "";
                c.EnderecoCadastralCliente.Endereco_tel_res = "";
                c.EnderecoCadastralCliente.Endereco_ddd_cel = "";
                c.EnderecoCadastralCliente.Endereco_tel_cel = "";
                c.EnderecoCadastralCliente.Endereco_ddd_com = "19";
                c.EnderecoCadastralCliente.Endereco_tel_com = "983462361";
                c.EnderecoCadastralCliente.Endereco_ramal_com = "12";
                c.EnderecoCadastralCliente.Endereco_ddd_com_2 = "19";
                c.EnderecoCadastralCliente.Endereco_tel_com_2 = "123456578";
                c.EnderecoCadastralCliente.Endereco_ramal_com_2 = "12";
                c.EnderecoCadastralCliente.Endereco_tipo_pessoa = "PJ";
                c.EnderecoCadastralCliente.Endereco_cnpj_cpf = "00371048000106";
                c.EnderecoCadastralCliente.Endereco_contribuinte_icms_status = 2;
                c.EnderecoCadastralCliente.Endereco_produtor_rural_status = 0;
                c.EnderecoCadastralCliente.Endereco_ie = "244.355.757.113";
                c.EnderecoCadastralCliente.Endereco_contato = "EDSON";

                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
            });
        }

    }
}
