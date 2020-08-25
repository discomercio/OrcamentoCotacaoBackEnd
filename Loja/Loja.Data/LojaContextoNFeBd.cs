using Loja.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loja.Data
{
    public class LojaContextoNFeBd : DbContext
    {
        public LojaContextoNFeBd (DbContextOptions<LojaContextoNFeBd > opt) : base(opt)
        {

        }

        public DbSet<NfeMunicipio> NfeMunicipios { get; set; }
        public DbSet<NfeUf> NfeUfs { get; set; }
    }
}
