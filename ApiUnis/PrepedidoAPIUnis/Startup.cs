using System;
using System.IO;
using System.Text;
using InfraIdentity.ApiUnis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PrepedidoApiUnisBusiness.UnisBll.ClienteUnisBll;
using PrepedidoApiUnisBusiness.UnisBll.PrePedidoUnisBll;
using PrepedidoUnisBusiness.UnisBll.AcessoBll;
using PrepedidoUnisBusiness.UnisBll.CepUnisBll;
using PrepedidoUnisBusiness.UnisBll.CoeficienteUnisBll;
using PrepedidoUnisBusiness.UnisBll.FormaPagtoUnisBll;
using PrepedidoUnisBusiness.UnisBll.ProdutoUnisBll;
using PrepedidoUnisBusiness.Utils;

namespace PrepedidoAPIUnis
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
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver())
                .AddJsonOptions(opt => opt.JsonSerializerOptions.PropertyNamingPolicy = null);


            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis>(appSettingsSection);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Unis V1", Version = "v1" });

                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);

                //agora os DTOs
                xmlFile = $"PrepedidoUnisBusiness.XML";
                xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<ClienteUnisBll, ClienteUnisBll>();
            services.AddTransient<PrePedidoUnisBll, PrePedidoUnisBll>();
            services.AddTransient<ProdutoUnisBll, ProdutoUnisBll>();
            services.AddTransient<AcessoUnisBll, AcessoUnisBll>();
            services.AddTransient<CepUnisBll, CepUnisBll>();
            services.AddTransient<FormaPagtoUnisBll, FormaPagtoUnisBll>();
            services.AddTransient<CoeficienteUnisBll, CoeficienteUnisBll>();

            services.AddSingleton<ConfiguracaoApiUnis>(c =>
            {
                IConfiguration configuration = c.GetService<IConfiguration>();
                var appSettingsSectionSingleton = configuration.GetSection("AppSettings");
                var configuracaoApiUnis = appSettingsSectionSingleton.Get<ConfiguracaoApiUnis>();
                return configuracaoApiUnis;
            });

            //Bll's da Arclube
            services.AddTransient<PrepedidoBusiness.Bll.CepBll, PrepedidoBusiness.Bll.CepBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.PrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ClienteBll.ClienteBll, PrepedidoBusiness.Bll.ClienteBll.ClienteBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll.FormaPagtoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.FormaPagtoBll.ValidacoesFormaPagtoBll, PrepedidoBusiness.Bll.FormaPagtoBll.ValidacoesFormaPagtoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.CoeficienteBll, PrepedidoBusiness.Bll.CoeficienteBll>();
            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<PrepedidoBusiness.Bll.ProdutoPrepedidoBll, PrepedidoBusiness.Bll.ProdutoPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.ValidacoesPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.ValidacoesPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll, PrepedidoBusiness.Bll.PrepedidoBll.MontarLogPrepedidoBll>();
            services.AddTransient<PrepedidoBusiness.UtilsNfe.IBancoNFeMunicipio, PrepedidoBusiness.UtilsNfe.BancoNFeMunicipio>();

            services.AddTransient<IServicoAutenticacaoApiUnis, ServicoAutenticacaoApiUnis>();
            //como singleton para melhorar a performance
            services.AddSingleton<IServicoValidarTokenApiUnis, ServicoValidarTokenApiUnis>();

            //ContextoProvider
            services.AddTransient<InfraBanco.ContextoBdProvider, InfraBanco.ContextoBdProvider>();
            services.AddTransient<InfraBanco.ContextoCepProvider, InfraBanco.ContextoCepProvider>();

            //banco de dados
            string conexaoBasica = Configuration.GetConnectionString("conexao");
            services.AddDbContext<InfraBanco.ContextoBdBasico>(options =>
            {
                //options.UseSqlServer(Configuration.GetConnectionString("conexaoLocal"));
                options.UseSqlServer(conexaoBasica);
                options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
                options.EnableSensitiveDataLogging();
            });


            // configure jwt authentication
            var appSettings = appSettingsSection.Get<PrepedidoUnisBusiness.Utils.ConfiguracaoApiUnis>();
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

                x.SecurityTokenValidators.Clear();

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            new InfraIdentity.ApiUnis.SetupAutenticacaoApiUnis().ConfigurarTokenApiUnis(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // Type or member is obsolete
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            // Route all unknown requests to app root
            app.Use(async (context, next) =>
            {
                //log das requisicoes
                logger.LogWarning("Log da requisição");
                await next();
            });


            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unis V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
