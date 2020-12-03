using Cliente;
using InfraBanco.Constantes;
using Prepedido;
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
    public class TesteEnderecoEntrega : TestesPrepedidoUnisBll
    {
        public TesteEnderecoEntrega(InicializarBanco.InicializarBancoGeral inicializarBanco, ITestOutputHelper Output, PrePedidoUnisBll prepedidoUnisBll,
            ClienteUnisBll clienteUnisBll, PrepedidoBll prepedidoBll, ClienteBll clienteBll) :
            base(inicializarBanco, Output, prepedidoUnisBll, clienteUnisBll, prepedidoBll, clienteBll)
        {

        }

        [Fact]
        public void DadosEnderecoEntrega()
        {
            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_cod_justificativa = "3";
            }, "CÓDIGO DA JUSTFICATIVA INVÁLIDO!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_endereco = "";
            }, "PREENCHA O ENDEREÇO DE ENTREGA.", true);

            string end = "123456789012345678901234567890123456789012345678901234567890123";
            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_endereco = end;
            }, "ENDEREÇO DE ENTREGA EXCEDE O TAMANHO MÁXIMO PERMITIDO:<br>TAMANHO ATUAL: " + end.Length +
                            " CARACTERES<br>TAMANHO MÁXIMO: " + Constantes.MAX_TAMANHO_CAMPO_ENDERECO + " CARACTERES", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_endereco_numero = "";
            }, "PREENCHA O NÚMERO DO ENDEREÇO DE ENTREGA.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_bairro = "";
            }, "PREENCHA O BAIRRO DO ENDEREÇO DE ENTREGA.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_cidade = "";
            }, "PREENCHA A CIDADE DO ENDEREÇO DE ENTREGA.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_uf = "";
            }, "UF INVÁLIDA NO ENDEREÇO DE ENTREGA.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_cep = "";
            }, "INFORME O CEP DO ENDEREÇO DE ENTREGA.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_cep = "12121212121";
            }, "CEP INVÁLIDO NO ENDEREÇO DE ENTREGA.", true);
        }

        [Fact]
        public void DadosPessoaEntrega_PF()
        {
            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
            }, "Endereço de entrega: valor de produtor rural inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "12345678908";
            }, "Endereço de entrega: CPF inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 0;
            }, "Endereço de entrega: valor de contribuinte do ICMS inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 1;
            }, "Endereço de entrega: para ser cadastrado como Produtor Rural, " +
                "é necessário ser contribuinte do ICMS e possuir nº de IE!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_ie = "";
            }, "Endereço de entrega: se o cliente é contribuinte do ICMS a " +
                "inscrição estadual deve ser preenchida!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_ie = "ISENTO";
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 1;
            }, "Endereço de entrega: se cliente é não contribuinte do ICMS, " +
                "não pode ter o valor ISENTO no campo de Inscrição Estadual!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_ie = "ISENTO";
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 2;
            }, "Endereço de entrega: se cliente é contribuinte do ICMS, " +
                "não pode ter o valor ISENTO no campo de Inscrição Estadual!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 3;
            }, "Endereço de entrega: se o Contribuinte ICMS é isento, " +
                "o campo IE deve ser vazio!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 1;
            }, "Endereço de entrega: se cliente é contribuinte do ICMS, ele dever ser Produtor Rural!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 1;
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 1;
            }, "Endereço de entrega: se cliente é não produtor rural, contribuinte do " +
                "ICMS deve ter o valor inicial!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 1;
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 1;
            }, "Endereço de entrega: se cliente é não produtor rural, o IE não deve ser preenchido!", true);
        }

        [Fact]
        public void DadosPessoaEntrega_PF_Tel()
        {
            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_res = "12345";
            }, "Endereço de entrega: telefone residencial inválido.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_ddd_res = "";
            }, "Endereço de entrega: preencha o ddd residencial.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_ddd_res = "1";
            }, "Endereço de entrega: ddd residencial inválido.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "12345";
            }, "Endereço de entrega: telefone celular inválido.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
            }, "Endereço de entrega: preencha o ddd do celular.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "1";
            }, "Endereço de entrega: ddd do celular inválido.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "11";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
            }, "Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "123456";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
            }, "Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "12";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
            }, "Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "11";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
            }, "Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial 2.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "123456";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
            }, "Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial 2.", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "12";
            }, "Endereço de entrega: se tipo pessoa PF, não pode conter telefone comercial 2.", true);
        }

        [Fact]
        public void DadosPessoaEnderecoEntrega()
        {
            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "";
            }, "Endereço de Entrega: Necessário escolher Pessoa Jurídica ou Pessoa Física no Endereço de entrega!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_nome = "";
            }, "Endereço de Entrega: Preencha o nome/razão social no endereço de entrega!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "";
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
            }, "Endereço de Entrega: se cliente é tipo PF, o tipo de pessoa do endereço de entrega deve ser PF.", true);
        }

        [Fact]
        public void DadosPessoaEnderecoEntrega_PJ()
        {
            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "";
            }, "Endereço de entrega: CNPJ inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "12345678908";
            }, "Endereço de entrega: CNPJ inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 1;
            }, "Endereço de entrega: Se tipo pessoa é PJ, não pode ser Produtor Rural!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_rg = "1234";
            }, "Endereço de entrega: Se tipo pessoa é PJ, não pode ter RG preenchido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 4;
            }, "Endereço de entrega: valor de contribuinte do ICMS inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 0;
            }, "Endereço de entrega: valor de contribuinte do ICMS inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 2;
                c.EnderecoEntrega.EndEtg_ie = "";
            }, "Endereço de entrega: se o cliente é contribuinte do ICMS a inscrição estadual deve ser preenchida!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 1;
                c.EnderecoEntrega.EndEtg_ie = "ISENTO";
            }, "Endereço de entrega: se cliente é não contribuinte do ICMS, " +
                "não pode ter o valor ISENTO no campo de Inscrição Estadual!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_contribuinte_icms_status = 2;
                c.EnderecoEntrega.EndEtg_ie = "ISENTO";
            }, "Endereço de entrega: se cliente é contribuinte do ICMS, " +
                "não pode ter o valor ISENTO no campo de Inscrição Estadual!", true);
        }

        [Fact]
        public void DadosPessoaEnderecoEntrega_PJ_Tel()
        {
            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
            }, "Endereço de entrega: preencha o ddd do telefone comercial!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_tel_com = "";
            }, "Endereço de entrega: preencha o telefone comercial!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_ddd_com = "";
                c.EnderecoEntrega.EndEtg_tel_com = "";
                c.EnderecoEntrega.EndEtg_ramal_com = "12";
            }, "Endereço de entrega: Ramal do telefone comercial preenchido sem telefone comercial", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_tel_com = "12345";
            }, "Endereço de entrega: telefone comercial inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_ddd_com = "1";
            }, "Endereço de entrega: ddd do telefone comercial inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
            }, "Endereço de entrega: preencha o ddd do telefone comercial 2!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
            }, "Endereço de entrega: preencha o telefone comercial 2!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "";
                c.EnderecoEntrega.EndEtg_tel_com_2 = "";
                c.EnderecoEntrega.EndEtg_ramal_com_2 = "12";
            }, "Endereço de entrega: Ramal do telefone comercial 2 preenchido sem telefone comercial 2!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_tel_com_2 = "12345";
            }, "Endereço de entrega: telefone comercial 2 inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
                c.EnderecoEntrega.EndEtg_ddd_com_2 = "1";
            }, "Endereço de entrega: ddd do telefone comercial 2 inválido!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "11";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
            }, "Endereço de entrega: se tipo pessoa PJ, não pode conter DDD residencial e telefone residencial!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "12345678";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
            }, "Endereço de entrega: se tipo pessoa PJ, não pode conter DDD residencial e telefone residencial!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "11";
                c.EnderecoEntrega.EndEtg_tel_cel = "";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
            }, "Endereço de entrega: se tipo pessoa PJ, não pode conter DDD celular e telefone celular!", true);

            TesteEnderecoEntrega(c =>
            {
                c.OutroEndereco = true;
                c.EnderecoEntrega.EndEtg_ddd_res = "";
                c.EnderecoEntrega.EndEtg_tel_res = "";
                c.EnderecoEntrega.EndEtg_ddd_cel = "";
                c.EnderecoEntrega.EndEtg_tel_cel = "12345678";
                c.EnderecoEntrega.EndEtg_rg = "";
                c.EnderecoEntrega.EndEtg_tipo_pessoa = "PJ";
                c.EnderecoEntrega.EndEtg_cnpj_cpf = "00371048000106";
                c.EnderecoEntrega.EndEtg_produtor_rural_status = 0;
            }, "Endereço de entrega: se tipo pessoa PJ, não pode conter DDD celular e telefone celular!", true);
        }
    }
}
