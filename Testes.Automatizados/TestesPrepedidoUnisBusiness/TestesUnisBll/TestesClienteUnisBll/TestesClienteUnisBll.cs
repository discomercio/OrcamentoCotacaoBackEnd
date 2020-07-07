using InfraBanco.Constantes;
using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
using System;
using System.Collections.Generic;
using System.Text;
using Testes.Automatizados.InicializarBanco;
using Xunit;
using Xunit.Abstractions;

namespace Testes.Automatizados.TestesPrepedidoUnisBusiness.TestesUnisBll.TestesClienteUnisBll
{
    [Collection("Testes não multithread porque o banco é unico")]
    public class TestesClienteUnisBll
    {
        private readonly ClienteUnisBll clienteUnisBll;
        private readonly InicializarBanco.InicializarBancoGeral inicializarBanco;

        //quando é instanciado não diretamente pelo executor dos testes, não resolve esta dependência
        public ITestOutputHelper Output { get => output; set => output = value; }
        private ITestOutputHelper output;

        public TestesClienteUnisBll(ClienteUnisBll clienteUnisBll, InicializarBanco.InicializarBancoGeral inicializarBanco)
        {
            this.clienteUnisBll = clienteUnisBll;
            this.inicializarBanco = inicializarBanco;
            output = null;
        }


        [Fact]
        public void CadastrarClienteUnis_IE_ICMS()
        {
            inicializarBanco.TclientesApagar();

            //if (qtdeDig < 2 && qtdeDig > 14)
            TestarCadastro(c => c.DadosCliente.Ie = "1",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                TipoPessoa.PJ);
            TestarCadastro(c => c.DadosCliente.Ie = "1",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                tipoPessoa: TipoPessoa.PF);

            TestarCadastro(c => c.DadosCliente.Ie = "11223344",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                TipoPessoa.PJ);
            TestarCadastro(c => c.DadosCliente.Ie = "11223344",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                tipoPessoa: TipoPessoa.PF);

            /*
            As situações possíveis seriam:

            Contribuinte ICMS = SIM -> Obrigatório ter IE

            Contribuinte ICMS = NÃO -> Pode ou não ter IE

            Contribuinte ICMS = ISENTO -> Não tem IE
            */

            TestarCadastro(c =>
            {
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                c.DadosCliente.Ie = "";
            },
                "PREENCHA A INSCRIÇÃO ESTADUAL.",
                TipoPessoa.PJ);

            TestarCadastro(c =>
            {
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_SIM;
                c.DadosCliente.Ie = "ISENTO";
            },
                "Se cliente é contribuinte do ICMS, não pode ter o valor ISENTO no campo de Inscrição Estadual!",
                TipoPessoa.PJ);

            TestarCadastro(c =>
            {
                c.DadosCliente.Contribuinte_Icms_Status = (byte)Constantes.ContribuinteICMS.COD_ST_CLIENTE_CONTRIBUINTE_ICMS_ISENTO;
                c.DadosCliente.Ie = InicializarClienteDados.ClienteNaoCadastradoPF().DadosCliente.Ie;
            },
                "Se o Contribuinte ICMS é isento, o campo IE deve ser vazio!",
                TipoPessoa.PJ);



            //agora validado, não pode ter o erro
            TestarCadastro(c => c.DadosCliente.Ie = c.DadosCliente.Ie,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                TipoPessoa.PJ,
                false);
        }

        [Fact]
        public void Endereco_endereco()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Endereco = "",
                "PREENCHA O ENDEREÇO.", TipoPessoa.PJ);
        }

        [Fact]
        public void Endereco_Numero()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Numero = "",
                "PREENCHA O NÚMERO DO ENDEREÇO.", TipoPessoa.PJ);
        }
        [Fact]
        public void Endereco_Bairro()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Bairro = "",
                "PREENCHA O BAIRRO.", TipoPessoa.PJ);
        }
        [Fact]
        public void Endereco_Cidade()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Cidade = "",
                "PREENCHA A CIDADE.", TipoPessoa.PJ);
        }
        [Fact]
        public void Endereco_Uf()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Uf = "",
                "INFORME O UF.", TipoPessoa.PJ);
        }
        [Fact]
        public void Endereco_Cep()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Cep = "",
                "INFORME O CEP.", TipoPessoa.PJ);
        }


        [Fact]
        public void CadastrarClienteUnis_Cidade_Nfe()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Cidade = "Abacate da Pedreira",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Municipio_nao_consta_na_relacao_IBGE("Abacate da Pedreira",
                InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Uf), TipoPessoa.PJ);
        }

        [Fact]
        public void CadastrarClienteUnis_Cep()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Cep = InicializarBancoCep.DadosCep.CepNaoExiste,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Cep_nao_existe,
                TipoPessoa.PJ);
        }
        [Fact]
        public void CadastrarClienteUnis_Estado()
        {
            inicializarBanco.TclientesApagar();

            TestarCadastro(c => c.DadosCliente.Uf = InicializarBancoCep.DadosCep.Ufe_sgNaoExiste,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Estado_nao_confere,
                TipoPessoa.PJ);
        }


        internal delegate void DeixarDtoErrado(ClienteCadastroUnisDto clienteDto);
        internal enum TipoPessoa { PF, PJ };
        internal void TestarCadastro(DeixarDtoErrado deixarDtoErrado, string mensagemErro, TipoPessoa tipoPessoa, bool incluirEsteErro = true)
        {
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            if (tipoPessoa == TipoPessoa.PF)
                clienteDto = InicializarClienteDados.ClienteNaoCadastradoPF();
            deixarDtoErrado(clienteDto);

            ClienteCadastroResultadoUnisDto res;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto).Result;

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
    }
}
