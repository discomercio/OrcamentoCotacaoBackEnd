using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Data
{
    public class LojaContextoNFeProvider
    {
        public LojaContextoNFeProvider(DbContextOptions<LojaContextoNFeBd > opt)
        {
            Opt = opt;
        }

        public DbContextOptions<LojaContextoNFeBd > Opt { get; }

        public LojaContextoNFeBd  GetContextoLeitura()
        {
            //para leitura, cada leitura com uma conexao nova
            return new LojaContextoNFeBd (Opt);
        }
    }
}
