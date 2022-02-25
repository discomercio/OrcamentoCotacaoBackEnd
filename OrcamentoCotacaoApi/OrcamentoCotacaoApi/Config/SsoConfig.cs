using InfraIdentity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrcamentoCotacaoApi.Utils;
using System.Text;

namespace OrcamentoCotacaoApi.Config
{
    public static class SsoConfig
    {
        public static IServiceCollection AddSSO(this IServiceCollection services, IConfiguration Configuration)
        {
            string conexaoBasica = Configuration.GetConnectionString("conexao");
            var appSettings = Configuration.GetSection("AppSettings").Get<Configuracao>();

            //isto deveria ser passado para o SetupAutenticacao
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;

                /*
	            verificar se usuário continua ativo a cada requisição? dá algum trabalho e fica mais lento. 
                Resposta em 26/09/2019: sim, verificar se o usuário está ativo em cada requisição.
                */
                x.SecurityTokenValidators.Clear();

                Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<InfraBanco.ContextoBdBasico> optionsBuilder;
                optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<InfraBanco.ContextoBdBasico>();
                optionsBuilder.UseSqlServer(conexaoBasica);

                //temos que crir os objetos com todas as suas dependencias aqui mesmo 
                //(nao podemos usar a resolução de serviços da injeção de dependencias do .net)
                x.SecurityTokenValidators.Add(new ValidarCredenciais(new ValidarCredenciaisServico(
                    new OrcamentoCotacaoBusiness.Bll.AcessoBll(new InfraBanco.ContextoBdProvider(optionsBuilder.Options,
                        new InfraBanco.ContextoBdGravacaoOpcoes(appSettings.TRATAMENTO_ACESSO_CONCORRENTE_LOCK_EXCLUSIVO_MANUAL_HABILITADO))))));

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SegredoToken)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            new InfraIdentity.SetupAutenticacao().ConfigurarToken(services);

            return services;
        }
    }
}
