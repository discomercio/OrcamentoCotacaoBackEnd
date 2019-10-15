using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Testes.Labs.InfraBancoLab
{
    class LeituraAsync
    {
        /*
         * Uma ideia interssante:
         * 
         
            https://stackoverflow.com/questions/10437058/how-to-make-entity-framework-data-context-readonly

         In places where I need to have a read-only context such as in the Read side of CQRS pattern, I use the following implementation. It doesn't provide anything other than Querying capabilities to its consumer.

        public class ReadOnlyDataContext
        {
            private readonly DbContext _dbContext;

            public ReadOnlyDataContext(DbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public IQueryable<TEntity> Set<TEntity>() where TEntity : class
            {
                return _dbContext.Set<TEntity>().AsNoTracking();
            }

            public void Dispose()
            {
                _dbContext.Dispose();
            }
        }
        By using ReadOnlyDataContext, you can have access to only querying capabilities of DbContext. Let's say you have an entity named Order, then you would use ReadOnlyDataContext instance in a way like below.

        readOnlyDataContext.Set<Order>().Where(q=> q.Status==OrderStatus.Delivered).ToArray();
        */

        private readonly Contextos contextos;
        private readonly List<string> filtroClientes;

        public LeituraAsync(Contextos contextos, List<string> filtroClientes)
        {
            this.contextos = contextos;
            this.filtroClientes = filtroClientes;
        }

        public async Task Executar(bool log = true)
        {
            /*
            está dando exceção....
            */
            Console.WriteLine("Com o mesmo contexto");
            var db1 = contextos.ContextoNovo();
            var db2 = db1;
            await ExecutarEmParalelo(db1, db2, log);
            await ExecutarSequencial(db1, db2, log);
            await ExecutarEmParalelo(db1, db2, log);
            await ExecutarSequencial(db1, db2, log);

            Console.WriteLine("Com contextos diferentes");
            db1 = contextos.ContextoNovo();
            db2 = contextos.ContextoNovo();
            await ExecutarEmParalelo(db1, db2, log);
            await ExecutarSequencial(db1, db2, log);
            await ExecutarEmParalelo(db1, db2, log);
            await ExecutarSequencial(db1, db2, log);
        }

        private async Task ExecutarSequencial(InfraBanco.ContextoBd bd1, InfraBanco.ContextoBd bd2, bool log)
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

        private async Task ExecutarEmParalelo(InfraBanco.ContextoBd bd1, InfraBanco.ContextoBd bd2, bool log)
        {
            Stopwatch enfileiradas = new Stopwatch();
            enfileiradas.Start();
            var t1 = LerClientes1(bd1);
            var t2 = LerPedidos1(bd2);
            var t1res = await t1;
            var t2res = await t2;
            enfileiradas.Stop();
            if (log)
                ImprimirTempos(enfileiradas, "async em apralelo");
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
