using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Data
{
    public class LojaContextoCepProvider
    {
        public LojaContextoCepProvider(DbContextOptions<LojaContextoCepBd> opt)
        {
            Opt = opt;
        }

        public DbContextOptions<LojaContextoCepBd> Opt { get; }

        public LojaContextoCepBd GetContextoLeitura()
        {
            //para leitura, cada leitura com uma conexao nova
            return new LojaContextoCepBd(Opt);
        }

    }
}
