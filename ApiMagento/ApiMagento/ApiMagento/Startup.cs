using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfraIdentity.ApiMagento;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MagentoBusiness.MagentoBll.AcessoBll;
using MagentoBusiness.UtilsMagento;

namespace ApiMagento
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
            services.Configure<MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento>(appSettingsSection);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Magento V1", Version = "v1" });

                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);

                //agora os DTOs
                xmlFile = $"MagentoBusiness.XML";
                xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            global::ApiMagento.Startup.ConfigurarServicosComuns(services);

            //Bll's locais
            services.AddTransient<Cep.IBancoNFeMunicipio, Cep.BancoNFeMunicipio>();

            //Bll's Loja
            //afazer: incluir as bll's que ser�o utilizadas da Loja

            services.AddSingleton<ConfiguracaoApiMagento>(c =>
            {
                IConfiguration configuration = c.GetService<IConfiguration>();
                var appSettingsSectionSingleton = configuration.GetSection("AppSettings");
                var configuracaoApiMagento = appSettingsSectionSingleton.Get<ConfiguracaoApiMagento>();
                return configuracaoApiMagento;
            });

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
            var appSettings = appSettingsSection.Get<MagentoBusiness.UtilsMagento.ConfiguracaoApiMagento>();
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

            new InfraIdentity.ApiMagento.SetupAutenticacaoApiMagento().ConfigurarTokenApiMagento(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS0618 // Type or member is obsolete
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, ILogger<Startup> logger)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            // Route all unknown requests to app root
            app.Use(async (context, next) =>
            {
                //log das requisicoes
                logger.LogWarning("Log da requisi��o");
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Magento V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        public static void ConfigurarServicosComuns(IServiceCollection services)
        {
            services.AddTransient<AcessoMagentoBll, AcessoMagentoBll>();
            services.AddTransient<MagentoBusiness.MagentoBll.MagentoBll.PedidoMagentoBll, MagentoBusiness.MagentoBll.MagentoBll.PedidoMagentoBll>();
            services.AddTransient<MagentoBusiness.MagentoBll.MagentoBll.ObterCodigoMarketplaceBll, MagentoBusiness.MagentoBll.MagentoBll.ObterCodigoMarketplaceBll>();
            services.AddTransient<MagentoBusiness.MagentoBll.MagentoBll.PedidoMagentoClienteBll, MagentoBusiness.MagentoBll.MagentoBll.PedidoMagentoClienteBll>();

            services.AddTransient<Cep.CepBll, Cep.CepBll>();
            services.AddTransient<Prepedido.PrepedidoBll, Prepedido.PrepedidoBll>();
            services.AddTransient<Cliente.ClienteBll, Cliente.ClienteBll>();
            services.AddTransient<Prepedido.FormaPagto.FormaPagtoBll, Prepedido.FormaPagto.FormaPagtoBll>();
            services.AddTransient<Prepedido.FormaPagto.ValidacoesFormaPagtoBll, Prepedido.FormaPagto.ValidacoesFormaPagtoBll>();
            services.AddTransient<Produto.CoeficienteBll, Produto.CoeficienteBll>();
            services.AddTransient<Produto.ProdutoGeralBll, Produto.ProdutoGeralBll>();
            services.AddTransient<Prepedido.ValidacoesPrepedidoBll, Prepedido.ValidacoesPrepedidoBll>();
            services.AddTransient<Prepedido.MontarLogPrepedidoBll, Prepedido.MontarLogPrepedidoBll>();
            services.AddTransient<Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll, Prepedido.PedidoVisualizacao.PedidoVisualizacaoBll>();
            services.AddTransient<Pedido.EfetivaPedidoBll, Pedido.EfetivaPedidoBll>();
            services.AddTransient<Pedido.MontarLogPedidoBll, Pedido.MontarLogPedidoBll>();
            services.AddTransient<Pedido.PedidoCriacao, Pedido.PedidoCriacao>();
            services.AddTransient<Pedido.PedidoBll, Pedido.PedidoBll>();

            services.AddTransient<IServicoAutenticacaoApiMagento, ServicoAutenticacaoApiMagento>();
            //como singleton para melhorar a performance
            services.AddSingleton<MagentoBusiness.MagentoBll.AcessoBll.IServicoValidarTokenApiMagento, ServicoValidarTokenApiMagento>();
            services.AddTransient<IServicoDecodificarTokenApiMagento, ServicoDecodificarTokenApiMagento>();

        }
    }
}
