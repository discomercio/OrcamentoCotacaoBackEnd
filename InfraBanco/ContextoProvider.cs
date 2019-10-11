using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco
{
    public class ContextoProvider
    {
        public ContextoProvider(DbContextOptions<ContextoBd> opt)
        {
            Opt = opt;
            Ctx = new ContextoBd(Opt);
        }

        public DbContextOptions<ContextoBd> Opt { get; }

        //temos somente est instacnia. O ContextoProvider esta sendo usado com AddTransient; isto é, cada novo objeto vai receber uma nova
        //instancia deste objeto. E todas as operações feitas por uma instância vão compartilhar a conexão.
        private ContextoBd Ctx;

        public ContextoBd GetContextoLeitura()
        {
            //para leitura, cada leitura com uma conexao nova
            return new ContextoBd(Opt);
        }
        public ContextoBd GetContextoGravacao()
        {
            //para gravacao, todos compartilham a mesma coenxao (todos nesta instancia)
            return Ctx;
        }
    }
}
