using Loja.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Data
{
    public class LojaContextoCepBd : DbContext
    {
        public LojaContextoCepBd(DbContextOptions<LojaContextoCepBd> opt) : base(opt)
        {

        }

        public DbSet<LogLogradouro> LogLogradouros { get; set; }
        public DbSet<LogBairro> LogBairros { get; set; }
        public DbSet<LogLocalidade> LogLocalidades { get; set; }
        public DbSet<TcepLogradouro> TcepLogradouros { get; set; }

    }
}
