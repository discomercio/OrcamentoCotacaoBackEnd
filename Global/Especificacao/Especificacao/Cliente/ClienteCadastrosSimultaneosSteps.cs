using InfraBanco;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;

namespace Especificacao.Especificacao.Cliente
{
    [Binding, Scope(Tag = "Especificacao.Cliente.ClienteCadastrosSimultaneos")]
    public class ClienteCadastrosSimultaneosSteps
    {
        private readonly ContextoBdProvider contextoBdProvider;
        private readonly ServiceProvider serviceProvider;
        private readonly string jsonClienteCadastro = "{\"DadosCliente\":{\"Loja\":\"201\",\"Indicador_Orcamentista\":\"FRETE\",\"Vendedor\":\"USRMAG\",\"Id\":\"\",\"Cnpj_Cpf\":\"14039603052\",\"Rg\":\"\",\"Ie\":\"\",\"Contribuinte_Icms_Status\":0,\"Tipo\":\"PF\",\"Observacao_Filiacao\":null,\"Nascimento\":null,\"Sexo\":\"\",\"Nome\":\"Vivian\",\"ProdutorRural\":1,\"Endereco\":\"Rua Professor F\\u00E1bio Fanucchi\",\"Numero\":\"97\",\"Complemento\":\"\",\"Bairro\":\"Jardim S\\u00E3o Paulo(Zona Norte)\",\"Cidade\":\"S\\u00E3o Paulo\",\"Uf\":\"SP\",\"Cep\":\"02045080\",\"DddResidencial\":\"11\",\"TelefoneResidencial\":\"11111111\",\"DddComercial\":\"11\",\"TelComercial\":\"12345678\",\"Ramal\":\"\",\"DddCelular\":\"11\",\"Celular\":\"981603313\",\"TelComercial2\":\"\",\"DddComercial2\":\"\",\"Ramal2\":\"\",\"Email\":\"testeCad@Gabriel.com\",\"EmailXml\":\"\",\"Contato\":\"\"},\"RefBancaria\":[],\"RefComercial\":[]}";

        public ClienteCadastrosSimultaneosSteps()
        {
            serviceProvider = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            this.contextoBdProvider = serviceProvider.GetRequiredService<InfraBanco.ContextoBdProvider>();
        }

        [Given(@"Testar cadastros simultâneos com pedidosPorThread = ""(.*)"" e numeroThreads = ""(.*)""")]
        public void GivenTestarCadastrosSimultaneosComPedidosPorThreadENumeroThreads(int pedidosPorThread, int numeroThreads)
        {
            var clientes = (from p in contextoBdProvider.GetContextoLeitura().Tcliente select p).Count();
            PrepararBancoDados();

            var threads = new List<Thread>();
            CriarThreads(pedidosPorThread, numeroThreads, threads);
            IniciarTHreads(threads);
            EsperarThreads(threads);


            //verifica o total de pedidos criados
            var novosClientes = (from p in contextoBdProvider.GetContextoLeitura().Tcliente select p).Count();
            Assert.Equal(clientes + 1, novosClientes);

            Assert.Equal(1, totalCadastrado);
            Assert.Equal(pedidosPorThread * numeroThreads - 1, totalComErro);
            Assert.Equal(0, ocorreuExecao);
        }

        private static void PrepararBancoDados()
        {
            var servicos = Testes.Utils.InjecaoDependencia.ProvedorServicos.ObterServicos();
            //precisa restaurar o banco
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(servicos.GetRequiredService<InfraBanco.ContextoBdProvider>(), servicos.GetRequiredService<InfraBanco.ContextoCepProvider>());
            bd.InicializarForcado();

            //dormimos um pouco
            //se não fizermos isso, temos erros de timeout no acesso ao banco por causa do multithread
            //porque o nosso gerenciador de serviços não é thread-safe
            //de qualquer forma, aqui não queremos testar o multithread do ambiente de testes, e sim
            //o multithread da criação do pedido
            Thread.Sleep(300);
        }

        private int totalCadastrado = 0;
        private int totalComErro = 0;
        private int ocorreuExecao = 0;
        private object _lockObjectCriarThreads = new object();
        private void CriarThreads(int pedidosPorThread, int numeroThreads, List<Thread> threads)
        {
            for (var i = 0; i < numeroThreads; i++)
            {
                threads.Add(new Thread(s =>
                {
                    //cada um com a sua instância, que é instanciada com lock porque o nosso gerenciador de serviços não é multi-thread
                    global::Cliente.ClienteBll clienteBll;
                    lock (_lockObjectCriarThreads)
                    {
                        clienteBll = serviceProvider.GetRequiredService<global::Cliente.ClienteBll>();
                    }
                    global::Cliente.Dados.ClienteCadastroDados clienteCadastroDados = Newtonsoft.Json.JsonConvert.DeserializeObject<global::Cliente.Dados.ClienteCadastroDados>(jsonClienteCadastro);
                    for (var i2 = 0; i2 < pedidosPorThread; i2++)
                    {
                        try
                        {
                            //dorme um tempo aleatório apra deslocar as threads
                            Thread.Sleep(new Random().Next(1, 50));
                            Testes.Utils.LogTestes.LogTestes.LogMensagemOperacao($"Especificacao.Cliente.ClienteCadastrosSimultaneos iniciar pedido thread {Thread.CurrentThread.ManagedThreadId} número {i2}", this.GetType());
                            var erros = clienteBll.CadastrarCliente(clienteCadastroDados, "indicador", InfraBanco.Constantes.Constantes.CodSistemaResponsavel.COD_SISTEMA_RESPONSAVEL_CADASTRO__API_MAGENTO, "usuario").Result.listaErros;
                            lock (_lockObjectCriarThreads)
                            {
                                if (!erros.Any())
                                    this.totalCadastrado++;
                                else
                                    this.totalComErro++;
                            }
                        }
                        catch (Exception e)
                        {
                            RegistrarExcecao(e);
                            lock (_lockObjectCriarThreads)
                                this.ocorreuExecao++;
                            //não damos o throw;
                        }
                    }
                }));
            }
        }

        private static void RegistrarExcecao(Exception e)
        {
            var msg = $"EXCECAO: ERRO: na criacao do cliente: {e.Message} {e.StackTrace} {e.ToString()}";
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
