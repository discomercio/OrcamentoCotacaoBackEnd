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

            //agora validado, não pode ter o erro
            TestarCadastro(c => c.DadosCliente.Ie = c.DadosCliente.Ie,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                TipoPessoa.PJ,
                false);
        }


        [Fact]
        public void CadastrarClienteUnis_Cidade_Nfe()
        {
            TestarCadastro(c => c.DadosCliente.Cidade = "Abacate da Pedreira",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Municipio_nao_consta_na_relacao_IBGE("Abacate da Pedreira", 
                InicializarClienteDados.ClienteNaoCadastradoPJ().DadosCliente.Uf), TipoPessoa.PJ);
        }

        [Fact]
        public void CadastrarClienteUnis_Cep()
        {
            TestarCadastro(c => c.DadosCliente.Cep = InicializarBancoCep.DadosCep.CepNaoExiste,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Cep_nao_existe,
                TipoPessoa.PJ);
        }
        [Fact]
        public void CadastrarClienteUnis_Estado()
        {
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

            //sempre apaga o regsitro
            inicializarBanco.TclientesApagar();

            if (incluirEsteErro)
            {
                if (!res.ListaErros.Contains(mensagemErro))
                    if(output!= null)
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
