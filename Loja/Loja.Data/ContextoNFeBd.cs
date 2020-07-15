using Loja.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Data
{
    public class ContextoNFeBd: DbContext
    {
        public ContextoNFeBd(DbContextOptions<ContextoNFeBd> opt) : base(opt)
        {

        }

        public DbSet<NfeMunicipio> NfeMunicipios { get; set; }
        public DbSet<NfeUf> NfeUfs { get; set; }
    }
}
