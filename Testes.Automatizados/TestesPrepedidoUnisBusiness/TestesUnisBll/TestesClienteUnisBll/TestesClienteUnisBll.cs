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
    public class TestesClienteUnisBll
    {
        private readonly ClienteUnisBll clienteUnisBll;
        private readonly ITestOutputHelper output;
        private readonly InicializarBanco.InicializarBancoGeral inicializarBanco;

        public TestesClienteUnisBll(ClienteUnisBll clienteUnisBll, ITestOutputHelper output, InicializarBanco.InicializarBancoGeral inicializarBanco)
        {
            this.clienteUnisBll = clienteUnisBll;
            this.output = output;
            this.inicializarBanco = inicializarBanco;
        }

        /*
         * TODO: terminar testes
         */
        [Fact]
        public void CadastrarClienteUnis_Sucesso()
        {
            //este é o que deve dar certo
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastrado();
            ClienteCadastroResultadoUnisDto res;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);
        }

        [Fact]
        public void CadastrarClienteUnis_IE_ICMS()
        {
            //if (qtdeDig < 2 && qtdeDig > 14)
            TestarCadastro(c => c.DadosCliente.Ie = "1",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual);

            TestarCadastro(c => c.DadosCliente.Ie = "11223344",
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual);

            //se nao for produtor rural, não pode ter IE
            TestarCadastro(c => c.DadosCliente.ProdutorRural = 1,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual);

            //se produtor rural, precisa de ICMS
            TestarCadastro(c => c.DadosCliente.Contribuinte_Icms_Status = 1,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual);
            TestarCadastro(c => c.DadosCliente.Contribuinte_Icms_Status = 3,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual);

            //agora validado, não pode ter o erro
            TestarCadastro(c => c.DadosCliente.Ie = c.DadosCliente.Ie,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Preencha_a_IE_Inscricao_Estadual,
                false);
        }


        [Fact]
        public void CadastrarClienteUnis_Orcamentista()
        {
            TestarCadastro(c => c.DadosCliente.Indicador_Orcamentista = InicializarBancoGeral.Dados.Orcamentista.ApelidoNaoExiste,
                PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll.PrePedidoUnisBll.MensagensErro.Orcamentista_nao_existe);
        }

        [Fact]
        public void CadastrarClienteUnis_Cep()
        {
            TestarCadastro(c => c.DadosCliente.Cep = InicializarBancoCep.DadosCep.CepNaoExiste,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Cep_nao_existe);
        }
        [Fact]
        public void CadastrarClienteUnis_Estado()
        {
            TestarCadastro(c => c.DadosCliente.Uf = InicializarBancoCep.DadosCep.Ufe_sgNaoExiste,
                PrepedidoBusiness.Bll.ClienteBll.ValidacoesClienteBll.MensagensErro.Estado_nao_confere);
        }


        private delegate void DeixarDtoErrado(ClienteCadastroUnisDto clienteDto);
        private void TestarCadastro(DeixarDtoErrado deixarDtoErrado, string mensagemErro, bool incluirEsteErro = true)
        {
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastrado();
            deixarDtoErrado(clienteDto);

            ClienteCadastroResultadoUnisDto res;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto).Result;

            if (incluirEsteErro)
            {
                if (!res.ListaErros.Contains(mensagemErro))
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.Contains(mensagemErro, res.ListaErros);
            }
            else
            {
                if (res.ListaErros.Contains(mensagemErro))
                    output.WriteLine(JsonConvert.SerializeObject(res));
                Assert.DoesNotContain(mensagemErro, res.ListaErros);
            }
        }
    }
}
