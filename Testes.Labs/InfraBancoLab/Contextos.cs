using InfraBanco;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Testes.Labs.InfraBancoLab
{
    class Contextos
    {
        //fornecemos contextos
        //queremos testar o funcionamento criando contextos novos a cada acesso ou usamos o mesmo
        public InfraBanco.ContextoBd ContextoNovo()
        {
            return CriarContexto();
        }
        public InfraBanco.ContextoBd ContextoCompartilhado()
        {
            return contexto;
        }

        public InfraBanco.ContextoBdGravacao ContextoGravacao()
        {
            return new ContextoBdProvider(optionsBuilder.Options).GetContextoGravacaoParaUsing();
        }

        private readonly InfraBanco.ContextoBd contexto;
        private Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<InfraBanco.ContextoBdBasico> optionsBuilder;
        public Contextos()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");
            IConfiguration configuration = configurationBuilder.Build();
            optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<InfraBanco.ContextoBdBasico>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("conexaoLocal"));
            contexto = CriarContexto();
        }
        private ContextoBd CriarContexto()
        {
            var ret = new ContextoBdProvider(optionsBuilder.Options).GetContextoLeitura();
            return ret;
        }

    }
}
