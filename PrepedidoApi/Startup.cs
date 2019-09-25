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
using Newtonsoft.Json.Serialization;

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

            services.AddMvc().
                SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                //nao usamos camelcase nos dados gerados
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<Configuracao>(appSettingsSection);

            //bll
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll>();
            services.AddTransient<InfraBanco.ContextoProvider, InfraBanco.ContextoProvider>();
            services.AddTransient<InfraBanco.ContextoCepProvider, InfraBanco.ContextoCepProvider>();
            services.AddTransient<PrepedidoBusiness.Bll.ClienteBll, PrepedidoBusiness.Bll.ClienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PedidoBll, PrepedidoBusiness.Bll.PedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.AcessoBll, PrepedidoBusiness.Bll.AcessoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ProdutoBll, PrepedidoBusiness.Bll.ProdutoBll>(); 

            //banco de dados
            services.AddDbContext<InfraBanco.ContextoBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexao"));
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
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
            // Route all unknown requests to app root
            app.Use(async (context, next) =>
            {
                await next();

                // If there's no available file and the request doesn't contain an extension, we're probably trying to access a page.
                // Rewrite request to use app root
                if (context.Response.StatusCode == 404 && !System.IO.Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html"; // Put your Angular root page here 
                    context.Response.StatusCode = 200; // Make sure we update the status code, otherwise it returns 404
                    await next();
                }
            });


            if (env.IsDevelopment())
            {
                /*
                 * 
                 * nao precisamo de CROs porque servimos tudo do aplicativo principal
                 * quer dizer, copiamos os arquivos do angular para o wwwroot
                 * 
                 * mas em dev precisamos sim!
                 * */
                // global cors policy
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
