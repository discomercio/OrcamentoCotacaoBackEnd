using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfraBanco
{
    public class ContextoBd : DbContext
    {
        public ContextoBd(DbContextOptions<ContextoBd> opt) : base(opt)
        {

        }
        public DbSet<Tcliente> Tcliente { get; set; }
        public DbSet<Torcamento> Torcamentos { get; set; }
    }
}
