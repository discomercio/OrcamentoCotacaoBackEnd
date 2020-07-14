using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco
{
    public class ContextoNFeProvider
    {
        public ContextoNFeProvider(DbContextOptions<ContextoNFeBd> opt)
        {
            Opt = opt;
        }

        public DbContextOptions<ContextoNFeBd> Opt { get; }

        public ContextoNFeBd GetContextoLeitura()
        {
            //para leitura, cada leitura com uma conexao nova
            return new ContextoNFeBd(Opt);
        }
    }
}
