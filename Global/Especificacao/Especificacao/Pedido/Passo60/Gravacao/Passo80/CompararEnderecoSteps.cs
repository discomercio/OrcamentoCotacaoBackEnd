using System;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo80
{
    [Binding, Scope(Tag = "@Especificacao.Pedido.Passo60.Gravacao.Passo80.CompararEndereco.feature")]
    public class CompararEnderecoSteps
    {
        private class CompararEnderecoDados
        {
            public string? End_logradouro_1 { get; set; }
            public string? End_numero_1 { get; set; }
            public string? End_cep_1 { get; set; }
            public string? End_logradouro_2 { get; set; }
            public string? End_numero_2 { get; set; }
            public string? End_cep_2 { get; set; }
        }
        private readonly CompararEnderecoDados dados = new CompararEnderecoDados();

        [When(@"IsEnderecoIgual: ""(.*)"" *= ""(.*)""")]
        public void WhenIsEnderecoIgual(string campo, string valor)
        {
            if (Testes.Utils.WhenInformoCampo.InformarCampo(campo, valor, dados))
                return;
            Assert.Equal("", $"{campo} desconhecido na rotina Especificacao.Especificacao.Pedido.Passo60.Gravacao.Passo80.CompararEnderecoSteps");
        }

        [Then(@"IsEnderecoIgual: sim")]
        public void ThenIsEnderecoIgualSim()
        {
            var ret = global::Pedido.Criacao.Passo60.Gravacao.Grava80.CompararEndereco.IsEnderecoIgual(
                dados.End_logradouro_1, dados.End_numero_1, dados.End_cep_1,
                dados.End_logradouro_2, dados.End_numero_2, dados.End_cep_2);
            Assert.True(ret);
        }

        [Then(@"IsEnderecoIgual: não")]
        public void ThenIsEnderecoIgualNao()
        {
            var ret = global::Pedido.Criacao.Passo60.Gravacao.Grava80.CompararEndereco.IsEnderecoIgual(
                dados.End_logradouro_1, dados.End_numero_1, dados.End_cep_1,
                dados.End_logradouro_2, dados.End_numero_2, dados.End_cep_2);
            Assert.False(ret);
        }
    }
}
