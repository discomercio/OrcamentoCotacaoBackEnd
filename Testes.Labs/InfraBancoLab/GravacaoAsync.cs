using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Testes.Labs.InfraBancoLab
{
    class GravacaoAsync
    {
        private readonly Contextos contextos;
        private readonly List<string> filtroClientes;

        public GravacaoAsync(Contextos contextos, List<string> filtroClientes)
        {
            this.contextos = contextos;
            this.filtroClientes = filtroClientes;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public async Task Executar(bool log = true)
#pragma warning restore IDE0060 // Remove unused parameter
        {

            /*
temos 2 opcoes:
        using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
ou
        using (var dbContextTransaction = context.Database.BeginTransaction())
*/

            /*          
                                  //ExecutionStrategyExtensions código gera exceçõa quando tentamos gravar e ler ao mesmo tempo:
                                  //PlatformNotSupportedException: This platform does not support distributed transactions.

                                  {
                                      using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                                      {
                                          var dbGravacao = contextos.ContextoNovo();
                                          var taskGravacao = InserirRegistro(dbGravacao);

                                          var db1 = contextos.ContextoNovo();
                                          var db2 = db1;
                                          await LeituraEmParalelo(db1, db2, log);
                                          await LeituraSequencial(db1, db2, log);

                                          taskGravacao.Wait();
                                      }
                                  }
                                  */


            /*
             * 
             * este código aparentemente funciona (está reclamando da chave primária, que está repetida, e está certo em dar o erro)
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var dbGravacao = contextos.ContextoNovo();
                    var taskGravacao = InserirRegistro(dbGravacao);

                    taskGravacao.Wait();
                }
            }
            */


            /*
             * este código funciona!
             * só dá erro por causa da chave primária repetida
             * */
            {
                try
                {
                    //using (var dbContextTransaction = dbGravacao.Database.BeginTransaction())
                    using (var dbgravacao = contextos.ContextoGravacao())
                    {
                        var taskGravacao = InserirRegistro(dbgravacao);

                        //sem log!
                        log = false;

                        //tem que permitir usando outro contexto (que fica fora da transação)
                        var db1 = contextos.ContextoNovo();
                        var db2 = contextos.ContextoNovo();
                        await LeituraEmParalelo(db1, db2, log);
                        await LeituraSequencial(db1, db2, log);

                        taskGravacao.Wait();
                    }
                }
                catch (Exception e)
                {
                    //sim, ignoramos, vai dar mesmo....
                    if (!e.ToString().Contains(@"iolation of PRIMARY KEY constraint 'PK_t_CLIENTE'"))
                        throw e;
                    //comemos a exceção
                }
            }

        }

        private async Task InserirRegistro(InfraBanco.ContextoBdGravacao bd1)
        {
            Tcliente tCliente = new Tcliente
            {
                Id = "000000000002",
                Dt_Cadastro = DateTime.Now,
                Usuario_Cadastrado = "Apagar",
                Indicador = "Apagar",
                Nome = "Apagar, registro de teste",
                Contribuinte_Icms_Data = DateTime.Now,
                Contribuinte_Icms_Data_Hora = DateTime.Now,
                Produtor_Rural_Data = DateTime.Now,
                Produtor_Rural_Data_Hora = DateTime.Now,
                Obs_crediticias = "",
                Midia = "",
                Email = "Apagar, registro de teste",
                Email_Xml = "Apagar, registro de teste",
                Dt_Ult_Atualizacao = DateTime.Now
            };

            bd1.Add(tCliente);
            await bd1.SaveChangesAsync();
        }

        private async Task LeituraSequencial(InfraBanco.ContextoBd bd1, InfraBanco.ContextoBd bd2, bool log)
        {
            Stopwatch enfileiradas = new Stopwatch();
            enfileiradas.Start();
            var t1 = LerClientes1(bd1);
            var t1res = await t1;
            var t2 = LerPedidos1(bd2);
            var t2res = await t2;
            enfileiradas.Stop();
            if (log)
                ImprimirTempos(enfileiradas, "async uma depois da outra");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "<Pending>")]
        private async Task LeituraEmParalelo(InfraBanco.ContextoBd bd1, InfraBanco.ContextoBd bd2, bool log)
        {
            Stopwatch enfileiradas = new Stopwatch();
            enfileiradas.Start();
            var t1 = LerClientes1(bd1);
            var t2 = LerPedidos1(bd2);
            var t1res = await t1;
            var t2res = await t2;
            enfileiradas.Stop();
            if (log)
                ImprimirTempos(enfileiradas, "async em paralelo");
        }

        private void ImprimirTempos(Stopwatch stopWatch, string msg)
        {
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine(msg + " " + elapsedTime);
        }

        private Task<List<Tcliente>> LerClientes1(InfraBanco.ContextoBd db)
        {
            var lista = (from c in db.Tclientes where filtroClientes.Contains(c.Id) select c).ToListAsync();
            return lista;
        }
        private Task<List<Tpedido>> LerPedidos1(InfraBanco.ContextoBd db)
        {
            var lista = (from c in db.Tpedidos where filtroClientes.Contains(c.Id_Cliente) select c).ToListAsync();
            return lista;
        }

    }
}
