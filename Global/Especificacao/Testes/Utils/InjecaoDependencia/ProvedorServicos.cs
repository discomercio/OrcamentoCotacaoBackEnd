using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Especificacao.Testes.Utils.InjecaoDependencia
{
    public class ProvedorServicos
    {
        public ProvedorServicos()
        {
            var logTestes = LogTestes.GetInstance();
            logTestes.Log("ProvedorServicos inicio");

            var services = new ServiceCollection();

            services.AddDbContext<InfraBanco.ContextoBdBasico>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("conexaoLocal"));
                options.UseInMemoryDatabase("bancomemoria");
                options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseInMemoryDatabase("bancocepmemoria");
                options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddTransient<InfraBanco.ContextoBdProvider, InfraBanco.ContextoBdProvider>();
            services.AddTransient<InfraBanco.ContextoCepProvider, InfraBanco.ContextoCepProvider>();
            services.AddTransient<Testes.Utils.InjecaoDependencia.ClasseInjetada, Testes.Utils.InjecaoDependencia.ClasseInjetada>();

            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll>();

            Servicos = services.BuildServiceProvider();


            //inicializa o banco de dados
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(Servicos.GetRequiredService<InfraBanco.ContextoBdProvider>());
            bd.Inicializar();

            logTestes.Log("ProvedorServicos fim");
        }
        public ServiceProvider Servicos { get; private set; }
    }
}
