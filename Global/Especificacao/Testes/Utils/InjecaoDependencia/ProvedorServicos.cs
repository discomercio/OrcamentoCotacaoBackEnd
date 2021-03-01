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
            logTestes.LogMemoria("ProvedorServicos inicio");

            var services = new ServiceCollection();

            services.AddDbContext<InfraBanco.ContextoBdBasico>(options =>
            {
                var usarSqlServer = true;
                if (usarSqlServer)
                {
                    var conexaolocal = "server=ITS-DBDEV\\SQL2017;database=ARCLUBE_TESTES;Uid=appAirClube;Pwd=appAirClube;MultipleActiveResultSets=True;";
                    options.UseSqlServer(conexaolocal);
                }
                else
                {
                    options.UseInMemoryDatabase("bancomemoria");
                    options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                }
                options.EnableSensitiveDataLogging();
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
            Ambiente.Loja.Loja_Bll.InjecaoDependencias.ConfigurarDependencias(services);
            Ambiente.PrepedidoApi.InjecaoDependencias.ConfigurarDependencias(services);

            Servicos = services.BuildServiceProvider();


            //inicializa o banco de dados
            var bd = new Testes.Utils.BancoTestes.InicializarBancoGeral(Servicos.GetRequiredService<InfraBanco.ContextoBdProvider>(), Servicos.GetRequiredService<InfraBanco.ContextoCepProvider>());
            bd.Inicializar();

            logTestes.LogMemoria("ProvedorServicos fim");
        }
    }
}
