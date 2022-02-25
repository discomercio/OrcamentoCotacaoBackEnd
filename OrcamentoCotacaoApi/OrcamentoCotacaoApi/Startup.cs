﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using OrcamentoCotacaoApi.Config;
using OrcamentoCotacaoApi.Utils;
using System.Linq;
using System.Text;

namespace OrcamentoCotacaoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Configuracao>(Configuration.GetSection("AppSettings"));
            services.Configure<Configuracoes>(Configuration.GetSection("Configuracoes"));

            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddCors();

            //nao usamos camelcase nos dados gerados
            services.AddMvc(option => option.EnableEndpointRouting = false).
                SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerX();

            services.AddSSO(Configuration);
            services.AddInjecaoDependencia(Configuration);

            services.ApplicationMappersIoC(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            if (env.IsDevelopment() || env.IsProduction())
            {
                /*
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

                app.UseSwaggerX(Configuration);

                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
