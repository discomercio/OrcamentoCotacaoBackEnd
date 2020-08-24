using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Ambiente.ApiUnis.PrepedidoUnis.CadastrarPrepedido
{
    public class Pedido : Testes.Utils.ExecucaoCruzada.ListaEspecificacoes, Testes.Pedido.IPedidoSteps
    {
        private readonly CadastrarPrepedido cadastrarPrepedido = new CadastrarPrepedido();
        private static readonly List<string> especificacoes = new List<string>();
        private string ambiente;
        public Pedido(string especificacao, string ambiente) : base(especificacao)
        {
            especificacoes.Add(especificacao);
            this.ambiente = ambiente;
        }
        public static void ThenVerificarQueExecutou(string especificacao)
        {
            //este teste somente passa se executar todos os testes
            Assert.Contains(especificacao, especificacoes);
            VerificarQueExecutou(especificacao);
        }

        public void WhenInformo(string p0, string p1)
        {
            cadastrarPrepedido.WhenInformo(p0, p1);
        }


        public void WhenPedidoBase()
        {
            cadastrarPrepedido.GivenPrepedidoBase();
        }

        public void ThenNoAmbienteErro(string ambiente, string erro)
        {
            if (this.ambiente != ambiente)
                return;
            cadastrarPrepedido.ThenErro(erro);
        }

        public void ThenNoAmbienteSemErro(string ambiente, string erro)
        {
            if (this.ambiente != ambiente)
                return;
            cadastrarPrepedido.ThenSemErro(erro);
        }
    }
}
