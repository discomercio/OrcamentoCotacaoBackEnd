using InfraBanco;
using InfraBanco.Constantes;
using Newtonsoft.Json;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisDto.ClienteUnisDto;
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
    public class SucessoEduplicado
    {
        private readonly ClienteUnisBll clienteUnisBll;
        private readonly ITestOutputHelper output;
        private readonly InicializarBancoGeral inicializarBanco;
        private readonly TestesClienteUnisBll testesClienteUnisBll;
        private readonly ContextoBdProvider contextoProvider;

        public SucessoEduplicado(ClienteUnisBll clienteUnisBll, ITestOutputHelper output, InicializarBanco.InicializarBancoGeral inicializarBanco,
            TestesClienteUnisBll testesClienteUnisBll, ContextoBdProvider contextoProvider)
        {
            this.clienteUnisBll = clienteUnisBll;
            this.output = output;
            this.inicializarBanco = inicializarBanco;
            this.testesClienteUnisBll = testesClienteUnisBll;
            this.contextoProvider = contextoProvider;
            testesClienteUnisBll.Output = output;
        }


        [Fact]
        public void CadastrarClienteUnis_Sucesso_PJ_SemRefs()
        {
            inicializarBanco.TclientesApagar();

            //podemos mandar null ou lista vazia em RefComercial e refBancaria
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            ClienteCadastroResultadoUnisDto res;
            clienteDto.RefBancaria = new List<RefBancariaClienteUnisDto>();
            clienteDto.RefComercial = new List<RefComercialClienteUnisDto>();
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto, clienteDto.DadosCliente.Indicador_Orcamentista).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //e apaga o registro
            inicializarBanco.TclientesApagar();


            //========================
            //com null
            clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            clienteDto.RefBancaria = null;
            clienteDto.RefComercial = null;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto, clienteDto.DadosCliente.Indicador_Orcamentista).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }

        [Fact]
        public void CadastrarClienteUnis_Sucesso_PF_SemRefs()
        {
            //podemos mandar null ou lista vazia em RefComercial e refBancaria
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPF();
            ClienteCadastroResultadoUnisDto res;
            clienteDto.RefBancaria = new List<RefBancariaClienteUnisDto>();
            clienteDto.RefComercial = new List<RefComercialClienteUnisDto>();
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto, clienteDto.DadosCliente.Indicador_Orcamentista).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //e apaga o registro
            inicializarBanco.TclientesApagar();


            //========================
            //com null
            clienteDto = InicializarClienteDados.ClienteNaoCadastradoPF();
            clienteDto.RefBancaria = null;
            clienteDto.RefComercial = null;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto, clienteDto.DadosCliente.Indicador_Orcamentista).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }

        [Fact]
        public void CadastrarClienteUnis_Sucesso_PJ()
        {
            //este é o que deve dar certo
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPJ();
            ClienteCadastroResultadoUnisDto res;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto, clienteDto.DadosCliente.Indicador_Orcamentista).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //se cadastrar de novo, tem que dar erro
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Cnpj_Cpf = c.DadosCliente.Cnpj_Cpf,
                Cliente.ClienteBll.MensagensErro.REGISTRO_COM_ID_JA_EXISTE(
                    (from n in contextoProvider.GetContextoLeitura().Tcontroles where n.Id_Nsu == Constantes.NSU_CADASTRO_CLIENTES select n.Nsu).First()
                    ),
                TipoPessoa.PJ);

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }

        [Fact]
        public void CadastrarClienteUnis_Sucesso_PF()
        {
            //este é o que deve dar certo
            ClienteCadastroUnisDto clienteDto = InicializarClienteDados.ClienteNaoCadastradoPF();
            ClienteCadastroResultadoUnisDto res;
            res = clienteUnisBll.CadastrarClienteUnis(clienteDto, clienteDto.DadosCliente.Indicador_Orcamentista).Result;

            if (res.ListaErros.Count > 0)
                output.WriteLine(JsonConvert.SerializeObject(res));

            Assert.Empty(res.ListaErros);

            //se cadastrar de novo, tem que dar erro
            testesClienteUnisBll.TestarCadastro(c => c.DadosCliente.Cnpj_Cpf = c.DadosCliente.Cnpj_Cpf,
                Cliente.ClienteBll.MensagensErro.REGISTRO_COM_ID_JA_EXISTE(
                    (from n in contextoProvider.GetContextoLeitura().Tcontroles where n.Id_Nsu == Constantes.NSU_CADASTRO_CLIENTES select n.Nsu).First()
                    ),
                TipoPessoa.PF);

            //e apaga o registro
            inicializarBanco.TclientesApagar();
        }
    }
}
