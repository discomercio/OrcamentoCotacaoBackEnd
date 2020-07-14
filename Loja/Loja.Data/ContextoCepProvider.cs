using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Data
{
    public class ContextoCepProvider
    {
        public ContextoCepProvider(DbContextOptions<ContextoCepBd> opt)
        {
            Opt = opt;
        }

        public DbContextOptions<ContextoCepBd> Opt { get; }

        public ContextoCepBd GetContextoLeitura()
        {
            //para leitura, cada leitura com uma conexao nova
            return new ContextoCepBd(Opt);
        }

    }
}
