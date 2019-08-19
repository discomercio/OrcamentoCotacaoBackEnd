using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using PrepedidoApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace PrepedidoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<Configuracao>(appSettingsSection);

            //bll
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll>();
            services.AddTransient<InfraBanco.ContextoProvider, InfraBanco.ContextoProvider>();
            services.AddTransient<PrepedidoBusiness.Bll.ClienteBll, PrepedidoBusiness.Bll.ClienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PedidoBll, PrepedidoBusiness.Bll.PedidoBll>();

            //banco de dados
            services.AddDbContext<InfraBanco.ContextoBd>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("conexao"));
            });

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<Configuracao>();
            var key = Encoding.ASCII.GetBytes(appSettings.SegredoToken);

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
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            new InfraIdentity.SetupAutenticacao().ConfigurarToken(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
