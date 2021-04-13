using Especificacao.Testes.Utils.ListaDependencias;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente
{
    [Binding, Scope(Tag = "Ambiente.ApiMagento.PedidoMagento.CadastrarPedido.CriacaoCliente")]
    public class CriacaoClienteSteps
    {
        private readonly CadastrarPedido cadastrarPedido = new CadastrarPedido();

        public CriacaoClienteSteps()
        {
        }

        [Given(@"Pedido base")]
        public void GivenPedidoBase()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBase(this);
            cadastrarPedido.GivenDadoBase();
        }
        [Given(@"Pedido base cliente PJ")]
        [When(@"Pedido base cliente PJ")]
        public void GivenPedidoBaseClientePJ()
        {
            Testes.Utils.LogTestes.LogOperacoes2.DadoBaseClientePJ(this);
            cadastrarPedido.GivenPedidoBaseClientePJ();
        }

        [Given(@"Limpar endereço de entrega")]
        public void GivenLimparEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.LimparEnderecoDeEntrega(this);
            cadastrarPedido.LimparEnderecoDeEntrega();
        }

        [Given(@"Limpar dados cadastrais e endereço de entrega")]
        public void GivenLimparDadosCadastraisEEnderecoDeEntrega()
        {
            Testes.Utils.LogTestes.LogOperacoes2.LimparDadosCadastraisEEnderecoDeEntrega(this);
            cadastrarPedido.LimparDadosCadastraisEEnderecoDeEntrega();
        }


        [Given(@"Informo ""(.*)"" = ""(.*)""")]
        [When(@"Informo ""(.*)"" = ""(.*)""")]
        public void GivenInformo(string p0, string p1)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1, this);
            cadastrarPedido.WhenInformo(p0, p1);
        }

        [When(@"Informo ""(.*)"" com ""(.*)"" caracteres")]
        public void WhenInformoCaracteres(string p0, int p1)
        {
            //                       10        20        30        40        50        60        70        80   
            string texto = "";
            for (int i = 0; texto.Length < p1; i++)
            {
                i = i == 10 ? 0 : i;
                texto += i.ToString();
            }

            Testes.Utils.LogTestes.LogOperacoes2.Informo(p0, p1.ToString(), this);
            cadastrarPedido.WhenInformo(p0, texto);
        }


        [Then(@"Erro ""(.*)""")]
        public void ThenErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.Erro(p0, this);
            cadastrarPedido.ThenErro(p0);
        }

        [Then(@"Sem erro ""(.*)""")]
        public void ThenSemErro(string p0)
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemErro(p0, this);
            cadastrarPedido.ThenSemErro(p0);
        }

        [Then(@"Sem nenhum erro")]
        public void ThenSemNenhumErro()
        {
            Testes.Utils.LogTestes.LogOperacoes2.SemNenhumErro(this);
            cadastrarPedido.ThenSemNenhumErro();
        }

        private readonly Testes.Utils.BancoTestes.GerenciamentoBancoSteps gerenciamentoBanco = new Testes.Utils.BancoTestes.GerenciamentoBancoSteps();

        [Then(@"Tabela ""t_PEDIDO"" registro criado, verificar campo ""(.*)"" = ""(.*)""")]
        public void ThenTabelaT_PEDIDORegistroCriadoVerificarCampo(string campo, string valor)
        {
            var dto = cadastrarPedido.UltimoPedidoResultadoMagentoDto();
            if (dto == null)
                throw new Exception("erro");
            Assert.Empty(dto.ListaErros);

            List<string> somentePai = new List<string>()
                { dto.IdPedidoCadastrado??"" };

            gerenciamentoBanco.TabelaT_PEDIDORegistroVerificarCampo(somentePai, campo, valor);
        }

    }
}
