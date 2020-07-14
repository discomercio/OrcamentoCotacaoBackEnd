using InfraBanco;
using InfraBanco.Constantes;
using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoBusiness.Bll.ClienteBll;
using System;
using System.Collections.Generic;
using System.Linq;
using Testes.Automatizados.InicializarBanco;
using Xunit;
using Xunit.Abstractions;
using static Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll.TestesClienteUnisBll;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class Pf
    {
        private readonly TestesClienteUnisBll testesClienteUnisBll;
        private readonly ITestOutputHelper output;
        private readonly PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll clienteUnisBll;
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly ContextoBdProvider contextoProvider;

        public Pf(TestesClienteUnisBll testesClienteUnisBll, ITestOutputHelper output,
            PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll clienteUnisBll,
            InicializarBanco.InicializarBancoGeral inicializarBanco,
            InfraBanco.ContextoBdProvider contextoProvider)
        {
            this.testesClienteUnisBll = testesClienteUnisBll;
            this.output = output;
            this.clienteUnisBll = clienteUnisBll;
            this.inicializarBanco = inicializarBanco;
            this.contextoProvider = contextoProvider;
            this.testesClienteUnisBll.Output = output;
        }

        [Fact]
        public void RefBancaria()
        {
            inicializarBanco.TclientesApagar();

            //nao pode ter
            testesClienteUnisBll.TestarCadastro(c => c.RefBancaria.Add(new RefBancariaClienteUnisDto()),
                "Se cliente tipo PF, não deve constar referência bancária!",
                TipoPessoa.PF);
        }

        [Fact]
        public void RefComercial()
        {
            inicializarBanco.TclientesApagar();

            //nao pode ter
            testesClienteUnisBll.TestarCadastro(c => c.RefComercial.Add(new RefComercialClienteUnisDto()),
                "Se cliente tipo PF, não deve constar referência comercial!",
                TipoPessoa.PF);
        }


        [Fact]
        public void Indicador_Orcamentista()
        {
            inicializarBanco.TclientesApagar();

            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Indicador_Orcamentista = InicializarBancoGeral.Dados.Orcamentista.ApelidoNaoExiste,
                PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll.MensagensErro.Orcamentista_nao_existe,
                TipoPessoa.PF);
        }
        [Fact]
        public void Cnpj_Cpf()
        {
            inicializarBanco.TclientesApagar();

            //vazio
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Cnpj_Cpf = "",
                ValidacoesClienteBll.MensagensErro.CPF_NAO_FORNECIDO,
                TipoPessoa.PF);

            //qq coisa
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Cnpj_Cpf = "1122",
                ValidacoesClienteBll.MensagensErro.CPF_INVALIDO,
                TipoPessoa.PF);

            //cnpj
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Cnpj_Cpf = "25.326.265/0001-05",
                ValidacoesClienteBll.MensagensErro.CPF_INVALIDO,
                TipoPessoa.PF);

            //cpf com digito inválido
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Cnpj_Cpf = "479.378.150-01",
                ValidacoesClienteBll.MensagensErro.CPF_INVALIDO,
                TipoPessoa.PF);
        }

        [Fact]
        public void Tipo()
        {
            inicializarBanco.TclientesApagar();

            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Tipo = "",
                ValidacoesClienteBll.MensagensErro.INFORME_SE_O_CLIENTE_E_PF_OU_PJ,
                TipoPessoa.PF);
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Tipo = "XX",
                ValidacoesClienteBll.MensagensErro.Tipo_de_cliente_nao_e_PF_nem_PJ,
                TipoPessoa.PF);
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Tipo = "PJ",
                "Se cliente é tipo PJ, não pode ser Produtor Rural",
                TipoPessoa.PF);
        }

        [Fact]
        public void ProdutorRural_Contribuinte_Icms_Status_codigos_invalidos()
        {
            inicializarBanco.TclientesApagar();

            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.ProdutorRural = (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_INICIAL,
                "Produtor Rural inválido!",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Contribuinte_Icms_Status = 99,
                "Se cliente é não Produtor Rural, contribuinte do ICMS tem que ter valor inicial!",
                    TipoPessoa.PF);
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.ProdutorRural = 99,
                "Produtor Rural inválido!",
                    TipoPessoa.PF);
        }


        [Fact]
        public void ProdutorRural_Contribuinte_Icms_zerar_campos()
        {
            inicializarBanco.TclientesApagar();


            /*
	if (s_produtor_rural = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO) Then
		s_contribuinte_icms=COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL
		s_ie = ""
		end if
*/

            //fizemos um pouco diferente: não aceitamos o cadastro
            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.ProdutorRural = (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
            },
                "Se cliente é não Produtor Rural, contribuinte do ICMS tem que ter valor inicial!",
                    TipoPessoa.PF);
        }

        [Fact]
        public void ProdutorRural_Contribuinte_Icms_Status()
        {
            inicializarBanco.TclientesApagar();


            /*
                    if (s_produtor_rural = COD_ST_CLIENTE_PRODUTOR_RURAL_SIM) Then
                        if (s_contribuinte_icms <> COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) Or (s_ie = "") then
                            alerta = "Para ser cadastrado como Produtor Rural, é necessário ser contribuinte do ICMS e possuir nº de IE"
                            end if
                        end if
            */
            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.ProdutorRural = (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_NAO;
                c.DadosCliente.Ie = InicializarClienteDados.ClienteNaoCadastradoPF().DadosCliente.Ie;
            },
                "Para ser cadastrado como Produtor Rural, é necessário ser contribuinte do ICMS e possuir nº de IE",
                TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.ProdutorRural = (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;
                c.DadosCliente.Ie = InicializarClienteDados.ClienteNaoCadastradoPF().DadosCliente.Ie;
            },
                "Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!",
                TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.ProdutorRural = (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                c.DadosCliente.Ie = "";
            },
                "Para ser cadastrado como Produtor Rural e contribuinte do ICMS é necessário possuir nº de IE",
                TipoPessoa.PF);


            /*
		elseif (s_ie="") And (s_contribuinte_icms = COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM) then
			alerta="PREENCHA A INSCRIÇÃO ESTADUAL."
*/
            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                c.DadosCliente.Ie = "";
            },
                "PREENCHA A INSCRIÇÃO ESTADUAL.",
                TipoPessoa.PF);

            /*
                        if s_ie <> "" then
                            if Not isInscricaoEstadualValida(s_ie, s_uf) then
                                alerta="Preencha a IE (Inscrição Estadual) com um número válido!!" & _
                                        "<br>" & "Certifique-se de que a UF informada corresponde à UF responsável pelo registro da IE."
                                end if
                            end if
*/
            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.Ie = "11112222";
            },
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                TipoPessoa.PF);

        }


        [Fact]
        public void Sexo()
        {
            inicializarBanco.TclientesApagar();

            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Sexo = "",
                "GÊNERO DO CLIENTE NÃO INFORMADO!.",
                    TipoPessoa.PF);
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Sexo = "X",
                "INDIQUE QUAL O SEXO.",
                    TipoPessoa.PF);
        }



        [Fact]
        public void Nome()
        {
            inicializarBanco.TclientesApagar();


            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Nome = "",
                "PREENCHA O NOME DO CLIENTE.",
                    TipoPessoa.PF);
        }

        [Fact]
        public void Email_EmailXml()
        {
            inicializarBanco.TclientesApagar();


            /*
		if s_email <> "" then
		'	CONSISTÊNCIA DESATIVADA TEMPORARIAMENTE
'			if Not email_AF_ok(s_email, s_cnpj_cpf, msg_erro_aux) then
'				alerta=texto_add_br(alerta)
'				alerta=alerta & "Endereço de email (" & s_email & ") não é válido!!<br />" & msg_erro_aux
'				end if
			end if
		end if

*/
        }


        [Fact]
        public void Telefones_vazios()
        {
            inicializarBanco.TclientesApagar();


            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddResidencial = "";
                c.DadosCliente.TelefoneResidencial = "";
                c.DadosCliente.DddComercial = "";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "";
                c.DadosCliente.DddCelular = "";
                c.DadosCliente.Celular = "";
            },
                "PREENCHA PELO MENOS UM TELEFONE (RESIDENCIAL, COMERCIAL OU CELULAR).",
                    TipoPessoa.PF);
        }


        [Fact]
        public void Telefones_repetidos()
        {
            inicializarBanco.TclientesApagar();


            //se cadastrarmos mais de 5 vzes, deve dizer que o telefone está repetido


            /*
                elseif verifica_telefones_repetidos(s_ddd_cel, s_tel_cel, s_cnpj_cpf) > NUM_MAXIMO_TELEFONES_REPETIDOS_CAD_CLIENTES then
alerta="TELEFONE CELULAR (" & s_ddd_cel & ") " & s_tel_cel & " JÁ ESTÁ SENDO UTILIZADO NO CADASTRO DE OUTROS CLIENTES. <br>Não foi possível concluir o cadastro."
*/
            //precisamos de ums lista de CPFs
            var listaCpfs = new List<string>();
            listaCpfs.Add("43131718005");
            listaCpfs.Add("75215195900");
            listaCpfs.Add("25176423898");
            listaCpfs.Add("18587605852");
            listaCpfs.Add("30369720059");
            listaCpfs.Add("01986026000");
            listaCpfs.Add("67405762700");

            for (int i = 0; i < Constantes.NUM_MAXIMO_TELEFONES_REPETIDOS_CAD_CLIENTES+1; i++)
            {
                //este é o que deve dar certo
                ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPF();

                //se nao tivermos CPFs suficentes, vai dar exceção. Isto significa que precisamos cadastar mais CPFs
                clienteDto.DadosCliente.Cnpj_Cpf = listaCpfs[i];

                ClienteCadastroResultadoUnisDto res;
                res = clienteUnisBll.CadastrarClienteUnis(clienteDto).Result;

                if (res.ListaErros.Count > 0)
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Empty(res.ListaErros);
            }


            //agora tem que dar o erro
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Nome = listaCpfs[Constantes.NUM_MAXIMO_TELEFONES_REPETIDOS_CAD_CLIENTES + 1],
                "TELEFONE COMERCIAL (19) 2285-9635 JÁ ESTÁ SENDO UTILIZADO NO CADASTRO DE OUTROS CLIENTES. Não foi possível concluir o cadastro.",
                TipoPessoa.PF);
        }



        [Fact]
        public void Telefones_incompletos()
        {
            inicializarBanco.TclientesApagar();


            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddResidencial = "";
                c.DadosCliente.TelefoneResidencial = "12345678";
            },
                "PREENCHA O DDD RESIDENCIAL.",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddResidencial = "1";
                c.DadosCliente.TelefoneResidencial = "12345678";
            },
                "DDD RESIDENCIAL INVÁLIDO.",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "12";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "";
            },
                "PREENCHA O TELEFONE COMERCIAL.",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "12";
                c.DadosCliente.DddCelular = "";
                c.DadosCliente.Celular = "";
            },
                "PREENCHA PELO MENOS UM TELEFONE (RESIDENCIAL, COMERCIAL OU CELULAR).",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "12";
                c.DadosCliente.DddCelular = "12";
                c.DadosCliente.Celular = "12345678";
            },
                "ramal incorreto",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial = "";
                c.DadosCliente.TelComercial = "";
                c.DadosCliente.Ramal = "12";
                c.DadosCliente.DddCelular = "12";
                c.DadosCliente.Celular = "12345678";
            },
                "ramal incorreto",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial2 = "12";
                c.DadosCliente.TelComercial2 = "12345678";
                c.DadosCliente.Ramal = "";
            },
                "pf semcomercial 2",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddComercial2 = "";
                c.DadosCliente.TelComercial2 = "";
                c.DadosCliente.Ramal2 = "12";
            },
                "ramal2 deve ser varzio (ou alguma mensagem de erro)",
                    TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.DddCelular = "";
                c.DadosCliente.Celular = "12345678";
            },
                "PREENCHA O DDD CELULAR.",
                    TipoPessoa.PF);
        }


        [Fact]
        public void Telefones_nao_aceitos()
        {
            inicializarBanco.TclientesApagar();

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.TelComercial2 = "11";
                c.DadosCliente.DddComercial2 = "12345678";
                c.DadosCliente.Ramal2 = "12";
                c.DadosCliente.TelefoneResidencial = "12345678";
            },
            "Se cliente é tipo PF, não pode ter os campos de Telefone e DDD comercial 2 preenchidos!",
                    TipoPessoa.PF);
        }


    }
}
