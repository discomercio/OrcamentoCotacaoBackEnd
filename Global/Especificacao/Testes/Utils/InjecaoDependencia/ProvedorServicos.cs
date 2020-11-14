﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Especificacao.Testes.Utils.InjecaoDependencia
{
    public class ProvedorServicos
    {
        //esquema de singleton para ter certeza que inicializa sempre
        private static ProvedorServicos? provedorServicos = null;
        public static ServiceProvider ObterServicos()
        {
            if (provedorServicos == null)
                provedorServicos = new ProvedorServicos();
            return provedorServicos.Servicos;
        }

        private ServiceProvider Servicos { get; set; }
        private ProvedorServicos()
        {
            var logTestes = LogTestes.LogTestes.GetInstance();
            logTestes.LogMensagem("ProvedorServicos inicio");

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

            Ambiente.ApiUnis.InjecaoDependencias.ConfigurarDependencias(services);
            Ambiente.ApiMagento.InjecaoDependencias.ConfigurarDependencias(services);

            Servicos = services.BuildServiceProvider();


            //inicializa o banco de dados
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(Servicos.GetRequiredService<InfraBanco.ContextoBdProvider>(), Servicos.GetRequiredService<InfraBanco.ContextoCepProvider>());
            bd.Inicializar(false);

            logTestes.LogMensagem("ProvedorServicos fim");
        }
    }
}
