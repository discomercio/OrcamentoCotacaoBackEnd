using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraBanco
{
    public class ContextoRelatorioProvider
    {
        /*
         * Esse contexto deve ser utilizado apenas para criação de relátorios
         */
        public ContextoRelatorioProvider(DbContextOptions<ContextoRelatorioBd> opt)
        {
            Opt = opt;
        }

        public DbContextOptions<ContextoRelatorioBd> Opt { get; }

        public ContextoRelatorioBd GetContextoLeitura()
        {
            //para leitura, cada leitura com uma conexao nova
            return new ContextoRelatorioBd(Opt);
        }
    }
}
