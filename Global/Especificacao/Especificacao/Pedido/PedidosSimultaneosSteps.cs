using InfraBanco;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Especificacao.Pedido
{
    [Binding, Scope(Tag = "Especificacao.Pedido.PedidosSimultaneos")]
    public class PedidosSimultaneosSteps
    {
        private readonly ContextoBdProvider contextoBdProvider;
        public PedidosSimultaneosSteps()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            this.contextoBdProvider = servicos.GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        [Given(@"Testar pedidos simultâneos")]
        public void GivenTestarPedidosSimultaneos()
        {


            //conta o número de pedidos
            var pedidos = (from p in contextoBdProvider.GetContextoLeitura().Tpedidos select p).Count();
            var multiplicadorPorPedido = 2; //magento e loja
            var pedidosPorThread = 4; //magento e loja
            var numeroThreads = 10;

            CadastrarPedidoIncialECliente();
            var threads = new List<Thread>();
            CriarThreads(pedidosPorThread, numeroThreads, threads);
            IniciarTHreads(threads);
            EsperarThreads(threads);


            //verifica o total de pedidos criados
            var novosPedidos = (from p in contextoBdProvider.GetContextoLeitura().Tpedidos select p).Count();
            Assert.Equal(pedidos + numeroThreads * pedidosPorThread * multiplicadorPorPedido, novosPedidos);
        }

        private static void CriarThreads(int pedidosPorThread, int numeroThreads, List<Thread> threads)
        {
            for (var i = 0; i < numeroThreads; i++)
            {
                threads.Add(new Thread(s =>
                {
                    PedidoSteps pedidoSteps = new PedidoSteps();
                    pedidoSteps.GivenIgnorarCenarioNoAmbiente("Especificacao.Prepedido.PrepedidoSteps");
                    //fazemos um pedido antes para o magento cadastrar o cliente
                    for (var i2 = 0; i2 < pedidosPorThread - 1; i2++)
                    {
                        Thread.Sleep(10);
                        pedidoSteps.GivenPedidoBase();
                        pedidoSteps.ThenSemNenhumErro();
                    }
                }));
            }
        }

        private static void IniciarTHreads(List<Thread> threads)
        {
            //inicia todas
            foreach (var t in threads)
                t.Start();
        }


        private static void EsperarThreads(List<Thread> threads)
        {
            Thread.Sleep(10);

            //espera todas terminarem
            foreach (var t in threads)
                t.Join();
        }


        private static void CadastrarPedidoIncialECliente()
        {
            /*
            como o magento cadastra o cliente se não existir, nesse processo dá sim problema com multiplas threads:
            ele verifica que o cliente não está cadastrado, começa a cadastrar o cliente,
            nisso outra thread verifica que não está cadastrado, começa a cadastrar e dá erro porque a primeira thread terminou de cadastrar
            */

            PedidoSteps pedidoSteps = new PedidoSteps();
            pedidoSteps.GivenIgnorarCenarioNoAmbiente("Especificacao.Prepedido.PrepedidoSteps");
            pedidoSteps.GivenPedidoBase();
            pedidoSteps.ThenSemNenhumErro();
        }
    }
}
