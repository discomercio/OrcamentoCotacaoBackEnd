using InfraBanco.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InfraBanco
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }


        public DbSet<TprodutoCatalogo> TprodutoCatalogos { get; set; }
        public DbSet<TProdutoCatalogoPropriedade> TProdutoCatalogoPropriedades { get; set; }
        public DbSet<TProdutoCatalogoPropriedadeOpcao> TProdutoCatalogoPropriedadeOpcoes { get; set; }
        public DbSet<TprodutoCatalogoItem> TprodutoCatalogoItem { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Contexto).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
    }
}
