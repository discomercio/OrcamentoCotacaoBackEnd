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
        //esquema de singleton para ter certeza que inicializa sempre
        private static ProvedorServicos? provedorServicos = null;
        public static ServiceProvider ObterServicos()
        {
            if (provedorServicos == null)
                provedorServicos = new ProvedorServicos();
            return provedorServicos.Servicos;
        }

        private ServiceProvider Servicos { get; set; }
        //todo: dando 12 erros com o sql server
        //se tira as transações não dá erro. com transação, dá erro de timeout.
        //System.Data.IsolationLevel.ReadUncommitted dá 3 erros, dizendo MultipleActiveResultSets
        //System.Data.IsolationLevel.Serializable dá 12 erros, dizendo timeout e MultipleActiveResultSets
        public static readonly bool UsarSqlServerNosTestesAutomatizados = false;
        private ProvedorServicos()
        {
            var logTestes = LogTestes.LogTestes.GetInstance();
            logTestes.LogMemoria("ProvedorServicos inicio");

            var services = new ServiceCollection();

            services.AddDbContext<InfraBanco.ContextoBdBasico>(options =>
            {
                if (UsarSqlServerNosTestesAutomatizados)
                {
                    string conexaolocal;
                    //algumas tentativas:
                    conexaolocal = "server=ITS-DBDEV\\SQL2017;database=ARCLUBE_TESTES;Uid=appAirClube;Pwd=appAirClube;Pooling=false;Max Pool Size=1;";
                    conexaolocal = "server=ITS-DBDEV\\SQL2017;database=ARCLUBE_TESTES;Uid=appAirClube;Pwd=appAirClube;Pooling=true;Max Pool Size=400;";
                    conexaolocal = "server=ITS-DBDEV\\SQL2017;database=ARCLUBE_TESTES;Uid=appAirClube;Pwd=appAirClube;MultipleActiveResultSets=true;";
                    //a que temos no appsettngs
                    conexaolocal = "server=ITS-DBDEV\\SQL2017;database=ARCLUBE_TESTES;Uid=appAirClube;Pwd=appAirClube;";
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
