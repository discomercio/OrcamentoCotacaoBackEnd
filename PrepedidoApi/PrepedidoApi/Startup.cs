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
using System.Linq;

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
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddCors();

            services.AddMvc(option => option.EnableEndpointRouting = false).
                SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                //nao usamos camelcase nos dados gerados
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<Configuracao>(appSettingsSection);

            //bll
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ClienteBll.ClienteBll, PrepedidoBusiness.Bll.ClienteBll.ClienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PedidoBll, PrepedidoBusiness.Bll.PedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.AcessoBll, PrepedidoBusiness.Bll.AcessoBll>();
            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ProdutoPrepedidoBll, PrepedidoBusiness.Bll.ProdutoPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CepBll, PrepedidoBusiness.Bll.CepBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll.ValidacoesFormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll.ValidacoesFormaPagtoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CoeficienteBll, PrepedidoBusiness.Bll.CoeficienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.ValidacoesPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.ValidacoesPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Utils.IBancoNFeMunicipio, PrepedidoBusiness.Utils.BancoNFeMunicipio>();
            

            //ContextoProvider
            services.AddTransient<InfraBanco.ContextoBdProvider, InfraBanco.ContextoBdProvider>();
            services.AddTransient<InfraBanco.ContextoCepProvider, InfraBanco.ContextoCepProvider>();

            //banco de dados
            string conexaoBasica = Configuration.GetConnectionString("conexao");
            services.AddDbContext<InfraBanco.ContextoBdBasico>(options =>
            {
                options.UseSqlServer(conexaoBasica);
                options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
                options.EnableSensitiveDataLogging();
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
#pragma warning disable CS0618 // Type or member is obsolete
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            IConfigurationBuilder configurationBuilderVersaoApi = new ConfigurationBuilder();
            configurationBuilderVersaoApi.AddJsonFile("versaoapi.json");
            var appSettingsSectionVersaoApi = configurationBuilderVersaoApi.Build().GetSection("VersaoApi");
            var nossaApiVersion = appSettingsSectionVersaoApi.Get<ConfiguracaoVersaoApi>().VersaoApi;

            // Route all unknown requests to app root
            app.Use(async (context, next) =>
            {
                if (context.Request.Method.ToUpper() == "POST" || context.Request.Method.ToUpper() == "GET")
                {
                    if (context.Request.Path.Value.ToLower().Contains("/api/"))
                    {
                        /*
                         * sem cache se for da API:
                            Cache-Control: no-store,no-cache
                            Pragma: no-cache

                        e tb retornamos a versão da API
                            */
                        context.Response.Headers.Add("Cache-Control", "no-store,no-cache");
                        context.Response.Headers.Add("Pragma", "no-cache");
                        context.Response.Headers.Add("X-API-Version", nossaApiVersion);
                        context.Response.Headers.Add("Access-Control-Expose-Headers", "X-API-Version");

                        /*
                         * exigimos a versão da API
                         * X-API-Version: SUBSTITUIR_VERSAO_API
                         * */
                        var apiVersion = context.Request.Headers["X-API-Version"];
                        if (!apiVersion.Any(r => r == nossaApiVersion))
                        {
                            context.Response.StatusCode = 412; // 412 Precondition Failed 

                            //os cabeçalhos devem ser definidos antes do conteúdo
                            if (env.IsDevelopment())
                                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            context.Response.Headers.Add("Content-Type", "text/html; charset=UTF-8");
                            var msg = $"Erro: use um cabeçalho \"X-API-Version\" com o valor {nossaApiVersion}. Formato: <br><br>X-API-Version: {nossaApiVersion}";
                            context.Response.Body.Write(Encoding.UTF8.GetBytes(msg));

                            return;
                        }
                    }
                }
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
                 * nao precisamo de CORS porque servimos tudo do aplicativo principal
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
