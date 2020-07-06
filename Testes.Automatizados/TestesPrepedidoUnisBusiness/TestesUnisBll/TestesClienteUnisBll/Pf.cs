using InfraBanco.Constantes;
using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using PrepedidoBusiness.Bll.ClienteBll;
using System;
using System.Collections.Generic;
using System.Text;
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

        public Pf(TestesClienteUnisBll testesClienteUnisBll, ITestOutputHelper output, PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll.ClienteUnisBll clienteUnisBll,
            InicializarBanco.InicializarBancoGeral inicializarBanco)
        {
            this.testesClienteUnisBll = testesClienteUnisBll;
            this.output = output;
            this.clienteUnisBll = clienteUnisBll;
            this.inicializarBanco = inicializarBanco;
            this.testesClienteUnisBll.Output = output;
        }

        [Fact]
        public void RefBancaria()
        {
            //nao pode ter
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            testesClienteUnisBll.TestarCadastro(c => c.RefBancaria.Add(new RefBancariaClienteUnisDto()),
                PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll.MensagensErro.Orcamentista_nao_existe,
                TipoPessoa.PF);
        }

        [Fact]
        public void RefComercial()
        {
            //nao pode ter
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            testesClienteUnisBll.TestarCadastro(c => c.RefComercial.Add(new RefComercialClienteUnisDto()),
                PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll.MensagensErro.Orcamentista_nao_existe,
                TipoPessoa.PF);
        }


        [Fact]
        public void Indicador_Orcamentista()
        {
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Indicador_Orcamentista = InicializarBancoGeral.Dados.Orcamentista.ApelidoNaoExiste,
                PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll.MensagensErro.Orcamentista_nao_existe,
                TipoPessoa.PF);
        }
        [Fact]
        public void Cnpj_Cpf()
        {
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
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Tipo = "",
                ValidacoesClienteBll.MensagensErro.INFORME_SE_O_CLIENTE_E_PF_OU_PJ,
                TipoPessoa.PF);
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Tipo = "XX",
                ValidacoesClienteBll.MensagensErro.Tipo_de_cliente_nao_e_PF_nem_PJ,
                TipoPessoa.PF);
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Tipo = "PJ",
                ValidacoesClienteBll.MensagensErro.CNPJ_INVALIDO,
                TipoPessoa.PF);
        }

        [Fact]
        public void ProdutorRural_Contribuinte_Icms_Status_codigos_invalidos()
        {
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Contribuinte_Icms_Status = 99,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                    TipoPessoa.PF);
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.ProdutorRural = 99,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                    TipoPessoa.PF);
        }


        [Fact]
        public void ProdutorRural_Contribuinte_Icms_zerar_campos()
        {
            /*
	if (s_produtor_rural = COD_ST_CLIENTE_PRODUTOR_RURAL_NAO) Then
		s_contribuinte_icms=COD_ST_CLIENTE_CONTRIBUINTE_ICMS_INICIAL
		s_ie = ""
		end if
*/

            //este é o que deve dar certo
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPF();
            ClienteCadastroResultadoUnisDto res;
            var c = clienteDto;
            c.DadosCliente.ProdutorRural = (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_NAO;
            c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
            c.DadosCliente.Ie = InicializarClienteDados.ClienteNaoCadastradoPF().DadosCliente.Ie;

            res = clienteUnisBll.CadastrarClienteUnis(clienteDto).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //todo: verificar se salvou com os valores corretos
            Assert.True(false);

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }

        [Fact]
        public void ProdutorRural_Contribuinte_Icms_Status()
        {

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
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                TipoPessoa.PF);

            testesClienteUnisBll.TestarCadastro(c =>
            {
                c.DadosCliente.ProdutorRural = (byte)Constantes.ProdutorRual.COD_ST_CLIENTE_PRODUTOR_RURAL_SIM;
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                c.DadosCliente.Ie = "";
            },
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
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
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
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


        /*
         * todo: testes
         *         public DadosClienteCadastroUnisDto DadosCliente { get; set; }        
         *         
         *         


        /// <summary>
        /// Sexo = "M", "F"
        /// </summary>
        [MaxLength(1)]
        public string Sexo { get; set; }

        [MaxLength(60)]
        [Required]
        public string Nome { get; set; }

        [MaxLength(80)]
        public string Endereco { get; set; }

        [MaxLength(20)]
        public string Numero { get; set; }

        [MaxLength(60)]
        public string Complemento { get; set; }

        [MaxLength(72)]
        public string Bairro { get; set; }

        [MaxLength(60)]
        public string Cidade { get; set; }

        [MaxLength(2)]
        public string Uf { get; set; }

        [MaxLength(8)]
        public string Cep { get; set; }

        [MaxLength(4)]
        public string DddResidencial { get; set; }

        [MaxLength(11)]
        public string TelefoneResidencial { get; set; }

        [MaxLength(4)]
        public string DddComercial { get; set; }

        [MaxLength(11)]
        public string TelComercial { get; set; }

        [MaxLength(4)]
        public string Ramal { get; set; }

        [MaxLength(2)]
        public string DddCelular { get; set; }

        [MaxLength(9)]
        public string Celular { get; set; }

        [MaxLength(9)]
        public string TelComercial2 { get; set; }

        [MaxLength(2)]
        public string DddComercial2 { get; set; }

        [MaxLength(4)]
        public string Ramal2 { get; set; }

        [MaxLength(60)]
        public string Email { get; set; }

        [MaxLength(60)]
        public string EmailXml { get; set; }

        [MaxLength(30)]
        public string Contato { get; set; }


    * 
    * 
    * 
    * *         
    */

    }
}
