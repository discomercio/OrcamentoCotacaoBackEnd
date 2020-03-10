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
using InfraIdentity;

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
            services.AddTransient<PrepedidoBusiness.Bll.ClienteBll, PrepedidoBusiness.Bll.ClienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PedidoBll, PrepedidoBusiness.Bll.PedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.AcessoBll, PrepedidoBusiness.Bll.AcessoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ProdutoBll, PrepedidoBusiness.Bll.ProdutoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CepBll, PrepedidoBusiness.Bll.CepBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CoeficienteBll, PrepedidoBusiness.Bll.CoeficienteBll>();

            //ContextoProvider
            services.AddTransient<InfraBanco.ContextoBdProvider, InfraBanco.ContextoBdProvider>();
            services.AddTransient<InfraBanco.ContextoCepProvider, InfraBanco.ContextoCepProvider>();
            services.AddTransient<InfraBanco.ContextoNFeProvider, InfraBanco.ContextoNFeProvider>();

            //banco de dados
            string conexaoBasica = Configuration.GetConnectionString("conexaohomologa");
            services.AddDbContext<InfraBanco.ContextoBdBasico>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("conexaoLocal"));
                options.UseSqlServer(conexaoBasica);
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
            });
            services.AddDbContext<InfraBanco.ContextoNFeBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoNfe"));
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
                    new PrepedidoBusiness.Bll.AcessoBll(new InfraBanco.ContextoBdProvider(optionsBuilder.Options)))));

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
                //Estamos fazendo essas verificaçoes para poder forçar o return de "403" para que 
                //possamos fazer o ratamento no Angular
                //O objetivo aqui é fazer a validação de que o usuário tem permissão de acesso
                //Em x.SecurityTokenValidators.Add estamos fazendo a chamada para validar a cada requisição que é feita
                if (context.Request.Method.ToLower() != "options" && context.Request.Path.HasValue &&
                context.Response.StatusCode != 404)
                {
                    var autenticado = context.User.Identity.IsAuthenticated;
                    if (!autenticado
                        && !context.Request.Path.Value.ToLower().Contains("fazerlogin")
                        && !context.Request.Path.Value.ToLower().Contains("alterarsenha"))
                    {
                        context.Response.StatusCode = 403; //negado!
                        return;
                    }
                }

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
