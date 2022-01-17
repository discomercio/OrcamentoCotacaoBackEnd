using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrcamentoCotacaoModel;

namespace OrcamentoCotacaoData
{
    public class ApplicationDBContext : DbContext
    {
        private readonly DbContextOptions<ApplicationDBContext> _options;
        private readonly Configuracao _configuracao;

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options,
            Configuracao configuracao) : base(options)
        {
            _options = options;
            _configuracao = configuracao;
        }

        public ApplicationDBContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
            optionsBuilder.UseSqlServer(_configuracao.Conexao);
            //,mySqlOptionsAction: options => { options.EnableRetryOnFailure(); });


            return new ApplicationDBContext(optionsBuilder.Options, _configuracao);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<ClienteSession>()
            //    .HasKey(o => new { o.IdCliente, o.SessionName });

            //modelBuilder.Entity<ClienteSessionContato>()
            //    .HasKey(o => new { o.IdClienteSessionContato, o.SessionName, o.IdCliente });
            //modelBuilder.Entity<ArvoreAtendimento>()
            //    .HasKey(o => new { o.IdArvoreAtendimento, o.IdCliente, o.SessionName });

        }

        //public DbSet<Cliente> Clientes { get; set; }
        //public DbSet<ClienteSession> ClienteSession { get; set; }
        //public DbSet<AcaoNovoReceptivo> AcaoNovoReceptivo { get; set; }
        //public DbSet<ClienteSessionContato> ClienteSessionContato { get; set; }
        //public DbSet<ClienteSessionContatoMensagem> ClienteSessionContatoMensagem { get; set; }
        //public DbSet<SentidoMensagem> SentidoMensagem { get; set; }
        //public DbSet<ArvoreAtendimento> ArvoreAtendimento { get; set; }
    }
}
