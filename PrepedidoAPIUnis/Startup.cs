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
            services.AddCors();

            services.AddMvc().
                SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                //nao usamos camelcase nos dados gerados
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());


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
            services.AddTransient<AcessoUnisBll, AcessoUnisBll>();
            services.AddTransient<CepUnisBll, CepUnisBll>();

            services.AddTransient<PrepedidoBusiness.Bll.CepBll, PrepedidoBusiness.Bll.CepBll>();

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
            });
            services.AddDbContext<InfraBanco.ContextoCepBd>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("conexaoCep"));
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
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
