﻿using InfraBanco;
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

        [Given(@"Testar pedidos simultâneos com multiplicadorPorPedido = ""(.*)"" \(magento e loja\), pedidosPorThread = ""(.*)"" e numeroThreads = ""(.*)""")]
        public void GivenTestarPedidosSimultaneos(int multiplicadorPorPedido, int pedidosPorThread, int numeroThreads)
        {
            //conta o número de pedidos
            var pedidos = (from p in contextoBdProvider.GetContextoLeitura().Tpedido select p).Count();

            PrepararBancoDados();

            var threads = new List<Thread>();
            CriarThreads(pedidosPorThread, numeroThreads, threads);
            IniciarTHreads(threads);
            EsperarThreads(threads);


            //verifica o total de pedidos criados
            var novosPedidos = (from p in contextoBdProvider.GetContextoLeitura().Tpedido select p).Count();
            Assert.Equal(pedidos + multiplicadorPorPedido + numeroThreads * pedidosPorThread * multiplicadorPorPedido, novosPedidos);
        }

        private static void PrepararBancoDados()
        {
            //criamos uma vez o pedido na mesma thread.
            //se não fizermos isso, temos erros de timeout no acesso ao banco por causa do multithread
            //porque o nosso gerenciador de serviços não é thread-safe
            //de qualquer forma, aqui não queremos testar o multithread do ambiente de testes, e sim
            //o multithread da criação do pedido
            {
                PedidoSteps pedidoSteps = new PedidoSteps();
                pedidoSteps.GivenIgnorarCenarioNoAmbiente("Especificacao.Prepedido.PrepedidoSteps");
                try
                {
                    Testes.Utils.LogTestes.LogTestes.LogMensagemOperacao($"CriarThreads iniciar pedido thread {Thread.CurrentThread.ManagedThreadId} na thread principal", typeof(PedidosSimultaneosSteps));
                    pedidoSteps.GivenPedidoBase();
                    pedidoSteps.ThenSemNenhumErro();
                }
                catch (Exception e)
                {
                    RegistrarExcecao(e);
                    //não damos o throw;
                }
            }
        }

        private static void CriarThreads(int pedidosPorThread, int numeroThreads, List<Thread> threads)
        {
            for (var i = 0; i < numeroThreads; i++)
            {
                threads.Add(new Thread(s =>
                {
                    for (var i2 = 0; i2 < pedidosPorThread; i2++)
                    {
                        PedidoSteps pedidoSteps = new PedidoSteps();
                        pedidoSteps.GivenIgnorarCenarioNoAmbiente("Especificacao.Prepedido.PrepedidoSteps");
                        try
                        {
                            //dorme um tempo aleatório apra deslocar as threads
                            Thread.Sleep(new Random().Next(1, 50));
                            Testes.Utils.LogTestes.LogTestes.LogMensagemOperacao($"CriarThreads iniciar pedido thread {Thread.CurrentThread.ManagedThreadId} número {i2}", typeof(PedidosSimultaneosSteps));
                            pedidoSteps.GivenPedidoBase();
                            pedidoSteps.ThenSemNenhumErro();
                        }
                        catch (Exception e)
                        {
                            RegistrarExcecao(e);
                            //não damos o throw;
                        }
                    }
                }));
            }
        }

        private static void RegistrarExcecao(Exception e)
        {
            var msg = $"EXCECAO: ERRO: na criacao do pedido: {e.Message} {e.StackTrace} {e.ToString()}";
            Testes.Utils.LogTestes.LogTestes.ErroNosTestes(msg);
        }

        private static void IniciarTHreads(List<Thread> threads)
        {
            //inicia todas
            foreach (var t in threads)
            {
                t.Start();
                //se estivermos usando o banco em memória, ele não faz bloqueio nem suporta transações, e vai dar erro
                //então, na prática, desabilitamos o teste de simultaneidade se for o banco de memória
                if (!Testes.Utils.InjecaoDependencia.ProvedorServicos.UsarSqlServerNosTestesAutomatizados)
                    t.Join();
            }
        }


        private static void EsperarThreads(List<Thread> threads)
        {
            Thread.Sleep(10);

            //espera todas terminarem
            foreach (var t in threads)
                t.Join();
        }

    }
}
