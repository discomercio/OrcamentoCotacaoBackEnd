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
        }

        public DbContextOptions<ContextoBd> Opt { get; }

        public ContextoBd GetContexto()
        {
            return new ContextoBd(Opt);
        }
    }
}
