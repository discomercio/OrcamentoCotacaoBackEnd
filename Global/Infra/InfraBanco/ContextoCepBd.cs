using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraBanco
{
    public class ContextoCepBd : DbContext
    {
        public ContextoCepBd(DbContextOptions<ContextoCepBd> opt) : base(opt)
        {

        }
        
        public DbSet<LogLogradouro> LogLogradouros { get; set; }
        public DbSet<LogBairro> LogBairros { get; set; }
        public DbSet<LogLocalidade> LogLocalidades { get; set; }
        public DbSet<TcepLogradouro> TcepLogradouros { get; set; }
        public DbSet<LogGrandeUsuario> LogGrandeUsuarios { get; set; }

    }
}
